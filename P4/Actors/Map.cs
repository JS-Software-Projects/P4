namespace p4.Actors;
public class Map
{
    public readonly Point Size = new(9, 8);
    public Tile[,] Tiles { get; }
    public Point TileSize { get; }

    public Vector2 MapToScreen(int x, int y) =>  new(x * TileSize.X, y * TileSize.Y);
    public (int x, int y) ScreenToMap(Vector2 pos) => ((int)pos.X / TileSize.X, (int)pos.Y / TileSize.Y);

    public Map()
    {
        Tiles = new Tile[Size.X, Size.Y];
        var texture = Globals.Content.Load<Texture2D>("tile");
        TileSize = new(texture.Width, texture.Height);

        for (int y = 0; y < Size.Y; y++)
        {
            for (int x = 0; x < Size.X; x++)
            {
                Tiles[x, y] = new(texture, MapToScreen(x, y), x, y);
            }
        }

        for (int x = 0; x < Size.X - 3; x++)
        {
            Tiles[x, 4].Blocked = true;
            Tiles[x,3].AddBoundry("bottom");
            if (x == Size.X -4)
            {
                Tiles[x+1,3].AddBoundry("bottom"); 
            }
        }
        for (int y = 4; y < Size.Y ; y++)
        {
            Tiles[Size.X-3, y].Blocked = true;
            Tiles[Size.X-2, y].AddBoundry("left");
        }


        for (int x = 0; x < Size.X -1; x++)
        {
            Tiles[x, 2].Blocked = true;
            Tiles[x, 3].AddBoundry("top");
        }
        for (int y = 2; y < Size.Y; y++)
        {
            Tiles[Size.X - 1, y].Blocked = true;
            if (y != 2) //ignore first 
            {
                Tiles[Size.X - 2, y].AddBoundry("right");
            }
            
        }

    }

    public void Update()
    {
        for (int y = 0; y < Size.Y; y++)
        {
            for (int x = 0; x < Size.X; x++) Tiles[x, y].Update();
        }
    }

    public void Draw()
    {
        for (int y = 0; y < Size.Y; y++)
        {
            for (int x = 0; x < Size.X; x++) Tiles[x, y].Draw();
        }
    }
}
