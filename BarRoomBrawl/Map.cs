using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BarRoomBrawl
{
    public class Map : IDrawable 
    {
        List<GameObject> m_mapObjects;
        List<Player> m_players;

        public Map()
        {
        }

        public void Draw()
        {

        }

        public void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public int DrawOrder
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;

        // map will never be invisible
        public bool Visible
        {
            get { return true; }
        }

        public event EventHandler<EventArgs> VisibleChanged;
    }
}
