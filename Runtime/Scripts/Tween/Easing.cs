// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

using System;
using UnityEngine;

/**
 * Contains easing functions to use for Tween
 *
 * Heavily inspired by shohei909 TweenX library
 */
namespace Components.Tween
{
    public enum Easing
    {
        Linear,
        SineIn,
        SineOut,
        SineInOut,
        SineOutIn,
        QuadIn,
        QuadOut,
        QuadInOut,
        QuadOutIn,
        CubicIn,
        CubicOut,
        CubicInOut,
        CubicOutIn,
        QuartIn,
        QuartOut,
        QuartInOut,
        QuartOutIn,
        QuintIn,
        QuintOut,
        QuintInOut,
        QuintOutIn,
        ExpoIn,
        ExpoOut,
        ExpoInOut,
        ExpoOutIn,
        CircIn,
        CircOut,
        CircInOut,
        CircOutIn,
        BounceIn,
        BounceOut,
        BounceInOut,
        BounceOutIn,
        BackIn,
        BackOut,
        BackInOut,
        BackOutIn,
        ElasticIn,
        ElasticOut,
        ElasticInOut,
        ElasticOutIn,
        WarpIn,
        WarpOut,
        WarpInOut,
        WarpOutIn
    }
    
    public static class EasingFunc
    {
        private const float Pi = 3.1415926535897932384626433832795f;
        private const float Pi2 = Pi / 2f;
        private const float Ln2 = 0.6931471805599453f;
        private const float Ln2Ten = 6.931471805599453f;

        private const float Overshoot = 1.70158f;

        private const float Amplitude = 1f;
        private const float Period = 0.0003f;

        // Using enum
        public static float Ease(this float t, Easing ease)
        {
            switch (ease)
            {
                case Easing.Linear:
                    return Linear(t);
                case Easing.SineIn:
                    return SineIn(t);
                case Easing.SineOut:
                    return SineOut(t);
                case Easing.SineInOut:
                    return SineInOut(t);
                case Easing.SineOutIn:
                    return SineOutIn(t);
                case Easing.QuadIn:
                    return QuadIn(t);
                case Easing.QuadOut:
                    return QuadOut(t);
                case Easing.QuadInOut:
                    return QuadInOut(t);
                case Easing.QuadOutIn:
                    return QuadOutIn(t);
                case Easing.CubicIn:
                    return CubicIn(t);
                case Easing.CubicOut:
                    return CubicOut(t);
                case Easing.CubicInOut:
                    return CubicInOut(t);
                case Easing.CubicOutIn:
                    return CubicOutIn(t);
                case Easing.QuartIn:
                    return QuartIn(t);
                case Easing.QuartOut:
                    return QuartOut(t);
                case Easing.QuartInOut:
                    return QuartInOut(t);
                case Easing.QuartOutIn:
                    return QuartOutIn(t);
                case Easing.QuintIn:
                    return QuintIn(t);
                case Easing.QuintOut:
                    return QuintOut(t);
                case Easing.QuintInOut:
                    return QuintInOut(t);
                case Easing.QuintOutIn:
                    return QuintOutIn(t);
                case Easing.ExpoIn:
                    return ExpoIn(t);
                case Easing.ExpoOut:
                    return ExpoOut(t);
                case Easing.ExpoInOut:
                    return ExpoInOut(t);
                case Easing.ExpoOutIn:
                    return ExpoOutIn(t);
                case Easing.CircIn:
                    return CircIn(t);
                case Easing.CircOut:
                    return CircOut(t);
                case Easing.CircInOut:
                    return CircInOut(t);
                case Easing.CircOutIn:
                    return CircOutIn(t);
                case Easing.BounceIn:
                    return BounceIn(t);
                case Easing.BounceOut:
                    return BounceOut(t);
                case Easing.BounceInOut:
                    return BounceInOut(t);
                case Easing.BounceOutIn:
                    return BounceOutIn(t);
                case Easing.BackIn:
                    return BackIn(t);
                case Easing.BackOut:
                    return BackOut(t);
                case Easing.BackInOut:
                    return BackInOut(t);
                case Easing.BackOutIn:
                    return BackOutIn(t);
                case Easing.ElasticIn:
                    return ElasticIn(t);
                case Easing.ElasticOut:
                    return ElasticOut(t);
                case Easing.ElasticInOut:
                    return ElasticInOut(t);
                case Easing.ElasticOutIn:
                    return ElasticOutIn(t);
                case Easing.WarpIn:
                    return WarpIn(t);
                case Easing.WarpOut:
                    return WarpOut(t);
                case Easing.WarpInOut:
                    return WarpInOut(t);
                case Easing.WarpOutIn:
                    return WarpOutIn(t);
                default:
                    throw new ArgumentOutOfRangeException(nameof(ease), ease, null);
            }
        }
        
