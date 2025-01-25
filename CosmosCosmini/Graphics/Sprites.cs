using CosmosCosmini;
using Custom2d_Engine.Rendering.Sprites;
using Custom2d_Engine.Rendering.Sprites.Atlas;
using Microsoft.Xna.Framework;

namespace Base.Constants;

public static class Sprites {
    
    public const float PPU = 16f;
    
    public static Vector2 GetWorldSize(this Sprite sprite) {
        var rect = sprite.TextureRect;
        var w = rect.width;
        var h = rect.height;
        
        return new Vector2(
            w.AtlasToPixels().PixelToWorld(), 
            h.AtlasToPixels().PixelToWorld());
    }

    public static float PixelToWorld(this float sizePixels) {
        return sizePixels / PPU;
    }
    
    private static float AtlasToPixels(this float atlasPos) {
        return atlasPos * CosmosGame.SpriteAtlasSize;
    }
}