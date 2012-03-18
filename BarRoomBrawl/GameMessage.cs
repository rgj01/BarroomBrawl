using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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

        public byte[] toBytes()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, this);
            ms.Position = 0;
            return ms.GetBuffer();
        }

        public static GameMessage fromBytes(byte[] bytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(bytes);
            ms.Position = 0;
            GameMessage gm = (GameMessage)bf.Deserialize(ms);
            return gm;
        }
    }
}
