using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace PenguinGameClone
{
    public class Ball : IEntity
    {
        public enum Team
        {
            TEAM_RED,
            TEAM_BLUE
        }

        public const float RADIUS = 3.0f;
        public const float LINE_THICKNESS = -0.7f;

        public bool Selected
        {
            get => _shape.OutlineThickness != 0.0f;
            set => _shape.OutlineThickness = value ? LINE_THICKNESS : 0.0f;
        }

        private static readonly Dictionary<Team, CircleShape> SHAPES = new()
        {
            {
                Team.TEAM_RED, new CircleShape
                {
                    FillColor = Color.Red,
                }
            },
            {
                Team.TEAM_BLUE, new CircleShape
                {
                    FillColor = Color.Blue,
                }
            }
        };

        private readonly CircleShape _shape;

        public Ball(Team team)
        {
            _shape = new CircleShape(SHAPES[team])
            {
                OutlineColor = Color.Black,
                Radius = RADIUS,
                Origin = new Vector2f(RADIUS, RADIUS)
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