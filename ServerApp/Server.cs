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
    static void HandleClient(TcpClient client)
    {
        int nr;
        lock (LockObj)
        {
            clientCount++;
            nr = clientCount;
        }

        using NetworkStream stream = client.GetStream();
        using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

        writer.WriteLine("PASS_REQUEST");
        string? pass = reader.ReadLine();

        bool fullAccess = false;
        lock (LockObj)
        {
            if (pass == AdminPassword && !adminConnected)
            {
                fullAccess = true;
                adminConnected = true;
            }
        }

        string permissions = fullAccess ? "write,read,execute" : "read";

        if (fullAccess)
        {
            Console.WriteLine("Klienti " + nr + " u lidh si ADMIN.");
            writer.WriteLine("ID:Klient-" + nr + " Privilegjet:" + permissions + " [ADMIN]");
        }
        else
        {
            Console.WriteLine("Klienti " + nr + " u lidh.");
            writer.WriteLine("ID:Klient-" + nr + " Privilegjet:" + permissions);
        }
    }
}