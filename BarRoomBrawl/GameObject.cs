using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace BarRoomBrawl
{
    public class GameObject
    {
        protected Game m_game;
        protected string m_textureName;

        public enum Directions { E = 0, SE, S, SW, W, NW, N, NE }
        protected Vector2[] m_directionTransforms = { new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, -1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1) };
        public int Id { get; set; }
        public Vector2 Location { get; set; }
        public Vector2 Bounds { get; set; }
        public float Speed { get; set; }
        public Texture2D Texture { get; set; }
        public Directions Direction { get; set; }
        public bool Mobile {get; set;}
        public bool Solid { get; set; }

        public GameObject(Game game, string texture, Vector2 bounds, Vector2 startLoc, float startSpeed, Directions startDir, int id)
        {
            m_game = game;
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
            BoundingBox bbThis = new BoundingBox(new Vector3(Location, 0.0f), new Vector3(Location.X + Bounds.X, Location.Y + Bounds.Y, 0.0f));
            Debug.WriteLine("Bounding box for this object: " + bbThis);

            BoundingBox bbOther = new BoundingBox(new Vector3(other.Location, 0.0f), new Vector3(other.Location.X + other.Bounds.X, other.Location.Y + other.Bounds.Y, 0.0f));
            Debug.WriteLine("Bounding box for other object:" + bbOther);

            return (bbThis.Intersects(bbOther));
        }

        public virtual void LoadContent() 
        {
            Texture = m_game.Content.Load<Texture2D>(m_textureName);
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, Location, Color.White);
        }

        public virtual void Update(GameTime gameTime, List<GameObject> objects)
        {
            if (!Mobile)
                return;

            Vector2 escapeUpLeft = new Vector2(-1, -1);
            Vector2 escapeDownLeft = new Vector2(-1, 1);
            Vector2 escapeUpRight = new Vector2(1, -1);
            Vector2 escapeDownRight = new Vector2(1, 1);

            double elapsed = gameTime.ElapsedGameTime.TotalMilliseconds;
            double distance = elapsed * Speed;
            Vector2 dir = m_directionTransforms[(int)Direction] * (float)distance;

            if (Solid)
            {
                Vector2 newLocation = Location + dir;
                Debug.WriteLine("Doing collisions for " + this.Id);
                bool moveOk = true;
                Vector2 escape = new Vector2();
                foreach (GameObject o in objects)
                {
                    if (o.Id == this.Id)
                    {
                        continue;
                    }
                    if (Intersects(o))
                    {
                        Debug.WriteLine("Collided with " + o.Id);
                        moveOk = false;
                        if (o.Location.X < Location.X)
                        {
                            if (o.Location.Y < Location.Y)
                            {
                                escape = escapeDownRight;
                            }
                            else
                            {
                                escape = escapeUpRight;
                            }
                        }
                        else
                        {
                            if (o.Location.Y < Location.Y)
                            {
                                escape = escapeDownLeft;
                            }
                            else
                            {
                                escape = escapeUpLeft;
                            }
                        }
                        break;
                    }

                }
                if (moveOk)
                {
                    Location = newLocation;
                }
                else
                {
                    Location = Location + escape;
                }
            }
            else
            {
                Location = Location + dir;
            }
        }

        
    }
}
