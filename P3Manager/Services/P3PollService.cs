using P3Api;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Manager.Services;

public class P3PollService
{
    public static readonly ConcurrentDictionary<TownId, Town?> Data = new();

    public P3PollService()
    {
        Task.Run(this.Run);
    }

    private async Task Run()
    {
        while (true)
        {
            var watch = Stopwatch.StartNew();
            var handle = new P3Handle();
            foreach (var town in (TownId[]) Enum.GetValues(typeof(TownId)))
            {
                var townData = handle.ReadTown(town);
                Data.AddOrUpdate(town, townData, (k, v) => townData);
            }
            watch.Stop();
            Debug.WriteLine($"Towns fetched in {watch.ElapsedMilliseconds}");
            await Task.Delay(1000);
        }
    }
}
