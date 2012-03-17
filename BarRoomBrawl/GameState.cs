using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BarRoomBrawl
{
    [Serializable()]
    class GameState
    {
        private List<GameObject> m_gameObjects;
        public List<GameObject> GameObjects 
        {
            get
            {
                return m_gameObjects;
            }
        }

        public GameState()
        {
            m_gameObjects = new List<GameObject>();
        }

        private void Contain(GameObject o)
        {
            if (o.Location.X < 0)
            {
                o.Location = new Vector2(0, o.Location.Y);
            }
            else if (o.Location.X > 10000)
            {
                o.Location = new Vector2(10000, o.Location.Y);
            }
            if (o.Location.Y < 0)
            {
                o.Location = new Vector2(o.Location.X, 0);
            }
            else if (o.Location.Y > 10000)
            {
                o.Location = new Vector2(o.Location.X, 10000);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (GameObject o in m_gameObjects)
            {
                o.Update(gameTime, m_gameObjects);
                Contain(o);
            }
            m_gameObjects = m_gameObjects.OrderBy(x => x.Location.Y).ToList();
        }
    }
}
