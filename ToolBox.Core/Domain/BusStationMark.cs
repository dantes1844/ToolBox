using System;
using System.Collections.Generic;
using System.Text;

namespace ToolBox.Core.Domain
{
    public class BusStationMark : ISoftDelete, IHasCreationTime
    {
        public string Id { get; set; }

        public string BusNumber { get; set; }

        public int MarkStationNumber { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
