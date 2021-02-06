using System.Net.Http.Headers;
using SFML.Graphics;
using SFML.System;

namespace PenguinGameClone
{
    public class GameBoard : IEntity
    {
        private readonly RectangleShape _shape;

        public Vector2f Position
        {
            get => _shape.Position;
            set => _shape.Position = value;
        }

        public Vector2f Size
        {
            get => _shape.Size;
            set => _shape.Size = value;
        }

        public Vector2f Center => _shape.Position + _shape.Size / 2;

        public GameBoard()
        {
            _shape = new RectangleShape
            {
                Size = new Vector2f(100, 100),
                FillColor = Color.White
            };
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