using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BarRoomBrawl
{
    [Serializable()]
    public class Player : GameObject
    {
        public bool Playing { get; set; } // if false, spectating
        public int Health { get; set; }
        public int Drunkenness { get; set; }
        public int CarriedItemId { get; set; }
        public int ServerAssignedId { get; set; }
        int drunkennessClock;

        public Player(string texture, Vector2 startLoc, float startSpeed, Directions startDir, int id)
            : base("Player", new Vector2(36, 72), startLoc, startSpeed, startDir, id)
        {
            drunkennessClock = 0;
            this.Solid = true;
        }

        public override void Draw(Dictionary<String,Texture2D> tdict, SpriteBatch batch)
        {
            batch.Draw(tdict[m_textureName], Location, new Rectangle(0, 0, 36, 72), Color.White);
        }

        public override void Update(GameTime gameTime, List<GameObject> objects, double xd = 0, double yd = 0)
        {
            drunkennessClock += gameTime.ElapsedGameTime.Milliseconds;
            Drunkenness += gameTime.ElapsedGameTime.Milliseconds / 10;
            double drukennessMult = (double)Drunkenness / 1000;
            double dclock = (double)drunkennessClock / 1000;
            double xmod = drukennessMult * Math.Sin(dclock);
            double ymod = drukennessMult * Math.Sin(dclock + .8);
            base.Update(gameTime, objects, xmod, ymod);
        }
        
    }
}
