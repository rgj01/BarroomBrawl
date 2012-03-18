using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BarRoomBrawl
{
    public class Player : GameObject
    {
        public bool Playing { get; set; } // if false, spectating
        public int Health { get; set; }
        public int Drunkenness { get; set; }
        public int CarriedItemId { get; set; }
        public int ServerAssignedId { get; set; }

        public Player(Game game, string texture, Vector2 startLoc, float startSpeed, Directions startDir, int id)
            : base(game, texture, startLoc, startSpeed, startDir, id)
        {
            this.solid = true;
        }

        public override void LoadContent()
        {
            m_textureName = "Player";
            base.LoadContent();
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, Location, new Rectangle(0, 0, 36, 72), Color.White);
        }

        public void UnloadContent()
        {
            Texture.Dispose();
        }

        
    }
}
