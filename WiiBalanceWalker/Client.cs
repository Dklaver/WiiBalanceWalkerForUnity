using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace WiiBalanceWalker
{
    public class Client
    {
        public static int dataBufferSize = 4096;

        public int id;
        public TCP tcp;

        public Client(int _clientId)
        {
            id = _clientId;
            tcp = new TCP(id);
        }
        
        //public Client()
        //{
        //    tcp = new TCP(id);
        //}

        public class TCP
        {
            public TcpClient socket;

            private readonly int id;

            private NetworkStream stream;
            private byte[] receiveBuffer;

            public TCP(int _id)
            {
                id = _id;
            }

            public void Connect(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();

                receiveBuffer = new byte[dataBufferSize];

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                ServerSend.Welcome(id, "hello!!!!");

            }

            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null && socket.Connected && stream.CanWrite)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                    }
                }
                catch (Exception ex)
                {
                    HandleDisconnection();
                    Console.WriteLine($"Error sending data to player {id} via TCP: {ex}");
                }
            }
            private void HandleDisconnection()
            {
                // Code to handle the disconnection, e.g., cleanup resources, notify the player, etc.
                if (socket != null)
                {
                    socket.Close();
                    socket = null;
                }
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"Connection to player {id} has been closed.");
                Console.ResetColor();
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLenght = stream.EndRead(_result);
                    if (_byteLenght <= 0)
                    {
                        return;
                    }
                    byte[] _data = new byte[_byteLenght];
                    Array.Copy(receiveBuffer, _data, _byteLenght);

                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error receiving TCP data: {ex}");
                }
            }

            public void getWiiBoardData(string data)
            {
                ServerSend.UpdateData(id, data);

            }

        }

        
    }

    
}
