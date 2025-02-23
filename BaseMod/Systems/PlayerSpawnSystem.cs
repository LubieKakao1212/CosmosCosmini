using Base.Def.Entities;
using Base.Entities;
using CosmosCosmini;
using Custom2d_Engine.Scenes;
using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Logger;
using JustLoaded.Util.Validation;
using Microsoft.Xna.Framework;

namespace Base.Systems;

public class PlayerSpawnSystem(ModLoaderSystem modLoader) : IGameSystem {
    
    public void InitHierarchy(Hierarchy gameHierarchy) {
        var mdb = modLoader.MasterDb;

        var manager = modLoader.GetRequiredAttachment<EntityManager>();
        
        var player = manager.CreateEntity(new ContentKey("base:player"), Vector2.Zero);

        if (player != null) {
            gameHierarchy.AddObject(player);
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