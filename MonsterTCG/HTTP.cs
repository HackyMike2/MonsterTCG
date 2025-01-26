#define DEBUG_MODE

using System;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata;

namespace MonsterTCG
{
    /// <summary>
    /// HTTP Server
    /// </summary>
    internal class HTTP
    {
        /// <summary>
        /// Is HTTP server already started
        /// </summary>
        public bool IsStarted { get; set; } = false;

        private TcpListener _httpServer = new TcpListener(IPAddress.Loopback, 10001);
        
        /// <summary>
        /// Start HTTP Server and accept incoming clients
        /// </summary>
        public void StartServer()
        {
            try
            {
                if (IsStarted) { return; }

                _httpServer.Start();
                IsStarted = true;

                Console.WriteLine("HTTP server started.");
                Console.WriteLine("Listening for incoming requests...");
                
                while (true)
                {
                    TcpClient clientSocket = _httpServer.AcceptTcpClient();
                    IPEndPoint remoteEndPoint = clientSocket.Client.RemoteEndPoint as IPEndPoint;

                    Console.WriteLine("Accepted request from {0}.", remoteEndPoint.Address.ToString());

                    Thread handleClientThread = new Thread(() => HandleClient(clientSocket));
                    handleClientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HTTP.StartServer: Error while listening for incoming clients. {0}", ex);
            }
        }

        /// <summary>
        /// Handle client communication in new thread
        /// </summary>
        /// <param name="client">Client to communicate with</param>
        private void HandleClient(TcpClient client)
        {
            try
            {
                Console.WriteLine("Handle client...");

                using (client)
                {
                    using (var stream = client.GetStream())
                    {
                        using (var writer = new StreamWriter(stream) { AutoFlush = true })
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                string request = reader.ReadLine();
                                Console.WriteLine("Request:\n{0}", request);

                                int contentLength = GetContentLength(reader);
                                Console.WriteLine("Content length: {0} ", contentLength);

                                _ = reader.ReadLine(); // Read empty line between content length and body

                                string body = GetRequestBody(reader, contentLength);
                                Console.WriteLine("Body:\n{0}", request);
                                
                                // Give it to the handler
                                string response = Handler.HandleRequest(request, body);
                                Console.WriteLine("Response:\n{0}", response);
                                
                                writer.WriteLine(response);
                                
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("HTTP.HandleClient: Error while reading incoming request. {0}", ex);
            }
        }

        private static string GetRequestBody(StreamReader reader, int contentLength)
        {
            string body = string.Empty;

            if (contentLength > 0)
            {
                char[] buffer = new char[contentLength];
                int readbytes = reader.Read(buffer, 0, buffer.Length);

                body = new string(buffer, 0, readbytes);
            }

            return body;
        }

        private static int GetContentLength(StreamReader reader)
        {
            string line = string.Empty;

            while ((line = reader.ReadLine()) != null && line != string.Empty)
            {
                if (line.StartsWith("Content-Length"))
                {
                    var parts = line.Split(':');
                    return int.Parse(parts[1].Trim());
                }
            }

            return 0;
        }
    }
}
