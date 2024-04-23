using P4.managers;
namespace p4.Actors;

public class Tile : Sprite
{
    public bool Blocked { get; set; }
    public bool Path { get; set; }
    private readonly int _mapX;
    private readonly int _mapY;

    public Tile(Texture2D texture, Vector2 position, int mapX, int mapY) : base(texture, position)
    {
        _mapX = mapX;
        _mapY = mapY;
        Path = false;
    }

    public void Update()
    {
        if (Pathfinder.Ready() && Rectangle.Contains(InputManager.tile))
        {
            if (InputManager.execute)
            {
                Pathfinder.BFSearch(_mapX, _mapY);
            }
        }

        Color = Path ? Color.Green : Color.White;
        Color = Blocked ? Color.Red : Color;
    }
}
