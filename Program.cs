using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HttpTransferPrototype
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {

            try
            {
                if (args.Length == 0)
                    throw new ArgumentException("File parameters expected");
                using (var client = new HttpClient())
                {
                    var g = Guid.NewGuid();
                    client.DefaultRequestHeaders.Add("abco_correlation_ID", g.ToString());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // combining to get date format
                    args[3] = String.Join(" ", args[3], args[4], args[5], args[6]);
                    var dateInUTC = TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse(args[3])).ToString();
                    var values = new Dictionary<string, string>
                    {
                        { "file_name", args[0]},
                        { "file_path", args[1]},
                        { "file_size", args[2]},
                        { "time_stamp", args[3]},
                        { "abco_correlation_ID", g.ToString()},
                        
                    };
                    var response = client.PostAsJsonAsync(ConfigurationManager.AppSettings.Get("destination"), values).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("File description received");
                        log.Info("File description received");
                    }
                    else
                    {
                        Console.WriteLine("Problem receiving file description");
                        log.Error("Problem receiving the file description");
                    }

                }
            }
            catch(ArgumentException e)
            {
                log.Error("Error in file description arguments", e);
            }
            catch (Exception e)
            {
                log.Error("Error sending file description", e);
            }

          }
     }  
}
