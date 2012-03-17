using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace BarRoomBrawl
{
    public class GameObject
    {
        public enum Direction { E, SE, S, SW, W, NW, N, NE }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        public Texture2D Texture { get; set; }
        public Direction Direction { get; set; }
    }
}
