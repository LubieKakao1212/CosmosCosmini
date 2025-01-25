using Base.Def;
using Custom2d_Engine.Input;
using Custom2d_Engine.Physics;
using Microsoft.Xna.Framework;

namespace Base.Ship.Ai;

public class PlayerAi(PlayerAiControllerDef def, FactionAlignment alignment, IInput thrustInput, IInput rotationInput, IInput weaponInput) : AiController<PlayerAiControllerDef>(def, alignment) {
    
    public override void DoAi(ShipObject controlledShip, ShipCollection ships, GameTime gameTime) {
        var shipDef = controlledShip.ShipDef;
        
        var forward = controlledShip.Transform.Up;
        var thrust = thrustInput.GetCurrentValue<float>();
        var angular = rotationInput.GetCurrentValue<float>();

        var pb = controlledShip.PhysicsBody;
        
        pb.ApplyForce(forward * (thrust * shipDef.MaxThrust));
        pb.ApplyTorque(-angular * shipDef.MaxAngular);
        foreach (var weapon in controlledShip.Weapons) {
            weapon.SetShooting(weaponInput.GetCurrentValue<bool>());
        }
    }
}