using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace TurnBasedGame.Input
{
    public class InputHandler
    {
        private readonly InputState _inputState;
        private readonly Dictionary<GameAction, Vector2> _activeActions;

        public InputHandler()
        {
            _inputState = new InputState();
            _activeActions = [];
        }

        public void Update()
        {
            _inputState.Update();
            _activeActions.Clear();

            // Handle WASD camera movement
            if (_inputState.IsKeyHeld(Keys.W))
                _activeActions[GameAction.CameraMoveUp] = Vector2.One;
            if (_inputState.IsKeyHeld(Keys.S))
                _activeActions[GameAction.CameraMoveDown] = Vector2.One;
            if (_inputState.IsKeyHeld(Keys.A))
                _activeActions[GameAction.CameraMoveLeft] = Vector2.One;
            if (_inputState.IsKeyHeld(Keys.D))
                _activeActions[GameAction.CameraMoveRight] = Vector2.One;

            // Handle zoom via R/F keys
            if (_inputState.IsKeyHeld(Keys.R))
                _activeActions[GameAction.CameraZoomIn] = Vector2.One;
            if (_inputState.IsKeyHeld(Keys.F))
                _activeActions[GameAction.CameraZoomOut] = Vector2.One;

            // Handle zoom via mouse wheel
            int scrollDelta = _inputState.CurrentMouse.ScrollWheelValue - 
                            _inputState.PreviousMouse.ScrollWheelValue;
            if (scrollDelta > 0)
                _activeActions[GameAction.CameraZoomIn] = Vector2.One;
            else if (scrollDelta < 0)
                _activeActions[GameAction.CameraZoomOut] = Vector2.One;

            // Handle camera pan via middle mouse
            if (_inputState.IsMouseButtonHeld(_inputState.CurrentMouse.MiddleButton))
            {
                _activeActions[GameAction.CameraPan] = _inputState.MouseDelta;
            }

            // Handle selection via left click
            if (_inputState.IsMouseButtonPressed(_inputState.CurrentMouse.LeftButton, 
                _inputState.PreviousMouse.LeftButton))
            {
                _activeActions[GameAction.Select] = _inputState.MousePosition;
            }
        }

        public bool IsActionActive(GameAction action) => 
            _activeActions.ContainsKey(action);

        public Vector2 GetActionValue(GameAction action) =>
            _activeActions.TryGetValue(action, out Vector2 value) ? value : Vector2.Zero;

        public Vector2 GetMousePosition() => _inputState.MousePosition;
    }
}
