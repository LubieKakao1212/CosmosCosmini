using Base.Def;
using Custom2d_Engine.Input;
using Custom2d_Engine.Physics;
using Microsoft.Xna.Framework;

namespace Base.Ship.Ai;

public abstract class AiController(FactionAlignment alignment) {

    public FactionAlignment Alignment { get; } = alignment;
    
    public abstract void DoAi(ShipObject controlledShip, ShipCollection ships, GameTime gameTime);
    
}

public abstract class AiController<TDef>(TDef def, FactionAlignment alignment) : AiController(alignment) where TDef : AiControllerDef {
    protected TDef Def { get; } = def;
}