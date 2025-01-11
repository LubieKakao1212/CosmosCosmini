using CosmosCosmini.Core.Def;
using CosmosCosmini.Graphics;
using Custom2d_Engine.Rendering.Sprites;
using JustLoaded.Content;

namespace Base.Def;

public class PlayerDef {

    public PhysicsDef physics = new PhysicsDef();

    public required float maxThrust = 0;
    public required float maxAngular = 0;
    public required float maxBreaking = 0;
    
    public required DatabaseReference<AnimatedSprite> sprite;
    
    // public required DatabaseReference<Sprite>[] refs;
}