using CommunityToolkit.Mvvm.ComponentModel;
using P3Api;
using P3Manager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Manager.ViewModels
{
    public class HubsPageViewModel : ObservableObject
    {
        private readonly HubData[] hubData;

        public HubsPageViewModel()
        {
            var handle = new P3Handle();
            var vispy = handle.ReadTown();
            Console.WriteLine(vispy);
            this.hubData = new HubData[1]
            {
                GetHubData(TownId.Visby, new TownId[2] { TownId.Stockholm, TownId.Reval }),
            };
        }

        public HubData[] Hubs => this.hubData;

        private HubData GetHubData(TownId hubId, TownId[] satellites)
        {
            return new HubData(hubId, satellites.Select(s => GetTownStatistic(s)).ToArray());
        }

        private TownStatistic GetTownStatistic(TownId satelliteId)
        {
            return new TownStatistic(
                satelliteId,
                new WareStatistic[4]
                {
                    new WareStatistic(WareId.Grain, 1.5, 3.3, 2.5, 3.5, 4.5),
                    new WareStatistic(WareId.Meat, 1.5, 3.5, 5.5, 3.5, 4.5),
                    new WareStatistic(WareId.Fish, 1.5, 4.2, 2.5, 7.5, 4.5),
                    new WareStatistic(WareId.Beer, 1.5, 42.3, 2.5, 3.5, 4.5),
                });
        }
    }
}
