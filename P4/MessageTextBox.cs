using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class MessageTextBox
{
    private string _message;
    private Rectangle _bounds;

    public MessageTextBox(Rectangle bounds, string message)
    {
        _bounds = bounds;
        _message = message;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Globals.Pixel, _bounds, Color.CadetBlue);
        spriteBatch.DrawString(Globals.spriteFont, _message, new Vector2(_bounds.X, _bounds.Y), Color.Black);
    }
    
    public void SetMessage(string message)
    {
        _message = message;
    }
}