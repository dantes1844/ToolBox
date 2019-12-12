using System;
using ToolBox.Services.Bus;

namespace Services.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IBusAppService busAppService = new BusAppService();
            var result = busAppService.GetBusListAsync().Result;
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Key}:{item.Value}");
            }

            Console.WriteLine("Hello World!");

            Console.ReadKey();
        }
    }
}
