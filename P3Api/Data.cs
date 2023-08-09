namespace P3Api;
#pragma warning disable CS8618
public class Town
{
    public Storage Storage { get; set; }
    public int[] DailyConsumptionsCitizens { get; set; }
    public Office[] Offices { get; set; }
};

public record Office
{
    public ushort MerchantId { get; set; }
    public Storage Storage { get; set; }
};


public record Storage
{
    public int[] Wares { get; set; }
    public int[] DailyConsumptionBusinesses { get; set; }
    public int[] DailyProduction { get; set; }
};
#pragma warning restore CS8618
