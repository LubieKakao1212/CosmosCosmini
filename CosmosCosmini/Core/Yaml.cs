using YamlDotNet.Serialization;

namespace CosmosCosmini.Core;

public class Yaml {

    public static IDeserializer Deserializer { get; } = new DeserializerBuilder()
        .WithEnforceRequiredMembers()
        .WithEnforceNullability()
        //TODO With value deserializers
        .Build();

}