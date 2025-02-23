using Base.Def.Entities;
using CosmosCosmini;
using Custom2d_Engine.Scenes;
using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Logger;
using Microsoft.Xna.Framework;

namespace Base.Systems;

public class PlayerSpawnSystem(ModLoaderSystem modLoader) : IGameSystem {
    
    public void InitHierarchy(Hierarchy gameHierarchy) {
        var mdb = modLoader.MasterDb;
        var playerDef = mdb.GetDatabase<EntityDef>().GetContent<EntityDef>(new ContentKey("base:player"));

        if (playerDef != null) {
            var game = modLoader.GetRequiredAttachment<CosmosGame>();
            gameHierarchy.AddObject(playerDef.Instantiate(game.PhysicsWorld));
        }
        else {
            modLoader.GetRequiredAttachment<ILogger>().Error("Could not find player entity def");
        }
    }

    public void Update(GameTime gameTime, Hierarchy gameHierarchy) { }

    public void SaveState() {
        throw new NotImplementedException();
    }

    public void LoadState() {
        throw new NotImplementedException();
    }
    
}