using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace BarRoomBrawl
{
    class Server
    {
        private Socket Listener;
        private List<Socket> Sockets;
        private bool started;

        private Server()
        {
            started = false;
            Sockets = new List<Socket>();
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }

        private void ConnectionReceived(object sender, SocketAsyncEventArgs args)
        {
            lock (Sockets)
            {
                Sockets.Add(args.ConnectSocket);
            }
        }

        public void BroadcastState(GameState state)
        {

            //Create game state
            lock (Sockets)
            {
                foreach(Socket s in Sockets)
                {
                    NetworkStream stream = new NetworkStream(s);
                    BinaryFormatter format = new BinaryFormatter();
                    format.Serialize(stream, state);
                }
            }
        }

        private bool Start(int port)
        {
            if (!started)
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, 44444);
                Listener.Bind(ep);
                Listener.Listen(16);
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += ConnectionReceived;
                Listener.AcceptAsync(args);
                started = true;
                return true;
            }
            return false;

        }


    }
}
