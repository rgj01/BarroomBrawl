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

        public Player(Game game, Texture2D texture, Vector2 startLoc, float startSpeed, Directions startDir, int id)
            : base(game, texture, startLoc, startSpeed, startDir, id)
        {

        }

        public void LoadContent()
        {
            Texture = m_game.Content.Load<Texture2D>("Player");
        }

        public void UnloadContent()
        {
            Texture.Dispose();
        }

        
    }
}
