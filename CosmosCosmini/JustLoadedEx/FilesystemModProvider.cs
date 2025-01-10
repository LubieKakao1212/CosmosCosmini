using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Reflection.Emit;
using JustLoaded.Core;
using JustLoaded.Core.Discovery;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Reflect;
using JustLoaded.Filesystem;
using JustLoaded.Util;
using PathLib;
using YamlDotNet.Serialization;

namespace CosmosCosmini.JustLoadedEx;

public class FilesystemModProvider(IFilesystem rootFs) : IModProvider {

    private static IDeserializer metadataDeserializer = new DeserializerBuilder()
        .WithEnforceNullability()
        .WithEnforceRequiredMembers()
        .Build();
    
    public IEnumerable<Mod> DiscoverMods() {
        var mods = new List<Mod>();
        foreach (var modDir in rootFs.ListPaths(PathExtensions.Local.FromAnyMod())) {
            using var rawFile = SearchForMod(modDir);
            if (rawFile != null) {
                var localFilesystem = new RelativeFilesystem(rootFs, modDir.path);
                var mod = ConstructMod(rawFile, localFilesystem);
                mods.Add(mod);
                mod.GetGlobalObject<ModLocalFilesystemContainer>(typeof(ModLocalFilesystemContainer), () => new ModLocalFilesystemContainer() { ModLocalFileSystem = localFilesystem });
            }
        }
        return mods;
    }
    
    private Stream? SearchForMod(ModAssetPath modDir) {
        var path = modDir.path.Join("mod.yaml").FromAnyMod();
        return rootFs.OpenFile(path);
    }

    private Mod ConstructMod(in Stream metadataStream, IFilesystem localFilesystem) {
        var parsedMeta = ParseMetadata(metadataStream);
        var metadata = ConstructMetadata(parsedMeta);
        
        var mod = new Mod(metadata);
        foreach (var assembly in GetModAssemblies(parsedMeta, localFilesystem)) {
            mod.AddAssembly(assembly);
            var assemblyInitializers = assembly.GetModTypeByBase<IModInitializer>(metadata.ModKey.path);
            foreach (var initializerType in assemblyInitializers) {
                var modInitializer = (IModInitializer?)Activator.CreateInstance(initializerType);
                if (modInitializer != null) {
                    //Performs duplication check
                    mod.AddInitializer(modInitializer);
                }
            }
        }
        
        return mod;
    }
    
    private ParsedMetadata ParseMetadata(Stream rawFile) {
        var parsedMetadata = metadataDeserializer.Deserialize<ParsedMetadata>(new StreamReader(rawFile));
        return parsedMetadata;
    }

    private ModMetadata ConstructMetadata(in ParsedMetadata parsedMetadata) {
        var metaBuilder = new ModMetadata.Builder(parsedMetadata.id);

        foreach (var dep in parsedMetadata.dependencies) {
            if (dep.required) {
                metaBuilder.AddRequiredDependency(dep.order, dep.id);
            }
            else {
                metaBuilder.AddOptionalDependency(dep.order, dep.id);
            }
        }
        
        return metaBuilder.Build();
    }
    
    private IEnumerable<Assembly> GetModAssemblies(ParsedMetadata parsedMetadata, IFilesystem localFilesystem) {
        foreach (var assemblyPathRaw in parsedMetadata.assemblies) {
            var assemblyPath = assemblyPathRaw.AsPath();
            var assemblyModAssetPath = assemblyPath.FromAnyMod();

            using var assemblyStream = localFilesystem.OpenFile(assemblyModAssetPath);
            if (assemblyStream == null) {
                throw new ApplicationException("Invalid Assembly Path");
            }

            using var symbolsStream = localFilesystem.OpenFile(assemblyPath.WithExtension(".pdb").FromAnyMod());

            byte[]? symbols = null;

            if (symbolsStream != null) {
                var symbolsMemStream = new MemoryStream();
                symbolsStream.CopyTo(symbolsMemStream);
                symbols = symbolsMemStream.ToArray();
            }
            
            var assemblyMemStream = new MemoryStream();
            
            assemblyStream.CopyTo(assemblyMemStream);
            
            var assembly = Assembly.Load(assemblyMemStream.ToArray(), symbols);
            yield return assembly;
        }
    }
    
    public struct ParsedMetadata {
        public required string id = "";
        public List<ParsedDependency> dependencies = new();
        public List<string> assemblies = new();

        public ParsedMetadata() {
        }
    }
    
    public struct ParsedDependency {
        public required string id = "";
        public bool required = true;
        public Order order = Order.After;

        public ParsedDependency() { }
    }
}