using Base.Entities.Behaviors.Def;

namespace Base.Entities.Behaviors;

public class DeathBehaviour(DeathBehaviourDef def, Entity entity) : EntityBehavior<DeathBehaviourDef>(def, entity)
{
    public override void Construct()
    {
        base.Construct();
        HealthBehaviour? healthBehaviour = entity.GetOnlyBehavior<HealthBehaviour>();
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
        if (entity.CurrentHierarchy != null)
        {
            if (entity.Parent != null)
            {
                entity.Parent = null;
            }
            else
            {
                entity.CurrentHierarchy.RemoveObject(entity);
            }
        }
    }
}