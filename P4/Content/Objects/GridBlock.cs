using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using P4;
namespace P4

{
    public class GridBlock
    {
        // Position and size of the block
        public Vector2 Position { get; set; }
        public int Width { get; }
        public int Height { get; }

        // Color of the block
        public Color Color { get; set; }

        // Constructor
        public GridBlock(Vector2 position, int width, int height, Color color, String property)
        {
            Position = position;
            Width = width;
            Height = height;
            Color = color;
            Property = property;
        }

        // Draw method to render the block
        /* public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture)
        {
            // Check if pixelTexture is null
            if (pixelTexture == null)
            {
                // Log or throw an exception indicating that pixelTexture is null
                return; // Or handle the null texture case gracefully
            }

            // Check if Width or Height is zero
            if (Width == 0 || Height == 0)
            {
                // Log or throw an exception indicating that Width or Height is zero
                return; // Or handle the zero size case gracefully
            }

            // Draw the pixelTexture
            spriteBatch.Draw(pixelTexture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Color);
        }*/

    }
}
