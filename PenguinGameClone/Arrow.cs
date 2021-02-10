using System;
using SFML.Graphics;
using SFML.System;

namespace PenguinGameClone
{
    public class Arrow : IEntity
    {
        private readonly Vector2f _triangleSize;
        private readonly float _thickness;

        private readonly RectangleShape _rect;
        private readonly ConvexShape _head;


        public readonly struct ArrowInfo
        {
            public static readonly ArrowInfo BASIC =
                new(
                    new Vector2f(4.0f, 5.6f) / 2,
                    1.0f,
                    new Color(255, 186, 8, 150),
                    Color.Black,
                    -.0f);

            public Vector2f TriangleSize { get; }
            public float Thickness { get; }
            public Color Color { get; }
            public Color LineColor { get; }
            public float LineThickness { get; }

            public ArrowInfo(Vector2f triangleSize, float thickness, Color color, Color lineColor, float lineThickness)
            {
                TriangleSize = triangleSize;
                Thickness = thickness;
                Color = color;
                LineColor = lineColor;
                LineThickness = lineThickness;
            }
        }


        public Arrow(ArrowInfo info)
        {
            _triangleSize = info.TriangleSize;
            _thickness = info.Thickness;

            _head = new ConvexShape(3);
            _head.SetPoint(0, new Vector2f(.0f, .0f));
            _head.SetPoint(1, new Vector2f(-_triangleSize.X, _triangleSize.Y / 2));
            _head.SetPoint(2, new Vector2f(-_triangleSize.X, -_triangleSize.Y / 2));
            _head.Origin = new Vector2f(.0f, .0f);
            _head.FillColor = info.Color;
            _head.OutlineColor = info.LineColor;
            _head.OutlineThickness = info.LineThickness;

            _rect = new RectangleShape(new Vector2f(1.0f, info.Thickness));
            _rect.FillColor = info.Color;
            _rect.OutlineColor = info.LineColor;
            _rect.OutlineThickness = info.LineThickness;

            Delta = new Vector2f(1.0f, 1.0f);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _rect.Draw(target, states);
            _head.Draw(target, states);
        }

        public void Update(Time elapsed)
        {
        }

        private Vector2f _delta;

        public Vector2f Position
        {
            get => _rect.Position;
            set
            {
                _rect.Position = value;
                _head.Position = value + Delta;
            }
        }

        public Vector2f Delta
        {
            get => _delta;
            set
            {
                _delta = value;

                _rect.Size =
                    new Vector2f(_delta.Length() - _triangleSize.X, _thickness);
                _rect.Origin = new Vector2f(0, _thickness / 2);
                _rect.Rotation = _delta.Angle().Degree();

                _head.Position = Position + _delta;
                _head.Rotation = _rect.Rotation;
            }
        }

        public Vector2f Point
        {
            get => Position + Delta;
            set => Delta = value - Position;
        }
    }
}