using AustinHarris.JsonRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Http;

namespace OilServiceLib
{
    /// <summary>
    /// web server class
    /// </summary>
    public class HttpWebServer
    {
        /// <summary>
        /// listener to wait for http requests
        /// </summary>
        private HttpListener Listener;

        /// <summary>
        /// json rpc services
        /// </summary>
        public static object[] services;

        /// <summary>
        /// starts the http server and sets up data
        /// </summary>
        public void Start()
        {
            Listener = new HttpListener();
            Listener.Prefixes.Add($"http://127.0.0.1:8080/");
            Listener.Start();
            Listener.BeginGetContext(_processRequest, Listener);
            var oilService = new OilService();
            _readOilDataFromDataHub(oilService);
            services = new object[] { oilService };
            Console.WriteLine("Connection Started");
        }

        /// <summary>
        /// stops http server
        /// </summary>
        public void Stop()
        {
            Listener.Stop();
        }

        /// <summary>
        /// dummy necessary for the lib to load services
        /// </summary>
        private static volatile int _ctr;

        /// <summary>
        /// function to process http request (only jsonrpc ones)
        /// </summary>
        /// <param name="result"></param>
        private void _processRequest(IAsyncResult result)
        {
            _ctr = 0;
            HttpListener listener = (HttpListener)result.AsyncState;
            HttpListenerContext context = listener.EndGetContext(result);
            HttpListenerRequest request = context.Request;
            var sessionId = Handler.DefaultSessionId();
            var toProcess = new StreamReader(request.InputStream).ReadToEnd();
            string response = JsonRpcProcessor.Process(sessionId, toProcess).Result;
            Console.WriteLine(response);
            byte[] buffer = Encoding.UTF8.GetBytes(response);

            context.Response.ContentLength64 = buffer.Length;
            context.Response.Headers["Access-Control-Allow-Origin"] = "*";
            context.Response.Headers["Access-Control-Allow-Credentials"] = "true";
            context.Response.Headers["Access-Control-Allow-Methods"] = "POST";
            context.Response.Headers["Access-Control-Allow-Headers"] = "Content-type";
            context.Response.ContentType = "application/json";
            System.IO.Stream output = context.Response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();

            Listener.BeginGetContext(_processRequest, Listener);
        }

        /// <summary>
        /// reads data from public json file
        /// </summary>
        /// <param name="service"></param>
        private void _readOilDataFromDataHub(OilService service)
        {
            HttpClient client = new HttpClient();
            var responseString = client.GetStringAsync("https://pkgstore.datahub.io/core/oil-prices/brent-daily_json/data/78b325d2b9b2be78282cfd9f62978149/brent-daily_json.json").Result;
            service.SetData(responseString);
        }
    }
}
