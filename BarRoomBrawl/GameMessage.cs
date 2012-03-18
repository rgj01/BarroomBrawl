using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarRoomBrawl
{
    [Serializable()]
    class GameMessage
    {
        public String messageType { get; set; }

        public GameState gs {get; set;}
        public Player PlayerState { get; set; }

        public GameMessage()
        {

        }
    }
}
