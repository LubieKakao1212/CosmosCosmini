namespace CosmosCosmini.Core.Math;

public interface ISampler {
    
    void Reset();
    
    void SetSeed(int seed);
    
    float Next();
}