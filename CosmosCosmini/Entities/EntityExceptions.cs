using System.Runtime.Serialization;

namespace CosmosCosmini.Entities;

public class EntityException : ApplicationException {
    public Entity Entity { get; }
    
    public EntityException(string? message, Entity entity) : base(message) {
        Entity = entity;
    }

    public EntityException(string? message, Exception? innerException, Entity entity) : base(message, innerException) {
        Entity = entity;
    }
}

public class EntitySpawningFailedException : EntityException {
    
    public EntitySpawningFailedException(string? message, Entity entity) : base(message, entity) { }

    public EntitySpawningFailedException(string? message, Exception? innerException, Entity entity) : base(message, innerException, entity) { }
}

public class CannotReparentEntityException : EntityException {
    public CannotReparentEntityException(string? message, Entity entity) : base(message, entity) {
    }

    public CannotReparentEntityException(string? message, Exception? innerException, Entity entity) : base(message, innerException, entity) {
    }
    
}