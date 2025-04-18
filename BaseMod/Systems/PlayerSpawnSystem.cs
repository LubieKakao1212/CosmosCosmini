using CosmosCosmini;
using CosmosCosmini.Entities;
using CosmosCosmini.Scene;
using Custom2d_Engine.Scenes;
using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Logger;
using Microsoft.Xna.Framework;

namespace Base.Systems;

public class PlayerSpawnSystem(ModLoaderSystem modLoader) : IGameSystem {
    
    public void InitHierarchy(Hierarchy gameHierarchy) {
        var mdb = modLoader.MasterDb;

        var manager = modLoader.GetRequiredAttachment<EntityManager>();
        
        var player = manager.MakeEntity(new ContentKey("base:player"));
        
        if (player != null) {
            player.Transform.GlobalPosition = Vector2.Zero;
            var game = modLoader.GetRequiredAttachment<CosmosGame>();
            _ = new FollowMeObject(game.GameCamera) {
                Parent = player
            };
            manager.SpawnEntity(player);
            // gameHierarchy.AddObject(player);
        }
        else {
            modLoader.GetRequiredAttachment<ILogger>().Error("Could not find player entity def");
        }
        
        var enemy = manager.MakeEntity(new ContentKey("base:enemy"));

        if (enemy != null) {
            enemy.Transform.GlobalPosition = Vector2.One * 3f;
            manager.SpawnEntity(enemy);
            // gameHierarchy.AddObject(enemy);
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