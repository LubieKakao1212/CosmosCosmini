using System.Runtime.Serialization;

namespace CosmosCosmini.Entities;

public class EntitySpawningFailedException : ApplicationException {

    public Entity Entity { get; }
    
    public EntitySpawningFailedException(string? message, Entity entity) : base(message) {
        Entity = entity;
    }

    public EntitySpawningFailedException(string? message, Exception? innerException, Entity entity) : base(message, innerException) {
        Entity = entity;
    }
}