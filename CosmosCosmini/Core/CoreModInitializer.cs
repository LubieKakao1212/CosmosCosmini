using CosmosCosmini.Base;
using JustLoaded.Core;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Loading;
using JustLoaded.Util;

namespace CosmosCosmini.Core;

using static CoreMod.Keys.Phase;

public class CoreModInitializer : IModInitializer {

    public void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        phases.New(ConstructFs, new ConstructFilesystemsPhase())
            .Register();
        
        phases.New(RegisterDb, new DatabaseRegistrationLoadingPhase())
            .WithOrder(ConstructFs, Order.After)
            .Register();

        phases.New(RegisterContentAuto, new RegisterContentLoadingPhase())
            .WithOrder(RegisterDb, Order.After)
            .Register();
    }
}