using JustLoaded.Core;
using YamlDotNet.Serialization;

namespace CosmosCosmini.Core;

public interface IDeserializerSetupCallback {

    void SetupDeserializer(DeserializerBuilder builder, ModLoaderSystem modLoader);

}