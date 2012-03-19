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
        protected override int CurrentAnimationId { get { return (int)currentAnimation; } }

        int invulnTimer;

        protected enum Animation { standing = 0, running, punching, drunkrunning, drunkstanding, drunkpunching };
        Animation currentAnimation;
        Animation lastAnimation;
        
        const int soberEveryMilliseconds = 1000;
        const int soberUpEverySecond = 25;
        int soberTimer = soberEveryMilliseconds;

        public Player(string texture, Vector2 startLoc, float startSpeed, Directions startDir, int id)
            : base(texture, new Vector2(50, 69), startLoc, startSpeed, startDir, id)
        {
            Health = 500;
            Drunkenness = 0;
            this.Solid = true;
            Speed = 0.2f;
            invulnTimer = 0;
            frameCounts = new int[] { 6, 8, 6, 8, 6, 6 };
            frameBounds = new Vector2[] { new Vector2(51, 70), new Vector2(51, 70), new Vector2(68, 70), new Vector2(51, 70), new Vector2(51, 70), new Vector2(68, 70) };
            frameVerticalOffset = new int[] { 210, 0, 70, 140, 280, 350 };
            Animated = true;
            currentAnimation = Animation.standing;
            lastAnimation = Animation.standing;
        }

        public override void Draw(Dictionary<string, Texture2D> tdict, SpriteBatch batch)
        {
            if ((Animation.punching == currentAnimation || Animation.drunkpunching == currentAnimation) &&  currentFrame + 1 == FrameCount)
            {
                currentAnimation = lastAnimation;
                currentFrame = 0;
            }

            base.Draw(tdict, batch);
        }

        public override void Update(GameTime gameTime, List<GameObject> objects, double xd = 0, double yd = 0)
        {
            // invulnerable after being hit
            invulnTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (invulnTimer < 0)
            {
                invulnTimer = 0;
            }

            // get sober as time goes on
            soberTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (soberTimer < 0)
            {
                soberTimer = soberEveryMilliseconds;
                Drunkenness = Drunkenness - soberUpEverySecond < 0 ? 0 : Drunkenness - soberUpEverySecond;
            }
            double drunkennessMult = (double)Drunkenness / 100;
            drunkennessMult = drunkennessMult > 2 ? 2 : drunkennessMult;
            double dclock = (double)gameTime.TotalGameTime.Seconds;

            double xmod = drunkennessMult * Math.Sin(dclock);
            double ymod = drunkennessMult * Math.Sin(dclock + .8);


            if (currentAnimation != Animation.punching && currentAnimation != Animation.drunkpunching)
            {
                if (Direction != Directions.None)
                {
                    currentAnimation = Drunkenness > 500 ? Animation.drunkrunning : Animation.running;

                }
                else
                {
                    currentAnimation = Drunkenness > 500 ? Animation.drunkstanding : Animation.standing;
                }
            }

            base.Update(gameTime, objects, xmod, ymod);
        }

        public Punch ThrowPunch()
        {
            if (Animation.punching != currentAnimation && Animation.drunkpunching != currentAnimation)
            {
                lastAnimation = currentAnimation;
                currentAnimation = Drunkenness > 500 ? Animation.drunkpunching : Animation.punching;
                currentFrame = 0;
            }


            Vector2 punchLoc = Location;

            Vector2 bounds = new Vector2(5, 5);
            
            if (Directions.N != Direction
              || Directions.S != Direction)
            {
                bounds.Y = Bounds.Y;
            }
            
            Punch punch = new Punch("Player", punchLoc, bounds, 0.0f, Direction, Id + 200, Id, Drunkenness);

            //finesse the location
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
            if (invulnTimer <= 0)
            {
                invulnTimer = 300;
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
