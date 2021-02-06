using System;
using System.Collections.Generic;
using SFML.Window;
using static SFML.Window.Keyboard;

namespace PenguinGameClone
{
    public static class InputManager
    {
        private static readonly HashSet<Key> HeldKeys = new();
        private static readonly HashSet<Key> PressedKeys = new();
        private static readonly HashSet<Key> ReleasedKeys = new();

        private static readonly HashSet<Mouse.Button> HeldButtons = new();
        private static readonly HashSet<Mouse.Button> PressedButtons = new();
        private static readonly HashSet<Mouse.Button> ReleasedButtons = new();

        public static void NewFrame()
        {
            PressedKeys.Clear();
            ReleasedKeys.Clear();
            
            PressedButtons.Clear();
            PressedButtons.Clear();
        }

        public static void PressKey(Key key)
        {
            if (!IsKeyHeld(key))
            {
                HeldKeys.Add(key);
                PressedKeys.Add(key);
            }
        }

        public static void ReleaseKey(Key key)
        {
            HeldKeys.Remove(key);
            ReleasedKeys.Add(key);
        }

        private static bool IsKeyHeld(Key key)
        {
            return HeldKeys.Contains(key);
        }

        public static bool IsKeyPressed(Key key)
        {
            return PressedKeys.Contains(key);
        }

        public static bool IsKeyReleased(Key key)
        {
            return ReleasedKeys.Contains(key);
        }

        public static void PressButton(Mouse.Button button)
        {
            if (!IsButtonHeld(button))
            {
                HeldButtons.Add(button);
                PressedButtons.Add(button);
            }
        }

        public static void ReleaseButton(Mouse.Button button)
        {
            ReleasedButtons.Add(button);
            HeldButtons.Remove(button);
        }

        public static bool IsButtonPressed(Mouse.Button button)
        {
            return PressedButtons.Contains(button);
        }

        public static bool IsButtonReleased(Mouse.Button button)
        {
            return ReleasedButtons.Contains(button);
        }
        
        private static bool IsButtonHeld(Mouse.Button button)
        {
            return HeldButtons.Contains(button);
        }
    }
}