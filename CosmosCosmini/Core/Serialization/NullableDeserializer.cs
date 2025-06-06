using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace CosmosCosmini.Core.Serialization;

public class NullableDeserializer : INodeDeserializer {

    private static readonly Type TypeNullable = typeof(Nullable<>);
    
    public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value,
        ObjectDeserializer rootDeserializer) {

        if (expectedType.IsGenericType && expectedType.GetGenericTypeDefinition() == TypeNullable) {
            var arg = expectedType.GetGenericArguments()[0];

            if (reader.Accept<Scalar>(out Scalar? evnt) && evnt.Value.ToLower() == "null") {
                value = null; //Activator.CreateInstance(expectedType);
                return true;
            }
            var v = nestedObjectDeserializer(reader, arg);

            value = Activator.CreateInstance(expectedType, v);

            return true;
        }

        value = null;
        return false;
    }
    
}