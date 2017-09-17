using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace GsmService
{
    class Program
    {
        static SerialPort p;
        [STAThread]
        static void Main(string[] args)
        {


            p = new SerialPort(Console.ReadLine(), 9600);
            p.Open();


            Console.WriteLine("Started");
            while (true)
            {

                string s = p.ReadLine();
                Console.WriteLine(s);

                if (s == "RING\r")
                {
                    p.WriteLine("ata");
                    try
                    {
                        TcpClient c = new TcpClient();
                        c.SendTimeout = 1000;
                        c.ReceiveTimeout = 1000;

                        IAsyncResult asr = c.BeginConnect(Dns.GetHostAddresses("homepcc.noip.me")[0], 33123, null, null);

                        bool res = asr.AsyncWaitHandle.WaitOne(1000, true);  // 10 sec timeout
                        if (res)
                        {
                            byte[] st = { 1, 0, 1, 0, 1, 11, 0 };
                            c.GetStream().Write(st, 0, st.Length);

                        }

                    }
                    catch (Exception)
                    {

                    }
                    System.Threading.Thread.Sleep(500);
                    p.WriteLine("ath");

                }


                System.Threading.Thread.Sleep(10);
            }
        }

        private static void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
        }
    }
}
