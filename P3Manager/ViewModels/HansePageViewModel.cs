using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using P3Api;
using P3Manager.Models;
using P3Manager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace P3Manager.ViewModels;

public class HansePageViewModel : ObservableObject
{
    private volatile bool Running = true;

    public HansePageViewModel()
    {
        for (int i = 0; i < WareData.Length; i++)
        {
            WareData[i] = new HubWareModel();
            WareData[i].Id = (WareId)i;
        }
        var poll = this.Poll();
    }

    public HubWareModel[] WareData { get; set; } = new HubWareModel[20];

    public void Stop()
    {
        this.Running = false;
    }

    private async Task Poll()
    {
        while (Running)
        {
            var watch = Stopwatch.StartNew();
            try
            {
                this.Update();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex}");
            }
            finally
            {
                watch.Stop();
                await Task.Delay(1000);
            }
        }
        Debug.WriteLine("HansePageViewModel stopping");
    }

    private void Update()
    {
        for (int i = 0; i < 20; i++)
        {
            WareData[i].Id = (WareId)i;
            WareData[i].Wares = 0;
            WareData[i].WeeklyTownProduction = 0;
            WareData[i].WeeklyTownCitizensConsumption = 0;
            WareData[i].WeeklyTownBusinessesConsumption = 0;
            WareData[i].WeeklyMerchantProduction = 0;
            WareData[i].WeeklyMerchantConsumption = 0;
        }

        Town?[] newData = P3PollService.Data;
        foreach (var town in newData)
        {
            if (town == null) continue;
            for (int i = 0; i < 20; i++)
            {
                WareData[i].Wares += town.Storage.Wares[i] / NativeMethods.get_ware_scaling((WareId)i);
                WareData[i].WeeklyTownProduction += town.Storage.DailyProduction[i] * 7 / NativeMethods.get_ware_scaling((WareId)i);
                WareData[i].WeeklyTownCitizensConsumption += town.DailyConsumptionsCitizens[i] * 7 / NativeMethods.get_ware_scaling((WareId)i);
                WareData[i].WeeklyTownBusinessesConsumption += town.Storage.DailyConsumptionBusinesses[i] * 7 / NativeMethods.get_ware_scaling((WareId)i);
                WareData[i].WeeklyMerchantProduction += town.Offices.Sum(e => e.Storage.DailyProduction[i]) * 7 / NativeMethods.get_ware_scaling((WareId)i);
                WareData[i].WeeklyMerchantConsumption += town.Offices.Sum(e => e.Storage.DailyConsumptionBusinesses[i]) * 7 / NativeMethods.get_ware_scaling((WareId)i);
            }
        }
        for (int i = 0; i < 20; i++)
        {
            WareData[i].NotifyPropertyChanged(nameof(HubWareModel.Wares));
            WareData[i].NotifyPropertyChanged(nameof(HubWareModel.WeeklyTownProduction));
            WareData[i].NotifyPropertyChanged(nameof(HubWareModel.WeeklyTownCitizensConsumption));
            WareData[i].NotifyPropertyChanged(nameof(HubWareModel.WeeklyTownBusinessesConsumption));
            WareData[i].NotifyPropertyChanged(nameof(HubWareModel.WeeklyMerchantProduction));
            WareData[i].NotifyPropertyChanged(nameof(HubWareModel.WeeklyMerchantConsumption));
            WareData[i].NotifyPropertyChanged(nameof(HubWareModel.TotalProduction));
            WareData[i].NotifyPropertyChanged(nameof(HubWareModel.TotalConsumption));
        }
    }
}
