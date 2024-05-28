using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiiBalanceWalker
{
    public class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            //PrintClients();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        //public static void PrintClients()
        //{
        //    Console.WriteLine("current clients: ");

        //    foreach (var client in Server.clients)
        //    {
        //        Console.WriteLine($"Client ID: {client.Key}");
        //    }
        //}
        public static void UpdateData(int _toClient, string data)
        {
            using (Packet _packet = new Packet((int)ServerPackets.updateData))
            {
                _packet.Write(data);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }
        public static void Welcome(int _toClient, string data)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(data);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }
    }
}
