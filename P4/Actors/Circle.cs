using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace p4.Actors
{
    public class Circle
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }
        public Color Color { get; set; }
        public int BorderThickness { get; set; }

        public Circle(Vector2 position, float radius, Color color, int borderThickness)
        {
            Position = position;
            Radius = radius;
            Color = color * 0.5f; // 50% transparency
            BorderThickness = borderThickness;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture)
        {
            // Draw the filled circle (50% transparent)
            DrawFilledCircle(spriteBatch, pixelTexture, Position, Radius, Color);

            // Draw the border
            DrawCircleBorder(spriteBatch, pixelTexture, Position, Radius, BorderThickness, Color.Black);
        }

        private void DrawFilledCircle(SpriteBatch spriteBatch, Texture2D pixelTexture, Vector2 center, float radius, Color color)
        {
            int diameter = (int)(radius * 2);
            Rectangle rect = new Rectangle((int)(center.X - radius), (int)(center.Y - radius), diameter, diameter);

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    int dx = x - (int)radius;
                    int dy = y - (int)radius;
                    if (dx * dx + dy * dy <= radius * radius)
                    {
                        spriteBatch.Draw(pixelTexture, new Vector2(rect.X + x, rect.Y + y), color);
                    }
                }
            }
        }

        private void DrawCircleBorder(SpriteBatch spriteBatch, Texture2D pixelTexture, Vector2 center, float radius, int borderThickness, Color borderColor)
        {
            int diameter = (int)(radius * 2);
            Rectangle rect = new Rectangle((int)(center.X - radius), (int)(center.Y - radius), diameter, diameter);

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    int dx = x - (int)radius;
                    int dy = y - (int)radius;
                    if (dx * dx + dy * dy <= radius * radius && dx * dx + dy * dy >= (radius - borderThickness) * (radius - borderThickness))
                    {
                        spriteBatch.Draw(pixelTexture, new Vector2(rect.X + x, rect.Y + y), borderColor);
                    }
                }
            }
        }
    }
}
