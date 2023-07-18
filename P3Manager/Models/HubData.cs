using P3Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Manager.Models
{
    public class HubData
    {
        public HubData(TownId hubId, TownStatistic[] townStatistics)
        {
            this.HubId = hubId;
            this.TownStatistics = townStatistics;
        }

        public TownId HubId { get; set; }

        public TownStatistic[] TownStatistics { get; set; }

        public WareStatistic[] AccumulatedStatistics => this.TownStatistics
            .SelectMany(ts => ts.WareStatistics)
            .GroupBy(ws => ws.WareId)
            .Select(e => new WareStatistic(
                e.Key,
                e.Aggregate(0.0, (acc, i) => acc + i.WeeklyTownProduction),
                e.Aggregate(0.0, (acc, i) => acc + i.WeeklyTownCitizensConsumption),
                e.Aggregate(0.0, (acc, i) => acc + i.WeeklyTownBusinessesConsumption),
                e.Aggregate(0.0, (acc, i) => acc + i.WeeklyPlayerProduction),
                e.Aggregate(0.0, (acc, i) => acc + i.WeeklyPlayerConsumption))
            ).ToArray();
    }
}
