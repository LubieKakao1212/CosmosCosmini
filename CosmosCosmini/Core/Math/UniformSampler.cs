using CosmosCosmini.Core.Math.Def;
using CosmosCosmini.Core.Serialization;
using Microsoft.Xna.Framework;

namespace CosmosCosmini.Core.Math;

public class UniformSampler(UniformSamplerDef def) : SamplerBase {
    
    public override void Reset() { }

    public override float Next() {
        return MathHelper.Lerp(def.Min, def.Max, _random.NextSingle());
    }
}

[SubType(typeof(SamplerDef), "uniform")]
public class UniformSamplerDef : MinMaxSamplerDef {
    
    public override ISampler Instantiate() {
        return new UniformSampler(this);
    }
}