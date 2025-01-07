using CosmosCosmini.Graphics;
using Custom2d_Engine.Scenes;
using Custom2d_Engine.Scenes.Factory;
using Microsoft.Xna.Framework;

namespace CosmosCosmini.Scene;

public class AnimatedDrawableObject : HierarchyObject {

    public AnimatedSprite? Sprite {
        get => _sprite;
        set {
            if (_sprite != null) {
                _sprite.FrameChanged -= OnFrameChanged;
            }

            _sprite = value;
            
            if (_sprite != null) {
                _sprite.FrameChanged += OnFrameChanged;
            }
            
            UpdateSprite();
        }
    }

    public int AnimationOffset {
        get => _animationOffset;
        set {
            _animationOffset = value;
            UpdateSprite();
        }
    }

    private int _animationOffset;
    private AnimatedSprite? _sprite;
    private DrawableObject _drawableChild;
    private Color _tint;
    
    public AnimatedDrawableObject(AnimatedSprite sprite, Color? tint = null, int phase = 0) {
        _drawableChild = this.CreateDrawableChild(sprite.GetCurrentFrame(phase), color: tint);
        _animationOffset = phase;
        _tint = _drawableChild.Color;
        Sprite = sprite;
    }

    private void UpdateSprite() {
        if (_sprite != null) {
            _drawableChild.Sprite = _sprite.GetCurrentFrame(_animationOffset);
            _drawableChild.Color = _tint;
        }
        else {
            _drawableChild.Color = Color.Transparent;
        }
    }
    
    private void OnFrameChanged(AnimatedSprite obj) {
        UpdateSprite();
    }
}