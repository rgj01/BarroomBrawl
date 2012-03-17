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

        public enum Directions { E, SE, S, SW, W, NW, N, NE }
        public int Id { get; set; }
        public Vector2 Location { get; set; }
        public float Speed { get; set; }
        public Texture2D Texture { get; set; }
        public Directions Direction { get; set; }
        public BoundingBox BoundingBox { get; set; }
        
        public GameObject(Game game, Texture2D texture, Vector2 startLoc, float startSpeed, Directions startDir, int id)
        {
            m_game = game;
            Texture = texture;
            Location = startLoc;
            Speed = startSpeed;
            Direction = startDir;
            Id = id;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, Location, Color.White);
        }

        
    }
}
