using System.Collections.Generic;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Color = SFML.Graphics.Color;
using Math = System.Math;

// ReSharper disable PossibleInvalidCastExceptionInForeachLoop

namespace PenguinGameClone
{
    public class GameStateMain : IGameState
    {
        private const float PHYSICS_INTERVAL = 10.0f;

        private readonly Game _game;
        
        private readonly GameBoard _board = new();
        
        private readonly Layer _backgroundLayer = new();
        private readonly Layer _balls = new();

        private readonly List<Body> _ballBodies = new();

        private readonly World _world;

        private View _view;
        
        private Ball _selectedBall;

        public GameStateMain(Game game)
        {
            _game = game;
            _backgroundLayer.Add(_board);
            _world = new World(new AABB
            {
                LowerBound = new Vec2(-PHYSICS_INTERVAL, -PHYSICS_INTERVAL).Devide(10.0f),
                UpperBound = new Vec2(GameBoard.BOARD_SIZE + PHYSICS_INTERVAL, GameBoard.BOARD_SIZE + PHYSICS_INTERVAL)
                    .Devide(10.0f)
            }, Vec2.Zero, true);
        }

        private Layer Arrows
        {
            get
            {
                var layer = new Layer();
                foreach (Ball ball in _balls) layer.Add(ball.Arrow);

                return layer;
            }
        }

        public void HandleInput()
        {
            HandleAddBall();
            HandleAddArrow();
            HandleMoveBall();
        }

        private void HandleMoveBall()
        {
            if (InputManager.IsKeyPressed(Keyboard.Key.Space))
                for (var i = 0; i < _ballBodies.Count; i++)
                {
                    _ballBodies[i].WakeUp();
                    _ballBodies[i].SetLinearVelocity((((Ball) _balls[i]).Arrow.Delta / 5).ToVec2());
                }
        }

        public void Update(Time elapsed)
        {
            _world.Step(elapsed.AsSeconds(), 8, 8);

            UpdateVelocity(elapsed);

            RemoveInvalidEntity();
            UpdateEntities(elapsed);
            UpdateView(5.0f);
            UpdateSelectedBall();
            UpdateArrow();
        }

        public void Render()
        {
            _game.Window.Clear(new Color(19, 18, 0));
            _game.Window.Draw(_backgroundLayer);
            _game.Window.Draw(_balls);
            _game.Window.Draw(Arrows);

            foreach (Ball ball in _balls)
                if (ball.Selected)
                    _game.Window.Draw(ball);

            if (_selectedBall != null) _game.Window.Draw(_selectedBall);
        }

        private void UpdateVelocity(Time elapsed)
        {
            foreach (var ballBody in _ballBodies)
            {
                var currentVelocity = ballBody.GetLinearVelocity().ToVector2f();
                var theta = currentVelocity.Angle();
                var delta = 5 * elapsed.AsSeconds(); // 5 = gravityAcceleration * coefficient of friction 
                var deltaVec = new Vector2f((float) Math.Cos(theta), (float) Math.Sin(theta)) * delta;
                ballBody.SetLinearVelocity(
                    currentVelocity.Length() < delta
                        ? Vec2.Zero
                        : (currentVelocity - deltaVec).ToVec2()
                );
            }
        }

        private void UpdateArrow()
        {
            if (_selectedBall == null) return;

            _selectedBall.Arrow.Point = _game.Window.MapPixelToCoords(Mouse.GetPosition(_game.Window));
        }

        private void HandleAddBall()
        {
            var mousePosition = _game.Window.MapPixelToCoords(Mouse.GetPosition(_game.Window));
            Ball.BallInfo team = null;
            if (InputManager.IsKeyPressed(Keyboard.Key.A))
                team = Ball.BallInfo.RED_BALL;
            else if (InputManager.IsKeyPressed(Keyboard.Key.S)) team = Ball.BallInfo.BLUE_BALL;

            if (team == null) return;
            var ball = new Ball(team) {Position = mousePosition};
            var body = _world.CreateBody(new BodyDef
            {
                Position = new Vec2(mousePosition.X, mousePosition.Y).Devide(10)
            });
            var circleDef = new CircleDef
            {
                Radius = Ball.RADIUS / 10,
                Density = 1.0f,
                Friction = .5f
            };
            body.CreateShape(circleDef);
            body.SetMassFromShapes();
            _ballBodies.Add(body);
            _balls.Add(ball);
        }

        private void HandleAddArrow()
        {
            if (InputManager.IsButtonPressed(Mouse.Button.Left))
            {
                Ball targetBall = null;
                foreach (Ball ball in _balls)
                    if (ball.Selected)
                    {
                        targetBall = ball;
                        break;
                    }

                if (targetBall == null) return;

                _selectedBall = targetBall;
            }
            else if (InputManager.IsButtonReleased(Mouse.Button.Left))
            {
                _selectedBall = null;
            }
        }

        private void RemoveInvalidEntity()
        {
            var removeIndexes = new List<int>();
            for (var i = 0; i < _ballBodies.Count; i++)
            {
                _balls[i].Position = _ballBodies[i].GetPosition().ToVector2f() * 10;
                if (_ballBodies[i].IsFrozen() || !_board.IsContain(_balls[i].Position)) removeIndexes.Insert(0, i);
            }

            foreach (var index in removeIndexes)
            {
                _balls.RemoveAt(index);
                _ballBodies.RemoveAt(index);
            }
        }

        private void UpdateSelectedBall()
        {
            if (_selectedBall != null)
            {
                _selectedBall.Selected = true;
                return;
            }

            var criteria = Ball.RADIUS * 1.1f;
            var minLength = float.MaxValue;
            Ball minLengthBall = null;
            foreach (Ball ball in _balls)
            {
                var diff = _game.Window.MapPixelToCoords(Mouse.GetPosition(_game.Window)) - ball.Position;
                var length = diff.Length();
                if (length < minLength)
                {
                    minLength = length;
                    minLengthBall = ball;
                }
            }

            if (minLength < criteria) minLengthBall!.Selected = true;
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
            foreach (var entity in _balls) entity.Update(elapsed);
        }
    }
}