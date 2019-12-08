# unity-tweener
Very simple tween for GameObject, Sprite and TextMeshPro in UNity.

[Read more](http://robertpenner.com/easing/) about Tween.

## Usage
Add `Tweener` MonoBehaviour to any GameObject (`alpha` is supported on `SpriteRenderer` and `TextMeshPro`)

Then you can call any of the following

- Alpha
- X
- Y
- Rotation
- ScaleX
- ScaleY

Like so
```csharp
obj.X(5f, 1.25f, Easing.ExpoOut);
```
Which will tween `obj` x position to `5f` for `1.25` seconds using the `ExpoOut` easing.

By default a second call of `X` will complete any previous tween on `X`, to prevent this behaviour, set the last parameter as `false`.

See `Samples~` folder for an example project in unity.

## Complex sequence
Use Coroutine for complex sequence of tweens, there is currently no callbacks in the API.
```csharp
private IEnumerator ComplexTween()
{
    float wait = 0.5f;
    obj.ScaleX(2, wait, Easing.BackOut);
    obj.ScaleY(2, wait, Easing.BackOut);
    
    yield return new WaitForSeconds(wait);

    wait = 2.0f;
    obj.Rotation(720, wait, Easing.BounceOut);
    obj.X(5, wait, Easing.BackIn);
    
    yield return new WaitForSeconds(wait);
    
    wait = 2.0f;
    obj.X(-5, wait, Easing.BackInOut);
    obj.ScaleX(1, wait, Easing.BackIn);
    obj.ScaleY(1, wait, Easing.BackIn);
    
    yield return new WaitForSeconds(wait);
}

StartCoroutine(ComplexTween());
```

## Easing
Check this [webpage](https://easings.net/en) for visualizing them.

List of supported easing:
- Linear
- SineIn
- SineOut
- SineInOut
- SineOutIn
- QuadIn
- QuadOut
- QuadInOut
- QuadOutIn
- CubicIn
- CubicOut
- CubicInOut
- CubicOutIn
- QuartIn
- QuartOut
- QuartInOut
- QuartOutIn
- QuintIn
- QuintOut
- QuintInOut
- QuintOutIn
- ExpoIn
- ExpoOut
- ExpoInOut
- ExpoOutIn
- CircIn
- CircOut
- CircInOut
- CircOutIn
- BounceIn
- BounceOut
- BounceInOut
- BounceOutIn
- BackIn
- BackOut
- BackInOut
- BackOutIn
- ElasticIn
- ElasticOut
- ElasticInOut
- ElasticOutIn
- WarpIn
- WarpOut
- WarpInOut
- WarpOutIn

## Credit
Heavily inspired by [TweenX](https://github.com/shohei909/tweenx) by [shohei909](https://github.com/shohei909)

## License
MIT License