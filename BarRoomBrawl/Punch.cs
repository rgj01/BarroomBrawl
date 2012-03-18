using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BarRoomBrawl
{
    [Serializable()]
    public class Punch : GameObject
    {
        int clock;
        int power;
        int _thrower;
         public Punch(string texture, Vector2 startLoc, float startSpeed, Directions startDir, int id, int thrower, int drunkenness)
            : base("Player", new Vector2(36, 72), startLoc, startSpeed, startDir, id)
        {
            clock = 50;
            this.Solid = false;
            _thrower = thrower;
            Bounds = new Vector2(50, 50);
            power = drunkenness / 500;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, List<GameObject> objects, double xd = 0, double yd = 0)
        {
            clock -= gameTime.ElapsedGameTime.Milliseconds;
            foreach (GameObject o in objects)
            {
                if (Intersects(o) && _thrower != o.Id)
                {
                    o.TakeHit(50 + power);
                }
            }
            base.Update(gameTime, objects, xd, yd);
        }

        public override bool IsDead()
        {
            return clock < 0;
        }
    }
}
