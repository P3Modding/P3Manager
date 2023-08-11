using P3Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Manager.Models;

public class HubWareModel : INotifyPropertyChanged
{
    public WareId Id { get; set; }

    public int Wares { get; set; }

    public int WeeklyTownProduction { get; set; }

    public int WeeklyTownCitizensConsumption { get; set; }

    public int WeeklyTownBusinessesConsumption { get; set; }

    public int WeeklyMerchantProduction { get; set; }

    public int WeeklyMerchantConsumption { get; set; }

    public int TotalProduction => this.WeeklyTownProduction + this.WeeklyMerchantProduction;

    public int TotalConsumption => this.WeeklyTownCitizensConsumption + this.WeeklyTownBusinessesConsumption + this.WeeklyMerchantConsumption;

    public event PropertyChangedEventHandler? PropertyChanged;

    public void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
