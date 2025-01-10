using JustLoaded.Content;
using JustLoaded.Content.Database;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.BufferedDeserialization;

namespace CosmosCosmini.Core.Serialization;

public class DatabaseReferenceDeserializer(IReadOnlyMasterDatabase masterDatabase) : INodeDeserializer {

    private static readonly Type TypeDbRef = typeof(DatabaseReference<>);
    
    public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value,
        ObjectDeserializer rootDeserializer) {

        if (expectedType.IsGenericType && expectedType.GetGenericTypeDefinition() == TypeDbRef) {
            var contentType = expectedType.GetGenericArguments()[0];
            
            if (reader.TryConsume<Scalar>(out var scalar)) {
                value = ConstructDbRef(new ContentKey(scalar.Value), null, contentType);
                return true;
            }
            
            var buffer = new ParserBuffer(reader, 2, -1);

            //TODO Figure out how to not use try-catch
            try {
                var key = nestedObjectDeserializer(buffer, typeof(ContentKey));
                if (key != null) {
                    value = ConstructDbRef((ContentKey)key, null, contentType);
                    return true;
                }
            }
            catch (YamlException e) {
            }
            
            buffer.Reset();
            var helper = nestedObjectDeserializer(buffer, typeof(HelperDbRef));
            if (helper != null) {
                var helperDbRef = (HelperDbRef)helper;
                value = ConstructDbRef(helperDbRef.content, helperDbRef.db, contentType);
                return true;
            }
        }
        value = null;
        return false;
    }
    
    public struct HelperDbRef {
        public required ContentKey content;
        public required ContentKey db;
    }

    private object ConstructDbRef(ContentKey id, ContentKey? dbId, Type contentType) {
        return Activator.CreateInstance(TypeDbRef.MakeGenericType(contentType), id, dbId, masterDatabase) ?? throw new ApplicationException($"Error creating {typeof(DatabaseReference<>) }");
    }
    
}