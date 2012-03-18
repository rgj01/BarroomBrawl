using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BarRoomBrawl
{
    public class Map : GameObject
    {
        public Map()
        {
        }
        public override void Draw(Dictionary<String,Texture2D> tdict, SpriteBatch spriteBatch)
        {
            Texture2D floortext = tdict["FloorTile"];
            Rectangle rect = new Rectangle(0, 0, 10 * floortext.Width, 10 * floortext.Height);
            spriteBatch.Draw(floortext, Vector2.Zero, rect, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                
        }



    }
}
