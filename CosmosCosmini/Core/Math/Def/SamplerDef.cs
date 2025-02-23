using CosmosCosmini.Core.Def;

namespace CosmosCosmini.Core.Math.Def;

public abstract class SamplerDef : PolymorphicDef {

    public abstract ISampler Instantiate();

}