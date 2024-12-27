using Microsoft.Xna.Framework;

namespace CosmosCosmini.Core.Def;

public struct Point2Def {
    public required int x;
    public required int y;

    public Point Construct() {
        return new Point(x, y);
    }
}