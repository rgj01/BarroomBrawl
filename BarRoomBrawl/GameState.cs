using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarRoomBrawl
{
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

        public void Update()
        {
            m_gameObjects = m_gameObjects.OrderBy(x => x.Location.Y).ToList();
        }
    }
}
