using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BarRoomBrawl
{
    [Serializable()]
    public class Punch : GameObject
    {
        int clock;
        int power;
        int _thrower;
         public Punch(string texture, Vector2 startLoc, Vector2 bounds, float startSpeed, Directions startDir, int id, int thrower, int drunkenness)
            : base("Whiskey", bounds, startLoc, startSpeed, startDir, id)
        {
            clock = 50;
            this.Solid = false;
            _thrower = thrower;
            power = drunkenness / 500;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, List<GameObject> objects, double xd = 0, double yd = 0)
        {
            clock -= gameTime.ElapsedGameTime.Milliseconds;
            foreach (GameObject o in objects)
            {
                if (o is Player && _thrower != o.Id && Intersects(o))
                {
                    o.TakeHit(50 + power);
                }
            }
            base.Update(gameTime, objects, xd, yd);
        }

        public override void Draw(Dictionary<string, Microsoft.Xna.Framework.Graphics.Texture2D> tdict, Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
        }

        public override bool IsDead()
        {
            return clock < 0;
        }
    }
}
