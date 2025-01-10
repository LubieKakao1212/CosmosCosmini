using BaseMod.Def;
using CosmosCosmini;
using CosmosCosmini.Core;
using JustLoaded.Core;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Loading;
using JustLoaded.Core.Reflect;
using JustLoaded.Filesystem;
using JustLoaded.Logger;
using JustLoaded.Util;
using YamlDotNet.Serialization;
using static BaseMod.BaseMod.Keys.Phases;

namespace BaseMod;

[FromMod(BaseMod.ModId)]
public class Initializer : IModInitializer {
    
    public void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        phases.New(LoadPlayerDef, new LoadPlayerPhase())
            .WithOrder(CoreMod.Keys.Phase.ConstructDeserializer, Order.After)
            .Register();
    }
}

public class LoadPlayerPhase : ILoadingPhase {
    public void Load(ModLoaderSystem modLoader) {
        var fs = modLoader.GetRequiredAttachment<ModFileSystem>().filesystem;
        var deserializer = modLoader.GetRequiredAttachment<IDeserializer>();
        var logger = modLoader.GetRequiredAttachment<ILogger>();
        
        var v = fs.DeserializeYamlFile<PlayerDef>("player/default.player.yaml".AsPath().FromMod("base"), deserializer);
        logger.Info("Done");
    }
}