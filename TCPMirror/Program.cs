using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using IPeople.Tools.ReactiveExtensions;
using TCP_Com;


namespace TCPMirror
{
    class Program
    {
        private static TcpMirrorSetting settings;

        static readonly Action<Socket> cb = t =>
        {
            Socket ss = new Socket(settings.DestinationEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            if (!ss.Connected)
                ss.Connect(settings.DestinationEndpoint);
            new CallbackSock(t, ss).DoCallback();
        };

        static void Main(string[] args)
        {
            Logwriter.Initialize();
            settings = new TcpMirrorSetting(true);
            
            
            SocketServer.StartListening(settings.LocalEndpoint, cb, "log.txt");
            Console.ReadKey();
        }



        private class CallbackSock
        {
            private Socket s;
            private Socket s_tar;
            private bool DoExit;

            public CallbackSock(Socket s, Socket s_tar)
            {
                this.s = s;
                this.s_tar = s_tar;
            }

            public void DoCallback()
            {
                using (Socket s_d = this.s)
                using (Socket s_tar_d = this.s_tar)
                using (var bqSS = new BinaryQueue(true))
                using (var bqS = new BinaryQueue(true))
                {
                    try

                    {
                        new Thread(() => { rcvLoop(s_d, bqS, ref DoExit); }).Start();
                        new Thread(() => { rcvLoop(s_tar_d, bqSS, ref DoExit); }).Start();
                        new Thread(() => { sndLoop(s_tar_d, bqS, ref DoExit); }).Start();
                        new Thread(() => { sndLoop(s_d, bqSS, ref DoExit); }).Start();

                        while (!DoExit)
                            Thread.Sleep(250);
                    }
                    catch
                    {
                        DoExit = true;
                    }
                }
            }


            void rcvLoop(Socket socI,  BinaryQueue bq, ref bool DoExit)
            {
                try
                {

                    using (var stream = new NetworkStream(socI))
                    using (var bstream = new BufferedStream(stream))
                        bstream.CopyTo(bq);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                   // throw;
                }
                finally
                {
                    DoExit = true;
                }
            }
            void sndLoop(Socket socO,  BinaryQueue bq, ref bool DoExit)
            {
                try
                {
                    using (var stream = new NetworkStream(socO))
                    using (var bstream = new BufferedStream(stream))
                        while (true)
                            if (bq.HowFarBehind > 0)
                            {
                                byte[] b = bq.GetBlock();
                                bstream.Write(b, 0, b.Length);
                                bstream.Flush();
                            }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // throw;
                }
                finally
                {
                    DoExit = true;
                }
            }

        }






        internal class TcpMirrorSetting
        {
            public TcpMirrorSetting(int listenPort, string targetIp, int targetPort)
            {
                DestinationPort = targetPort;
                DestinationIp = IPAddress.Parse(targetIp);
                ListenPort = listenPort;
            }
            public TcpMirrorSetting(string[] arr)
            {
                ConfigWithArr(arr);
            }
            public TcpMirrorSetting(bool debug)
            {
                if (!debug)
                    ConfigWithArr(File.ReadAllText("mirror.ini").Split('|'));
            }
            internal void ConfigWithArr(string[] arr)
            {
                ListenPort = Convert.ToInt32(arr[0]);
                DestinationIp = IPAddress.Parse(arr[1]);
                DestinationPort = Convert.ToInt32(arr[2]);
            }

            public TcpMirrorSetting()
            {
                try { ConfigWithArr(File.ReadAllText("mirror.ini").Split('|')); }
                catch { }
            }
            IPEndPoint d_ipe;
            IPEndPoint l_ipe;
            int DestinationPort { get; set; } = 8081;
            int ListenPort { get; set; } = 8080;
            public IPAddress DestinationIp { get; set; } = Dns.Resolve(Dns.GetHostName()).AddressList[0];
            IPAddress LocalIp => Dns.Resolve(Dns.GetHostName()).AddressList[0];
            public IPEndPoint LocalEndpoint => l_ipe ?? (l_ipe = new IPEndPoint(LocalIp, ListenPort));
            public IPEndPoint DestinationEndpoint => d_ipe ?? (d_ipe = new IPEndPoint(DestinationIp, DestinationPort));
        }


    }
}
