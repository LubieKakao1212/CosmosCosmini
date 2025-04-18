using CosmosCosmini.Entities.Def;

namespace CosmosCosmini.Entities;

public class EntityPool {
    
    public int FreeCount => _freeEntities.Count;

    private EntityDef _def;
    
    private readonly HashSet<Entity> _entities;
    private readonly List<Entity> _freeEntities;

    public EntityPool(int initialCapacity, EntityDef def) {
        if (!def.UsePool) {
            throw new ApplicationException("Cannot create a pool for non poolable entities");
        }
        
        _entities = new HashSet<Entity>(initialCapacity);
        _freeEntities = new List<Entity>(initialCapacity);
        
        _def = def;
    }
    
    public Entity GetOrCreate(Func<EntityDef, Entity> factory) {
        if (FreeCount > 0) {
            var ent = _freeEntities[^1];
            _freeEntities.RemoveAt(FreeCount - 1);
            
            return ent;
        }

        var entity = factory(_def);
        _entities.Add(entity);

        return entity;
    }

    public void Return(Entity entity) {
        if (!_entities.Contains(entity)) {
            throw new ApplicationException("Attempting to return an entity to a pool it did not originate from");
        }
        _freeEntities.Add(entity);
    }
    
}