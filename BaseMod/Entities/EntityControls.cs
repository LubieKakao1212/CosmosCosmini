using Custom2d_Engine.Input;
using Custom2d_Engine.Input.Binding;
using Microsoft.Xna.Framework.Input;
using YamlDotNet.Core.Tokens;

namespace Base.Entities;

public class EntityControls : IEntityControls {
    public IInput Move { get; }
    public IInput Rotate { get; }
    public IInput Boost { get; }
    public IInput MainAction { get; }
    public IInput SecondaryAction { get; }
    public IInput Modifier1 { get; }
    public IInput Modifier2 { get; }

    public EntityControls(InputManager inputManager) {
        var gamepad = inputManager.GetGamePad(0);
        Move = CombineKeyAndAxis(inputManager, Keys.S, Keys.W, gamepad.GetAnalogAxis(Side.Left, AnalogAxis.Vertical));
        Rotate = CombineKeyAndAxis(inputManager, Keys.D, Keys.A, 
            CombineAxisAndAxisNoClamp(inputManager, gamepad.GetTrigger(Side.Right).Negate(), gamepad.GetTrigger(Side.Left))
            );
        
        MainAction = inputManager.CreateSimpleBinding("", inputManager.GetMouse(MouseButton.Left), gamepad.GetButton(GamePadButton.Left));
    }

    private static IInput CombineKeyAndAxis(InputManager manager, Keys negative, Keys positive, ValueInputBase<float> axis) {
        return CombineAxisAndAxis(manager, manager.CreateSimpleAxisBinding("", negative, positive), axis);
    }
    
    private static IInput CombineAxisAndAxis(InputManager manager, ValueInputBase<float> axis1, ValueInputBase<float> axis2) {
        return CombineAxisAndAxisNoClamp(manager, axis1, axis2).Clamp(-1f, 1f);
    }

    private static ValueInputBase<float> CombineAxisAndAxisNoClamp(InputManager manager, ValueInputBase<float> axis1, ValueInputBase<float> axis2) {
        var combinedInput = new CompoundAxixBindingInput("").Bind(axis1).Bind(axis2);

        manager.RegisterBinding(combinedInput);

        return combinedInput;
    }
    
}