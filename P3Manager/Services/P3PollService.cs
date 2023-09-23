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
    public static volatile Town?[] Data = new Town[40];
    public static volatile string? GameFolder;

    public P3PollService()
    {
        Task.Run(this.Run);
    }

    private async Task Run()
    {
        while (true)
        {
            try
            {
                var watch = Stopwatch.StartNew();
                var handle = new P3Handle();
                GameFolder = handle.GameFolder;
                foreach (var town in (TownId[])Enum.GetValues(typeof(TownId)))
                {
                    var townData = handle.ReadTown(town);
                    Data[(int)town] = townData;
                }
                watch.Stop();
                //Debug.WriteLine($"Towns fetched in {watch.ElapsedMilliseconds}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                await Task.Delay(1000);
            }
        }
    }
}