        // Linear
        public static float Linear(this float t)
        {
            return t;
        }

        // Sine
        public static float SineIn(this float t)
        {
            if (Mathf.Approximately(t, 0f))
            {
                return 0f;
            }

            if (Mathf.Approximately(t, 1f))
            {
                return 1f;
            }

            return 1f - Mathf.Cos(t * Pi2);
        }

        public static float SineOut(this float t)
        {
            if (Mathf.Approximately(t, 0f))
            {
                return 0f;
            }

            if (Mathf.Approximately(t, 1f))
            {
                return 1f;
            }

            return Mathf.Sin(t * Pi2);
        }

        public static float SineInOut(this float t)
        {
            if (Mathf.Approximately(t, 0f))
            {
                return 0f;
            }

            if (Mathf.Approximately(t, 1f))
            {
                return 1f;
            }

            return -0.5f * (Mathf.Cos(Pi * t) - 1f);
        }

        public static float SineOutIn(this float t)
        {
            if (Mathf.Approximately(t, 0f))
            {
                return 0f;
            }

            if (Mathf.Approximately(t, 1f))
            {
                return 1f;
            }

            if (t < 0.5f)
            {
                return 0.5f * Mathf.Sin((t * 2f) * Pi2);
            }

            return -0.5f * Mathf.Cos((t * 2f - 1f) * Pi2) + 1f;
        }

        // Quad
        public static float QuadIn(this float t)
        {
            return t * t;
        }

        public static float QuadOut(this float t)
        {
            return -t * (t - 2f);
        }

        public static float QuadInOut(this float t)
        {
            return (t < 0.5f) ? 2f * t * t : -2f * ((t -= 1f) * t) + 1f;
        }

        public static float QuadOutIn(this float t)
        {
            return (t < 0.5f)
                ? -0.5f * (t = (t * 2f)) * (t - 2f)
                : 0.5f * (t = (t * 2f - 1f)) * t + 0.5f;
        }

        // Cubic
        public static float CubicIn(this float t)
        {
            return t * t * t;
        }

        public static float CubicOut(this float t)
        {
            return (t -= 1f) * t * t + 1f;
        }

        public static float CubicInOut(this float t)
        {
            return ((t *= 2f) < 1f)
                ? 0.5f * t * t * t
                : 0.5f * ((t -= 2f) * t * t + 2f);
        }

        public static float CubicOutIn(this float t)
        {
            return 0.5f * ((t = t * 2f - 1f) * t * t + 1f);
        }

        // Quart
        public static float QuartIn(this float t)
        {
            return (t *= t) * t;
        }

        public static float QuartOut(this float t)
        {
            return 1f - (t = (t -= 1f) * t) * t;
        }

        public static float QuartInOut(this float t)
        {
            return ((t *= 2f) < 1f)
                ? 0.5f * (t *= t) * t
                : -0.5f * ((t = (t -= 2f) * t) * t - 2f);
        }

        public static float QuartOutIn(this float t)
        {
            return (t < 0.5f)
                ? -0.5f * (t = (t = t * 2f - 1f) * t) * t + 0.5f
                : 0.5f * (t = (t = t * 2f - 1f) * t) * t + 0.5f;
        }

        // Quint
        public static float QuintIn(this float t)
        {
            return t * (t *= t) * t;
        }

        public static float QuintOut(this float t)
        {
            return (t -= 1f) * (t *= t) * t + 1f;
        }

        public static float QuintInOut(this float t)
        {
            return ((t *= 2f) < 1f) ? 0.5f * t * (t *= t) * t : 0.5f * (t -= 2f) * (t *= t) * t + 1f;
        }

        public static float QuintOutIn(this float t)
        {
            return 0.5f * ((t = t * 2f - 1f) * (t *= t) * t + 1f);
        }

        // Expo
        public static float ExpoIn(this float t)
        {
            return Mathf.Approximately(t, 0f) ? 0f : Mathf.Exp(Ln2Ten * (t - 1f));
        }

        public static float ExpoOut(this float t)
        {
            return Mathf.Approximately(t, 1f) ? 1f : (1f - Mathf.Exp(-Ln2Ten * t));
        }

