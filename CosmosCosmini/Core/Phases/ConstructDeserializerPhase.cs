using System.Reflection;
using CosmosCosmini.Core.Serialization;
using Custom2d_Engine.Util;
using JustLoaded.Core;
using JustLoaded.Core.Reflect;
using JustLoaded.Loading;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.BufferedDeserialization;
using YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators;
using YamlDotNet.Serialization.NamingConventions;

namespace CosmosCosmini.Core.Phases;

public class ConstructDeserializerPhase : EntrypointLoadingPhase<IDeserializerSetupCallback> {

    private readonly DeserializerBuilder _builder = new();

    private readonly List<INodeDeserializer> _deserializers = new();
    
    protected override void HandleEntrypointFor(Mod mod, IDeserializerSetupCallback entrypoint, ModLoaderSystem modLoader) { 
        entrypoint.SetupDeserializer(_builder, modLoader);
    }

    protected override void Finish(ModLoaderSystem modLoader) {
        Dictionary<Type, Dictionary<string, Type>> mappings = new();

        foreach (var (modId, type) in modLoader.GetAllModTypesByAttribute<SubTypeAttribute>()) {
            var attrib = type.GetCustomAttribute<SubTypeAttribute>()!;

            var mapping = mappings.GetOrSetToDefaultLazy(attrib.BaseType, t => new Dictionary<string, Type>());
            mapping.Add($"{modId}:{attrib.Key}", type);
        }

        //var discriminators = new List<ITypeDiscriminator>();
        
        foreach (var mapping in mappings) {
            _builder.AddDiscriminator(new KeyValueTypeDiscriminator(mapping.Key, "type", mapping.Value));
        }
        
        // _builder.WithNodeDeserializer(new TypeDiscriminatingNodeDeserializer(, discriminators, -1, -1));
        
        modLoader.AddAttachment<IDeserializer>(_builder.MakeDeserializer());
    }
}

public class DeserializerBuilder {

    private readonly List<INodeDeserializer> _deserializers = new();
    private readonly List<ITypeDiscriminator> _discriminators = new();
    
    public DeserializerBuilder AddDeserializer(INodeDeserializer deserializer) {
        _deserializers.Add(deserializer);
        return this;
    }

    public DeserializerBuilder AddDiscriminator(ITypeDiscriminator discriminator) {
        _discriminators.Add(discriminator);
        return this;
    }

    public IDeserializer MakeDeserializer() {
        var builder = new YamlDotNet.Serialization.DeserializerBuilder()
            .WithEnforceNullability()
            .WithEnforceRequiredMembers()
            .WithNamingConvention(CamelCaseNamingConvention.Instance);

        foreach (var nodeDeserializer in _deserializers) {
            builder.WithNodeDeserializer(nodeDeserializer);
        }

        builder.WithTypeDiscriminatingNodeDeserializer(o => {
        foreach (var discriminator in _discriminators) {
            o.AddTypeDiscriminator(discriminator);
        }});

        return builder.Build();
    }
    
}
