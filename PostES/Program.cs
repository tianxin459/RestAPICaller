using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Headers;

namespace PostES
{
    class Program
    {
        static void Main(string[] args)
        {
            int callTimes = 100;

            string filePath = @"c:\postingLog.txt";
            try
            {
                StreamWriter sw = new StreamWriter(filePath);
                for (int i = 0; i < callTimes; i++)
                {
                    if (i % 20 == 0)
                    {
                        Console.WriteLine($"=>{i} times called ============================================================");
                        sw.Flush();
                    }
                    sw.Write(PostWebUserAPI(i));
                    //sw.Write(PostAPI());
                    if (i % 10 == 0)
                    {
                        Console.WriteLine("\r\n");
                    }
                    Thread.Sleep(500);
                }
                sw.Flush();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static string PostAPI()
        {
            StringBuilder strRet = new StringBuilder();
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                //var url = "https://api.gobank.com/v3/rest/User/ExtendSession";
                //var url = "https://qa3-api-gobankdev.nextestate.com/v3/rest/User/ExtendSession";
                //var url = "http://gdcsvc/Partner/Flex/User/ExtendSession";
                var url = "https://api.gobank.com/v3/rest/User/Login";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-GDC-ApplicationID", "20033");
                client.DefaultRequestHeaders.Add("X-GDC-DeviceID", "20FE6F3B-BCD3-48E8-B6BF-E8A61573C8CB");
                client.DefaultRequestHeaders.Add("X-GDC-Digest", "uze8oBXOzFTSMdnc/D+a5DPG4VDIVER1ofxjT950fKA=");
                client.DefaultRequestHeaders.Add("X-GDC-MessageID", "9551D25F-374F-4BA0-9C74-414ABA6B9292");
                client.DefaultRequestHeaders.Add("X-GDC-SessionToken", "a009fa68-35ff-4701-865a-9b3fbda9a612");
                client.DefaultRequestHeaders.Add("X-GDC-Method", "1");
                var dt = DateTime.Now.AddHours(-8).AddSeconds(-10).ToString("yyyy-MM-ddThh:mm:ss.fff");
                client.DefaultRequestHeaders.Add("X-GDC-Timestamp", dt);
                client.DefaultRequestHeaders.Add("X-GDC-Version", "1.001");
                //client.DefaultRequestHeaders.Add("Content-Type", @"application/json");
                //client.DefaultRequestHeaders.Add("Accept", @"application/json");

                //"2017-11-23T03:00:11.313"
                var resp = client.GetAsync(url).Result;
                var serializedResponse = resp.Content.ReadAsStringAsync().Result;
                dynamic respJ = JsonConvert.DeserializeObject<dynamic>(serializedResponse);
                var er = respJ.Errors;
                foreach (var e in er)
                {
                    if (e.Code == "220")
                    {
                        strRet.Append("#");
                        strRet.Append(serializedResponse);
                        strRet.Append(DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fff"));
                        strRet.Append("#");
                    }
                    else
                    {
                        strRet.Append("|");
                        strRet.Append(e.Code);
                        strRet.Append("|");
                    }
                }
            }
            return strRet.ToString();
        }

        private static string PostWebUserAPI(int? seq = null)
        {
            StringBuilder strRet = new StringBuilder();
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;


                var url = "https://api.gobank.com/v3/rest/User/Login";
                //var url = "http://localhost/Partner/Flex/User/Login";
                //var url = "http://gdcsvc/Partner/Flex/User/login";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-GDC-ApplicationID", "20033");
                client.DefaultRequestHeaders.Add("X-GDC-DeviceID", "20FE6F3B-BCD3-48E8-B6BF-E8A61573C8CB");
                client.DefaultRequestHeaders.Add("X-GDC-Digest", "uze8oBXOzFTSMdnc/D+a5DPG4VDIVER1ofxjT950fKA=");
                client.DefaultRequestHeaders.Add("X-GDC-MessageID", "9551D25F-374F-4BA0-9C74-414ABA6B9292");
                client.DefaultRequestHeaders.Add("X-GDC-SessionToken", "a009fa68-35ff-4701-865a-9b3fbda9a612");
                client.DefaultRequestHeaders.Add("X-GDC-Method", "1");
                var dt = DateTime.Now.AddHours(-8).AddSeconds(-10).ToString("yyyy-MM-ddThh:mm:ss.fff");
                client.DefaultRequestHeaders.Add("X-GDC-Timestamp", dt);
                client.DefaultRequestHeaders.Add("X-GDC-Version", "1.001");

                var content = new StringContent(JsonConvert.SerializeObject(new { name="aa"}), Encoding.UTF8, "application/json");

                //"2017-11-23T03:00:11.313"
                var resp = client.PostAsync(url, content).Result;
                var serializedResponse = resp.Content.ReadAsStringAsync().Result;
                //dynamic respJ = JsonConvert.DeserializeObject<dynamic>(serializedResponse);
                //var er = respJ.Errors;

                if (!serializedResponse.Contains("WebUserToken"))
                {
                    Console.WriteLine($"{seq}=> missing webUserToken!");
                    var strDateTime = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fff");
                    strRet.Append($"=>{strDateTime} - start -------------------------------------------------\r\n");
                    strRet.Append(serializedResponse);
                    strRet.Append("\r\n");
                    strRet.Append($"=>{strDateTime} - end -------------------------------------------------\r\n");
                    strRet.Append("\r\n");
                }
                else
                {
                    strRet.Append("|");
                    strRet.Append(".");
                    strRet.Append("|");
                }
            }
            return strRet.ToString();
        }
    }
}
