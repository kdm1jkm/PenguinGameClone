using Box2DX.Common;
using Microsoft.VisualBasic.CompilerServices;
using SFML.System;
using Math = System.Math;

namespace PenguinGameClone
{
    public static class VectorExtensions
    {
        public static Vec2 ToVec2(this Vector2f value)
        {
            return new(value.X, value.Y);
        }

        public static Vector2f ToVector2f(this Vec2 value)
        {
            return new(value.X, value.Y);
        }

        public static Vec2 Devide(this Vec2 original, float value)
        {
            return new(original.X / value, original.Y / value);
        }

        public static float Length(this Vector2f value)
        {
            return (float) Math.Sqrt(Math.Pow(value.X, 2) + Math.Pow(value.Y, 2));
        }

        public static float Angle(this Vector2f value)
        {
            return (float) Math.Atan2(value.Y, value.X);
        }

        public static float Degree(this float value)
        {
            return (float) (value / Math.PI * 180);
        }

        public static Vector2f Pow(this Vector2f value, float e)
        {
            return new((float) Math.Pow(value.X, e), (float) Math.Pow(value.Y, e));
        }
    }
}