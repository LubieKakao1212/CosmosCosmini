namespace CosmosCosmini.Core.Math;

public abstract class SamplerBase : ISampler {

    protected Random _random = new Random();
    
    public abstract void Reset();

    public virtual void SetSeed(int seed) {
        _random = new Random(seed);
    }
    
    public abstract float Next();
    
}