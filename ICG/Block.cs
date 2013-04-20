using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICG
{
	public class Block : IsometricObject
	{
	    /// <summary>
	    /// A list of features the block has.
	    /// </summary>
		public List<Feature> Features = new List<Feature>();

		public bool HasShading;
		public bool HasLighting;
		public bool HasFeature;

		public Color InfluenceColor;
		public bool ShowInfluence;

		public Color FeatureColor = Color.White;

		public Block ()
		{

		}

		public void SetIndex (int index)
		{
			Index = index;

			//First go through the blocks texture and see what it has.
			HasShading = Assets.CheckBuildingTexture (index, Blocks.SHADING);
			HasLighting = Assets.CheckBuildingTexture (index, Blocks.LIGHTING);
			HasFeature = Assets.CheckBuildingTexture (index, Blocks.FEATURE);

			//Reset some of the variables
			Buildable = true;
			Direction = Facing.North;
			Features.Clear();

			//Block
			if (index == Blocks.BLOCK) {
				//Block logic. 
				//Here we will add features and colors etc, etc.
			}

			else if (index == Blocks.ROOF1) {
				Buildable = false;
			}
			else if (index == Blocks.ROOF2) {
				Buildable = false;
			}
			else if (index == Blocks.ROOF3) {
				Buildable = false;
			}
			else if (index == Blocks.ROOF4) {
				Buildable = false;
			}
		}

		public void Draw(SpriteBatch sb, Camera c)
		{
			if(Index == Blocks.AIR)
				return;

			Rectangle newdrawrect = DrawRect;
			newdrawrect.X -= c.Position.X;
			newdrawrect.Y -= c.Position.Y;

			Color color = ShowInfluence ? InfluenceColor : Color;

			sb.Draw(Assets.GetBuildingTexture(Index, Blocks.BASE), 
			        newdrawrect, 
			        null, 
			        color, 
			        0f, Vector2.Zero, SpriteEffect, 0f);

			//Draw Features Here
			if(HasShading)
				sb.Draw(Assets.GetBuildingTexture(Index, Blocks.SHADING), 
				        newdrawrect, 
				        null, 
				        Color.White * Blocks.SHADEALPHA, 
				        0f, Vector2.Zero, SpriteEffect, 0f);
			if(HasLighting)
				sb.Draw(Assets.GetBuildingTexture(Index, Blocks.LIGHTING), 
				        newdrawrect, 
				        null, 
				        Color.White * Blocks.LIGHTALPHA, 
				        0f, Vector2.Zero, SpriteEffect, 0f);
			if(HasFeature)
				sb.Draw(Assets.GetBuildingTexture(Index, Blocks.FEATURE), 
				        newdrawrect, null, FeatureColor);
		}
	}
}

