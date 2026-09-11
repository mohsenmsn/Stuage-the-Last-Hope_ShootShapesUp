using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShootShapesUp
{
    static class Input
    {
        private static KeyboardState keyboardState, lastKeyboardState;
        private static MouseState mouseState, lastMouseState;
        private static GamePadState gamepadState, lastGamepadState;

        public static Vector2 MousePosition { get { return new Vector2(mouseState.X, mouseState.Y); } }

        public static void Update()
        {
            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;
            lastGamepadState = gamepadState;

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            gamepadState = GamePad.GetState(PlayerIndex.One);
        }

        public static bool WasKeyPressed(Keys key)
        {
            return lastKeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key);
        }

        public static bool WasButtonPressed(Buttons button)
        {
            return lastGamepadState.IsButtonUp(button) && gamepadState.IsButtonDown(button);
        }

        public static bool IsFiring()
        {
            return mouseState.LeftButton == ButtonState.Pressed
                || keyboardState.IsKeyDown(Keys.LeftControl)
                || keyboardState.IsKeyDown(Keys.RightControl)
                || gamepadState.Triggers.Right > 0.3f
                || gamepadState.IsButtonDown(Buttons.A)
                || gamepadState.IsButtonDown(Buttons.RightShoulder);
        }

        public static bool WasConfirmPressed()
        {
            return WasKeyPressed(Keys.Space)
                || WasKeyPressed(Keys.Enter)
                || WasButtonPressed(Buttons.A)
                || WasButtonPressed(Buttons.Start);
        }

        public static bool WasMenuUpPressed()
        {
            return WasKeyPressed(Keys.Up) || WasKeyPressed(Keys.W) || WasButtonPressed(Buttons.DPadUp);
        }

        public static bool WasMenuDownPressed()
        {
            return WasKeyPressed(Keys.Down) || WasKeyPressed(Keys.S) || WasButtonPressed(Buttons.DPadDown);
        }

        public static Vector2 GetMovementDirection()
        {
            Vector2 direction = gamepadState.ThumbSticks.Left;
            direction.Y *= -1;

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                direction.X -= 1;
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                direction.X += 1;
            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
                direction.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
                direction.Y += 1;

            if (direction.LengthSquared() > 1)
                direction.Normalize();

            return direction;
        }

        public static Vector2 GetAimDirection()
        {
            Vector2 stick = gamepadState.ThumbSticks.Right;
            stick.Y *= -1;
            if (stick.LengthSquared() > 0.09f)
                return Vector2.Normalize(stick);

            Vector2 direction = MousePosition - PlayerShip.Instance.Position;
            if (direction.LengthSquared() < 1f)
                return Vector2.Zero;

            return Vector2.Normalize(direction);
        }
    }
}
