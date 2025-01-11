using System.Reflection;
using CosmosCosmini.Core;
using CosmosCosmini.JustLoadedEx;
using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Filesystem;

namespace CosmosCosmini;

public static class CoreMod {

    public const string ModId = "core";
    
    internal static Mod Construct(IFilesystem modDirFs) {
        var baseMod = new Mod(new ModMetadata.Builder(ModId).Build())
            .AddAssembly(Assembly.GetExecutingAssembly())
            .AddInitializer(new CoreModInitializer());

        var modLocalFs = new RelativeFilesystem(modDirFs, "core".AsPath());
        baseMod.GetGlobalObject<ModLocalFilesystemContainer>(typeof(ModLocalFilesystemContainer),  () => new ModLocalFilesystemContainer { ModLocalFileSystem = modLocalFs});

        return baseMod;
    }

    private static ContentKey CreateKey(string path) {
        return new ContentKey(ModId, path);
    }

    public static class Keys {
        public static class Phase {
            public static readonly ContentKey ConstructFs = CreateKey("construct-filesystem");
            public static readonly ContentKey ConstructDeserializer = CreateKey("construct-deserializer");
            public static readonly ContentKey RegisterDb = CreateKey("register-db");
            public static readonly ContentKey RegisterContentAuto = CreateKey("register-content-auto");
            public static readonly ContentKey LoadSprites = CreateKey("load-sprites");
            public static readonly ContentKey LoadAnimations = CreateKey("load-animations");
            
            public static readonly ContentKey AssetLoadEnd = CreateKey("asset-load-end");
            
            public static readonly ContentKey RegisterSystems = CreateKey("register-systems");
        }
        
        public static class Database {
            public static readonly ContentKey Sprites = CreateKey("sprites");
            public static readonly ContentKey GameSystems = CreateKey("systems");
            public static readonly ContentKey SpriteAnimations = CreateKey("animations");
        }
    }
}