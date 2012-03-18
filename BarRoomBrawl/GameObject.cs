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
        public enum Directions {  SW = -5, W = -4, NW = -3, N = -1, None = 0, S = 1, SE = 3, E = 4,  NE = 5  }

        private Vector2 getMovementVector(Directions dir)
        {
            Vector2[] directionTransforms = 
            { 
                new Vector2(-0.7071067811865f, -0.7071067811865f), // SW
                new Vector2(-1, 0), // W
                new Vector2(-0.7071067811865f, 0.7071067811865f), // NW
                Vector2.Zero,
                new Vector2(0, -1), // S
                Vector2.Zero, // None
                new Vector2(0, 1), // N
                Vector2.Zero,
                new Vector2(0.7071067811865f, -0.7071067811865f), // SE
                new Vector2(1, 0), // E
                new Vector2(0.7071067811865f, 0.7071067811865f)  // NE
            };

            int offset = 5 + (int)dir;
            return directionTransforms[offset];
        }

        public int Id { get; set; }
        public Vector2 Location { get; set; }
        public Vector2 Bounds { get; set; }
        public float Speed { get; set; }

        public Directions Direction { get; set; }
        public bool Mobile {get; set;}
        public bool Solid { get; set; }

        public GameObject(string texture, Vector2 bounds, Vector2 startLoc, float startSpeed, Directions startDir, int id)
        {
            m_textureName = texture;
            Bounds = bounds;
            Location = startLoc;
            Speed = startSpeed;
            Direction = startDir;
            Id = id;
            Mobile = true;
            Solid = false;
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

        public virtual void Draw(Dictionary<String,Texture2D> tdict, SpriteBatch batch)
        {
            Texture2D tex = tdict[m_textureName];
             batch.Draw(tex, Location, Color.White);
        }

        public virtual void Update(GameTime gameTime, List<GameObject> objects, double xd = 0, double yd = 0)
        {
            if (!Mobile)
                return;

            double elapsed = gameTime.ElapsedGameTime.TotalMilliseconds;
            double distance = elapsed * Speed;
            Vector2 dir = getMovementVector(Direction) * (float)distance;
            dir.X += (float)xd;
            dir.Y += (float)yd;
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

        public virtual bool IsDead()
        {
            return false;
        }

        public virtual void TakeHit(int damage)
        {

        }
        
    }
}
