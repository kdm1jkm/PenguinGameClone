using System;
using System.Collections.Generic;
using System.Resources;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace PenguinGameClone
{
    public class GameStateMain : IGameState
    {
        private readonly Game _game;

        private readonly List<IEntity> _entities;
        private readonly GameBoard _board;
        private View _view;

        public GameStateMain(Game game)
        {
            _game = game;
            _board = new GameBoard();

            _entities = new List<IEntity>
            {
                _board
            };
        }

        public void HandleInput()
        {
            if (InputManager.IsKeyPressed(Keyboard.Key.A))
            {
                _entities.Add(new Ball(Ball.Team.TEAM_RED) {Position = (Vector2f) Mouse.GetPosition(_game.Window)});
            }
        }

        public void Update(Time elapsed)
        {
            foreach (var entity in _entities)
            {
                entity.Update(elapsed);
            }

            var interval = 10.0f;

            float screenRatio = _game.Height / (float) _game.Width;

            float viewHeight;
            float viewWidth;
            
            if (_game.Width > _game.Height)
            {
                viewHeight = _board.Size.Y + interval * 2;
                viewWidth = viewHeight / screenRatio;
            }
            else
            {
                viewWidth = _board.Size.X + interval * 2;
                viewHeight = viewWidth * screenRatio;
            }

            _view = new View(_board.Center, new Vector2f(viewWidth, viewHeight));

            // Console.Out.WriteLine($"Center: {_board.Center}");
            // Console.Out.WriteLine($"Mouse: {Mouse.GetPosition(_game.Window)}");
            // Console.Out.WriteLine($"================================");

            _game.Window.SetView(_view);
        }

        public void Render()
        {
            foreach (var entity in _entities)
            {
                _game.Window.Draw(entity);
            }
        }
    }
}