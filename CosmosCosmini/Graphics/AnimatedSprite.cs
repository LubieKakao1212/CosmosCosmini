using Custom2d_Engine.Math;
using Custom2d_Engine.Rendering.Sprites;
using Custom2d_Engine.Ticking;
using JustLoaded.Content;
using Microsoft.Xna.Framework;

namespace CosmosCosmini.Graphics;

public class AnimatedSprite {

    public event Action<AnimatedSprite> FrameChanged = delegate { };
    
    private readonly DatabaseReference<Sprite>[] _frames;

    public int FrameCount { get; }

    public TimeSpan FrameDuration {
        get => _animator.Interval;
        set => _animator.Interval = value;
    }

    private readonly AutoTimeMachine _animator;

    private int _currentFrame;
    
    public AnimatedSprite(DatabaseReference<Sprite>[] frames, TimeSpan frameDuration) {
        this._frames = frames;
        _animator = new AutoTimeMachine(() => {
            _currentFrame = (_currentFrame + 1 % frames.Length);
            FrameChanged(this);
        }, frameDuration);
        
        FrameCount = frames.Length;
    }

    // public AnimatedSprite(IEnumerable<BoundContentKey<Sprite>> frameIds, TimeSpan frameDuration) : this(
    //     frameIds
    //         .Select(id => new DatabaseReference<Sprite>(id))
    //         .ToArray(), frameDuration) {
    // }
    //
    // public AnimatedSprite(IEnumerable<ContentKey> frameIds, TimeSpan frameDuration) : this(
    //     frameIds
    //         .Select(key => new DatabaseReference<Sprite>(BoundContentKey<Sprite>.Make(key)))
    //         .ToArray(), frameDuration) {
    // }

    public Sprite GetCurrentFrame(int phase = 0) {
        var frame = (_currentFrame + phase);
        var l = _frames.Length;
        var frameDiv =  MathUtil.FloorDiv(frame, l);
        frame -= frameDiv * l;
        return _frames[frame].Value ?? throw new ApplicationException($"Invalid sprite {_frames[(_currentFrame + phase) % _frames.Length].Key}");
    }

    public void Update(TimeSpan deltaTime) {
        this._animator.Forward(deltaTime);
    }
    
}