using CosmosCosmini.Graphics;
using Custom2d_Engine.Rendering.Sprites;
using JustLoaded.Content.Database;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Reflect;

namespace CosmosCosmini.Core;

using static CoreMod.Keys.Database;

[FromMod(CoreMod.ModId)]
public class CoreDatabaseRegisterer : IDatabaseRegisterer {
    
    public void RegisterDatabases(IDatabaseRegistrationContext context) {
        context.CreateDatabase<Sprite>(Sprites);
        context.CreateDatabase<AnimatedSprite>(SpriteAnimations);
        context.RegisterDatabase(GameSystems, new ArrayDatabase<IGameSystem>());
    }
    
}