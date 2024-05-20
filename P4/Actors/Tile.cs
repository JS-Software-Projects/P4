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
    
    // List to store circles
    private Circle _circle;

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
        if (Pathfinder.Ready() && Rectangle.Contains(InputManager.CurrentTarget))
        {
            if (InputManager.execute)
            {
                Pathfinder.BFSearch(_mapX, _mapY);
                InputManager.MoveToNextTarget();
            }
        }


        Color = Path ? Color.Green : Color.White;
        //Color = Blocked ? Color.Red : Color;
        if (_boundry.Count == 2)
        {
            Color = Color.RosyBrown;
        }
    }
    public Circle getCircle()
    {
            return _circle;
    }
    public void AddCircle(Color color)
    {
        // Create a circle with a radius of 10 and a border thickness of 2
        _circle = new Circle(new Vector2(Position.X + texture.Width / 2, Position.Y + texture.Height / 2), 20, color, 1);
    }
    public void removeCircle()
    {
        _circle = null;
    }
    public bool CircleExist()
    {
        if (_circle != null)
        {
            return true;
        }
        return false;
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
        // Draw the circles on the tile

        _circle?.Draw(Globals.SpriteBatch, Globals.Pixel);

    }

}
