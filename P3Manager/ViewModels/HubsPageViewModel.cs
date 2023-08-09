using CommunityToolkit.Mvvm.ComponentModel;
using P3Api;
using P3Manager.Models;
using P3Manager.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Gaming.Input;

namespace P3Manager.ViewModels
{
    public class HubsPageViewModel : ObservableObject
    {
        private readonly HubData[] hubData;
        private ObservableCollection<Town> towns;

        public HubsPageViewModel()
        {
            this.hubData = new HubData[2]
            {
                GetHubData(TownId.Visby, new TownId[5]
                {
                    TownId.Stockholm,
                    TownId.Reval,
                    TownId.Ladoga,
                    TownId.Novgorod,
                    TownId.Riga,
                }),
                GetHubData(TownId.Stettin, new TownId[7]
                {
                    TownId.Aalborg,
                    TownId.Luebeck,
                    TownId.Rostock,
                    TownId.Gdansk,
                    TownId.Torun,
                    TownId.Koenigsberg,
                    TownId.Malmoe,
                }),
            };

            Task.Run(async () =>
            {
                while (true)
                {
                    this.Towns = new ObservableCollection<Town>(P3PollService.Data.Values.Where(e => e != null)!);
                    await Task.Delay(100);
                }
            });
        }

        public HubData[] Hubs => this.hubData;

        private HubData GetHubData(TownId hubId, TownId[] satellites)
        {
            return new HubData(hubId, satellites.Select(s => GetTownStatistic(s)).Concat(new[] { GetTownStatistic(hubId) }).ToArray());
        }

        private TownStatistic GetTownStatistic(TownId satelliteId)
        {
            var handle = new P3Handle();
            var town = handle.ReadTown(satelliteId);
            var wareStatistics = new List<WareStatistic>();
            for (int i = 0; i < 20; i++)
            {
                wareStatistics.Add(
                    new WareStatistic(
                        (WareId)i,
                        town.Storage.DailyProduction[i] * 7 / NativeMethods.get_ware_scaling((WareId)i),
                        town.DailyConsumptionsCitizens[i] * 7 / NativeMethods.get_ware_scaling((WareId)i),
                        town.Storage.DailyConsumptionBusinesses[i] * 7 / NativeMethods.get_ware_scaling((WareId)i),
                        town.Offices.Sum(e => e.Storage.DailyProduction[i]) * 7 / NativeMethods.get_ware_scaling((WareId)i),
                        town.Offices.Sum(e => e.Storage.DailyConsumptionBusinesses[i]) * 7 / NativeMethods.get_ware_scaling((WareId)i)));
            }
            return new TownStatistic(
                satelliteId,
                wareStatistics.ToArray());
        }

        public ObservableCollection<Town> Towns
        {
            get { return this.towns; }
            set { this.towns = value; OnPropertyChanged(); }
        }
    }
}
