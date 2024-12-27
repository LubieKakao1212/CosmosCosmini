using CosmosCosmini.Core;
using Microsoft.Xna.Framework;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Utilities;
using YamlDotNet.Serialization.ValueDeserializers;

namespace CosmosCosmini.Def;

public class RectangleParser : YamlDotNet.Serialization.IValueDeserializer {
    
    public object? DeserializeValue(IParser parser, Type expectedType, SerializerState state,
        IValueDeserializer nestedObjectDeserializer) {
        if (expectedType != typeof(Rectangle)) {
            throw new YamlException($"Invalid type requested, expected {nameof(Rectangle)}");
        }

        throw new Exception("TODO");
    }
    
}