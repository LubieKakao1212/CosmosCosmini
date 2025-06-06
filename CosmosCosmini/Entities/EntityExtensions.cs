namespace CosmosCosmini.Entities;

public static class EntityExtensions {
    public static void Spawn(this Entity entity) {
        entity.Manager.SpawnEntity(entity);
    }

    public static void Despawn(this Entity entity) {
        entity.Manager.DespawnEntity(entity);
    }
    
    public static void DespawnAndReturn(this Entity entity) {
        //Cannot use entity.Despawn(); since there is an internal method with that name
        entity.Manager.DespawnEntity(entity);
        entity.Manager.ReturnEntity(entity);
    }
}