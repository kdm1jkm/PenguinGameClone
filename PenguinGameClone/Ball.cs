using SFML.Graphics;
using SFML.System;

namespace PenguinGameClone
{
    public class Ball : IEntity
    {
        public class BallInfo
        {
            public static readonly BallInfo RED_BALL =
                new BallInfo(new Color(179,39,30), new Color(231,123,115));

            public static readonly BallInfo BLUE_BALL =
                new BallInfo(new Color(10, 36, 99), new Color(20, 75, 204));
            
            BallInfo(Color basicColor, Color selectedColor)
            {
                BasicColor = basicColor;
                SelectedColor = selectedColor;
            }
            public Color BasicColor { get; }
            public Color SelectedColor { get; }
            
        }

        public const float RADIUS = 3.0f;
        public const float LINE_THICKNESS = -0.7f;

        private readonly BallInfo _info;

        private bool _selected;

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                if (_selected)
                {
                    _shape.OutlineThickness = LINE_THICKNESS;
                    _shape.FillColor = _info.SelectedColor;
                }
                else
                {
                    _shape.OutlineThickness = .0f;
                    _shape.FillColor = _info.BasicColor;
                }
            }
        }

        private readonly CircleShape _shape;

        public Ball(BallInfo info)
        {
            _info = info;
            _shape = new CircleShape
            {
                OutlineColor = Color.Black,
                Radius = RADIUS,
                Origin = new Vector2f(RADIUS, RADIUS),
                FillColor = _info.BasicColor
            };
            Selected = false;
        }


        public Vector2f Position
        {
            get => _shape.Position;
            set => _shape.Position = value;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _shape.Draw(target, states);
        }

        public void Update(Time elapsed)
        {
            Selected = false;
        }
    }
}