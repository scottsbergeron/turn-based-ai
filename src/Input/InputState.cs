using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TurnBasedGame.Input
{
    public class InputState
    {
        public MouseState CurrentMouse { get; private set; }
        public MouseState PreviousMouse { get; private set; }
        public KeyboardState CurrentKeyboard { get; private set; }
        public KeyboardState PreviousKeyboard { get; private set; }
        public Vector2 MousePosition => new(CurrentMouse.X, CurrentMouse.Y);
        public Vector2 MouseDelta => new(
            CurrentMouse.X - PreviousMouse.X,
            CurrentMouse.Y - PreviousMouse.Y
        );

        public void Update()
        {
            PreviousMouse = CurrentMouse;
            PreviousKeyboard = CurrentKeyboard;
            CurrentMouse = Mouse.GetState();
            CurrentKeyboard = Keyboard.GetState();
        }

        public bool IsKeyPressed(Keys key) => 
            CurrentKeyboard.IsKeyDown(key) && PreviousKeyboard.IsKeyUp(key);

        public bool IsKeyHeld(Keys key) => 
            CurrentKeyboard.IsKeyDown(key);

        public bool IsMouseButtonPressed(ButtonState currentState, ButtonState previousState) =>
            currentState == ButtonState.Pressed && previousState == ButtonState.Released;

        public bool IsMouseButtonHeld(ButtonState state) =>
            state == ButtonState.Pressed;
    }
}
