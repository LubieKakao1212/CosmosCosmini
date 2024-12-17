using JustLoaded.Core;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Loading;
using JustLoaded.Core.Reflect;

namespace BaseMod;

[FromMod(ModId)]
public class BaseMod : IModInitializer {

    public const string ModId = "base";

    public void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        Console.WriteLine("Hello from base mod");
    }
}