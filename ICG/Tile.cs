using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace ICG
{
	public class Tile : IsometricObject
	{
		public Color InfluenceColor;
		public bool ShowInfluence;

		public Tile ()
		{

		}

		public void SetIndex (int index)
		{
			if (index < Tiles.NONE)
				throw new Exception ("Index cannot be less than -1");

			///Here we will set index specific things. If you want to change the index directly you can do so but,
			///But things like color and special properties might not be respected if you do.
			Index = index;

			//Reset variables
			Buildable = false;
			Color = Color.White;

			//Blank
			if (index == Tiles.BLANK) {
				Color = Color.White;
			}
			//Dirt
			else if (index == Tiles.DIRT) {
				Color = Tiles.DirtColor;
				Buildable = true;
			}

			//Grass
			else if (index == Tiles.GRASS) {
				Color = Tiles.GrassColor;
				Buildable = true;
			}

			//Sand
			else if (index == Tiles.SAND) {
				Color = Tiles.SandColor;
				Buildable = false;
			}

			//Water
			else if (index == Tiles.WATER) {
				Color = Tiles.WaterColor;
				Buildable = false;
			} 

			//Road
			else if (index == Tiles.ROAD) {
				Buildable = false;
			}

			//Road2way
			else if (index == Tiles.CORNER) {
				Buildable = false;
			}

			//Road2way2
			else if (index == Tiles.CORNER2) {
				Buildable = false;
			}

			//Road3way
			else if (index == Tiles.ROAD3WAY) {
				Buildable = false;
			}

			//Road4way
			else if (index == Tiles.ROAD4WAY) {
				Buildable = false;
			}



			else
				throw new Exception("Tile index does not currently exist!");

		}

		public void Draw(SpriteBatch sb, Camera c)
		{
			Rectangle newdrawrect = DrawRect;
			newdrawrect.X -= c.Position.X;
			newdrawrect.Y -= c.Position.Y;

			Color color = ShowInfluence ? InfluenceColor : Color;

			if(Index != Tiles.NONE)
				sb.Draw(Assets.GetTileTexture(Index), newdrawrect, null, color, 0f, Vector2.Zero, SpriteEffect, 0f);
		}
	}
}

