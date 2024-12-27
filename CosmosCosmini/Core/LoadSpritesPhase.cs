using CosmosCosmini.Core.Def;
using CosmosCosmini.Def;
using Custom2d_Engine.Rendering.Sprites;
using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Core.Loading;
using JustLoaded.Filesystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CosmosCosmini.Core;

public class LoadSpritesPhase : ILoadingPhase {
    
    
    public void Load(ModLoaderSystem modLoader) {

        var spritesFs = new RelativeFilesystem(CosmosGame.Filesystem, "sprites".AsPath());

        var spritesDb = modLoader.MasterDb.GetDatabase<Sprite>();
        
        var graphics = CosmosGame.Game.GraphicsDevice;
        //TODO Dispose temporary textures
        var atlas = CosmosGame.Game.SpriteAtlas;
        
        foreach (var spriteFile in 
                 spritesFs.ListFiles(
                     ".".AsPath().FromAnyMod(), 
                     "*.tex.yaml", true)) {
            var parsed = spritesFs.DeserializeYamlFile<ParsedSprite>(spriteFile);
            
            var texAssetPath = spriteFile.path.Directory.AsPath().Join(parsed.source.AsPath()).FromMod(spriteFile.modSelector);
            using var texStream = spritesFs.OpenRequiredFile(texAssetPath);

            var tex = Texture2D.FromStream(graphics, texStream);

            var sprites = atlas.AddTextureRects(tex, parsed.regions.Select(parsedRegion => new Rectangle(
                parsedRegion.position.Construct(), 
                parsedRegion.rect.Construct()
                )).ToArray());
            
            var idBase = spriteFile.path.BasenameWithoutExtensions;
            idBase = spriteFile.path.ToPosix();
            idBase = idBase.Substring(0, idBase.Length - ".tex.yaml".Length);
            
            for (int i=0; i<sprites.Length; i++) {
                spritesDb.AddContent(new ContentKey(spriteFile.modSelector, $"{idBase}_{i}"), sprites[i]);
            }
        }
        
        atlas.Compact();
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