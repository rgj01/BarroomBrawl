using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BarRoomBrawl
{
    public class GameObject: IDrawable, IUpdateable
    {
        public enum Directions { E, SE, S, SW, W, NW, N, NE }
        public int Id { get; set; }
        public Vector2 Location { get; set; }
        public float Speed { get; set; }
        public Texture2D Texture { get; set; }
        public Directions Direction { get; set; }
        public BoundingBox BoundingBox { get; set; }
        
        // we have no invisible objects
        public bool Visible
        {
            get { return true; }
        }

        public GameObject(Texture2D texture, Vector2 startLoc, float startSpeed, Directions startDir, int id)
        {
            Texture = texture;
            Location = startLoc;
            Speed = startSpeed;
            Direction = startDir;
            Id = id;
        }

        public override void Draw(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }




        public int DrawOrder
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;

        

        public event EventHandler<EventArgs> VisibleChanged;

        public bool Enabled
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> EnabledChanged;

        public int UpdateOrder
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> UpdateOrderChanged;
    }
}
