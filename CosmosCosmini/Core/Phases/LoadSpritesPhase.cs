using CosmosCosmini.Core.Def;
using Custom2d_Engine.Rendering.Sprites;
using JustLoaded.Core;
using JustLoaded.Core.Loading;
using JustLoaded.Filesystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CosmosCosmini.Core.Phases;

public class LoadSpritesPhase : ILoadingPhase {
    
    
    public void Load(ModLoaderSystem modLoader) {

        var spritesFs = modLoader.GetModDirectory(FileSystems.SpritesDir);

        var game = modLoader.GetRequiredAttachment<CosmosGame>();
        
        var graphics = game.GraphicsDevice;
        //TODO Dispose temporary textures
        var atlas = game.SpriteAtlas;

        var rawTextures = new List<Texture2D>();
        
        modLoader.LoadFilesMulti<ParsedSprite, Sprite>(spritesFs, "tex", (sprite, key, dir) => {
            var texAssetPath = dir.Join(sprite.source.AsPath()).FromMod(key.source);
            using var texStream = spritesFs.OpenRequiredFile(texAssetPath);
            
            var tex = Texture2D.FromStream(graphics, texStream);
            
            rawTextures.Add(tex);
            var sprites = atlas.AddTextureRects(tex, sprite.regions.Select(parsedRegion => new Rectangle(
                parsedRegion.position.Construct(), 
                parsedRegion.rect.Construct()
                )).ToArray());
            return sprites;
        }, (sprite, i) => i.ToString());
        
        atlas.Compact();
        
        foreach (var texture in rawTextures) {
            texture.Dispose();
        }
    }
    
    public struct ParsedSprite() {
        public required string source = "";
        public required ParsedRegion[] regions = [];
    }

    public struct ParsedRegion() {
        public required Point2Def rect = default;
        public Point2Def position = default;
    }
}