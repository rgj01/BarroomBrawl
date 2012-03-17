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
        private List<GameObject> Updates;
        private String UpdateLock;

        public Server()
        {
            Updates = new List<GameObject>();
            Started = false;
            Sockets = new List<Socket>();
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }

        private void DataReceived(object sender, SocketAsyncEventArgs args)
        {
            UpdateLock = "I'm just a lock";
            String Input = System.Text.Encoding.Default.GetString(args.Buffer);
            Debug.WriteLine("Received: {0}", Input);
            BinaryFormatter des = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(args.Buffer);
            object Object = des.Deserialize(ms);

            lock (UpdateLock)
            {
                Updates.Add((GameObject)Object);
            }

            Socket s = (Socket)args.UserToken;
            args.SetBuffer(0, 2056);
            s.ReceiveAsync(args);
        }

        private void ConnectionReceived(object sender, SocketAsyncEventArgs args)
        {
            Debug.WriteLine("Connection received");
            lock (Sockets)
            {
                byte[] buffer = new byte[2056];
                Socket s = args.AcceptSocket;
                SocketAsyncEventArgs receive_args = new SocketAsyncEventArgs();
                receive_args.Completed += DataReceived;
                receive_args.SetBuffer(buffer, 0, 2056);
                receive_args.UserToken = s;
                s.ReceiveAsync(receive_args);
                Sockets.Add(s);
                Debug.WriteLine("Got {0}", s);
                
            }
            StartAccept(args);
        }

        public List<GameObject> GetUpdates()
        {

            lock (UpdateLock)
            {
                List<GameObject> Output = Updates;
                Updates = new List<GameObject>();
                return Output;
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
