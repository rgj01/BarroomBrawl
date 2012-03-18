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

        public enum Directions { E = 0, SE, S, SW, W, NW, N, NE }
        protected Vector2[] m_directionTransforms = { new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, -1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1) };
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
            BoundingBox bbThis = new BoundingBox(new Vector3(Location, 0.0f), new Vector3(Location.X + Bounds.X, Location.Y + Bounds.Y, 0.0f));
            //Debug.WriteLine("Bounding box for this object: " + bbThis);

            BoundingBox bbOther = new BoundingBox(new Vector3(other.Location, 0.0f), new Vector3(other.Location.X + other.Bounds.X, other.Location.Y + other.Bounds.Y, 0.0f));
            //Debug.WriteLine("Bounding box for other object:" + bbOther);

            return (bbThis.Intersects(bbOther));
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

            Vector2 escapeUpLeft = new Vector2(-1, -1);
            Vector2 escapeDownLeft = new Vector2(-1, 1);
            Vector2 escapeUpRight = new Vector2(1, -1);
            Vector2 escapeDownRight = new Vector2(1, 1);

            double elapsed = gameTime.ElapsedGameTime.TotalMilliseconds;
            double distance = elapsed * Speed;
            Vector2 dir = m_directionTransforms[(int)Direction] * (float)distance;
            dir.X += (float)xd;
            dir.Y += (float)yd;

            if (Solid)
            {
                Vector2 newLocation = Location + dir;
                //Debug.WriteLine("Doing collisions for " + this.Id);
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
                        //Debug.WriteLine("Collided with " + o.Id);
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
