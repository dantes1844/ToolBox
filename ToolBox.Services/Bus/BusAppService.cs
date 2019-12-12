using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToolBox.Data;
using ToolBox.Services.Bus.Dto;

namespace ToolBox.Services.Bus
{
    public class BusAppService : IBusAppService
    {
        private BusStationMarkRepository repository = new BusStationMarkRepository();

        public BusAppService() { }

        private readonly string urlFormat = "http://bus.wuhancloud.cn:9087/website//web/420100/line/027-{0}-{1}.do?Type=LineDetail";
        private readonly HttpClient _client = new HttpClient();



        public async Task<Dictionary<string, string>> GetBusListAsync()
        {
            var busList = await repository.GetItemsAsync();
            Dictionary<string, string> busInfoDict = new Dictionary<string, string>();
            foreach (var busInfo in busList.Where(c => !c.IsDeleted))
            {
                try
                {
                    var stationIndex = Convert.ToInt32(busInfo.MarkStationNumber);

#if DEBUG
                    busInfoDict[busInfo.BusNumber] = "1,2,4";
#else
                    var dto = await BusInfoAsync(stationIndex, busInfo.BusNumber, (int)BusDirection.ToCompany);
                    busInfoDict[busInfo.BusNumber] = string.Join(",", dto.OrderBy(c => c));
#endif
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"查询{busInfo.BusNumber}时出错:{ex}");
                    throw;
                }
            }
            return busInfoDict;

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