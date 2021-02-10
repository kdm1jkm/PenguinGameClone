using System;
using System.Collections;
using System.Collections.Generic;
using SFML.Graphics;

namespace PenguinGameClone
{
    public class Layer : Drawable, IEnumerable<IEntity>
    {
        private readonly List<IEntity> _drawables;

        public Layer()
        {
            _drawables = new List<IEntity>();
        }

        public IEntity this[int index] => _drawables[index];

        public void Add(IEntity value)
        {
            // Console.Out.WriteLine($"[DBG]Layer Add entity: {value}");
            _drawables.Add(value);
        }

        public bool Remove(IEntity value)
        {
            return _drawables.Remove(value);
        }

        public void RemoveAt(int index)
        {
            _drawables.RemoveAt(index);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var drawable in _drawables) drawable.Draw(target, states);
        }

        public IEnumerator<IEntity> GetEnumerator()
        {
            return _drawables.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}