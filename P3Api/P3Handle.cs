using CsBindgen;
namespace P3Api;

public unsafe class P3Handle
{
    private DotnetOpenProcessP3AccessApi* api;
    public P3Handle()
    {
        this.api = NativeMethods.new_api(26820);
    }

    public Town ReadTown()
    {
        var town_ptr = NativeMethods.read_town(this.api, 0x10);
        var town = new Town(
            new Span<int>(town_ptr->wares, 0x18).ToArray(),
            new Span<int>(town_ptr->daily_consumption_businesses, 0x18).ToArray(),
            new Span<int>(town_ptr->daily_production, 0x18).ToArray());
        //TODO free town_ptr
        return town;
    }
}
