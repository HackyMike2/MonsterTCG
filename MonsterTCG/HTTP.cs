﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

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
                            int ContentLength = 0;
                            string request = "";

                            request = reader.ReadLine();
                            //Console.WriteLine("Request:"); //DEBUG
                            //Console.WriteLine(request); //DEBUG
                            //Console.WriteLine("rest"); //DEBUG
                            while((line = reader.ReadLine()) != null && line != string.Empty) 
                            {
                               // Console.WriteLine(line);//DEBUG!
                                if (line.StartsWith("Content-Length")) 
                                {
                                    var parts = line.Split(':');
                                    ContentLength = int.Parse(parts[1].Trim());
                                }
                            }

                            string body = "";
                            if (ContentLength > 0) 
                            {
                                char[] buffer = new char[ContentLength];
                                int readbytes = reader.Read(buffer,0,buffer.Length);
                                body = new string(buffer,0,readbytes);
                                //Console.WriteLine("Body:");//Debug
                                //Console.WriteLine(body);//Debug
                            }
                            //give it to the handler
                            string response = Handler.HandleRequest(request,body);
                            Console.WriteLine("Response:"); //DEBUG?
                            Console.WriteLine(response);  //DEBUG?
                            writer.WriteLine(response);
                        }
                    }

                }

                   
            }
        }
    }
}
