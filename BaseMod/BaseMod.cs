using JustLoaded.Content;
using JustLoaded.Core.Reflect;

namespace Base;

[FromMod(ModId)]
public class BaseMod {

    public const string ModId = "base";
    
    public static ContentKey MakeKey(string id) {
        return new ContentKey(ModId, id);
    }
    
    public static class Keys {
        
        public static class Phases { }
        
        public static class GameSystems {
            public static readonly ContentKey Background = MakeKey("background");
            public static readonly ContentKey SpawnPlayer = MakeKey("spawn-player");
        }

        public static class Databases { }
    }
}