using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MPW_Server
{
    class Program
    {
        public static PriorityQueue<Request> RequestQueue;
        public static Thread threadLoadBalancer;
        public static List<Server> ServerList;
        public static List<Tuple<int, string>> clientList;

        static void Main(string[] args)
        {
            RequestQueue = new PriorityQueue<Request>();

            clientList = new List<Tuple<int, string>>();
            ServerList = new List<Server>
            {
                new Server("California", false),
                new Server("Chicago", false),
                new Server("New York", false),
                new Server("Lavonia", false),
                new Server("Detroit", false)
            };

            threadLoadBalancer = new Thread(LoadBalancer);
            threadLoadBalancer.Start();
            StartServer();
            Console.ReadLine();
        }

        public static void StartServer()
        {
            TcpListener serverSocket = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), 24568);
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();
            int counter = 0;

            try
            {
                while (true)
                {
                    counter++;
                    clientSocket = serverSocket.AcceptTcpClient();
                    ClientHandler client = new ClientHandler();
                    clientList.Add(new Tuple<int, string>(counter, ""));
                    client.StartClient(clientSocket, counter.ToString(), ref RequestQueue);
                }

            }
            catch (Exception)
            {
                clientSocket.Close();
                serverSocket.Stop();
                Console.WriteLine("UNEXPECTED SERVER BREAK...");
                Console.ReadKey();
            }
        }

        private static void LoadBalancer()
        {
            while (true)
            {
                Thread.Sleep(50);
                Console.Clear();
                foreach (Server s in ServerList)
                {
                    Console.WriteLine("");
                    Console.Write(s.Name + " \t[" + s.FileList.Count + "]:\t");
                    if (s.SyncingNow)
                    {
                        double percent = (double)(s.ProcessedRequestStartingValue - s.ProcessedRequest.Priority) / (double)s.ProcessedRequestStartingValue * 100.0;
                        Console.Write("Processing now [" + percent.ToString("00.0") + "%]:\t" + s.ProcessedRequest.Username + "\t" + s.ProcessedRequest.Filename );
                    }
                    else Console.Write("Open");
                    
                }

                if (RequestQueue.Count > 0)
                {
                    RequestQueue.Update();
                    Console.WriteLine("\n\n" + RequestQueue.ToString());
               
                    foreach (Server s in ServerList)
                    {
                        if (!s.SyncingNow)
                        {
                            s.SyncingNow = true;
                            
                            Request latestRequest = RequestQueue.Dequeue();
                            s.ProcessedRequest = latestRequest;
                            s.ProcessRequest();
                        }
                    }
                    Thread.Sleep(100);
                }
            }
        }
    }

    public class ClientHandler
    {
        public static bool Processed { get; set; }
        TcpClient clientSocket;
        PriorityQueue<Request> priorityQueue;
        string clNo;

        public void StartClient(TcpClient inputClientSocket, string clientNo, ref PriorityQueue<Request> q)
        {
            this.clientSocket = inputClientSocket;
            this.clNo = clientNo;
            this.clientSocket.ReceiveBufferSize = 1024;
            priorityQueue = q;
            Processed = false;
            
            Thread ctThread = new Thread(DoChat);
            ctThread.Start();
        }

        private void DoChat()
        {
            byte[] bytesFrom = new byte[2048];
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;

            while (true)
            {
                NetworkStream networkStream = clientSocket.GetStream();
                networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                string[] data = dataFromClient.Split(';');
                
                for (int i = 0; i < data.Length; i++)
                {
                    string[] req = data[i].Split(',');
                    if (req.Length > 2) priorityQueue.Enqueue(new Request(int.Parse(clNo), req[0], int.Parse(req[2]), req[1]));
                }
                networkStream.Flush();

                if (Processed)
                {
                    serverResponse = "FINISHED";
                    sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    networkStream.Close();
                }
            }

        }
    }

    public class PriorityQueue<T> where T : Request
    {
        private List<T> data;
        private Mutex mutex = new Mutex();

        public PriorityQueue() => this.data = new List<T>();

        public int Count => data.Count;

        public void Enqueue(T item)
        {
            mutex.WaitOne();
            data.Add(item);
            data.Sort((x, y) => x.Priority.CompareTo(y.Priority));
            mutex.ReleaseMutex();
        }

        public T Dequeue()
        {
            mutex.WaitOne();
            T item = data[0];
            data.RemoveAt(0);
            mutex.ReleaseMutex();
            return item;
        }

        public void Update()
        {
            mutex.WaitOne();

            foreach (T req in data)
            {
                if (req.TimeToLive > 0) req.TimeToLive--;
                else
                {
                    req.TimeToLive = 75;
                    req.Priority--;
                    data.Sort((x, y) => x.Priority.CompareTo(y.Priority));
                }
            }
            mutex.ReleaseMutex();
        }

        public override string ToString()
        {
            mutex.WaitOne();
            string s = "";
            for (int i = 0; i < data.Count(); ++i) s += data[i].ToString();
            s += "\ncount: " + data.Count;
            mutex.ReleaseMutex();
            return s; 
        }
    }

    public class Request : IComparable<Request>
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public int Priority { get; set; }
        public string Filename { get; set; }
        public int TimeToLive { get; set; }

        public Request(int id, string u, int p, string f)
        {
            UserID = id;
            Username = u;
            Priority = p;
            Filename = f;
            TimeToLive = 100;
        }

        public int CompareTo(Request other) => other.Priority.CompareTo(this.Priority);

        public override string ToString() => "[" + UserID + "] " + Username + "\t" + Priority + "\t" + Filename + "\n";
    }


    public class Server
    {
        public Thread serverThread;
        public string Name { get; set; }
        public List<Tuple<string, string>> FileList { get; set; }
        public int ProcessedRequestStartingValue = -1;
        private Request procReq;
        public Request ProcessedRequest
        {
            get { return procReq; }
            set
            {
                procReq = value;
                ProcessedRequestStartingValue = procReq.Priority;
            }
        }
        
        public bool SyncingNow { get; set; }

        public Server(string name, bool t)
        {
            Name = name;
            SyncingNow = t;
            FileList = new List<Tuple<string, string>>();
        }

        public void ProcessRequest()
        {
            serverThread = new Thread(Proccess);
            serverThread.Start();
        }

        public void Proccess()
        {
            while (ProcessedRequest.Priority > 0)
            {
                Thread.Sleep(100);
                ProcessedRequest.Priority--;
            }
            FileList.Add(new Tuple<string, string>(ProcessedRequest.Username, ProcessedRequest.Filename));
            SyncingNow = false;
        }
    }
}
