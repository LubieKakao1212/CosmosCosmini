using Base.Def.Weapon;
using CosmosCosmini;
using Custom2d_Engine.Physics;
using Custom2d_Engine.Ticking;
using Custom2d_Engine.Ticking.Actions;
using Microsoft.Xna.Framework;

namespace Base.Weapons;

public abstract class WeaponInstance(PhysicsBodyObject ownerObject, AttachmentPoint attachmentPoint, TimeSpan cooldown) {

    protected readonly PhysicsBodyObject _ownerObject = ownerObject;
    protected TickMachineBase? _activeTicker;
    protected bool _active;

    public void SetShooting(bool active) {
        if (!_active && active) {
            StartShooting();
        }
        else if (_active && !active) {
            StopShooting();
        }
        _active = active;
    }
    
    public virtual void StartShooting() {
        if (cooldown > TimeSpan.Zero) {
            _activeTicker = _ownerObject.AddAccurateRepeatingAction(DoShoot, cooldown);
        }
        else {
            _activeTicker = _ownerObject.AddSimpleRepeatingAction(DoShoot, TimeSpan.FromMicroseconds(1));
        }
    }
    
    protected abstract void DoShoot();

    public virtual void StopShooting() {
        if (_activeTicker != null) {
            _activeTicker.Dispose();
        }
        else {
            CosmosGame.Logger.Warning($"Weapon {GetType().Name} experienced unscheduled shutdown but it was never active");
        }
    }
    
    public virtual void Hit() { }
}

public abstract class WeaponInstance<TDef>(TDef def, PhysicsBodyObject ownerObject, AttachmentPoint attachmentPoint) : WeaponInstance(ownerObject, attachmentPoint, TimeSpan.FromSeconds(1f / def.Rps)) where TDef : WeaponDef {
    public TDef Def { get; set; } = def;

    protected override void DoShoot() {
        DoShoot2(_ownerObject.Transform.GlobalRotation + 
                 MathHelper.ToRadians(attachmentPoint.Def.Direction) + 
                 MathHelper.ToRadians(Def.Scatter.ScatterAngle()));
    }

    protected abstract void DoShoot2(float globalDirection);

}
