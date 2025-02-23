using Base.Def.Weapon;
using Base.Entities.Behaviors;
using CosmosCosmini;
using CosmosCosmini.Core.Math;
using Custom2d_Engine.Ticking;
using Custom2d_Engine.Ticking.Actions;
using Microsoft.Xna.Framework;

namespace Base.Weapons;

public abstract class WeaponInstance(WeaponsBehavior ownerBehavior, AttachmentPoint attachmentPoint, TimeSpan cooldown) {

    public AttachmentPoint AttachmentPoint { get; } = attachmentPoint;

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
            _activeTicker = _ownerBehavior.entity.AddAccurateRepeatingAction(DoShoot, cooldown, cooldown);
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

    protected ISampler scatter = def.Scatter.Instantiate();
    protected ISampler shotsPerBurst = def.ShotsPerBurst.Instantiate();
    
    protected override void DoShoot() {
        var shotCount = shotsPerBurst.Next();
        float[] angles = new float[(int)shotCount];
        for (int i = 0; i < shotCount; i++) {
            var angleExtra = MathHelper.ToRadians(AttachmentPoint.Def.Direction) +
                             MathHelper.ToRadians(scatter.Next());
            DoShoot2(_ownerBehavior.entity.Transform.GlobalRotation + angleExtra);

            angles[i] = angleExtra;
        }

        for (int i = 0; i < shotCount; i++) {
            var pb = ownerBehavior.entity.PhysicsBody;
            var up = ownerBehavior.entity.Transform.Up;
            up.Rotate(angles[i]);

            pb.ApplyLinearImpulse(up * -def.Recoil);
        }
    }

    protected abstract void DoShoot2(float globalDirection);

}
