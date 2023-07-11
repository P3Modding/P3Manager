using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Api
{
    public class WareStatistic
    {
        public WareStatistic(
            WareId wareId,
            double weeklyTownProduction,
            double weeklyTownCitizenConsumption,
            double weeklyTownBusinessesConsumption,
            double weeklyPlayerProduction,
            double weeklyPlayerConsumption)
        {
            this.WareId = wareId;
            this.WeeklyTownProduction = weeklyTownProduction;
            this.WeeklyTownCitizensConsumption = weeklyTownCitizenConsumption;
            this.WeeklyTownBusinessesConsumption = weeklyTownBusinessesConsumption;
            this.WeeklyPlayerProduction = weeklyPlayerProduction;
            this.WeeklyPlayerConsumption = weeklyPlayerConsumption;
        }

        public WareId WareId { get; set; }

        public double WeeklyTownProduction { get; set; }

        public double WeeklyTownCitizensConsumption { get; set; }

        public double WeeklyTownBusinessesConsumption { get; set; }

        public double WeeklyPlayerProduction { get; set; }

        public double WeeklyPlayerConsumption { get; set; }

        public double TotalProduction => this.WeeklyTownProduction + this.WeeklyPlayerProduction;

        public double TotalConsumption => this.WeeklyTownCitizensConsumption + this.WeeklyTownBusinessesConsumption + this.WeeklyPlayerConsumption;
    }
}
