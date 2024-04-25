using System;

namespace p4.Actors;

public class Sprite
{
    protected Texture2D texture;
    public Vector2 Position { get; protected set; }
    public Vector2 Origin { get; protected set; }
    public Color Color { get; set; }
    public Rectangle Rectangle => new((int)Position.X, (int)Position.Y, texture.Width, texture.Height);

    public Sprite(Texture2D texture, Vector2 position)
    {
        this.texture = texture;
        Position = position;
        Origin = Vector2.Zero;
        Color = Color.White;
    }

    public virtual void Draw()
    {
        Globals.SpriteBatch.Draw(texture, Position, null, Color, 0f, Origin, 1f, SpriteEffects.None, 0f);
    }
    public void DrawLine(SpriteBatch spriteBatch, Texture2D pixel, Vector2 start, Vector2 end, Color color, int width)
    {
        var distance = Vector2.Distance(start, end);
        var angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
        spriteBatch.Draw(pixel, start, null, color, angle, Vector2.Zero, new Vector2(distance, width), SpriteEffects.None, 0);
    }

}
