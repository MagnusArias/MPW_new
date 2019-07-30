using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MPW_Client
{
    class Program
    {
        static readonly string[] filenames1 =
        {
            "My",
            "Sturdy",
            "Epic",
            "Glorious",
            "Normal",
            "Small",
            "Big",
            "Suspicious",
            "Dummy",
            "Loafy",
            "Classical"
        };

        static readonly string[] filenames2 =
        {
            "Application",
            "Photos",
            "Movie",
            "Holidays",
            "Computer",
            "Phone",
            "Something",
            "Letter",
            "Paper",
            "Book",
            "Window"
        };

        static readonly string[] extensions =
        {
            ".exe",
            ".jpg",
            ".png",
            ".dat",
            ".bin",
            ".dvg",
            ".txt",
            ".wmv",
            ".mp3",
            ".avi",
            ".lua"
        };

        static List<User> UserList;
        static void Main(string[] args)
        {
            UserList = new List<User>()
            {
                new User("Maras", new List<Tuple<string, int>>()
                {
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10)
                }),
                new User("Yankes", new List<Tuple<string, int>>()
                {
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10)
                }),
                new User("Gorky", new List<Tuple<string, int>>()
                {
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10)
                }),
                new User("Magnus", new List<Tuple<string, int>>()
                {
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10)
                }),
                new User("Rita", new List<Tuple<string, int>>()
                {
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10)
                }),
                new User("Oscar", new List<Tuple<string, int>>()
                {
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10)
                }),
                new User("Hans", new List<Tuple<string, int>>()
                {
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10),
                    new Tuple<string, int>("file", 12),
                    new Tuple<string, int>("file", 10)
                })
            };


            TcpClient clientSocket = new TcpClient();
            string Username = "";

            try
            {
                Username = args[0];
                string u = UserList.Find(x => x.Name.ToLower().Equals(Username.ToLower())).Name;
            }
            catch (IndexOutOfRangeException) { Environment.Exit(1); }
            catch (NullReferenceException) { Environment.Exit(1); }

            List<Tuple<string, int>> files = UserList.Find(x => x.Name.ToLower().Equals(Username.ToLower())).Files;

            clientSocket.Connect("127.0.0.1", 24568);
            clientSocket.ReceiveBufferSize = 1024;
            Console.WriteLine("Connected to server");
            NetworkStream networkStream = clientSocket.GetStream();
            foreach (var f in files)
            {
                byte[] outStream = Encoding.ASCII.GetBytes(Username + "," + f.Item1 + "," + f.Item2 + ";");
                networkStream.Write(outStream, 0, outStream.Length);
                networkStream.Flush();
            }

            // username, filename, size
            Console.WriteLine("Sent message");
            byte[] inStream = new byte[1024];
            networkStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
            string returnData = Encoding.ASCII.GetString(inStream);

            if (returnData.Equals("FINISHED")) Environment.Exit(0);

        }
    }

    public class User
    {
        public string Name { get; set; }
        public List<Tuple<string, int>> Files { get; set; }

        public User(string n, List<Tuple<string, int>> f)
        {
            Name = n;
            Files = f;
        }
    }
}
