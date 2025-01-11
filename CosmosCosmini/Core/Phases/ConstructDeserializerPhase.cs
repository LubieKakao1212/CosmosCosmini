using JustLoaded.Core;
using JustLoaded.Core.Loading;
using YamlDotNet.Serialization;

namespace CosmosCosmini.Core.Phases;

public class ConstructDeserializerPhase : EntrypointLoadingPhase<IDeserializerSetupCallback> {

    private readonly DeserializerBuilder _builder = new();
    
    protected override void HandleEntrypointFor(Mod mod, IDeserializerSetupCallback entrypoint, ModLoaderSystem modLoader) { 
        entrypoint.SetupDeserializer(_builder, modLoader);
    }

    protected override void Finish(ModLoaderSystem modLoader) {
        modLoader.AddAttachment<IDeserializer>(_builder.Build());
    }
}