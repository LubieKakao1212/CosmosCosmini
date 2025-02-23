using Base.Def.Entities.Behaviors;

namespace Base.Entities.Behaviors;

public class DeathBehaviour(DeathBehaviourDef def, Entity entity) : EntityBehavior<DeathBehaviourDef>(def, entity)
{
    public override void Construct()
    {
        base.Construct();
        HealthBehaviour? healthBehaviour = _entity.GetOnlyBehavior<HealthBehaviour>();
        if (healthBehaviour == null)
        {
            throw new ApplicationException("Expected health behaviour in entity");
        }
        else
        {
            healthBehaviour.HealthDepleted += Death;
        }
    }

    private void Death()
    {
        if (_entity.CurrentHierarchy != null)
        {
            if (_entity.Parent != null)
            {
                _entity.Parent = null;
            }
            else
            {
                _entity.CurrentHierarchy.RemoveObject(_entity);
            }
        }
    }
}