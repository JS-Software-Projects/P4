﻿using System;

namespace P4.Actors.Towers;

internal class AdvancedTower : Tower
{
    public int pov = 90;

    public AdvancedTower(Texture2D texture, Vector2 position, Color color) : base(texture, position, color)
    {
        radius = 50;
        damage = 20;
        cost = 20;
        attackSpeed = 2;
        povLine1 = new Vector2(position.X + radius, position.Y + radius);
        povLine2 = new Vector2(position.X + radius, position.Y - radius);
    }

    public Vector2 povLine1 { get; set; }
    public Vector2 povLine2 { get; set; }

    public void DrawPOV(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, povLine1, null, color, 0f, new Vector2(texture.Width / 2, texture.Height / 2),
            Vector2.One, SpriteEffects.None, 0f);
        spriteBatch.Draw(texture, povLine2, null, color, 0f, new Vector2(texture.Width / 2, texture.Height / 2),
            Vector2.One, SpriteEffects.None, 0f);
    }

    private void rotate(int angle)
    {
        // Convert rotation angle to radians
        var radians = Math.PI / 180 * angle;

        // Calculate the midpoint between povLine1 and povLine2 to rotate around
        var center = new Vector2(position.X + radius, position.Y);

        // Rotate povLine1
        var newX1 = Math.Cos(radians) * (povLine1.X - center.X) - Math.Sin(radians) * (povLine1.Y - center.Y) +
                    center.X;
        var newY1 = Math.Sin(radians) * (povLine1.X - center.X) + Math.Cos(radians) * (povLine1.Y - center.Y) +
                    center.Y;
        povLine1 = new Vector2((float)newX1, (float)newY1);

        // Rotate povLine2
        var newX2 = Math.Cos(radians) * (povLine2.X - center.X) - Math.Sin(radians) * (povLine2.Y - center.Y) +
                    center.X;
        var newY2 = Math.Sin(radians) * (povLine2.X - center.X) + Math.Cos(radians) * (povLine2.Y - center.Y) +
                    center.Y;
        povLine2 = new Vector2((float)newX2, (float)newY2);
    }

    public void rotateLeft()
    {
        rotate(-pov);
    }

    public void rotateRight()
    {
        rotate(pov);
    }
}