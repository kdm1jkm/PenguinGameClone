using SFML.Graphics;
using SFML.System;

namespace PenguinGameClone
{
    public interface IEntity : Drawable
    {
        public Vector2f Position { get; set; }
        public void Update(Time elapsed);
    }
}