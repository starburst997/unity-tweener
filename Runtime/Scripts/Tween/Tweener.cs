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
namespace Components.Tween
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum Property
    {
        Alpha,
        X,
        Y,
        Rotation,
        ScaleX,
        ScaleY
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
        [SerializeField] [NotNull] private List<Tween> tweens = new List<Tween>();

        [NotNull] private readonly Dictionary<Property, Tween> _tweenMap = new Dictionary<Property, Tween>();

        [NotNull] private readonly List<Tween> _toRemove = new List<Tween>();

        [NonSerialized] public bool HasStarted;

        public bool snap;

        // For alpha tween
        private SpriteRenderer[] _renderers;
        private TextMeshProUGUI[] _texts;

        private readonly List<Tween> _pending = new List<Tween>();
        private float _pendingAlpha = -1f;

        public void Start()
        {
            HasStarted = true;
            foreach (var tween in tweens) _tweenMap[tween.property] = tween;

            // For alpha we also need to look up all children
            var renderers = new List<SpriteRenderer>();
            var texts = new List<TextMeshProUGUI>();

            CheckChild(gameObject, renderers, texts);

            _renderers = renderers.ToArray();
            _texts = texts.ToArray();

            // Update alpha property on pending
            var value = 1.0f;
            if (_pendingAlpha >= 0f)
            {
                SetAlpha(_pendingAlpha);
                value = _pendingAlpha;
                _pendingAlpha = -1f;
            }

            if (_renderers.Length > 0 || _texts.Length > 0)
            {
                foreach (var tween in _pending)
                {
                    if (_renderers.Length > 0)
                    {
                        value = _renderers[0].color.a;
                    }
                    else if (_texts.Length > 0)
                    {
                        value = _texts[0].alpha;
                    }

                    tween.start = value;
                }
            }

            _pending.Clear();
        }

        private void CheckChild(GameObject obj, List<SpriteRenderer> renderers, List<TextMeshProUGUI> texts)
        {
            var sprite = obj.GetComponent<SpriteRenderer>();
            var text = obj.GetComponent<TextMeshProUGUI>();

            if (sprite != null) renderers.Add(sprite);
            if (text != null) texts.Add(text);

            foreach (Transform child in obj.transform)
            {
                CheckChild(child.gameObject, renderers, texts);
            }
        }

        private void Update()
        {
            RemoveAll();

            foreach (var tween in tweens)
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
                case Property.Rotation:
                    SetRotation(value);
                    break;
                case Property.ScaleX:
                    SetScaleX(value);
                    break;
                case Property.ScaleY:
                    SetScaleY(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetAlpha(float value)
        {
            if (_renderers == null || _texts == null)
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

            foreach (var text in _texts)
            {
                text.alpha = value;
            }
        }

        public void SetX(float value)
        {
            var o = gameObject;
            var vector3 = o.transform.localPosition;
            vector3.x = snap ? (int) value : value;
            o.transform.localPosition = vector3;
        }

        public void SetY(float value)
        {
            var o = gameObject;
            var vector3 = o.transform.localPosition;
            vector3.y = snap ? (int) value : value;
            o.transform.localPosition = vector3;
        }

        public void SetRotation(float value)
        {
            var o = gameObject;
            var vector3 = o.transform.rotation.eulerAngles;
            vector3.z = value;
            o.transform.rotation = Quaternion.Euler(vector3);
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

        public IEnumerable<Tween> GetTweens(Property property)
        {
            return tweens.Where(tween => tween.property == property);
        }

        private void RemoveAll()
        {
            if (_toRemove.Count <= 0) return;

            foreach (var tween in _toRemove) tweens.Remove(tween);
            _toRemove.Clear();
        }

        public void Alpha(float to, float duration = 0.25f, Easing ease = Easing.Linear, bool complete = true)
        {
            if (HasStarted && (_renderers == null || _renderers.Length == 0) && (_texts == null || _texts.Length == 0))
                return;

            var value = 1.0f;
            if (_renderers != null && _texts != null)
            {
                if (_renderers.Length > 0)
                {
                    value = _renderers[0].color.a;
                }
                else if (_texts.Length > 0)
                {
                    value = _texts[0].alpha;
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
            Remove(Property.X, complete);
            Add(new Tween(
                Property.X,
                gameObject.transform.localPosition.x,
                to,
                duration,
                ease));
        }

        public void Y(float to, float duration = 0.25f, Easing ease = Easing.ExpoOut, bool complete = true)
        {
            Remove(Property.Y, complete);
            Add(new Tween(
                Property.Y,
                gameObject.transform.localPosition.y,
                to,
                duration,
                ease));
        }

        public void ScaleX(float to, float duration = 0.25f, Easing ease = Easing.ExpoOut, bool complete = true)
        {
            Remove(Property.ScaleX, complete);
            Add(new Tween(
                Property.ScaleX,
                gameObject.transform.localScale.x,
                to,
                duration,
                ease));
        }

        public void ScaleY(float to, float duration = 0.25f, Easing ease = Easing.ExpoOut, bool complete = true)
        {
            Remove(Property.ScaleY, complete);
            Add(new Tween(
                Property.ScaleY,
                gameObject.transform.localScale.y,
                to,
                duration,
                ease));
        }

        public void Rotation(float to, float duration = 0.25f, Easing ease = Easing.ExpoOut, bool complete = true)
        {
            Remove(Property.Rotation, complete);
            Add(new Tween(
                Property.Rotation,
                gameObject.transform.rotation.eulerAngles.z,
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
            Remove(tween.property);

            tweens.Add(tween);
            _tweenMap[tween.property] = tween;
        }

        public void Destroy()
        {
            RemoveAll();

            _renderers = null;
            _texts = null;
        }
    }
}