        public static float ExpoInOut(this float t)
        {
            if (Mathf.Approximately(t, 0f))
            {
                return 0f;
            }

            if (Mathf.Approximately(t, 1f))
            {
                return 1f;
            }

            if ((t *= 2f) < 1f)
            {
                return 0.5f * Mathf.Exp(Ln2Ten * (t - 1f));
            }

            return 0.5f * (2f - Mathf.Exp(-Ln2Ten * (t - 1f)));
        }

        public static float ExpoOutIn(this float t)
        {
            if (t < 0.5f)
            {
                return 0.5f * (1f - Mathf.Exp(-20f * Ln2 * t));
            }

            if (Mathf.Approximately(t, 0.5f))
            {
                return 0.5f;
            }

            return 0.5f * (Mathf.Exp(20f * Ln2 * (t - 1f)) + 1f);
        }

        // Circ
        public static float CircIn(this float t)
        {
            return (t < -1f || 1f < t) ? 0f : 1f - Mathf.Sqrt(1f - t * t);
        }

        public static float CircOut(this float t)
        {
            return (t < 0f || 2f < t) ? 0f : Mathf.Sqrt(t * (2f - t));
        }

        public static float CircInOut(this float t)
        {
            if (t < -0.5f || 1.5f < t)
            {
                return 0.5f;
            }

            if ((t *= 2f) < 1f)
            {
                return -0.5f * (Mathf.Sqrt(1f - t * t) - 1f);
            }

            return 0.5f * (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f);
        }

        public static float CircOutIn(this float t)
        {
            if (t < 0f)
            {
                return 0f;
            }

            if (1f < t)
            {
                return 1f;
            }

            if (t < 0.5f)
            {
                return 0.5f * Mathf.Sqrt(1f - (t = t * 2f - 1f) * t);
            }

            return -0.5f * ((Mathf.Sqrt(1f - (t = t * 2f - 1f) * t) - 1f) - 1f);
        }

        // Bounce
        public static float BounceIn(this float t)
        {
            if ((t = 1f - t) < (1f / 2.75f))
            {
                return 1f - ((7.5625f * t * t));
            }

            if (t < (2f / 2.75f))
            {
                return 1f - ((7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f));
            }

            if (t < (2.5f / 2.75f))
            {
                return 1f - ((7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f));
            }

            return 1f - ((7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f));
        }

        public static float BounceOut(this float t)
        {
            if (t < (1f / 2.75f))
            {
                return (7.5625f * t * t);
            }

            if (t < (2f / 2.75f))
            {
                return (7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f);
            }

            if (t < (2.5f / 2.75f))
            {
                return (7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f);
            }

            return (7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f);
        }

        public static float BounceInOut(this float t)
        {
            if (t < 0.5f)
            {
                if ((t = (1f - t * 2f)) < (1f / 2.75f))
                {
                    return (1f - ((7.5625f * t * t))) * 0.5f;
                }

                if (t < (2f / 2.75f))
                {
                    return (1f - ((7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f))) * 0.5f;
                }

                if (t < (2.5f / 2.75f))
                {
                    return (1f - ((7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f))) * 0.5f;
                }

                return (1f - ((7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f))) * 0.5f;
            }

            if ((t = (t * 2f - 1f)) < (1f / 2.75f))
            {
                return ((7.5625f * t * t)) * 0.5f + 0.5f;
            }

            if (t < (2f / 2.75f))
            {
                return ((7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f)) * 0.5f + 0.5f;
            }

            if (t < (2.5f / 2.75f))
            {
                return ((7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f)) * 0.5f + 0.5f;
            }

            return ((7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f)) * 0.5f + 0.5f;
        }

        public static float BounceOutIn(this float t)
        {
            if (t < 0.5f)
            {
                if ((t = (t * 2f)) < (1f / 2.75f))
                {
                    return 0.5f * (7.5625f * t * t);
                }

                if (t < (2f / 2.75f))
                {
                    return 0.5f * (7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f);
                }

                if (t < (2.5f / 2.75f))
                {
                    return 0.5f * (7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f);
                }

                return 0.5f * (7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f);
            }

            if ((t = (1f - (t * 2f - 1f))) < (1f / 2.75f))
            {
                return 0.5f - (0.5f * (7.5625f * t * t)) + 0.5f;
            }

            if (t < (2f / 2.75f))
            {
                return 0.5f - (0.5f * (7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f)) + 0.5f;
            }

            if (t < (2.5f / 2.75f))
            {
                return 0.5f - (0.5f * (7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f)) + 0.5f;
            }

            return 0.5f - (0.5f * (7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f)) + 0.5f;
        }

