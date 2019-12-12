using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoubanJiang.Bus
{
    public interface IBusAppService
    {
        Task<Dictionary<string, string>> GetBusListAsync();
    }
}