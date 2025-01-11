using Base.Def;
using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Reflect;

namespace Base.Mod;

[FromMod(BaseMod.ModId)]
public class BaseDatabaseRegisterer : IDatabaseRegisterer {
    
    public void RegisterDatabases(IDatabaseRegistrationContext context) {
        context.CreateDatabase<PlayerDef>(new ContentKey(BaseMod.ModId, "player-def"));
    }
    
}