using CommunityToolkit.Mvvm.ComponentModel;
using P3Api;
using P3Manager.Models;
using P3Manager.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tomlyn;
using Tomlyn.Model;
using Windows.Gaming.Input;

namespace P3Manager.ViewModels;

public class HubsPageViewModel : ObservableObject
{
    private volatile bool Running = true;

    public HubsPageViewModel()
    {
        var poll = this.Poll();
    }

    public ObservableCollection<HubModel> Hubs { get; set; } = new ObservableCollection<HubModel>();

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
                var config = Toml.ToModel(File.ReadAllText(P3PollService.GameFolder + @"\P3.toml"));
                Town?[] newData = P3PollService.Data;
                var p3ManagerConfig = (TomlTable)config["P3Manager"];
                var hubsConfig = (TomlTableArray)p3ManagerConfig["hubs"];
                int hubsIndex = 0;

                // Iterate over all configured hubs, update if already present, insert if not, remove if stale
                foreach (TomlTable configuredHub in hubsConfig)
                {
                    var hub = Enum.Parse<TownId>((string)configuredHub["town"]);
                    if (hubsIndex < this.Hubs.Count)
                    {
                        var oldHub = this.Hubs[hubsIndex];
                        if (hub == oldHub.Town)
                        {
                            // Hub is at expected position, we have to update it
                            UpdateHubWareModels(oldHub.WareData, hub, newData, configuredHub);
                        }
                        else
                        {
                            // Another hub is at the expected position
                            var hubWareModels = new HubWareModel[20];
                            for (int i = 0; i < hubWareModels.Length; i++)
                            {
                                hubWareModels[i] = new HubWareModel();
                            }
                            UpdateHubWareModels(hubWareModels, hub, newData, configuredHub);
                            this.Hubs.Insert(hubsIndex, new HubModel(hub, hubWareModels));
                        }
                    }
                    else
                    {
                        // Hub is new
                        var hubWareModels = new HubWareModel[20];
                        for (int i = 0; i < hubWareModels.Length; i++)
                        {
                            hubWareModels[i] = new HubWareModel();
                        }
                        UpdateHubWareModels(hubWareModels, hub, newData, configuredHub);
                        this.Hubs.Add(new HubModel(hub, hubWareModels));
                    }
                    hubsIndex += 1;
                }
                for (; hubsIndex < this.Hubs.Count; hubsIndex++)
                {
                    this.Hubs.RemoveAt(hubsIndex);
                }
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
        Debug.WriteLine("HubsPageViewModel stopping");
    }

    private void UpdateHubWareModels(HubWareModel[] models, TownId hub, Town?[] data, TomlTable configuration)
    {
        var satellites = (TomlTableArray)configuration["satellites"];
        for (int i = 0; i < 20; i++)
        {
            models[i].Id = (WareId)i;
            models[i].Wares = 0;
            models[i].WeeklyTownProduction = 0;
            models[i].WeeklyTownCitizensConsumption = 0;
            models[i].WeeklyTownBusinessesConsumption = 0;
            models[i].WeeklyMerchantProduction = 0;
            models[i].WeeklyMerchantConsumption = 0;
        }
        foreach (var town in data)
        {
            if (town == null) continue;
            if (hub == town.Id || satellites.Any(e => (string)e["town"] == town.Id.ToString()))
            {
                for (int i = 0; i < 20; i++)
                {
                    models[i].Wares += town.Storage.Wares[i] / NativeMethods.get_ware_scaling((WareId)i);
                    models[i].WeeklyTownProduction += town.Storage.DailyProduction[i] * 7 / NativeMethods.get_ware_scaling((WareId)i);
                    models[i].WeeklyTownCitizensConsumption += town.DailyConsumptionsCitizens[i] * 7 / NativeMethods.get_ware_scaling((WareId)i);
                    models[i].WeeklyTownBusinessesConsumption += town.Storage.DailyConsumptionBusinesses[i] * 7 / NativeMethods.get_ware_scaling((WareId)i);
                    models[i].WeeklyMerchantProduction += town.Offices.Sum(e => e.Storage.DailyProduction[i]) * 7 / NativeMethods.get_ware_scaling((WareId)i);
                    models[i].WeeklyMerchantConsumption += town.Offices.Sum(e => e.Storage.DailyConsumptionBusinesses[i]) * 7 / NativeMethods.get_ware_scaling((WareId)i);
                }
            }
        }
        for (int i = 0; i < 20; i++)
        {
            models[i].NotifyPropertyChanged(nameof(HubWareModel.Wares));
            models[i].NotifyPropertyChanged(nameof(HubWareModel.WeeklyTownProduction));
            models[i].NotifyPropertyChanged(nameof(HubWareModel.WeeklyTownCitizensConsumption));
            models[i].NotifyPropertyChanged(nameof(HubWareModel.WeeklyTownBusinessesConsumption));
            models[i].NotifyPropertyChanged(nameof(HubWareModel.WeeklyMerchantProduction));
            models[i].NotifyPropertyChanged(nameof(HubWareModel.WeeklyMerchantConsumption));
            models[i].NotifyPropertyChanged(nameof(HubWareModel.TotalProduction));
            models[i].NotifyPropertyChanged(nameof(HubWareModel.TotalConsumption));
        }
    }
}
