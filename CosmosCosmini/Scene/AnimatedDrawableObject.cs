using Base.Constants;
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
    private bool _respectPPU;
    
    public AnimatedDrawableObject(AnimatedSprite sprite, Color? tint = null, int phase = 0, bool respectPPU = true) {
        _drawableChild = this.CreateDrawableChild(sprite.GetCurrentFrame(phase), color: tint);
        _animationOffset = phase;
        _tint = _drawableChild.Color;
        _respectPPU = respectPPU;
        Sprite = sprite;
    }

    private void UpdateSprite() {
        if (_sprite != null) {
            var sprite = _sprite.GetCurrentFrame(_animationOffset);
            _drawableChild.Sprite = sprite;
            _drawableChild.Color = _tint;
            Transform.LocalScale = _respectPPU ? sprite.GetWorldSize() : Transform.LocalScale;
        }
        else {
            _drawableChild.Color = Color.Transparent;
        }
    }
    
    private void OnFrameChanged(AnimatedSprite obj) {
        UpdateSprite();
    }
}