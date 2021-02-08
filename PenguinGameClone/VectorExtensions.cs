using Box2DX.Common;
using Microsoft.VisualBasic.CompilerServices;
using SFML.System;

namespace PenguinGameClone
{
    public static class VectorExtensions
    {
        public static Vec2 ToVec2(Vector2f value)
        {
            return new Vec2(value.X, value.Y);
        }

        public static Vector2f ToVector2f(this Vec2 value)
        {
            return new Vector2f(value.X, value.Y);
        }

        public static Vec2 Devide(this Vec2 original, float value)
        {
            return new Vec2(original.X / value, original.Y / value);
        }
    }
}