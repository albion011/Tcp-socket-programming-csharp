using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Server
{
   

    static string IP = " ";
    static int PORT = 8080;
    static int clientCount = 0;
    static bool adminConnected = false;
    static readonly object LockObj = new object();

    static void Main()
    {
        Directory.CreateDirectory(DataFolder);

        TcpListener server = new TcpListener(IPAddress.Any, PORT);
        server.Start();

        Console.WriteLine("Serveri startoi ne IP: " + IP + " Port: " + PORT);
        Console.WriteLine("Folderi i file-ve: " + DataFolder);

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Thread t = new Thread(() => HandleClient(client));
            t.Start();
        }
    }
}