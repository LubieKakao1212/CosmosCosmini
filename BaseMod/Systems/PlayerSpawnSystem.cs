using Base.Entities;
using CosmosCosmini;
using CosmosCosmini.Entities;
using CosmosCosmini.Scene;
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
            var game = modLoader.GetRequiredAttachment<CosmosGame>();
            _ = new FollowMeObject(game.GameCamera) {
                Parent = player
            };
            gameHierarchy.AddObject(player);
        }
        else {
            modLoader.GetRequiredAttachment<ILogger>().Error("Could not find player entity def");
        }
        
        var enemy = manager.CreateEntity(new ContentKey("base:enemy"), Vector2.One * 3f);

        if (enemy != null) {
            gameHierarchy.AddObject(enemy);
        }
        else {
            modLoader.GetRequiredAttachment<ILogger>().Error("Could not find enemy entity def");
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