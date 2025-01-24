using JustLoaded.Core;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Loading;
using JustLoaded.Core.Reflect;

namespace Base.Mod;

[FromMod(BaseMod.ModId)]
public class BaseModInitializer : IModInitializer {
    
    public void SystemInit(JustLoaded.Core.Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
    }
    
}