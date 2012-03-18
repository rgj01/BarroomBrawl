using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BarRoomBrawl
{
    class Client
    {
        private Socket Socket { get; set; }
        private GameState Latest;
        private byte[] Buffer;

        public Client()
        {
            Buffer = new byte[2056];
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public Player Connect(string IPAddr, int port)
        {
            IPAddress addr = IPAddress.Parse(IPAddr);
            Player p = null;
            try
            {
                Socket.Connect(addr, port);
                GameMessage join = new GameMessage();
                join.messageType = "Join";
                byte[] outbytes = join.toBytes();
                Debug.WriteLine("Outbytes length: {0}", outbytes.Length);
                Socket.Send(outbytes);

                Socket.Receive(Buffer);

                GameMessage response = GameMessage.fromBytes(Buffer);

                p = response.PlayerState;
            }
            catch (SocketException e)
            {
                Debug.WriteLine("Failed to connect: " + e.Message);
                return null;
            }

            SocketAsyncEventArgs rec_args = new SocketAsyncEventArgs();
            rec_args.Completed += ReceiveData;
            rec_args.SetBuffer(Buffer, 0, 2056);
            return p;
        }
        private void ReceiveData(object sender, SocketAsyncEventArgs args)
        {
            MemoryStream ms = new MemoryStream(args.Buffer);
            ms.Position = 0;
            BinaryFormatter decoder = new BinaryFormatter();
            GameMessage gm = (GameMessage)decoder.Deserialize(ms);
            HandleMessage(gm);

        }

        private void HandleMessage(GameMessage gm)
        {
            if (gm.messageType.CompareTo("ServerUpdate") == 0)
            {
                lock (Latest)
                {
                    Latest = gm.gs;
                }
            }
        }

        public void SendUpdate(Player player)
        {
            GameMessage gm = new GameMessage();
            gm.messageType = "PlayerUpdate";
            gm.PlayerState = player;
            BinaryFormatter serial = new BinaryFormatter();
            //NetworkStream n = new NetworkStream(Socket);
            MemoryStream ms = new MemoryStream();
            serial.Serialize(ms, gm);
            //serial.Serialize(n, gm);
            ms.Position = 0;
            Socket.Send(ms.GetBuffer());

        }

        /*
         * Return null if there isn't a new state since the last time this was called
         */
        public GameState getLatest()
        {
            lock (Latest)
            {
                GameState Output = Latest;
                Latest = null;
                return Output;
            }
        }


    }
}
