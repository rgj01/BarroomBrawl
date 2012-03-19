using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace BarRoomBrawl
{
    [Serializable()]
    public class GameObject
    {
        protected string m_textureName;

        // S -1 + E 4 = SE 3
        // S -1 + W -4 = SW -5
        // N 1 + E 4 = NE 5
        // N 1 + W -4 = NW -3
        public enum Directions {  SW = -5, W = -4, NW = -3, S = -1, None = 0, N = 1, SE = 3, E = 4,  NE = 5  }

        protected Vector2 getMovementVector(Directions dir)
        {
            Vector2[] directionTransforms = 
            { 
                new Vector2(-0.7071067811865f, 0.7071067811865f), // SW
                new Vector2(-1, 0), // W
                new Vector2(-0.7071067811865f,-0.7071067811865f), // NW
                Vector2.Zero,
                new Vector2(0, 1), // S
                Vector2.Zero, // None
                new Vector2(0, -1), // N
                Vector2.Zero,
                new Vector2(0.7071067811865f, 0.7071067811865f), // SE
                new Vector2(1, 0), // E
                new Vector2(0.7071067811865f, -0.7071067811865f)  // NE
            };

            int offset = 5 + (int)dir;
            return directionTransforms[offset];
        }

        protected float animationFPS;
        protected int[] frameCounts;
        protected Vector2[] frameBounds;
        protected int[] frameVerticalOffset;
        protected int currentFrame;
        float totalElapsedFrameTime;
        const float targetFPS = 30f;

        public int Id { get; set; }
        public Vector2 Location { get; set; }
        public Vector2 Bounds { get { return frameBounds[CurrentAnimationId]; } }
        public float Speed { get; set; }
        protected virtual int CurrentAnimationId { get; set; }
        protected int FrameVerticalOffset { get { return frameVerticalOffset[CurrentAnimationId]; } }
        protected virtual float TimePerFrame { get { return targetFPS / animationFPS / 1000; } }
        protected int FrameCount { get { return frameCounts[CurrentAnimationId]; } }

        public Directions Direction { get; set; }
        public bool Mobile {get; set;}
        public bool Solid { get; set; }
        public bool Animated { get; set; }
        public bool FlippedHorizontally { get; set; }

        public GameObject(string texture, Vector2 bounds, Vector2 startLoc, float startSpeed, Directions startDir, int id)
        {
            m_textureName = texture;
            frameBounds = new Vector2[] { bounds };
            frameVerticalOffset = new int[] { 0 };
            animationFPS = .5f;
            CurrentAnimationId = 0;
            Location = startLoc;
            Speed = startSpeed;
            Direction = startDir;
            Id = id;
            Mobile = true;
            Solid = false;
            Animated = false;
            FlippedHorizontally = false;
        }

        protected GameObject()
        {

        }

        public bool Intersects(GameObject other)
        {
            if (Location.X + Bounds.X < other.Location.X || Location.X > other.Location.X + other.Bounds.X)
            {
                return false;
            }

            if (Location.Y + Bounds.Y < other.Location.Y || Location.Y > other.Location.Y + other.Bounds.Y)
            {
                return false;
            }

            return true;
        }
        protected void UpdateFrame(float elapsed)
        {
            totalElapsedFrameTime += elapsed;
            if (totalElapsedFrameTime >= TimePerFrame)
            {
                currentFrame++;
                // Keep the Frame between 0 and the total frames, minus one.
                currentFrame = currentFrame % FrameCount;
                totalElapsedFrameTime -= TimePerFrame;
            }
        }

        public virtual void Draw(Dictionary<String,Texture2D> tdict, SpriteBatch batch)
        {
            Rectangle sourceRect = new Rectangle(currentFrame * (int)Bounds.X, FrameVerticalOffset, (int)Bounds.X, (int)Bounds.Y);
            Rectangle destRect = new Rectangle((int)Location.X, (int)Location.Y, (int)Bounds.X, (int)Bounds.Y);
            SpriteEffects effect = FlippedHorizontally? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            batch.Draw(tdict[m_textureName], destRect, sourceRect, Color.White, 0.0f, Vector2.Zero, effect, 0.0f); 
        }

        public virtual void Update(GameTime gameTime, List<GameObject> objects, double xd = 0, double yd = 0)
        {
            if (!Mobile)
                return;

            if (Animated)
            {
                // frame animation needs to be rotated
                UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            double elapsed = gameTime.ElapsedGameTime.TotalMilliseconds;
            double distance = elapsed * Speed;

            if (Directions.None != Direction)
            {
                Vector2 dir = getMovementVector(Direction) * (float)distance;
                dir.X += (float)xd;
                dir.Y += (float)yd;

                FlippedHorizontally = dir.X < 0;

                Vector2 oldLocation = Location;
                Location = Location + dir;

                if (Solid)
                {
                    foreach (GameObject o in objects)
                    {
                        if (o.Id == this.Id || !o.Solid)
                        {
                            continue;
                        }
                        if (Intersects(o))
                        {
                            Location = oldLocation;
                            break;
                        }
                    }
                }
            }
        }

        public virtual bool IsDead()
        {
            return false;
        }

        public virtual void TakeHit(int damage)
        {

        }
        
    }
}
