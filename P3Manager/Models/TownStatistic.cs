using P3Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Manager.Models
{
    public class TownStatistic
    {
        public TownStatistic(TownId satelliteId, WareStatistic[] wareStatistics)
        {
            this.SatelliteId = satelliteId;
            this.WareStatistics = wareStatistics;
        }

        public TownId SatelliteId { get; set; }

        public WareStatistic[] WareStatistics { get; set; }
    }
}
