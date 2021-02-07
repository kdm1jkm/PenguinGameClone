using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace PenguinGameClone
{
    public class GameStateMain : IGameState
    {
        private readonly GameBoard _board = new();

        private readonly Layer _backgroundLayer = new();
        private readonly Layer _balls = new();
        private readonly List<Layer> _layers;
        private readonly Game _game;
        private View _view;

        public GameStateMain(Game game)
        {
            _game = game;
            _layers = new List<Layer>
            {
                _backgroundLayer,
                _balls
            };
            _backgroundLayer.Add(_board);
        }

        public void HandleInput()
        {
            HandleAddBall();
        }

        private void HandleAddBall()
        {
            var mousePosition = _game.Window.MapPixelToCoords(Mouse.GetPosition(_game.Window));
            Ball.Team? team = null;
            if (InputManager.IsKeyPressed(Keyboard.Key.A))
            {
                team = Ball.Team.TEAM_RED;
            }
            else if (InputManager.IsKeyPressed(Keyboard.Key.S))
            {
                team = Ball.Team.TEAM_BLUE;
            }

            if (!team.HasValue) return;
            Ball ball = new Ball(team.Value) {Position = mousePosition};
            _balls.Add(ball);
        }

        public void Update(Time elapsed)
        {
            UpdateEntities(elapsed);
            UpdateView(10.0f);
            UpdateSelectedBall();
        }

        private void UpdateSelectedBall()
        {
            float criteria = Ball.RADIUS * 1.1f;
            float minLength = float.MaxValue;
            Ball minLengthBall = null;
            foreach (var _ball in _balls)
            {
                Ball ball = (Ball) _ball;
                Vector2f diff = _game.Window.MapPixelToCoords(Mouse.GetPosition(_game.Window)) - ball.Position;
                float length = (float) Math.Sqrt(Math.Pow(diff.X, 2) + Math.Pow(diff.Y, 2));
                if (length < minLength)
                {
                    minLength = length;
                    minLengthBall = ball;
                }
            }

            if (minLength < criteria)
            {
                minLengthBall!.Selected = true;
            }
        }

        private void UpdateView(float interval)
        {
            var screenRatio = _game.Height / (float) _game.Width;

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

            _game.Window.SetView(_view);
        }

        private void UpdateEntities(Time elapsed)
        {
            foreach (var entity in _layers.SelectMany(layer => layer))
            {
                entity.Update(elapsed);
            }
        }

        public void Render()
        {
            foreach (var layer in _layers)
            {
                _game.Window.Draw(layer);
            }

            foreach (var ball in _balls)
            {
                if (((Ball) ball).Selected)
                {
                    _game.Window.Draw(ball);
                }
            }
        }
    }
}