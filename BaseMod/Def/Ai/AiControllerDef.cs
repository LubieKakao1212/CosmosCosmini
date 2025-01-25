using Base.Ship.Ai;
using CosmosCosmini.Core.Def;
using JustLoaded.Core;

namespace Base.Def;

public abstract class AiControllerDef : PolymorphicDef {

    public abstract AiController Instantiate(FactionAlignment alignment, ModLoaderSystem modLoader);

}