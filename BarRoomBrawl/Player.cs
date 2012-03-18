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
        int invtimer;

        public Player(string texture, Vector2 startLoc, float startSpeed, Directions startDir, int id)
            : base("Player", new Vector2(36, 72), startLoc, startSpeed, startDir, id)
        {
            Health = 500;
            drunkennessClock = 0;
            this.Solid = true;
            Speed = 0.2f;
            invtimer = 0;
        }

        public override void Draw(Dictionary<String,Texture2D> tdict, SpriteBatch batch)
        {
            batch.Draw(tdict[m_textureName], Location, new Rectangle(0, 0, 36, 72), Color.White);
        }

        public override void Update(GameTime gameTime, List<GameObject> objects, double xd = 0, double yd = 0)
        {
            invtimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (invtimer < 0)
            {
                invtimer = 0;
            }
            drunkennessClock += gameTime.ElapsedGameTime.Milliseconds;
            //Drunkenness += gameTime.ElapsedGameTime.Milliseconds / 10;
            double drukennessMult = (double)Drunkenness / 1000;
            double dclock = (double)drunkennessClock / 1000;
            double xmod = drukennessMult * Math.Sin(dclock);
            double ymod = drukennessMult * Math.Sin(dclock + .8);
            base.Update(gameTime, objects, xmod, ymod);
        }

        public Punch ThrowPunch()
        {
            Vector2 punchLoc = Location;

            Vector2 bounds = new Vector2(5, 5);
            
            if ( Directions.S == Direction
              || Directions.N == Direction)
            {
                bounds.X = Bounds.X;
            }
            if (Directions.E == Direction
              || Directions.W == Direction)
            {
                bounds.Y = Bounds.Y;
            }
            
            Punch punch = new Punch("Player", punchLoc, bounds, 0.0f, Direction, Id + 200, Id, Drunkenness);

            //finesse the location
            if ( Directions.S == Direction
              || Directions.SE == Direction
              || Directions.SW == Direction)
            {
                punchLoc.Y += (Bounds.Y + punch.Bounds.Y + 5);
            }
            else if (Directions.N == Direction
              || Directions.NE == Direction
              || Directions.NW == Direction)
            {
                punchLoc.Y -= (2 * punch.Bounds.Y + 5);
            }

            if (Directions.W == Direction
              || Directions.SW == Direction
              || Directions.NW == Direction)
            {
                punchLoc.X -= (2* punch.Bounds.X + 5);
            }
            else if (Directions.E == Direction
              || Directions.SE == Direction
              || Directions.NE == Direction)
            {
                punchLoc.X += (Bounds.X + punch.Bounds.X + 5);
            }

            

            punch.Location = punchLoc;
            
            return punch;
        }

        public override void TakeHit(int damage)
        {
            if (invtimer <= 0)
            {
                invtimer = 300;
                Health -= damage;
            }
            Console.WriteLine("Took hit, health:{0}", Health);
            base.TakeHit(damage);
        }

        public override bool IsDead()
        {
            return Health <= 0;
        }
        
    }
}
