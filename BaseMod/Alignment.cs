namespace Base;

public enum FactionAlignment {
    Friendly,
    Hostile,
    Obstacle
}

public static class AlignmentExtensions {

    public static FactionAlignment GetOpposite(this FactionAlignment thisAlignment) {
        if (thisAlignment == FactionAlignment.Friendly) {
            return FactionAlignment.Hostile;
        }
        if (thisAlignment == FactionAlignment.Hostile) {
            return FactionAlignment.Friendly;
        }
        
        return FactionAlignment.Obstacle;
    }
    
}
