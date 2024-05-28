using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class WiiBalanceBoardServer
{
    private TcpListener server;
    private bool isRunning;

    public WiiBalanceBoardServer(int port)
    {
        server = new TcpListener(IPAddress.Any, port);
        server.Start();
        isRunning = true;

        
        Thread serverThread = new Thread(new ThreadStart(Run));
        serverThread.Start();
    }

    private void Run()
    {
        while (isRunning)
        {
            Console.WriteLine("Waiting for a connection...");
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Connected!");

            NetworkStream stream = client.GetStream();

            while (client.Connected)
            {
                try
                {
                    // Simulate reading data from the Wii Balance Board
                    // Replace this part with actual data reading from the Wii Balance Board
                    string data = "0.1,0.2,0.3,0.4\n";
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    stream.Write(msg, 0, msg.Length);
                    Thread.Sleep(100);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    client.Close();
                    break;
                }
            }
        }
    }

    public float getData()
    {

    }

    public void Stop()
    {
        isRunning = false;
        server.Stop();
    }

    public static void Main(string[] args)
    {
        WiiBalanceBoardServer server = new WiiBalanceBoardServer(12345);
        Console.WriteLine("Press Enter to stop the server...");
        Console.ReadLine();
        server.Stop();
    }
}
