using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Logic;

namespace ComponentSearcher
{
    class Program
    {
        static UdpClient udpClient = new UdpClient(51234);

        const int PORT = 51234;

        const string whoIsThereMessage = "0000000000";

        static void Main(string[] args)
        {

            while(true)
            {
                Thread udpThread = new Thread(UDPServer);

                SendUdp("10.0.0.255", PORT, Encoding.ASCII.GetBytes(whoIsThereMessage));
                udpThread.Start();

                Thread.Sleep(3* 60 * 1000);
                udpThread.Abort();
                Thread.Sleep(500);
                
            }

        }

        static void GotData(string str)
        {
            Console.WriteLine(str);
        }

        static void SendUdp(string dstIp, int dstPort, byte[] data)
        {
            using (UdpClient c = new UdpClient())
                c.Send(data, data.Length, dstIp, dstPort);
        }

        static void UDPServer()
        {
            Connection con = new Connection();
            con.SendPostData("http://api.lime-tree.eu/light/modify.php", new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("modi", "3") }, GotData);
            SendUdp("10.0.0.255", PORT, Encoding.ASCII.GetBytes(whoIsThereMessage));
            while (true)
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, PORT);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);
                Console.WriteLine(returnData);
                if (returnData.Contains('+'))
                {
                    string[] arr = returnData.Split('+');
                    string name = arr[1];

                    string ipAddress = RemoteIpEndPoint.Address.ToString();
                    Console.WriteLine(ipAddress + ": " + name);
                    con.SendPostData("http://api.lime-tree.eu/light/modify.php", new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("modi", "1"),
                                                                                                                    new KeyValuePair<string, string>("name", name),
                                                                                                                    new KeyValuePair<string, string>("ip_address", ipAddress)}, GotData);

                }
            }
        }
    }
}
