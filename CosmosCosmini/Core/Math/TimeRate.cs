namespace CosmosCosmini.Core.Math;

public struct TimeRate {

    public TimeSpan TimeSpan { get; set; }

    public TimeRate(TimeSpan timeSpan) {
        TimeSpan = timeSpan;
    }
    
    public static implicit operator TimeSpan(in TimeRate timeRate) {
        return timeRate.TimeSpan;
    }
    
}