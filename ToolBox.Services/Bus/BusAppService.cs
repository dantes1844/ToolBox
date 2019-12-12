using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToolBox.Services.Bus.Dto;

namespace ToolBox.Services.Bus
{
    public class BusAppService : IBusAppService
    {
        public BusAppService() { }

        private readonly string urlFormat = "http://bus.wuhancloud.cn:9087/website//web/420100/line/027-{0}-{1}.do?Type=LineDetail";
        private readonly HttpClient _client = new HttpClient();

        private static Dictionary<string, int> BusList()
        {
            return new Dictionary<string, int>() {
                {"405",13 },{ "380",20},{ "789",29}
            };
        }

        public async Task<Dictionary<string, string>> GetBusListAsync()
        {
#if DEBUG

            Dictionary<string, string> busInfoDict = new Dictionary<string, string>()
            {
                { "723","1,2,3"},{ "596","2,3,4"},{"789","2,5" }
            };

            return busInfoDict;
#else
            
            Dictionary<string, string> busInfoDict = new Dictionary<string, string>();
            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach (var busInfo in BusList())
            {
                try
                {
                    var stationIndex = Convert.ToInt32(busInfo.Value);
                    //  Debug.WriteLine($"{DateTime.Now.Ticks},开始查询");
                    var busList = await BusInfoAsync(stationIndex, busInfo.Key, (int)BusDirection.ToCompany);
                    //  Debug.WriteLine($"{DateTime.Now.Ticks},查询结束");
                    busInfoDict[busInfo.Key] = string.Join(",", busList.OrderBy(c => c));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"查询{busInfo.Key}时出错:{ex}");
                    throw;
                }
            }
            stopwatch.Stop();
            Debug.WriteLine($"总共:{stopwatch.ElapsedMilliseconds}");
            return busInfoDict;
#endif

        }

        async Task<List<int>> BusInfoAsync(int indexOfBus, string busNumber, int direction)
        {
            var apiUrl = string.Format(urlFormat, busNumber, direction);
            var jsonStr = await _client.GetStringAsync(apiUrl);

            JsonSerializer jss = new JsonSerializer();
            var responseJson = jss.Deserialize<Dto.Bus>(new JsonTextReader(new StringReader(jsonStr)));

            List<int> busList = new List<int>();
            foreach (var bus in responseJson.data.buses)
            {
                var details = bus.Split('|');
                var distanceStation = Convert.ToInt32(details[2]);

                if (distanceStation >= indexOfBus)
                {
                    continue;
                }
                busList.Add(indexOfBus - distanceStation);
            }

            return busList;
        }
    }
}