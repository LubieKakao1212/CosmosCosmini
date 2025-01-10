using JustLoaded.Content;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace CosmosCosmini.Def;

public class ContentKeyDeserializer : INodeDeserializer {
    
    public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value,
        ObjectDeserializer rootDeserializer) {

        if (expectedType == typeof(ContentKey)) {
            if (reader.TryConsume<Scalar>(out var scalar)) {
                value = new ContentKey(scalar.Value);
                return true;
            }

            var helper = nestedObjectDeserializer(reader, typeof(HelperKey));
            
            if (helper != null) {
                var h = (HelperKey)helper;
                value = new ContentKey(h.mod, h.path);
                return true;
            }
        }
        value = null;
        return false;
    }

    private struct HelperKey() {
        public required string mod = "";
        public required string path = "";
    }
}