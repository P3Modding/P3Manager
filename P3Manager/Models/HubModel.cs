using P3Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomlyn.Model;

namespace P3Manager.Models;

public class HubModel : INotifyPropertyChanged
{
    public HubModel(TownId hub, HubWareModel[] data)
    {
        this.Town = hub;
        this.WareData = data;
    }

    public TownId Town { get; set; }

    public HubWareModel[] WareData { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
