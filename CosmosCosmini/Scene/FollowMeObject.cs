using Custom2d_Engine.Scenes;
using Microsoft.Xna.Framework;

namespace CosmosCosmini.Scene;

public class FollowMeObject : HierarchyObject {
    private readonly HierarchyObject _targetObject;
    
    public FollowMeObject(HierarchyObject targetObject) {
        _targetObject = targetObject;
        EnableUpdates = true;
    }

    protected override void CustomUpdate(GameTime time) {
        _targetObject.Transform.GlobalPosition = Transform.GlobalPosition;
    }
    
}