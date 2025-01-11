using Base.Def;
using Base.Player;
using CosmosCosmini;
using Custom2d_Engine.Input;
using Custom2d_Engine.Scenes;
using JustLoaded.Content;
using JustLoaded.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Base.Systems;

public class TestPlayerSpawnerSystem(ModLoaderSystem modLoader) : IGameSystem {
    
    public void InitHierarchy(Hierarchy gameHierarchy) {
        var playerDef = modLoader.MasterDb.GetDatabase<PlayerDef>().GetContent<PlayerDef>(new ContentKey(BaseMod.ModId, "default"));
        var game = modLoader.GetRequiredAttachment<CosmosGame>();
        var input = game.Input;

        var ws = input.CreateSimpleAxisBinding("Linear", Keys.S, Keys.W);
        var ad = input.CreateSimpleAxisBinding("Radial", Keys.A, Keys.D);
        var space = input.GetKey(Keys.Space);
        
        gameHierarchy.AddObject(new PlayerObject(playerDef!, game.PhysicsWorld, ws, ad, space));
    }

    public void Update(GameTime gameTime, Hierarchy gameHierarchy) {
    }

    public void SaveState() {
    }

    public void LoadState() {
    }
    
}