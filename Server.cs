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
        try
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                string response = ProcessRequest(line, fullAccess);
                Console.WriteLine("[Klient-" + nr + "] " + line);
                writer.WriteLine(response);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Gabim me klientin " + nr + ": " + ex.Message);
        }

        if (fullAccess)
        {
            lock (LockObj)
            {
                adminConnected = false;
            }
            Console.WriteLine("Klienti " + nr + " (ADMIN) u shkeput.");
        }
        else
        {
            Console.WriteLine("Klienti " + nr + " u shkeput.");
        }

        client.Close();
    
    }

    static readonly string DataFolder = 
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "server_files");

    static readonly string AdminPassword = "admin123";

    static string ProcessRequest(string line, bool FullAccess)
    {
      if(string.IsNullOrWhiteSpace(line))
      return "Kerkese e zbrazet.";

      string[] parts = line.Split('',3,
      StringSplitOptions.RemoveEmptyEntries);
      string command = parts[0].ToUpper();

      if(command == "MSG")
        {
            if(parts.Length < 2) return "Perdorimi: MSG teksti";
            string message = line.Length > 4 ? line.Substring(4):"";
            return "Mesazhi u pranua:" + message;        
        }

        if(command == "LIST")
        {
            string[] files = Directory.GetFiles(DataFolder);
            if(files.Length == 0) return "Nuk ka file ne server.";

            for(int i = 0; i < files.Length; i++)
            files[i] = Path.GetFileName(files[i]);

            return "Files:" + string.Join(",",files);
        }

        if(command == "READ")
        {
            if(parts.Length < 2) return "Perdorimi: READ emriFile";

            string filePath =GetSafePath(parts[1]);
            if(filePath == "") return "Emri i file-it nuk lejohet.";
            if(!File.Exists(filePath)) return "File nuk ekziston.";

            return "Permbajtja: " + File.ReadAllText(filePath, Encoding.UTF8);
        }

    
         if(command == "WRITE")
        {
            if(!fullAccess) return "Nuk keni privilegje per WRITE.";
            if(parts.Length < 3) return "Perdorimi: WRITE emriFile permbajtja";

            string filePath = GetSafePath(parts[1]);
            if(filePath == "") return "Emri i file-it nuk lejohet.";

            File.WriteAllText(filePath, parts[2], Encoding.UTF8);
            return "File u ruajt me sukses";
        }
        

    }
}