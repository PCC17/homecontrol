using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic;
using Newtonsoft.Json;


namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = "http://localhost:8080/getToken.php";
            Connection c = new Connection();

            c.SendPostDataJson(host, new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("user", "user"), new KeyValuePair<string, string>("password", "user") }, GotData);
            Console.ReadLine();
        }

        static void GotData(Newtonsoft.Json.Linq.JObject obj)
        {
            Console.WriteLine(obj.SelectToken("success"));
        }

        static void GotData(string str)
        {
            Console.WriteLine(str);
        }
    }
}
