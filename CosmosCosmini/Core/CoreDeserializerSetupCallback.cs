using CosmosCosmini.Core.Serialization;
using JustLoaded.Core;
using JustLoaded.Core.Reflect;
using DeserializerBuilder = CosmosCosmini.Core.Phases.DeserializerBuilder;

namespace CosmosCosmini.Core;

[FromMod(CoreMod.ModId)]
public class CoreDeserializerSetupCallback : IDeserializerSetupCallback {
    
    public void SetupDeserializer(DeserializerBuilder builder, ModLoaderSystem modLoader) {
        builder
            .AddDeserializer(new ContentKeyDeserializer())
            .AddDeserializer(new DatabaseReferenceDeserializer(modLoader.MasterDb))
            .AddDeserializer(new TimeDeserializer());
    }
}