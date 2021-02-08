using System.Collections.Generic;
using System.Linq;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Color = SFML.Graphics.Color;
using Math = System.Math;

namespace PenguinGameClone
{
    public class GameStateMain : IGameState
    {
        private readonly GameBoard _board = new();

        private readonly Layer _backgroundLayer = new();
        private readonly Layer _balls = new();
        private readonly List<Body> _ballBodies = new();
        private readonly List<Layer> _layers;
        private readonly Game _game;
        private View _view;
        private const float PHYSICS_INTERVAL = 10.0f;
        private World _world = new World(new AABB()
        {
            LowerBound = new Vec2(-PHYSICS_INTERVAL,y: -PHYSICS_INTERVAL).Devide(10.0f),
            UpperBound = new Vec2(GameBoard.BOARD_SIZE + PHYSICS_INTERVAL, GameBoard.BOARD_SIZE + PHYSICS_INTERVAL)
                .Devide(10.0f)
        }, Vec2.Zero, true);

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
            Ball.BallInfo team = null;
            if (InputManager.IsKeyPressed(Keyboard.Key.A))
            {
                team = Ball.BallInfo.RED_BALL;
            }
            else if (InputManager.IsKeyPressed(Keyboard.Key.S))
            {
                team = Ball.BallInfo.BLUE_BALL;
            }

            if (team == null) return;
            Ball ball = new Ball(team) {Position = mousePosition};
            Body body = _world.CreateBody(new BodyDef
            {
                Position = new Vec2(mousePosition.X, mousePosition.Y).Devide(10)
            });
            CircleDef circleDef = new CircleDef()
            {
                Radius = Ball.RADIUS/10,
                Density = 1.0f,
                Friction = .5f,
            };
            body.CreateShape(circleDef);
            body.SetMassFromShapes();
            _ballBodies.Add(body);
            _balls.Add(ball);
        }

        public void Update(Time elapsed)
        {
            _world.Step(elapsed.AsSeconds(), 8, 8);
            for (int i = 0; i < _ballBodies.Count; i++)
            {
                _balls[i].Position = _ballBodies[i].GetPosition().ToVector2f()*10;
            }
            UpdateEntities(elapsed);
            UpdateView(10.0f);
            UpdateSelectedBall();
        }

        private void UpdateSelectedBall()
        {
            float criteria = Ball.RADIUS * 1.1f;
            float minLength = float.MaxValue;
            Ball minLengthBall = null;
            foreach (Ball ball in _balls)
            {
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
            _game.Window.Clear(new Color(19,18,0));
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