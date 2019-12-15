using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

/**
 * Add basic tween capabilities
 *
 * Heavily inspired by shohei909 TweenX library
 */
namespace Tween
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum Property
    {
        Alpha,
        X,
        Y,
        Z,
        Rotation,
        ScaleX,
        ScaleY,
        ScaleZ
    }

    [Serializable]
    public class Tween : object
    {
        public Easing easing;
        public Property property;

        public float time;
        public float duration;

        public float start;
        public float end;

        public float value;

        public Tween(Property property,
            float start,
            float end,
            float duration = 0.25f,
            Easing easing = Easing.ExpoOut,
            float time = 0.0f)
        {
            this.property = property;
            this.start = start;
            this.end = end;
            this.time = time;
            this.duration = duration;
            this.easing = easing;
        }
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class Tweener : MonoBehaviour
    {
        [NotNull] private readonly List<Tween> _tweens = new List<Tween>();

        [NotNull] private readonly Dictionary<Property, Tween> _tweenMap = new Dictionary<Property, Tween>();

        [NotNull] private readonly List<Tween> _toRemove = new List<Tween>();

        [NonSerialized] public bool HasStarted;

        public bool Snap;

        // For alpha tween
        private SpriteRenderer[] _renderers;
        private Renderer[] _renderers2;
        private TextMeshProUGUI[] _texts;
        private TextMeshPro[] _texts2;

        private readonly List<Tween> _pending = new List<Tween>();
        private float _pendingAlpha = -1f;
        private static readonly int Color = Shader.PropertyToID("_Color");

        public void Start()
        {
            HasStarted = true;
            foreach (var tween in _tweens) _tweenMap[tween.property] = tween;

            // For alpha we also need to look up all children
            var renderers = new List<SpriteRenderer>();
            var renderers2 = new List<Renderer>();
            var texts = new List<TextMeshProUGUI>();
            var texts2 = new List<TextMeshPro>();
            
            CheckChild(gameObject, renderers, renderers2, texts, texts2);

            _renderers = renderers.ToArray();
            _renderers2 = renderers2.ToArray();
            _texts = texts.ToArray();
            _texts2 = texts2.ToArray();

            // Update alpha property on pending
            var value = 1.0f;
            if (_pendingAlpha >= 0f)
            {
                SetAlpha(_pendingAlpha);
                value = _pendingAlpha;
                _pendingAlpha = -1f;
            }

            if (_renderers.Length > 0 || _renderers2.Length > 0 || _texts.Length > 0 || _texts2.Length > 0)
            {
                foreach (var tween in _pending)
                {
                    if (_renderers.Length > 0)
                    {
                        value = _renderers[0].color.a;
                    }
                    else if (_renderers2.Length > 0)
                    {
                        value = _renderers2[0].material.color.a;
                    }
                    else if (_texts.Length > 0)
                    {
                        value = _texts[0].alpha;
                    }
                    else if (_texts2.Length > 0)
                    {
                        value = _texts2[0].alpha;
                    }

                    tween.start = value;
                }
            }

            _pending.Clear();
        }

        private void CheckChild(GameObject obj, List<SpriteRenderer> renderers, List<Renderer> renderers2, List<TextMeshProUGUI> texts, List<TextMeshPro> texts2)
        {
            var sprite = obj.GetComponent<SpriteRenderer>();
            var renderer = obj.GetComponent<Renderer>();
            var text = obj.GetComponent<TextMeshProUGUI>();
            var text2 = obj.GetComponent<TextMeshPro>();

            if (sprite != null) renderers.Add(sprite);
            if (renderer != null) renderers2.Add(renderer);
            if (text != null) texts.Add(text);
            if (text2 != null) texts2.Add(text2);

            foreach (Transform child in obj.transform)
            {
                CheckChild(child.gameObject, renderers, renderers2, texts, texts2);
            }
        }

        private void Update()
        {
            RemoveAll();

            foreach (var tween in _tweens)
            {
                tween.time += Time.deltaTime;
                if (tween.time >= tween.duration)
                {
                    tween.time = tween.duration;
                    _toRemove.Add(tween);
                    _tweenMap.Remove(tween.property);
                }

                var value = tween.start + (tween.end - tween.start) * (tween.time / tween.duration).Ease(tween.easing);
                SetProperty(tween, value);
            }

            RemoveAll();
        }

        private void SetProperty(Tween tween, float value)
        {
            tween.value = value;

            switch (tween.property)
            {
                case Property.Alpha:
                    SetAlpha(value);
                    break;
                case Property.X:
                    SetX(value);
                    break;
                case Property.Y:
                    SetY(value);
                    break;
                case Property.Z:
                    SetZ(value);
                    break;
                case Property.Rotation:
                    SetRotation(value);
                    break;
                case Property.ScaleX:
                    SetScaleX(value);
                    break;
                case Property.ScaleY:
                    SetScaleY(value);
                    break;
                case Property.ScaleZ:
                    SetScaleZ(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetAlpha(float value)
        {
            if (_renderers == null || _texts == null || _texts2 == null)
            {
                _pendingAlpha = value;
                return;
            }

            foreach (var sprite in _renderers)
            {
                var color = sprite.color;
                color.a = value;
                sprite.color = color;
            }
            
            foreach (var renderer in _renderers2)
            {
                var color = renderer.material.color;
                color.a = value;

                renderer.material.color = color;

                /*MaterialPropertyBlock props = new MaterialPropertyBlock();
                props.SetColor(Color, color);
                renderer.SetPropertyBlock(props);*/
            }

            foreach (var text in _texts)
            {
                text.alpha = value;
            }
            
            foreach (var text in _texts2)
            {
                text.alpha = value;
            }
        }

        public void SetX(float value)
        {
            var o = gameObject;
            var vector3 = o.transform.localPosition;
            vector3.x = Snap ? (int) value : value;
            o.transform.localPosition = vector3;
        }

        public void SetY(float value)
        {
            var o = gameObject;
            var vector3 = o.transform.localPosition;
            vector3.y = Snap ? (int) value : value;
            o.transform.localPosition = vector3;
        }
        
        public void SetZ(float value)
        {
            var o = gameObject;
            var vector3 = o.transform.localPosition;
            vector3.z = Snap ? (int) value : value;
            o.transform.localPosition = vector3;
        }

        public void SetRotation(float value)
        {
            var o = gameObject;
            var vector3 = o.transform.rotation.eulerAngles;
            o.transform.rotation = Quaternion.Euler(new Vector3(vector3.x, vector3.y, value));
        }

        public void SetScaleX(float value)
        {
            var o = gameObject;
            var vector3 = o.transform.localScale;
            vector3.x = value;
            o.transform.localScale = vector3;
        }

        public void SetScaleY(float value)
        {
            var o = gameObject;
            var vector3 = o.transform.localScale;
            vector3.y = value;
            o.transform.localScale = vector3;
        }
        
        public void SetScaleZ(float value)
        {
            var o = gameObject;
            var vector3 = o.transform.localScale;
            vector3.z = value;
            o.transform.localScale = vector3;
        }

        public IEnumerable<Tween> GetTweens(Property property)
        {
            return _tweens.Where(tween => tween.property == property);
        }

        private void RemoveAll()
        {
            if (_toRemove.Count <= 0) return;

            foreach (var tween in _toRemove) _tweens.Remove(tween);
            _toRemove.Clear();
        }

        public void Alpha(float to, float duration = 0.25f, Easing ease = Easing.Linear, bool complete = true)
        {
            if (HasStarted && (_renderers == null || _renderers.Length == 0) && (_renderers2 == null || _renderers2.Length == 0) && (_texts == null || _texts.Length == 0) && (_texts2 == null || _texts2.Length == 0))
                return;

            var value = 1.0f;
            if (_renderers != null && _texts != null)
            {
                if (_renderers.Length > 0)
                {
                    value = _renderers[0].color.a;
                }
                
                if (_renderers2.Length > 0)
                {
                    value = _renderers2[0].material.color.a;
                }
                
                if (_texts.Length > 0)
                {
                    value = _texts[0].alpha;
                }
                
                if (_texts2.Length > 0)
                {
                    value = _texts2[0].alpha;
                }
            }

            var tween = new Tween(Property.Alpha, value, to, duration, ease);

            Remove(Property.Alpha, complete);
            Add(tween);

            if (!HasStarted)
            {
                _pending.Add(tween);
            }
        }

        public void X(float to, float duration = 0.25f, Easing ease = Easing.ExpoOut, bool complete = true)
        {
            float start = gameObject.transform.localPosition.x;
            Remove(Property.X, complete);
            Add(new Tween(
                Property.X,
                start,
                to,
                duration,
                ease));
        }

        public void Y(float to, float duration = 0.25f, Easing ease = Easing.ExpoOut, bool complete = true)
        {
            float start = gameObject.transform.localPosition.y;
            Remove(Property.Y, complete);
            Add(new Tween(
                Property.Y,
                start,
                to,
                duration,
                ease));
        }
        
        public void Z(float to, float duration = 0.25f, Easing ease = Easing.ExpoOut, bool complete = true)
        {
            float start = gameObject.transform.localPosition.z;
            Remove(Property.Z, complete);
            Add(new Tween(
                Property.Z,
                start,
                to,
                duration,
                ease));
        }

        public void ScaleX(float to, float duration = 0.25f, Easing ease = Easing.ExpoOut, bool complete = true)
        {
            float start = gameObject.transform.localScale.x;
            Remove(Property.ScaleX, complete);
            Add(new Tween(
                Property.ScaleX,
                start,
                to,
                duration,
                ease));
        }

        public void ScaleY(float to, float duration = 0.25f, Easing ease = Easing.ExpoOut, bool complete = true)
        {
            float start = gameObject.transform.localScale.y;
            Remove(Property.ScaleY, complete);
            Add(new Tween(
                Property.ScaleY,
                start,
                to,
                duration,
                ease));
        }
        
        public void ScaleZ(float to, float duration = 0.25f, Easing ease = Easing.ExpoOut, bool complete = true)
        {
            float start = gameObject.transform.localScale.z;
            Remove(Property.ScaleZ, complete);
            Add(new Tween(
                Property.ScaleZ,
                start,
                to,
                duration,
                ease));
        }

        public void Rotation(float to, float duration = 0.25f, Easing ease = Easing.ExpoOut, bool complete = true)
        {
            float start = gameObject.transform.rotation.eulerAngles.z;
            Remove(Property.Rotation, complete);
            Add(new Tween(
                Property.Rotation,
                start,
                to,
                duration,
                ease));
        }

        public void Remove(Property property, bool complete = true)
        {
            if (!_tweenMap.ContainsKey(property)) return;

            var tween = _tweenMap[property];
            if (complete)
            {
                SetProperty(tween, tween.end);
            }

            _toRemove.Add(tween);
        }

        private void Add(Tween tween)
        {
            Remove(tween.property, false);

            _tweens.Add(tween);
            _tweenMap[tween.property] = tween;
        }

        public void Destroy()
        {
            RemoveAll();

            _renderers = null;
            _renderers2 = null;
            _texts = null;
            _texts2 = null;
        }
    }
}