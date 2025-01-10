using CosmosCosmini.Core.Serialization;
using CosmosCosmini.Def;
using JustLoaded.Core;
using JustLoaded.Core.Reflect;
using YamlDotNet.Serialization;

namespace CosmosCosmini.Core;

[FromMod(CoreMod.ModId)]
public class CodeDeserializerSetupCallback : IDeserializerSetupCallback {
    
    public void SetupDeserializer(DeserializerBuilder builder, ModLoaderSystem modLoader) {
        builder.WithEnforceNullability()
            .WithEnforceRequiredMembers()
            .WithNodeDeserializer(new ContentKeyDeserializer())
            .WithNodeDeserializer(new DatabaseReferenceDeserializer(modLoader.MasterDb));
    }
    
}