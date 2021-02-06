using System;
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
        };

        public const float RADIUS = 3.0f;

        private static readonly Dictionary<Team, CircleShape> SHAPES = new Dictionary<Team, CircleShape>()
        {
            {
                Team.TEAM_RED, new CircleShape()
                {
                    Radius = RADIUS,
                    OutlineColor = Color.Black,
                    FillColor = Color.Red
                }
            },
            {
                Team.TEAM_BLUE, new CircleShape()
                {
                    Radius =   RADIUS,
                    OutlineColor = Color.Black,
                    FillColor = Color.Blue
                }
            }
        };

        private CircleShape _shape;


        public Vector2f Position
        {
            get => _shape.Position;
            set => _shape.Position = value;
        }

        public Ball(Team team)
        {
            _shape = new CircleShape(SHAPES[team]);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _shape.Draw(target, states);
        }

        public void Update(Time elapsed)
        {
            
        }
    }
}