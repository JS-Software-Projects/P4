﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace P4
{
    public class Button
    {
        public Rectangle Bounds { get; private set; }
        public string Text { get; private set; }
        private SpriteFont font;
        private Texture2D texture;
        private Color textColor;
        private Color backgroundColor;
        private Color hoverColor;
        private bool isHovering;

        public event Action Click;
        private MouseState previousMouseState;

        public Button(Rectangle bounds, string text, SpriteFont font, Texture2D texture, Color textColor, Color backgroundColor, Color hoverColor)
        {
            this.Bounds = bounds;
            this.Text = text;
            this.font = font;
            this.texture = texture;
            this.textColor = textColor;
            this.backgroundColor = backgroundColor;
            this.hoverColor = hoverColor;
        }

        public void Update(MouseState currentMouseState)
        {
            // Check if the mouse is over the button
            isHovering = Bounds.Contains(currentMouseState.X, currentMouseState.Y);

            // Check for click: mouse button was just released while over the button
            if (isHovering && currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                Click?.Invoke(); // Raise the Click event
            }

            // Update the previousMouseState for the next frame
            previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the button (change color when hovering)
            Color currentColor = isHovering ? hoverColor : backgroundColor;
            spriteBatch.Draw(texture, Bounds, currentColor);

            // Draw the text
            Vector2 textSize = font.MeasureString(Text);
            Vector2 textPosition = new Vector2(Bounds.Center.X - textSize.X / 2, Bounds.Center.Y - textSize.Y / 2);
            spriteBatch.DrawString(font, Text, textPosition, textColor);
        }
    }
}
