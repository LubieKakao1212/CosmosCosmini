using Base.Ship.Ai;
using CosmosCosmini.Core.Serialization;
using JustLoaded.Core;

namespace Base.Def.Ai;

[SubType(typeof(AiControllerDef), "target-follow")]
public class TargetFollowerAiDef : AiControllerDef {
    
    public override AiController Instantiate(FactionAlignment alignment, ModLoaderSystem modLoader) {
        return new TargetFollowerAi(this, alignment);
    }
    
}