using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;

namespace BarRoomBrawl
{
    class Server
    {
        private Socket Listener;
        private List<Socket> Sockets;
        private bool Started;
        private List<Player> Updates;
        private String UpdateLock;

        public Server()
        {
            UpdateLock = "I'm just a lock";
            Updates = new List<Player>();
            Started = false;
            Sockets = new List<Socket>();
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }

        private void DataReceived(object sender, SocketAsyncEventArgs args)
        {
            
            String Input = System.Text.Encoding.Default.GetString(args.Buffer);
            Debug.WriteLine("Received: {0}", Input);
            BinaryFormatter des = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(args.Buffer);

            GameMessage message = (GameMessage) des.Deserialize(ms);

            Socket s = (Socket)args.UserToken;
            args.SetBuffer(0, 2056);
            s.ReceiveAsync(args);
        }

        private void processMessage(GameMessage gs)
        {
            if(gs.messageType.CompareTo("PlayerUpdate") == 0)
            {
                lock (UpdateLock)
                {
                    Updates.Add(gs.PlayerState);
                }
            }
        }

        private void ConnectionReceived(object sender, SocketAsyncEventArgs args)
        {
            Socket s = args.AcceptSocket;
            Debug.WriteLine("Connection received");
            lock (Sockets)
            {

                Sockets.Add(s);
                Debug.WriteLine("Got {0}", s);
                
            }
            byte[] buffer = new byte[2056];

            SocketAsyncEventArgs receive_args = new SocketAsyncEventArgs();
            receive_args.Completed += DataReceived;
            receive_args.SetBuffer(buffer, 0, 2056);
            receive_args.UserToken = s;
            s.ReceiveAsync(receive_args);
            StartAccept(args);
        }

        public List<Player> GetUpdates()
        {

            lock (UpdateLock)
            {
                List<Player> Output = Updates;
                Updates = new List<Player>();
                return Output;
            }
        }

        public void BroadcastState(GameState state)
        {
            GameMessage msg = new GameMessage();
            msg.messageType = "ServerUpdate";
            msg.gs = state;
            //Create game state
            lock (Sockets)
            {
                foreach(Socket s in Sockets)
                {
                    NetworkStream stream = new NetworkStream(s);
                    BinaryFormatter format = new BinaryFormatter();
                    format.Serialize(stream, msg);
                }
            }
        }

        private void StartAccept(SocketAsyncEventArgs args)
        {

            if (args == null)
            {
                args = new SocketAsyncEventArgs();
                args.Completed += ConnectionReceived;
            }
            else
            {
                args.AcceptSocket = null;
            }

            Listener.AcceptAsync(args);
        }

        public bool Start(int port)
        {
            if (!Started)
            {
                Debug.WriteLine("Starting server");
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);
                Listener.Bind(ep);
                Listener.Listen(16);
                StartAccept(null);
                Started = true;
                return true;
            }
            return false;

        }


    }
}
