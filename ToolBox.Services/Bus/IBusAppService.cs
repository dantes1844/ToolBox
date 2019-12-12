using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToolBox.Services.Bus
{
    public interface IBusAppService
    {
        Task<Dictionary<string, string>> GetBusListAsync();
    }
}