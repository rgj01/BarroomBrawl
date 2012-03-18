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
            m_floorTexture = m_game.Content.Load<Texture2D>("FloorTile");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            Rectangle rect = new Rectangle(0, 0, 10 * m_floorTexture.Width, 10 * m_floorTexture.Height);
            spriteBatch.Draw(m_floorTexture, Vector2.Zero, rect, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                
        }



    }
}
