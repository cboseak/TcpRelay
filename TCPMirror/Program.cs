using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TCP_Com;


namespace TCPMirror
{
    class Program
    {

        static Socket ss;
        static IPEndPoint ipe;
        static readonly Action<Socket> cb = t =>
        {
            if (!ss.Connected)
                ss.Connect(ipe);
            new CallbackSock(t, ss).DoCallback();
        };

        static void Main(string[] args)
        {
            IPeople.Tools.ReactiveExtensions.Logwriter.Initialize();
            var settings = new TcpMirrorSetting();
            ipe = settings.DestinationEndpoint;
            ss = new Socket(settings.DestinationEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            SocketServer.StartListening(settings.LocalEndpoint, cb, "log.txt");
            Console.ReadKey();
        }



        private class CallbackSock
        {
            private readonly Socket s;
            private readonly Socket s_tar;
            private bool DoExit;

            public CallbackSock(Socket s, Socket s_tar)
            {

                this.s = s;
                this.s_tar = s_tar;
            }

            public void DoCallback()
            {
                new Thread(RCV).Start();
                new Thread(SND).Start();
                while (!DoExit)
                    Thread.Sleep(250);
            }

            private void RCV()
            {
                try
                {
                    while (true)
                        using (NetworkStream n = new NetworkStream(s))
                        using (NetworkStream n_tar = new NetworkStream(s_tar))
                            if (n.DataAvailable && n.CanRead)
                            {
                                    n.CopyTo(n_tar);
                                    n.Flush();
                                    n_tar.Flush();
                            }
                }
                catch{DoExit = true;}
            }

            private void SND()
            {
                try
                {
                    while (true)
                        using (NetworkStream n = new NetworkStream(s))
                        using (NetworkStream n_tar = new NetworkStream(s_tar))
                            if (n_tar.DataAvailable && n_tar.CanRead)
                            {
                                    n_tar.CopyTo(n);
                                    n.Flush();
                                    n_tar.Flush();
                            }
                }
                catch
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
