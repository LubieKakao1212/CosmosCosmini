using CosmosCosmini.Core.Math.Def;
using CosmosCosmini.Core.Serialization;

namespace CosmosCosmini.Core.Math;

public class ConstSampler(ConstSamplerDef def) : ISampler {
    
    public void Reset() {
        
    }

    public void SetSeed(int seed) {
        
    }

    public float Next() {
        return def.Value;
    }
}

[SubType(typeof(SamplerDef), "const")]
public class ConstSamplerDef : SamplerDef {

    public float Value { get; init; } = 0;

    public override ISampler Instantiate() {
        return new ConstSampler(this);
    }
}