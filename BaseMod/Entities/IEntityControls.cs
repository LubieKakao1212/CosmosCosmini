using Custom2d_Engine.Input;

namespace Base.Entities;

public interface IEntityControls {
    
    public IInput Move { get; }
    public IInput Rotate { get; }

    public IInput Boost { get; }

    public IInput MainAction { get; }
    
    public IInput SecondaryAction { get; }
    
    public IInput Modifier1 { get; }
    public IInput Modifier2 { get; }
    
}