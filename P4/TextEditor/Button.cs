using System;

namespace P4;

public class Button
{
    private readonly Color backgroundColor;
    private readonly SpriteFont font;
    private readonly Color hoverColor;
    private bool isHovering;
    private MouseState previousMouseState;
    private readonly Color textColor;
    private readonly Texture2D texture;

    public Button(Rectangle bounds, string text, SpriteFont font, Texture2D texture, Color textColor,
        Color backgroundColor, Color hoverColor)
    {
        Bounds = bounds;
        Text = text;
        this.font = font;
        this.texture = texture;
        this.textColor = textColor;
        this.backgroundColor = backgroundColor;
        this.hoverColor = hoverColor;
    }

    public Rectangle Bounds { get; }
    public string Text { get; }

    public event Action Click;

    public void Update(MouseState currentMouseState)
    {
        // Check if the mouse is over the button
        isHovering = Bounds.Contains(currentMouseState.X, currentMouseState.Y);

        // Check for click: mouse button was just released while over the button
        if (isHovering && currentMouseState.LeftButton == ButtonState.Released &&
            previousMouseState.LeftButton == ButtonState.Pressed) Click?.Invoke(); // Raise the Click event

        // Update the previousMouseState for the next frame
        previousMouseState = currentMouseState;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw the button (change color when hovering)
        var currentColor = isHovering ? hoverColor : backgroundColor;
        spriteBatch.Draw(texture, Bounds, currentColor);

        // Draw the text
        var textSize = font.MeasureString(Text);
        var textPosition = new Vector2(Bounds.Center.X - textSize.X / 2, Bounds.Center.Y - textSize.Y / 2);
        spriteBatch.DrawString(font, Text, textPosition, textColor);
    }
}