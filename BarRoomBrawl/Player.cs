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
            Drunkenness += gameTime.ElapsedGameTime.Milliseconds / 10;
            double drukennessMult = (double)Drunkenness / 1000;
            double dclock = (double)drunkennessClock / 1000;
            double xmod = drukennessMult * Math.Sin(dclock);
            double ymod = drukennessMult * Math.Sin(dclock + .8);
            base.Update(gameTime, objects, xmod, ymod);
        }

        public Punch ThrowPunch()
        {
            int punchdist = 50;
            Vector2 ploc = Location;
            if (Direction == Directions.E)
            {
                ploc.X += punchdist + Bounds.X;
            }
            else if (Direction == Directions.NE)
            {
                ploc.X += punchdist + Bounds.X;
                ploc.Y += punchdist;
            }
            else if (Direction == Directions.SE)
            {
                ploc.X += punchdist + Bounds.X;
                ploc.Y -= punchdist - Bounds.Y;
            }
            if (Direction == Directions.W)
            {
                ploc.X -= punchdist;
            }
            else if (Direction == Directions.NW)
            {
                ploc.X -= punchdist;
                ploc.Y += punchdist;
            }
            else if (Direction == Directions.SW)
            {
                ploc.X = punchdist;
                ploc.Y -= punchdist - Bounds.Y;
            }
            else if (Direction == Directions.N)
            {
                ploc.Y += punchdist;
            }
            else if (Direction == Directions.S)
            {
                ploc.Y -= punchdist;
            }

            Punch punch = new Punch("Player", ploc, 0.0f, Direction, Id + 200, Id);
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
