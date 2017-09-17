using System;
using System.Collections.Generic;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using Logic;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Net;

namespace Logic
{
    public delegate void FinishedGettingDataJsonDelegate(Newtonsoft.Json.Linq.JObject obj);
    public delegate void FinishedGettingDataStringDelegate(string str);

    public class Connection
    {
        private HttpClient hc;

        public Connection ()
        {
            ServicePointManager.CertificatePolicy = new MyPolicy();
            hc = new HttpClient();
        }

        public async void GetDataJson(string uri, KeyValuePair<string, string>[] kv, FinishedGettingDataJsonDelegate fgd)
        {

            string paras = "";
            foreach (var p in kv)
                paras += WebUtility.UrlEncode(p.Key+"="+p.Value+"&");

            string url = uri + "?" + paras;
            string str = await hc.GetStringAsync(url);
            fgd(Newtonsoft.Json.Linq.JObject.Parse(str));
        }

        public async void SendPostDataJson(string uri, KeyValuePair<string, string>[] kv, FinishedGettingDataJsonDelegate fgd)
        {
            HttpContent content = new FormUrlEncodedContent(kv);
            HttpResponseMessage responseHttp = await hc.PostAsync(uri.ToString(), content);
            string responseString = await responseHttp.Content.ReadAsStringAsync();
            fgd(Newtonsoft.Json.Linq.JObject.Parse(responseString));
        }

        public async void GetData(string uri, KeyValuePair<string, string>[] kv, FinishedGettingDataStringDelegate fgd)
        {

            string paras = "";
            foreach (var p in kv)
                paras += (p.Key + "=" + p.Value + "&");

            string url = uri + "?" + paras;
            string str = await hc.GetStringAsync(url);
            fgd(str);
        }

        public async void SendPostData(string uri, KeyValuePair<string, string>[] kv, FinishedGettingDataStringDelegate fgd)
        {
            HttpContent content = new FormUrlEncodedContent(kv);
            HttpResponseMessage responseHttp = await hc.PostAsync(uri.ToString(), content);
            string responseString = await responseHttp.Content.ReadAsStringAsync();
            fgd(responseString);
        }


        public class MyPolicy : ICertificatePolicy
        {
            public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request,
          int certificateProblem)
            {
                //Return True to force the certificate to be accepted.
                return true;
            }
        }


    }
}
