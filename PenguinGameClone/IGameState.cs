using SFML.System;

namespace PenguinGameClone
{
    public interface IGameState
    {
        public void HandleInput();

        public void Update(Time elapsed);

        public void Render();
    }
}