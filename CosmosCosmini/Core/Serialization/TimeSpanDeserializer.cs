using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace CosmosCosmini.Core.Serialization;

public class TimeSpanDeserializer : INodeDeserializer {
    
    public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value,
        ObjectDeserializer rootDeserializer) {

        if (expectedType == typeof(TimeSpan)) {
            var scalar = reader.Consume<Scalar>().Value;
            var unit = scalar[^1];
            var rawValue = scalar.Substring(0, scalar.Length - 1);
            var floatValue = double.Parse(rawValue, NumberStyles.Float, CultureInfo.InvariantCulture);

            switch (unit) {
                case 'u':
                    value = TimeSpan.FromMicroseconds(floatValue);
                    return true;
                case 'm':
                    value = TimeSpan.FromMilliseconds(floatValue);
                    return true;
                case 's':
                    value = TimeSpan.FromSeconds(floatValue);
                    return true;
                case 'M':
                    value = TimeSpan.FromMinutes(floatValue);
                    return true;
                case 'H':
                    value = TimeSpan.FromHours(floatValue);
                    return true;
                default:
                    throw new YamlException("Invalid unit for TimeSpan");
            }
        }
        value = null;
        return false;
    }
}