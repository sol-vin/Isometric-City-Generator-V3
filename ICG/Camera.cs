using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using InputEngine.Input;
namespace ICG
{
	public class Camera
	{
		public Point Position;
        public int Speed;
        public Rectangle ViewPort;
		public static Facing Direction = Facing.North;

        private KeyBoardInput up, down, left, right;

        public Camera(Rectangle ScreenSize)
        {
            up = new KeyBoardInput(Keys.Up);
            down = new KeyBoardInput(Keys.Down);
            left = new KeyBoardInput(Keys.Left);
            right = new KeyBoardInput(Keys.Right);

            Position = Point.Zero;
            ViewPort = ScreenSize;
            Speed = 2;
        }

        public void Update()
        {
            if (up.Down())
                Position.Y -= Speed;
            if (down.Down())
                Position.Y += Speed;
            if (left.Down())
                Position.X -= Speed;
            if (right.Down())
                Position.X += Speed;

            //Update our viewport
            ViewPort.X = Position.X;
            ViewPort.Y = Position.Y;
		}
	}
}