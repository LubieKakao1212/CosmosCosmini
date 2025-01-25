using CosmosCosmini.Scene;
using Custom2d_Engine.Physics;
using Custom2d_Engine.Scenes;
using Custom2d_Engine.Util;
using Microsoft.Xna.Framework;

namespace Base.Ship;

public class ShipCollection(Hierarchy hierarchy) {
    
    private Dictionary<FactionAlignment, HashSet<ShipObject>> _ships = new();

    public ShipObject? GetNearest(FactionAlignment targetAlignment, Vector2 point, float maxDistance = float.PositiveInfinity, ShipObject? excluded = null) {
        var set = _ships.GetOrSetToDefaultLazy(targetAlignment, _ => new HashSet<ShipObject>());
        set.RemoveWhere(o => o.CurrentHierarchy != hierarchy);

        var distanceSq = maxDistance * maxDistance;
        ShipObject? nearest = null;
        
        foreach (var obj in set) {
            var distSq = Vector2.DistanceSquared(obj.Transform.GlobalPosition, point);
            if (distSq < distanceSq) {
                nearest = obj;
                distanceSq = distSq;
            }
        }

        return nearest;
    }

    public DefinedPhysicsObject? GetAny(FactionAlignment targetAlignment) {
        var set = _ships.GetOrSetToDefaultLazy(targetAlignment, _ => new HashSet<ShipObject>());
        set.RemoveWhere(o => o.CurrentHierarchy != hierarchy);
        return set.FirstOrDefault();
    }

    public void Add(FactionAlignment targetAlignment, ShipObject ship) {
        var set = _ships.GetOrSetToDefaultLazy(targetAlignment, _ => new HashSet<ShipObject>());
        set.Add(ship);
        set.RemoveWhere(o => o.CurrentHierarchy != hierarchy);

    }
    
}