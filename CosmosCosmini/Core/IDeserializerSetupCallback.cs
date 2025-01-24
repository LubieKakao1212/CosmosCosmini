using JustLoaded.Core;
using YamlDotNet.Serialization;
using DeserializerBuilder = CosmosCosmini.Core.Phases.DeserializerBuilder;

namespace CosmosCosmini.Core;

public interface IDeserializerSetupCallback {

    void SetupDeserializer(DeserializerBuilder builder, ModLoaderSystem modLoader);

}