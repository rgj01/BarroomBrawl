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

        public bool Connect(string IPAddr, int port)
        {
            IPAddress addr = IPAddress.Parse(IPAddr);
            try
            {
                Socket.Connect(addr, port);
            }
            catch (SocketException e)
            {
                Debug.WriteLine("Failed to connect: " + e.Message);
                return false;
            }

            SocketAsyncEventArgs rec_args = new SocketAsyncEventArgs();
            rec_args.Completed += ReceiveData;
            rec_args.SetBuffer(Buffer, 0, 2056);
            return true;
        }
        private void ReceiveData(object sender, SocketAsyncEventArgs args)
        {
            MemoryStream ms = new MemoryStream(args.Buffer);
            BinaryFormatter decoder = new BinaryFormatter();
            GameState gs = (GameState)decoder.Deserialize(ms);
            lock (Latest)
            {
                Latest = gs;
            }
        }

        /*
         * Return null if there isn't a new state since the last time this was called
         */
        private GameState getLatest()
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
