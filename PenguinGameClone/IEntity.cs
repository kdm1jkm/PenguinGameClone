using SFML.Graphics;
using SFML.System;

namespace PenguinGameClone
{
    public interface IEntity : Drawable
    {
        public void Update(Time elapsed);
    }
}