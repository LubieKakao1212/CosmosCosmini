using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Loading;
using JustLoaded.Core.Reflect;
using YamlDotNet.Serialization;

namespace BaseMod;

[FromMod(ModId)]
public class BaseMod {

    public const string ModId = "base";

    public static ContentKey MakeKey(string id) {
        return new ContentKey(ModId, id);
    }
    
    public static class Keys {
        
        public static class Phases {
            public static readonly ContentKey LoadPlayerDef = MakeKey("player-def");
        }
        
        public static class Sprites {
            public static readonly ContentKey Player0 = MakeKey("player_0");
        }
    }
}