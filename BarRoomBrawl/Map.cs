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
        Texture2D m_wallTexture;
        Texture2D m_cornerTexture;
        Texture2D m_floorTexture;

        public Map(Game game)
        {
            m_game = game;
        }

        public override void LoadContent()
        {
            m_floorTexture = m_game.Content.Load<Texture2D>("FloorTile2");
        }

        public override void Draw(Dictionary<String,Texture2D> tdict, SpriteBatch spriteBatch)
        {
            Texture2D floortext = tdict["FloorTile"];
            Rectangle rect = new Rectangle(0, 0, 10 * floortext.Width, 10 * floortext.Height);
            spriteBatch.Draw(floortext, Vector2.Zero, rect, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                
        }



    }
}
