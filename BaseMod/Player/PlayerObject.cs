using Base.Constants;
using Base.Def;
using Base.Weapons;
using CosmosCosmini.Scene;
using Custom2d_Engine.Input;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace Base.Player;

public class PlayerObject : DefinedPhysicsObject {
    private readonly IInput _thrustInput;
    private readonly IInput _rotationInput;
    private readonly IInput _breakTriggerInput;
    private readonly IInput _weaponInput;
    private ShipDef _def;

    private WeaponInstance[] _weapons;
    
    public PlayerObject(ShipDef def, World world, IInput thrustInput, IInput rotationInput, IInput breakTriggerInput, IInput weaponInput) : base(def.Physics, world) {
        _thrustInput = thrustInput;
        _rotationInput = rotationInput;
        _breakTriggerInput = breakTriggerInput;
        _weaponInput = weaponInput;
        _def = def;
        _weapons = def.Weapons.Select(def => def.InstantiateWeapon(this)).ToArray();

        foreach (var fixture in PhysicsBody.FixtureList) {
            fixture.CollisionCategories = Collisions.Cats.Player;
            fixture.CollidesWith = Collisions.CollidesWith.Player;
        }
        
        var display = new AnimatedDrawableObject(def.Sprite.Value!) {
            Parent = this
        };
    }

    protected override void CustomUpdate(GameTime time) {
        base.CustomUpdate(time);
        
        var forward = Transform.Up;
        var thrust = _thrustInput.GetCurrentValue<float>();
        var angular = _rotationInput.GetCurrentValue<float>();
        
        PhysicsBody.ApplyLinearImpulse(forward * (float)(thrust * _def.MaxThrust * time.ElapsedGameTime.TotalSeconds));
        PhysicsBody.ApplyTorque(-angular * _def.MaxAngular);
        
        if (_breakTriggerInput.GetCurrentValue<bool>()) {
            var v = PhysicsBody.LinearVelocity;
            var mag = Math.Min(v.Length() * 64f, _def.MaxBreaking);
            if (mag > 1f / 128f) {
                v.Normalize();
                PhysicsBody.ApplyForce(v * -mag);
            }
        }
    }

    protected void ActivateWeapons(IInput input) {
        SetWeapons(weapon => weapon.StartShooting());
    }

    protected void DeactivateWeapons(IInput input) {
        SetWeapons(weapon => weapon.StopShooting());
    }

    protected void SetWeapons(Action<WeaponInstance> action) {
        foreach (var weapon in _weapons) {
            action(weapon);
        }
    }

    protected override void AddedToScene() {
        _weaponInput.Started += ActivateWeapons;
        _weaponInput.Canceled += DeactivateWeapons;
    }

    public override void RemovedFromScene() {
        _weaponInput.Started -= ActivateWeapons;
        _weaponInput.Canceled -= DeactivateWeapons;
    }
}