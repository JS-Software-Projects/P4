using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4.Actors.Towers
{
    internal abstract class Tower
    {
        public Texture2D texture { get; set; }
        public Vector2 position { get; set; }
        public Color color { get; set; }
        public int radius { get; set; }
        public int damage { get; set; }
        public int cost { get; set; }  
        public int attackSpeed { get; set; }
        public Tower(Texture2D texture, Vector2 position, Color color)
        {
            texture = texture;
            position = position;
            color = color;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color, 0f, new Vector2(texture.Width / 2, texture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
