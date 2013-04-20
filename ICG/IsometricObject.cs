using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace ICG
{
	public class IsometricObject
	{
		public bool Buildable = true;
		public int Index;
		public Rectangle DrawRect;
		public Color Color;
		public Facing Direction = Facing.North;
		public bool FlipHorizontal;
		public bool FlipVectical;
		public SpriteEffects SpriteEffect {
			get {
				if(FlipHorizontal && !FlipVectical)
					return SpriteEffects.FlipHorizontally;
				else if (FlipVectical && !FlipHorizontal)
					return SpriteEffects.FlipVertically;
				else if (!FlipHorizontal && !FlipVectical)
					return SpriteEffects.None;
				else{
					// The pipe symbol is a combiner to combine two flags.
					return SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
				}
			}
		}



		public Vector2 Position {
			get {
				return new Vector2 (DrawRect.X, DrawRect.Y);
			}
			set {
				DrawRect.X = (int)value.X;
				DrawRect.Y = (int)value.Y;
			}
		}

		public IsometricObject ()
		{
		}

		public void ChangeDirection (Facing f)
		{
			Direction = f;
			if (f == Facing.North) {
				FlipHorizontal = false;
				FlipVectical = false;
			}
			else if (f == Facing.West) {
				FlipHorizontal = true;
				FlipVectical = false;
			}
			else if (f == Facing.East) {
				FlipHorizontal = false;
				FlipVectical = true;
			}
			else if (f == Facing.South) {
				FlipHorizontal = true;
				FlipVectical = true;
			}
		}
	}
}

