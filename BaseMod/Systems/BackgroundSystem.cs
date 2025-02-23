using CosmosCosmini;
using CosmosCosmini.Graphics;
using CosmosCosmini.Scene;
using Custom2d_Engine.Scenes;
using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Logger;
using Microsoft.Xna.Framework;

namespace Base.Systems;

public class BackgroundSystem(ModLoaderSystem modLoader) : IGameSystem {

    private static Random random = new Random(1337);
    
    public void InitHierarchy(Hierarchy gameHierarchy) {
        const float xRadius = 32f;
        const float yRadius = 32f;
        
        var sprite = modLoader.MasterDb.GetDatabase<AnimatedSprite>().GetContent<AnimatedSprite>(new ContentKey("base:bg-star"));
        
        if (sprite == null) {
            modLoader.GetRequiredAttachment<ILogger>().Error("No background star sprite");
            return;
        }

        var frameCount = sprite.FrameCount;
        var stars = new HierarchyObject();
        
        for (int i = 0; i < 1024; i++) {
            var x = random.NextSingle() * 2f - 1f;
            var y = random.NextSingle() * 2f - 1f;

            x *= xRadius;
            y *= yRadius;

            _ = new AnimatedDrawableObject(sprite, phase: random.Next(0, frameCount)) {
                Parent = stars,
                Transform = {
                    GlobalPosition = new Vector2(x, y)
                }
            };
        }
        
        gameHierarchy.AddObject(stars);
    }

    public void Update(GameTime gameTime, Hierarchy gameHierarchy) { }

    public void SaveState() {
        throw new NotImplementedException();
    }

    public void LoadState() {
        throw new NotImplementedException();
    }
}