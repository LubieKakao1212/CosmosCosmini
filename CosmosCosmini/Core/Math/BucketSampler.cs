using CosmosCosmini.Core.Math.Def;
using CosmosCosmini.Core.Serialization;

namespace CosmosCosmini.Core.Math;

public class BucketSampler(BucketSamplerDef def) : SamplerBase {

    private readonly List<float> _bucket = [..def.Values];
    
    public override void Reset() {
        _bucket.Clear();
        _bucket.AddRange(def.Values);
    }

    public override float Next() {
        var i = _random.Next(0, _bucket.Count);
        var v = _bucket[i];
        _bucket.RemoveAt(i);
        if (_bucket.Count == 0) {
            _bucket.AddRange(def.Values);
        }

        return v;
    }
}

[SubType(typeof(SamplerDef), "bucket")]
public class BucketSamplerDef : SamplerDef {

    public required float[] Values { get; init; }

    public override ISampler Instantiate() {
        return new BucketSampler(this);
    }
}