using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace PenguinGameClone
{
    public class Game
    {
        private static readonly string WindowTitle = "DeveloperRPG";
        public readonly Queue<IGameState> States = new();
        private bool _isFullScreen;
        public uint Height =  VideoMode.DesktopMode.Height / 2;
        public uint Width = VideoMode.DesktopMode.Width / 2;
        public RenderWindow Window;

        public Game()
        {
            Window = new RenderWindow(new VideoMode(Width, Height), WindowTitle, Styles.Default | Styles.Resize);
            InitWindow();

            States.Enqueue(new GameStateMain(this));
        }

        private IGameState CurrentState => States.TryPeek(out var state) ? state : null;

        private void InitWindow()
        {
            Window.Closed += (sender, _) => ((Window) sender)?.Close();
            Window.KeyPressed += (_, e) => InputManager.PressKey(e.Code);
            Window.KeyReleased += (_, e) => InputManager.ReleaseKey(e.Code);
            Window.MouseButtonPressed += (sender, e) => Console.Out.WriteLine($"press");
            Window.Resized += (_, args) =>
            {
                Width = args.Width;
                Height = args.Height;
            };
            Window.SetVerticalSyncEnabled(true);
        }

        public void GameLoop()
        {
            var clock = new Clock();
            while (Window.IsOpen)
            {
                Window.DispatchEvents();

                var elapsed = clock.Restart();

                CurrentState.HandleInput();
                CurrentState.Update(elapsed);

                if (InputManager.IsKeyPressed(Keyboard.Key.F11)) ToggleFullScreen();

                InputManager.NewFrame();

                Window.Clear(Color.Black);
                CurrentState.Render();
                Window.Display();
            }
        }

        private void ToggleFullScreen()
        {
            _isFullScreen = !_isFullScreen;
            if (_isFullScreen)
            {
                Window.Close();
                Window = new RenderWindow(VideoMode.DesktopMode, WindowTitle,
                    Styles.Fullscreen | Styles.Default);
                InitWindow();
            }
            else
            {
                Window.Close();
                Window = new RenderWindow(new VideoMode(Width, Height), WindowTitle,
                    Styles.Default | Styles.Resize);
                InitWindow();
            }
        }
    }
}