        // Back
        public static float BackIn(this float t)
        {
            if (Mathf.Approximately(t, 0f))
            {
                return 0f;
            }

            if (Mathf.Approximately(t, 1f))
            {
                return 1f;
            }

            return t * t * ((Overshoot + 1f) * t - Overshoot);
        }

        public static float BackOut(this float t)
        {
            if (Mathf.Approximately(t, 0f))
            {
                return 0f;
            }

            if (Mathf.Approximately(t, 1f))
            {
                return 1f;
            }

            return ((t -= 1f) * t * ((Overshoot + 1f) * t + Overshoot) + 1f);
        }

        public static float BackInOut(this float t)
        {
            if (Mathf.Approximately(t, 0f))
            {
                return 0f;
            }

            if (Mathf.Approximately(t, 1f))
            {
                return 1f;
            }

            if ((t *= 2f) < 1f)
            {
                return 0.5f * (t * t * (((Overshoot * 0.984375f) + 1f) * t - Overshoot * 1.525f));
            }

            return 0.5f * ((t -= 2f) * t * (((Overshoot * 1.525f) + 1f) * t + Overshoot * 1.525f) + 2f);
        }

        public static float BackOutIn(this float t)
        {
            if (Mathf.Approximately(t, 0f))
            {
                return 0f;
            }

            if (Mathf.Approximately(t, 1f))
            {
                return 1f;
            }

            if (t < 0.5f)
            {
                return 0.5f * ((t = t * 2f - 1f) * t * ((Overshoot + 1f) * t + Overshoot) + 1f);
            }

            return 0.5f * (t = t * 2f - 1f) * t * ((Overshoot + 1f) * t - Overshoot) + 0.5f;
        }

        // Elastic
        public static float ElasticIn(this float t)
        {
            if (Mathf.Approximately(t, 0f))
            {
                return 0f;
            }

            if (Mathf.Approximately(t, 1f))
            {
                return 1f;
            }

            const float s = Period / 4f;
            return -(Amplitude *
                     Mathf.Exp(Ln2Ten * (t -= 1f)) *
                     Mathf.Sin((t * 0.001f - s) * (2f * Pi) / Period));
        }

        public static float ElasticOut(this float t)
        {
            if (Mathf.Approximately(t, 0f))
            {
                return 0f;
            }

            if (Mathf.Approximately(t, 1f))
            {
                return 1f;
            }

            const float s = Period / 4f;
            return Mathf.Exp(-Ln2Ten * t) * Mathf.Sin((t * 0.001f - s) * (2f * Pi) / Period) + 1f;
        }

        public static float ElasticInOut(this float t)
        {
            if (Mathf.Approximately(t, 0f))
            {
                return 0f;
            }

            if (Mathf.Approximately(t, 1f))
            {
                return 1f;
            }

            const float s = Period / 4f;

            if ((t *= 2f) < 1f)
            {
                return -0.5f * (Amplitude * Mathf.Exp(Ln2Ten * (t -= 1f)) *
                                Mathf.Sin((t * 0.001f - s) * (2f * Pi) / Period));
            }

            return Amplitude * Mathf.Exp(-Ln2Ten * (t -= 1f)) *
                   Mathf.Sin((t * 0.001f - s) * (2f * Pi) / Period) * 0.5f + 1f;
        }

        public static float ElasticOutIn(this float t)
        {
            if (t < 0.5f)
            {
                if (Mathf.Approximately((t *= 2f), 0f))
                {
                    return 0f;
                }

                return ((Amplitude / 2f) *
                        Mathf.Exp(-Ln2Ten * t) *
                        Mathf.Sin((t * 0.001f - Period / 4f) * (2f * Pi) / Period)) +
                       0.5f;
            }

            if (Mathf.Approximately(t, 0.5f))
            {
                return 0.5f;
            }

            if (Mathf.Approximately(t, 1f))
            {
                return 1f;
            }

            {
                t = t * 2f - 1f;
                return -((Amplitude / 2f) *
                         Mathf.Exp(Ln2Ten * (t -= 1f)) *
                         Mathf.Sin((t * 0.001f - Period / 4f) * (2f * Pi) / Period)) +
                       0.5f;
            }
        }

        // Warp
        public static float WarpOut(this float t)
        {
            return t <= 0f ? 0f : 1f;
        }

        public static float WarpIn(this float t)
        {
            return t < 1f ? 0f : 1f;
        }

        public static float WarpInOut(this float t)
        {
            return t < 0.5f ? 0f : 1f;
        }

        public static float WarpOutIn(this float t)
        {
            if (t <= 0f)
            {
                return 0f;
            }

            return t < 1f ? 0.5f : 1f;
        }
    }
}