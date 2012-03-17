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
        public float Speed { get; set; }
        public Texture2D Texture { get; set; }
        public Directions Direction { get; set; }
        public bool mobile {get; set;}

        public GameObject(Game game, string texture, Vector2 startLoc, float startSpeed, Directions startDir, int id)
        {
            m_game = game;
            m_textureName = texture;
            Location = startLoc;
            Speed = startSpeed;
            Direction = startDir;
            Id = id;
            mobile = true;
        }

        protected GameObject()
        {

        }

        public bool Intersects(Vector2 position, GameObject other)
        {
            BoundingBox bb1 = new BoundingBox(new Vector3(position, 0.0f), new Vector3(Location.X + Texture.Width, Location.Y + Texture.Height, 0.0f));
            Debug.WriteLine("Bounding box 1" + bb1);

            BoundingBox bb2 = new BoundingBox(new Vector3(other.Location, 0.0f), new Vector3(other.Location.X + other.Texture.Width, other.Location.Y + other.Texture.Height, 0.0f));
            Debug.WriteLine("Bounding box 2" + bb2);

            return (bb1.Intersects(bb2));
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
            if (!mobile)
                return;

            Vector2 escapeUpLeft = new Vector2(-1, -1);
            Vector2 escapeDownLeft = new Vector2(-1, 1);
            Vector2 escapeUpRight = new Vector2(1, -1);
            Vector2 escapeDownRight = new Vector2(1, 1);

            double elapsed = gameTime.ElapsedGameTime.TotalMilliseconds;
            double distance = elapsed * Speed;
            Vector2 dir = m_directionTransforms[(int)Direction] * (float)distance;


            Vector2 Location2 = Location + dir;
            Debug.WriteLine("Doing collisions for " + this.Id);
            bool moveOk = true;
            Vector2 escape =new Vector2();
            foreach( GameObject o in objects )
            {
                if (o.Id == this.Id)
                {
                    continue;
                }
                if( Intersects(Location2, o) )
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
                Location = Location2;
            }
            else
            {
                Location = Location + escape;
            }
        }

        
    }
}
