using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4.Content.Actors.Towers
{
    internal class BasicTower : Tower
    {
        public BasicTower(Texture2D texture, Vector2 position, Color color) : base(texture, position, color)
        {
            radius = 100;
            damage = 10;
            cost = 10;
            attackSpeed = 1;
        }
    }
}
