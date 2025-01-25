using Base.Def;
using Base.Def.Ai;
using Custom2d_Engine.Ticking;
using Microsoft.Xna.Framework;

namespace Base.Ship.Ai;

public class TargetFollowerAi(TargetFollowerAiDef def, FactionAlignment alignment) : AiController<TargetFollowerAiDef>(def, alignment) {

    private ShipObject? _target = null;

    private readonly TimeMachine _retargetTimer = new();
    
    public override void DoAi(ShipObject controlledShip, ShipCollection ships, GameTime gameTime) {
        _retargetTimer.Accumulate(gameTime.ElapsedGameTime);
        //TODO Move 1s to def
        if (_retargetTimer.TryRetrieve(TimeSpan.FromSeconds(1))) {
            _target = null;
        }
        
        var pos = controlledShip.Transform.GlobalPosition;
        
        _target ??= ships.GetNearest(alignment.GetOpposite(), pos);

        if (_target != null) {
            var targetPos = _target.Transform.GlobalPosition;

            var delta = targetPos - pos;
            delta.Normalize();

            var pb = controlledShip.PhysicsBody;
            
            controlledShip.PhysicsBody.LinearVelocity = delta;
            
            pb.Rotation = -MathF.Atan2(delta.X, delta.Y);
            
            // controlledShip.Transform.GlobalRotation = 
        }

    }
    
}