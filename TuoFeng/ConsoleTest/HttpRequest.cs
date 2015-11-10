using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleTest
{
    public class HttpRequestUtil
    {
        private static readonly string UrlBase = "http://www.test.com/";
        public static string HttpGet(string url)
        {
            var requestUrl = UrlBase + url;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = "GET";
            var httpResponse = request.GetResponse();
            return "";
        }

        public static string HttpPost(string url, IDictionary<string, string> parameters)
        {
            var requestUrl = UrlBase + url;
            var request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            
            if (parameters!=null&&parameters.Count>0)
            {
                var paraStr = new StringBuilder();
                var i = 0;
                foreach (var parameter in parameters)
                {
                    var paraName = parameter.Key;
                    var paraValue = parameter.Value;
                    if (i==0)
                    {
                        paraStr.AppendFormat("{0}={1}", paraName, paraValue);

                    }
                    else
                    {
                        paraStr.AppendFormat("&{0}={1}", paraName, paraValue);
                    }
                    i++;
                }
                byte[] data = Encoding.UTF8.GetBytes(paraStr.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }  
            }

            var response = request.GetResponse();
            var dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            var result = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return result;
        }
    }
}