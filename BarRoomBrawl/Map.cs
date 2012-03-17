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

        public Map(Game game)
        {
            m_game = game;
            

        }

        public override void LoadContent()
        {
            m_textureName = "FloorTile";
            base.LoadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            Rectangle rect = new Rectangle(0, 0, 10 * Texture.Width, 10 * Texture.Height);
            spriteBatch.Draw(Texture, rect, null, Color.White);
                
        }



    }
}
