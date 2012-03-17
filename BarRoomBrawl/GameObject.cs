using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BarRoomBrawl
{
    public class GameObject
    {
        protected Game m_game;
        protected string m_textureName;

        public enum Directions { E, SE, S, SW, W, NW, N, NE }
        public int Id { get; set; }
        public Vector2 Location { get; set; }
        public float Speed { get; set; }
        public Texture2D Texture { get; set; }
        public Directions Direction { get; set; }

        public GameObject(Game game, string texture, Vector2 startLoc, float startSpeed, Directions startDir, int id)
        {
            m_game = game;
            m_textureName = texture;
            Location = startLoc;
            Speed = startSpeed;
            Direction = startDir;
            Id = id;
        }

        protected GameObject()
        {

        }

        public bool Intersects(GameObject other)
        {
            BoundingBox bb1 = new BoundingBox(new Vector3(this.Location, 0.0f), new Vector3(Location.X + Texture.Width, Location.Y + Texture.Height, 0.0f));

            BoundingBox bb2 = new BoundingBox(new Vector3(other.Location, 0.0f), new Vector3(other.Location.X + other.Texture.Width, other.Location.Y + other.Texture.Height, 0.0f));

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

        
    }
}
