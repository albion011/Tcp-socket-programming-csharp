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
            