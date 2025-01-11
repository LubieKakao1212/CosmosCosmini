using Base.Def;
using CosmosCosmini.Scene;
using Custom2d_Engine.Input;
using Custom2d_Engine.Scenes.Factory;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace Base.Player;

public class PlayerObject : DefinedPhysicsObject {

    private IInput _thrustInput, _rotationInput, _breakTriggerInput;
    private PlayerDef _def;
    
    public PlayerObject(PlayerDef def, World world, IInput thrustInput, IInput rotationInput, IInput breakTriggerInput) : base(def.physics, world) {
        _thrustInput = thrustInput;
        _rotationInput = rotationInput;
        _breakTriggerInput = breakTriggerInput;
        _def = def;
        var display = new AnimatedDrawableObject(def.sprite.Value!) {
            Parent = this
        };
    }

    protected override void CustomUpdate(GameTime time) {
        base.CustomUpdate(time);

        var forward = Transform.Up;
        var thrust = _thrustInput.GetCurrentValue<float>();
        var angular = _rotationInput.GetCurrentValue<float>();
        
        PhysicsBody.ApplyLinearImpulse(forward * (float)(thrust * _def.maxThrust * time.ElapsedGameTime.TotalSeconds));
        PhysicsBody.ApplyTorque(-angular * _def.maxAngular);

        if (_breakTriggerInput.GetCurrentValue<bool>()) {
            var v = PhysicsBody.LinearVelocity;
            var mag = Math.Min(v.Length() * 64f, _def.maxBreaking);
            if (mag > 1f / 128f) {
                v.Normalize();
                PhysicsBody.ApplyForce(v * -mag);
            }
        }
    }
}