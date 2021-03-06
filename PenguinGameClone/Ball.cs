﻿using System;
using SFML.Graphics;
using SFML.System;

namespace PenguinGameClone
{
    public class Ball : IEntity
    {
        public const float RADIUS = 3.0f;
        private const float LINE_THICKNESS = -0.7f;

        private readonly BallInfo _info;

        private readonly CircleShape _shape;

        private bool _selected;

        public readonly Team CurrentTeam;

        public enum Team
        {
            BEG,
            BLUE=BEG,
            RED,
            NONE
        }

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
            CurrentTeam = info.Team;

            Arrow = new Arrow(Arrow.ArrowInfo.BASIC)
                {Delta = new Vector2f(.0f, .0f)};
        }

        public Arrow Arrow { get; }

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                _shape.OutlineThickness = _selected ? LINE_THICKNESS : .0f;
                _shape.FillColor = _selected ? _info.SelectedColor : _info.BasicColor;
            }
        }


        public Vector2f Position
        {
            get => _shape.Position;
            set => _shape.Position = Arrow.Position = value;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _shape.Draw(target, states);
        }

        public void Update(Time elapsed)
        {
            Selected = false;
        }

        public class BallInfo
        {
            public static readonly BallInfo RED_BALL =
                new(
                    new Color(179, 39, 30), 
                    new Color(231, 123, 115),
                    Team.RED);

            public static readonly BallInfo BLUE_BALL =
                new(
                    new Color(10, 36, 99), 
                    new Color(20, 75, 204),
                    Team.BLUE);

            private BallInfo(Color basicColor, Color selectedColor, Team team)
            {
                BasicColor = basicColor;
                SelectedColor = selectedColor;
                Team = team;
            }

            public Color BasicColor { get; }
            public Color SelectedColor { get; }
            
            public Team Team { get; }
        }
    }
}