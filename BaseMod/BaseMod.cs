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
        
        public static class Phases {
            public static readonly ContentKey LoadPlayerDef = MakeKey("player-def");
        }
        
        // public static class Sprites {
        //     public static readonly ContentKey Player0 = MakeKey("player_0");
        // }
        
        public static class GameSystems {
            public static readonly ContentKey PlayerTest = MakeKey("test-player-spawner");
        }
    }
}