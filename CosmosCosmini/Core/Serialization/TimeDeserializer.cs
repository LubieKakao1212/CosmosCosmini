using System.Text.RegularExpressions;
using CosmosCosmini.Core.Math;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace CosmosCosmini.Core.Serialization;

public class TimeDeserializer : INodeDeserializer {

    public static readonly Regex Matcher = new("(\\d+)(\\/?)([a-z]+)");
    
    public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value,
        ObjectDeserializer rootDeserializer) {

        if (expectedType == typeof(TimeSpan)) {
            var scalar = reader.Consume<Scalar>();
            value = ParseTimeSpan(scalar.Value, out var isFrequency);
            if (isFrequency) {
                throw new YamlException(scalar.Start, scalar.End, "Frequency syntax not supported here");
            }
            return true;
        }
        else if (expectedType == typeof(TimeRate)) {
            var scalar = reader.Consume<Scalar>();
            var time = ParseTimeSpan(scalar.Value, out var isFrequency);
            value = new TimeRate(time);
            return true;
        }
        
        value = null;
        return false;
    }

    public static TimeSpan ParseTimeSpan(string str, out bool isFrequency) {
        var match = Matcher.Match(str);
        var number = int.Parse(match.Groups[1].Value);
        isFrequency = match.Groups[2].Value != "";
        var unit = match.Groups[3].Value;

        TimeSpan result = unit switch {
                "us" => TimeSpan.FromMicroseconds(number),
                "ms" => TimeSpan.FromMilliseconds(number),
                "s" => TimeSpan.FromSeconds(number),
                "m" => TimeSpan.FromMinutes(number),
                "h" => TimeSpan.FromHours(number),
            _ => throw new YamlException("Invalid unit for TimeSpan")
        };

        if (isFrequency) {
            result = TimeSpan.FromSeconds(1.0 / result.TotalSeconds);
        }

        return result;
    }
}