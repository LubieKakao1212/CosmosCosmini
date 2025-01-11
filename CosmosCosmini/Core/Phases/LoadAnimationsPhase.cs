using CosmosCosmini.Graphics;
using Custom2d_Engine.Rendering.Sprites;
using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Core.Loading;

namespace CosmosCosmini.Core.Phases;

public class LoadAnimationsPhase : ILoadingPhase {
    
    public void Load(ModLoaderSystem modLoader) {
        var spritesFs = modLoader.GetModDirectory(FileSystems.SpritesDir);

        modLoader.LoadFilesMono<ParsedAnimation, AnimatedSprite>(spritesFs, "anim", (animation, key, dir) => 
            new AnimatedSprite(
                animation.sequence.frames.Select(frameId => new DatabaseReference<Sprite>(BoundContentKey<Sprite>.Make(new ContentKey(frameId)), modLoader.MasterDb)).ToArray(),
                TimeSpan.FromSeconds(animation.frameDuration)));
    }

    private struct ParsedAnimation() {
        public required double frameDuration = 0f;
        public required ScatteredFrames sequence = default;
    }

    private struct ScatteredFrames() {
        public required string[] frames = [];
    }

}