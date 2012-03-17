using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarRoomBrawl
{
    [Serializable()]
    class GameState
    {
        public List<GameObject> StaticObjects { get; set; }

        public List<GameObject> DynamicObjects { get; set; }

        public GameState()
        {
            StaticObjects = new List<GameObject>();
            DynamicObjects = new List<GameObject>();
        }
    }
}
