using System;
using SFML.Graphics;
using SFML.System;

namespace PenguinGameClone
{
    public class GameBoard : IEntity
    {
        private readonly RectangleShape _shape;

        public const float BOARD_SIZE = 100.0f;

        public GameBoard()
        {
            _shape = new RectangleShape
            {
                Size = new Vector2f(BOARD_SIZE, BOARD_SIZE),
                FillColor = new Color(253, 255, 252)
            };
        }

        public Vector2f Position
        {
            get => _shape.Position;
            set => _shape.Position = value;
        }

        public Vector2f Size
        {
            get => _shape.Size;
            set
            {
                var center = Center;
                _shape.Size = value;
                Center = center;
            }
        }

        public Vector2f Center
        {
            get => _shape.Position + _shape.Size / 2;
            set => _shape.Position = value - _shape.Size / 2;
        }

        public bool IsContain(Vector2f position)
        {
            return position.X > Position.X && position.X < Position.X + Size.X &&
                   position.Y > Position.Y && position.Y < Position.Y + Size.Y;
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