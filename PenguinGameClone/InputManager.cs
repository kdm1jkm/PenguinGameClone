using System.Collections.Generic;
using SFML.Window;
using static SFML.Window.Keyboard;

namespace PenguinGameClone
{
    public static class InputManager
    {
        private static readonly HashSet<Key> HELD_KEYS = new();
        private static readonly HashSet<Key> PRESSED_KEYS = new();
        private static readonly HashSet<Key> RELEASED_KEYS = new();

        private static readonly HashSet<Mouse.Button> HELD_BUTTONS = new();
        private static readonly HashSet<Mouse.Button> PRESSED_BUTTONS = new();
        private static readonly HashSet<Mouse.Button> RELEASED_BUTTONS = new();

        public static void NewFrame()
        {
            PRESSED_KEYS.Clear();
            RELEASED_KEYS.Clear();

            PRESSED_BUTTONS.Clear();
            RELEASED_BUTTONS.Clear();
        }

        public static void PressKey(Key key)
        {
            if (!IsKeyHeld(key))
            {
                HELD_KEYS.Add(key);
                PRESSED_KEYS.Add(key);
            }
        }

        public static void ReleaseKey(Key key)
        {
            HELD_KEYS.Remove(key);
            RELEASED_KEYS.Add(key);
        }

        public static bool IsKeyHeld(Key key)
        {
            return HELD_KEYS.Contains(key);
        }

        public static bool IsKeyPressed(Key key)
        {
            return PRESSED_KEYS.Contains(key);
        }

        public static bool IsKeyReleased(Key key)
        {
            return RELEASED_KEYS.Contains(key);
        }

        public static void PressButton(Mouse.Button button)
        {
            if (!IsButtonHeld(button))
            {
                HELD_BUTTONS.Add(button);
                PRESSED_BUTTONS.Add(button);
            }
        }

        public static void ReleaseButton(Mouse.Button button)
        {
            RELEASED_BUTTONS.Add(button);
            HELD_BUTTONS.Remove(button);
        }

        public static bool IsButtonPressed(Mouse.Button button)
        {
            return PRESSED_BUTTONS.Contains(button);
        }

        public static bool IsButtonReleased(Mouse.Button button)
        {
            return RELEASED_BUTTONS.Contains(button);
        }

        public static bool IsButtonHeld(Mouse.Button button)
        {
            return HELD_BUTTONS.Contains(button);
        }
    }
}