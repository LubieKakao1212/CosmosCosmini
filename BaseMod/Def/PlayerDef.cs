using CosmosCosmini.Core.Def;
using CosmosCosmini.Def;
using CosmosCosmini.Graphics;
using Custom2d_Engine.Rendering.Sprites;
using JustLoaded.Content;
using YamlDotNet.Serialization;

namespace BaseMod.Def;

public class PlayerDef {

    public required PhysicsDef physics = new PhysicsDef();

    public required float maxThrust = 1;
    public required float maxAngular = 1;
    
    public required DatabaseReference<AnimatedSprite> sprite;
    
    public required DatabaseReference<Sprite>[] refs;
}