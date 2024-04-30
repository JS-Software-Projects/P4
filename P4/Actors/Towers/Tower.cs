using System;
using p4.Actors;

namespace P4.Actors.Towers;

public abstract class Tower : Sprite
{
    public Tower(Texture2D texture, Vector2 position, Color color) : base(texture, position)
    {
        this.Position = position;
        this.Color = color;
        Origin = Vector2.Zero;
    }

    public int Radius { get; set; }
    public int Damage { get; set; }
    public int Cost { get; set; }
    public int AttackSpeed { get; set; }

    public override void Draw()
    {
        // Calculate the scale to make the tower slightly smaller than the tile
        float scale = Math.Min((Globals.TileSize - 10) / (float)texture.Width, (Globals.TileSize - 10) / (float)texture.Height);

        // Calculate the size of the texture after scaling
        float scaledWidth = texture.Width * scale;
        float scaledHeight = texture.Height * scale;

        // Calculate how much empty space remains within the tile after scaling the texture
        float remainingWidth = Globals.TileSize - scaledWidth;
        float remainingHeight = Globals.TileSize - scaledHeight;

        // Adjust the position to center the scaled texture within the same tile
        Vector2 newPosition = new Vector2(Position.X + remainingWidth / 2 - Globals.TileSize, Position.Y + remainingHeight / 2 - Globals.TileSize);

        // Draw the tower with the specified scale
        Globals.SpriteBatch.Draw(texture, newPosition, null, Color, 0f, Origin, scale, SpriteEffects.None, 0f);
    }

}