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
using System.Xml.Serialization;

namespace BarRoomBrawl
{
    class Server
    {
        private Socket Listener;
        private List<Socket> Sockets;
        private bool Started;
        private List<Player> Updates;
        private String UpdateLock;
        private String CurIdLock;
        private int CurId;

        public Server()
        {
            UpdateLock = "I'm just a lock";
            CurIdLock = "I'm just a lock";
            Updates = new List<Player>();
            Started = false;
            Sockets = new List<Socket>();
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            CurId = 100;
        }

        private void DataReceived(object sender, SocketAsyncEventArgs args)
        {
        
            Debug.WriteLine("Received Message");
            BinaryFormatter des = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(args.Buffer);

            ms.Position = 0;

            Debug.WriteLine("Stream stats:{0} pos:{1} transfered:{2}", ms.Length, ms.Position, args.BytesTransferred);

            GameMessage message = (GameMessage) des.Deserialize(ms);

            Socket s = (Socket)args.UserToken;
            processMessage(message, s);

            args.SetBuffer(0, 20560);
            s.ReceiveAsync(args);
        }

        private void processMessage(GameMessage gm, Socket sender)
        {
            if(gm.messageType.CompareTo("PlayerUpdate") == 0)
            {
                lock (UpdateLock)
                {
                    Updates.Add(gm.PlayerState);
                }
            }
            else if (gm.messageType.CompareTo("Join") == 0)
            {
                Player p;
                lock (CurIdLock)
                {
                    p = new Player("Player", new Microsoft.Xna.Framework.Vector2(500, 500), 0.0f, GameObject.Directions.N, CurId);
                    CurId++;
                }
                lock (UpdateLock)
                {
                    Updates.Add(p);
                }
                GameMessage response = new GameMessage();
                response.messageType = "JoinResponse";
                response.PlayerState = p;
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, response);
                sender.Send(ms.GetBuffer());
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
            byte[] buffer = new byte[20560];

            SocketAsyncEventArgs receive_args = new SocketAsyncEventArgs();
            receive_args.Completed += DataReceived;
            receive_args.SetBuffer(buffer, 0, 20560);
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

            Debug.WriteLine("Broadcasting state");
            BinaryFormatter format = new BinaryFormatter();

            MemoryStream ms = new MemoryStream();

            format.Serialize(ms, msg);

            ms.Position = 0;

            GameMessage msgout = (GameMessage)format.Deserialize(ms);
            Debug.WriteLine("Message:{0}", msgout);

            lock (Sockets)
            {
                List<Socket> kept = new List<Socket>();
                foreach(Socket s in Sockets)
                {
                    try
                    {
                        s.Send(ms.GetBuffer());
                        kept.Add(s);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Exception: " + e.Message);
                    }
                }
                Sockets = kept;
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
