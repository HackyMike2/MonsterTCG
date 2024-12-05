using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace MonsterTCG
{
    internal class HTTP
    {
        TcpListener httpServer = new TcpListener(IPAddress.Loopback, 10001);

        public bool Started { get; set; } = false;

        public void startServer()
        {
            if (Started) { return; }
            httpServer.Start();
            Started = true;

            while (true)
            {
                var clientSocket = httpServer.AcceptTcpClient();
                using (var stream = clientSocket.GetStream()) 
                {
                    using (var writer = new StreamWriter(stream) {AutoFlush = true })
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            string line;

                            line = reader.ReadLine();
                            if (line != null)
                            {
                                Console.WriteLine(line);
                            }

                            while((line = reader.ReadLine()) != null) 
                            {
                                Console.WriteLine(line);
                            }

                        }
                    }

                }

                   
            }
        }
    }
}
