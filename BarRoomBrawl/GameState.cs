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

        public void Update(GameTime gameTime)
        {
            foreach (GameObject o in m_gameObjects)
            {
                o.Update(gameTime, m_gameObjects);
            }
            m_gameObjects = m_gameObjects.OrderBy(x => x.Location.Y).ToList();
        }
    }
}
