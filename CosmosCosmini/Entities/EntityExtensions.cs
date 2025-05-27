namespace CosmosCosmini.Entities;

public static class EntityExtensions {
    public static void Spawn(this Entity entity) {
        entity.Manager.SpawnEntity(entity);
    }

    public static void Despawn(this Entity entity) {
        entity.Manager.DespawnEntity(entity);
    }
}