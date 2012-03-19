using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BarRoomBrawl
{
    public class Enemy : Player
    {

        protected int changeDirEvery = 1000;
        protected int changeDirTimer = 0;
        protected override int CurrentAnimationId { get { return 0; } }

        public Enemy(string texture, Vector2 startLoc, float startSpeed, Directions startDir, int id)
            : base(texture, startLoc, startSpeed, startDir, id)
        {
            Health = 200;
            frameCounts = new int[] { 6 };
            frameVerticalOffset = new int[] { 0 };
        }

        public override void Update(GameTime gameTime, List<GameObject> objects, double xd = 0, double yd = 0)
        {
            changeDirTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (changeDirTimer < 0)
            {
                Random r = new Random();
                Direction = (Directions)r.Next(7) - 5;
                changeDirTimer = changeDirEvery;
            }
            
            base.Update(gameTime, objects, xd, yd);
        }
    }
}
