using CosmosCosmini.Core.Serialization;

namespace CosmosCosmini.Core.Math.Def;

public abstract class MinMaxSamplerDef : SamplerDef {

    public float Min { get; init; } = float.NegativeInfinity;
    public float Max { get; init; } = float.PositiveInfinity;

}