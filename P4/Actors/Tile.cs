using System.Collections.Generic;
using P4.managers;
namespace p4.Actors;

public class Tile : Sprite
{
    public bool Blocked { get; set; }
    public bool Path { get; set; }
    public readonly int _mapX;
    public readonly int _mapY;
    public List<string> _boundry = new();

    public Tile(Texture2D texture, Vector2 position, int mapX, int mapY) : base(texture, position)
    {
        _mapX = mapX;
        _mapY = mapY;
        Path = false;
    }
    public void AddBoundry(string b)
    {
        _boundry.Add(b);
    }

    public void Update()
    {
        if (Pathfinder.Ready() && Rectangle.Contains(InputManager.tile))
        {
            if (InputManager.execute)
            {
                Pathfinder.BFSearch(_mapX, _mapY);
                InputManager.SetExecute(false,_mapX, _mapY);
            }
        }

        Color = Path ? Color.Green : Color.White;
        //Color = Blocked ? Color.Red : Color;
        if (_boundry.Count == 2)
        {
            Color = Color.RosyBrown;
        }
    }
    public override void Draw()
    {
        base.Draw(); // Draws the tile itself

        int lineWidth = 2; // Width of the border line
        Color lineColor = Color.Brown; // Default color for boundaries

        // Loop through each boundary in the list and draw the appropriate line
        foreach (var boundary in _boundry)
        {
            switch (boundary.ToLower())
            {
                case "top":
                    DrawLine(Globals.SpriteBatch,Globals.Pixel, new Vector2(Position.X, Position.Y), new Vector2(Position.X + texture.Width, Position.Y), lineColor, lineWidth);
                    break;
                case "bottom":
                    DrawLine(Globals.SpriteBatch,Globals.Pixel, new Vector2(Position.X, Position.Y + texture.Height-2), new Vector2(Position.X + texture.Width, Position.Y + texture.Height-2), lineColor, lineWidth);
                    break;
                case "left":
                    DrawLine(Globals.SpriteBatch,Globals.Pixel, new Vector2(Position.X, Position.Y), new Vector2(Position.X, Position.Y + texture.Height), lineColor, lineWidth);
                    break;
                case "right":
                    DrawLine(Globals.SpriteBatch,Globals.Pixel, new Vector2(Position.X + texture.Width, Position.Y), new Vector2(Position.X + texture.Width, Position.Y + texture.Height), lineColor, lineWidth);
                    break;
            }
        }
        
    }

}
