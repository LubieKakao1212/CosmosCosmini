using Microsoft.Xna.Framework;

namespace CosmosCosmini.Core.Def;

public struct Vector2Def() {

    public required float x = 0;
    public required float y = 0;

    public Vector2 Construct() {
        return new Vector2(x, y);
    }
    
}