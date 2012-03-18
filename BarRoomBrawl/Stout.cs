using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BarRoomBrawl
{
    class Stout : GameObject
    {
        bool pickedup;
        public Stout(Vector2 startLoc, float startSpeed, Directions startDir, int id)
            : base("Stout", new Vector2(10, 14), startLoc, startSpeed, startDir, id)
        {
            pickedup = false;
        }


        public override void Update(GameTime gameTime, List<GameObject> objects, double xd = 0, double yd = 0)
        {
            foreach(GameObject o in objects)
            {
                if (Intersects(o) && o is Player)
                {
                    Player p = (Player)o;
                    p.Drunkenness += 200;
                    pickedup = true;
                    break;
                }
            }
            base.Update(gameTime, objects, xd, yd);

        }

        public override bool IsDead()
        {
            return pickedup;
        }
        
    }
}
