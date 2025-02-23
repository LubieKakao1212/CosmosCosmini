using Base.Def.Weapon;
using Base.Entities.Behaviors;
using CosmosCosmini;
using Custom2d_Engine.Physics;
using Custom2d_Engine.Ticking;
using Custom2d_Engine.Ticking.Actions;
using Microsoft.Xna.Framework;

namespace Base.Weapons;

public abstract class WeaponInstance(WeaponsBehavior ownerBehavior, AttachmentPoint attachmentPoint, TimeSpan cooldown) {

    protected readonly WeaponsBehavior _ownerBehavior = ownerBehavior;
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
            _activeTicker = _ownerBehavior.entity.AddAccurateRepeatingAction(DoShoot, cooldown);
        }
        else {
            _activeTicker = _ownerBehavior.entity.AddSimpleRepeatingAction(DoShoot, TimeSpan.FromMicroseconds(1));
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

public abstract class WeaponInstance<TDef>(TDef def, WeaponsBehavior ownerBehavior, AttachmentPoint attachmentPoint) : WeaponInstance(ownerBehavior, attachmentPoint, def.FireRate) where TDef : WeaponDef {
    public TDef Def { get; set; } = def;

    protected override void DoShoot() {
        DoShoot2(_ownerBehavior.entity.Transform.GlobalRotation + 
                 MathHelper.ToRadians(attachmentPoint.Def.Direction) + 
                 MathHelper.ToRadians(Def.Scatter.ScatterAngle()));
    }

    protected abstract void DoShoot2(float globalDirection);

}
