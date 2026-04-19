using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

class Client
{
    static string IP = " ";
    static int PORT = 8080;

    static void Main()
    {
        Console.WriteLine("Duke u lidhur me serverin...");

        try
        {
            using TcpClient client = new TcpClient(IP, PORT);
            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            // Fjalëkalimi
            string? passRequest = reader.ReadLine();
            if (passRequest == "PASS_REQUEST")
            {
                Console.Write("Fjalekalimi per admin (ENTER per read-only): ");
                string? pass = Console.ReadLine();
                writer.WriteLine(pass ?? "");
            }

            // Emri
            string? nameRequest = reader.ReadLine();
            if (nameRequest == "NAME_REQUEST")
            {
                Console.Write("Shkruaj emrin tend: ");
                string? name = Console.ReadLine();
                writer.WriteLine(name ?? "I panjohur");
            }

            string? info = reader.ReadLine();
            if (string.IsNullOrEmpty(info))
            {
                Console.WriteLine("Serveri nuk dha informacionin e pritshem.");
                return;
            }

            Console.WriteLine("U lidh! " + info);
            Console.WriteLine();
            Console.WriteLine("Komandat:");
            Console.WriteLine("MSG teksti");
            Console.WriteLine("LIST");
            Console.WriteLine("READ emriFile");
            Console.WriteLine("WRITE emriFile permbajtja");
            Console.WriteLine("DELETE emriFile");
            Console.WriteLine("EXECUTE");
            Console.WriteLine("EXIT");
            Console.WriteLine();
            
            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (input == null) break;
                if (input.Trim().Equals("EXIT", StringComparison.OrdinalIgnoreCase)) break;
                if (input.Trim() == "") continue;

                if (!KomandeValide(input))
                {
                    Console.WriteLine("Shkruaj nje komande valide: MSG, LIST, READ, WRITE, DELETE, EXECUTE, EXIT");
                    Console.WriteLine();
                    continue;
                }

                writer.WriteLine(input);
                string? response = reader.ReadLine();

                if (response == null)
                {
                    Console.WriteLine("Lidhja me serverin u nderpre.");
                    break;
                }

                Console.WriteLine(response);
                Console.WriteLine();
            }
            