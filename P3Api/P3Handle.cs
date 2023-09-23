using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace P3Api;

public unsafe class P3Handle
{
    private DotnetOpenProcessP3AccessApi* api;
    public P3Handle()
    {
        var processes = Process.GetProcessesByName("Patrician3");
        if (processes.Length == 0)
        {
            throw new InvalidOperationException("no patrician3 found");
        }
        // Debug.WriteLine($"Opening {processes[0].Id}");
        this.api = NativeMethods.new_api((uint)processes[0].Id);
        this.GameFolder = Path.GetDirectoryName(processes[0].MainModule!.FileName)!;

    }

    public string GameFolder { get; set; }

    public Town? ReadTown(TownId townId)
    {
        var town_ptr = NativeMethods.read_town(this.api, townId);
        if (town_ptr == null)
        {
            return null;
        }

        var town = new Town()
        {
            Id = townId,
            Storage = new Storage()
            {
                Wares = new Span<int>(town_ptr->storage.wares, 0x18).ToArray(),
                DailyConsumptionBusinesses = new Span<int>(town_ptr->storage.daily_consumption_businesses, 0x18).ToArray(),
                DailyProduction = new Span<int>(town_ptr->storage.daily_production, 0x18).ToArray()
            },
            DailyConsumptionsCitizens = new Span<int>(town_ptr->get_daily_consumptions_citizens, 0x18).ToArray(),
            Offices = town_ptr->offices.AsSpan<OfficeData>()
                .ToArray()
                .Select(e => new Office()
                {
                    MerchantId = e.merchant_id,
                    Storage = new Storage()
                    {
                        Wares = new Span<int>(e.storage.wares, 0x18).ToArray(),
                        DailyConsumptionBusinesses = new Span<int>(e.storage.daily_consumption_businesses, 0x18).ToArray(),
                        DailyProduction = new Span<int>(e.storage.daily_production, 0x18).ToArray(),
                    }
                }).ToArray(),
        };



        NativeMethods.free_town(town_ptr);
        return town;
    }
}

partial struct ByteBuffer
{
    public unsafe Span<byte> AsSpan()
    {
        return new Span<byte>(ptr, length);
    }

    public unsafe Span<T> AsSpan<T>()
    {
        return MemoryMarshal.CreateSpan(ref Unsafe.AsRef<T>(ptr), length / Unsafe.SizeOf<T>());
    }
}
