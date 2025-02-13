using JustLoaded.Content;
using JustLoaded.Core.Reflect;
using JustLoaded.Logger;

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
            public static readonly ContentKey ShipController = MakeKey("ship-controller");
        }

        public static class Databases { }
    }
}