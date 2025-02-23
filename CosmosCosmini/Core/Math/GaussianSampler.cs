using CosmosCosmini.Core.Math.Def;
using CosmosCosmini.Core.Serialization;

namespace CosmosCosmini.Core.Math;

public class GaussianSampler(GaussianSamplerDef def) : SamplerBase {
    private float value;
    private bool hasValue = false;
    
    public override void Reset() {
        hasValue = false;
    }

    public override float Next() {
        float v;
        while ((v = NextValue()) > def.Max || v < def.Min) { }
        return v;
    }

    private float NextValue() {
        if (hasValue) {
            hasValue = false;
            return value;
        }
        
        var random = Random.Shared;
        // The method requires sampling from a uniform random of (0,1]
        // but Random.NextDouble() returns a sample of [0,1).
        var x1 = random.NextSingle();
        var x2 = random.NextSingle();

        var l1 = MathF.Sqrt(-2f * MathF.Log(x1));
            
        var y1 = l1 * MathF.Cos(2f * MathF.PI * x2);
            
        value = l1 * MathF.Sin(2f * MathF.PI * x2);
        value *= def.StdDev + def.Mean;
        return y1 * def.StdDev + def.Mean;
    }
    
}

[SubType(typeof(SamplerDef), "gaussian")]
public class GaussianSamplerDef : MinMaxSamplerDef {

    public required float StdDev { get; init; }
    public float Mean { get; init; } = 0;

    public override ISampler Instantiate() {
        return new GaussianSampler(this);
    }
    
}