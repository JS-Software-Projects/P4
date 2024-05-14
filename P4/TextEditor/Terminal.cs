using System.Collections.Generic;

namespace P4;

public class Terminal
{
    private readonly Rectangle _terminal;
    private readonly SpriteBatch _spriteBatch;
    private readonly SpriteFont _spriteFont;
    private readonly GraphicsDevice _graphicsDevice;
    private  static readonly List<string> Lines = new() { "" };
    private readonly int _textAreaY;
    private static bool _error = true;
    
    public Terminal(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, SpriteFont spriteFont)
    {
        this._spriteBatch = spriteBatch;
        this._spriteFont = spriteFont;
        this._graphicsDevice = graphicsDevice;
        
        // Calculate dimensions of TextEditor
        var textAreaWidth =
            (int)(Globals.WindowSize.X * 0.38+44); // 35% of the window width, accessed directly from Globals
        var textAreaHeight = Globals.WindowSize.Y; // Full height, accessed directly from Globals
              var textAreaX = Globals.WindowSize.X - textAreaWidth; // Positioned on the right, accessed directly from Globals
              _textAreaY = textAreaHeight - 128; // Box height
        
        _terminal = new Rectangle(0, _textAreaY,textAreaX, 128);
    }
    public static void SetError(bool err,string line)
    {
        _error = err;
        if (Lines.Count > 0)
        {
            Lines[0] = line; // Replace the first line if Lines is not empty
        }
        else
        {
            Lines.Add(line); // Add the line as the first element if Lines is empty
        }
    }   
    public static void AddMessage(bool err,string line)
    {
        _error = err;
        if (Lines.Count > 0)
        {
            Lines[0] = line; // Replace the first line if Lines is not empty
        }
        else
        {
            Lines.Add(line); // Add the line as the first element if Lines is empty
        }
    }   
    
    public void Draw()
    {
        _spriteBatch.Begin();
        var textAreaBackground = new Texture2D(_graphicsDevice, 1, 1);
        textAreaBackground.SetData(new[] { Color.LightGray });
        _spriteBatch.Draw(textAreaBackground, _terminal, Color.DarkGray);
        DrawBorder(_spriteBatch, _terminal, 2, Color.Black);
        

        for (var i = 0; i < Lines.Count; i++)
        {
            var lineNumber = i + 1 + "."; // Line numbers start at 1
            var line = Lines[i];
            _spriteBatch.DrawString(_spriteFont, line, new Vector2(40, _textAreaY + i * _spriteFont.LineSpacing + 10),
                _error ? Color.Firebrick : Color.Black); // Draw line number
            // Draw line number
            if (Lines[i] !="")
                _spriteBatch.DrawString(_spriteFont, lineNumber,
                    new Vector2(5, _textAreaY + i * _spriteFont.LineSpacing+10), _error ? Color.Firebrick : Color.Black);
        }
        _spriteBatch.End();
    }
    private void DrawBorder(SpriteBatch spriteBatch, Rectangle rectangle, int thickness, Color color)
    {
        var pixelTexture = new Texture2D(_graphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White }); // Fill the texture with white color

        // Draw top line
        spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
        
        // Draw right line
        spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.Right, rectangle.Y, thickness, rectangle.Height), color);

        // Draw bottom line
        spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Bottom, rectangle.Width, thickness), color);
    }
}