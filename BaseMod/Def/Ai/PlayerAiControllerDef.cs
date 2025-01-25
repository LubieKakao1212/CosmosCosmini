using Base.Ship.Ai;
using CosmosCosmini;
using CosmosCosmini.Core.Serialization;
using Custom2d_Engine.Input;
using JustLoaded.Core;
using Microsoft.Xna.Framework.Input;

namespace Base.Def;

[SubType(typeof(AiControllerDef), "player")]
public class PlayerAiControllerDef : AiControllerDef {
    
    public override AiController Instantiate(FactionAlignment alignment, ModLoaderSystem modLoader) {
        var input = modLoader.GetRequiredAttachment<CosmosGame>().Input;
        
        var ws = input.CreateSimpleAxisBinding("Linear", Keys.S, Keys.W);
        var ad = input.CreateSimpleAxisBinding("Radial", Keys.A, Keys.D);
        var mouseLeft = input.GetMouse(MouseButton.Left);
        
        return new PlayerAi(this, alignment, ws, ad,mouseLeft);
    }
}