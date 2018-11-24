using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Zu.AsyncChromeDriver.Tests.Environment
{
    public class UrlBuilder
    {
        string _protocol;
        string _port;
        string _securePort;

        public string AlternateHostName { get; }

        public string HostName { get; }

        public string Path { get; }

        public UrlBuilder()
        {
            _protocol = EnvironmentManager.GetSettingValue("Protocol");
            HostName = EnvironmentManager.GetSettingValue("HostName");
            _port = EnvironmentManager.GetSettingValue("Port");
            _securePort = EnvironmentManager.GetSettingValue("SecurePort");
            // TODO(andre.nogueira): Remove trailing / from folder
            Path = EnvironmentManager.GetSettingValue("Folder");
            //Use the first IPv4 address that we find
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            foreach (IPAddress ip in Dns.GetHostEntry(HostName).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = ip;
                    break;
                }
            }
            AlternateHostName = ipAddress.ToString();
        }

        public string LocalWhereIs(string page)
        {
            string location = string.Empty;
            location = "http://localhost:" + _port + "/" + Path + "/" + page;

            return location;
        }

        public string WhereIs(string page)
        {
            string location = string.Empty;
            location = "http://" + HostName + ":" + _port + "/" + Path + "/" + page;

            return location;
        }

        public string WhereElseIs(string page)
        {
            string location = string.Empty;
            location = "http://" + AlternateHostName + ":" + _port + "/" + Path + "/" + page;

            return location;
        }

        public string WhereIsSecure(string page)
        {
            string location = string.Empty;
            location = "https://" + HostName + ":" + _securePort + "/" + Path + "/" + page;

            return location;
        }

        public string CreateInlinePage(InlinePage page)
        {
            Uri createPageUri = new Uri(new Uri(WhereIs(string.Empty)), "createPage");
            Dictionary<string, object> payloadDictionary = new Dictionary<string, object>();
            payloadDictionary["content"] = page.ToString();
            string commandPayload = JsonConvert.SerializeObject(payloadDictionary);
            byte[] data = Encoding.UTF8.GetBytes(commandPayload);
            HttpWebRequest request = HttpWebRequest.Create(createPageUri) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf8";
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            // StreamReader.Close also closes the underlying stream.
            Stream responseStream = response.GetResponseStream();
            StreamReader responseStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = responseStreamReader.ReadToEnd();
            responseStreamReader.Close();

            // The response string from the Java remote server has trailing null
            // characters. This is due to the fix for issue 288.
            if (responseString.IndexOf('\0') >= 0) {
                responseString = responseString.Substring(0, responseString.IndexOf('\0'));
            }

            if (responseString.Contains("localhost")) {
                responseString = responseString.Replace("localhost", this.HostName);
            }

            return responseString;
        }
    }
}
