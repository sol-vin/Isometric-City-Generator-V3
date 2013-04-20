using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ICG
{
	public enum Facing {North, West, South, East}
	public struct Tiles
	{
		public const int NONE = -1;
		public const int BLANK = 0;
		public const int DIRT = 1;
		public const int GRASS = 2;
		public const int SAND = 3;
		public const int WATER = 4;
		public const int ROAD = 5;
		public const int CORNER = 6;
		public const int CORNER2 = 7;
		public const int ROAD3WAY = 8;
		public const int ROAD4WAY = 9;

		public const int GRIDHEIGHT = 15;
		public const int GRIDWIDTH = 31;

		//Colors
		public static Color DirtColor{
			get{
				return new Color(148,91,0);
			}
		}

		public static Color GrassColor{
			get{
				return new Color(83,217,76);
			}
		}

		public static Color SandColor{
			get{
				return new Color(245,245,184);
			}
		}

		public static Color WaterColor{
			get{
				return Color.DodgerBlue;
			}
		}

		//Collections
		public static int[] Roads = new int[]{Tiles.ROAD, Tiles.CORNER, Tiles.CORNER2, Tiles.ROAD3WAY, Tiles.ROAD4WAY};
	}
	public struct Blocks
	{
		//Indexes
		public const int AIR = -1;
		public const int BLOCK = 0;
		public const int ROOF1 = 1;
		public const int ROOF2 = 2;
		public const int ROOF3 = 3;
		public const int ROOF4 = 4;
		//Subindexes
		public const int BASE = 0;
		public const int SHADING = 1;
		public const int LIGHTING = 2;
		public const int FEATURE = 3;

		public const int BLOCKHEIGHT = 16;
		public const float SHADEALPHA = .2f;
		public const float LIGHTALPHA = .14f;
	}
	public struct Features
	{
		public const int NONE = -1;
		public const int DOOR1 = 0;
		public const int DOOR2 = 1;
		public const int DOOR3 = 2;
		public const int DOOR4 = 3;
		public const int DOOR5 = 4;
		public const int WINDOW1 = 5;
		public const int WINDOW2 = 6;
		public const int WINDOW3 = 7;
		public const int WINDOW4 = 8;
	}

	public static class Assets
	{
		public static Random Random = new Random();
		private static List<Color> _blockcolors = new List<Color>();
		private static List<Facing> _facing = new List<Facing>() {Facing.North, Facing.West, 
			Facing.South, Facing.East};
		private static Dictionary<Facing, Point> _facingdirections = new Dictionary<Facing, Point>()
		{
			{Facing.North, new Point(1,0)},
			{Facing.South, new Point(-1,0)},
			{Facing.West, new Point(0,1)},
			{Facing.East, new Point(0,-1)}
		};
		public struct Influence
		{
			public static ICG.Influence Heavy, Medium, Low, Rural;
			public static List<ICG.Influence> Influences = new List<ICG.Influence>();

			public static void InitInfluences()
			{
				Heavy = new ICG.Influence();
				Heavy.LandCoverage = Values.Heavy.LANDPERCENTAGE;
				Heavy.BuildRate = Values.Heavy.BUILDRATE;
				Heavy.TopHighBuildingHeight = Values.Heavy.TOPHIGHBUILDHEIGHT;
				Heavy.BottomHighBuildingHeight = Values.Heavy.BOTTOMHIGHBUILDHEIGHT;
				Heavy.TopLowBuildingHeight = Values.Heavy.TOPLOWBUILDHEIGHT;
				Heavy.BottomLowBuildingHeight = Values.Heavy.BOTTOMLOWBUILDHEIGHT;
				Heavy.HighBuildChance = Values.Heavy.HIGHBUILDCHANCE;

				Medium = new ICG.Influence();
				Medium.LandCoverage = Values.Medium.LANDPERCENTAGE;
				Medium.BuildRate = Values.Medium.BUILDRATE;
				Medium.TopHighBuildingHeight = Values.Medium.TOPHIGHBUILDHEIGHT;
				Medium.BottomHighBuildingHeight = Values.Medium.BOTTOMHIGHBUILDHEIGHT;
				Medium.TopLowBuildingHeight = Values.Medium.TOPLOWBUILDHEIGHT;
				Medium.BottomLowBuildingHeight = Values.Medium.BOTTOMLOWBUILDHEIGHT;
				Medium.HighBuildChance = Values.Medium.HIGHBUILDCHANCE;

				Low = new ICG.Influence();
				Low.LandCoverage = Values.Low.LANDPERCENTAGE;
				Low.BuildRate = Values.Low.BUILDRATE;
				Low.TopHighBuildingHeight = Values.Low.TOPHIGHBUILDHEIGHT;
				Low.BottomHighBuildingHeight = Values.Low.BOTTOMHIGHBUILDHEIGHT;
				Low.TopLowBuildingHeight = Values.Low.TOPLOWBUILDHEIGHT;
				Low.BottomLowBuildingHeight = Values.Low.BOTTOMLOWBUILDHEIGHT;
				Low.HighBuildChance = Values.Low.HIGHBUILDCHANCE;

				Rural = new ICG.Influence();
				Rural.LandCoverage = Values.Rural.LANDPERCENTAGE;
				Rural.BuildRate = Values.Rural.BUILDRATE;
				Rural.TopHighBuildingHeight = Values.Rural.TOPHIGHBUILDHEIGHT;
				Rural.BottomHighBuildingHeight = Values.Rural.BOTTOMHIGHBUILDHEIGHT;
				Rural.TopLowBuildingHeight = Values.Rural.TOPLOWBUILDHEIGHT;
				Rural.BottomLowBuildingHeight = Values.Rural.BOTTOMLOWBUILDHEIGHT;
				Rural.HighBuildChance = Values.Rural.HIGHBUILDCHANCE;

				Influences.Add (Heavy);
				Influences.Add (Medium);
				Influences.Add (Low);
				Influences.Add (Rural);
			}
			public struct Values
			{
				public struct Heavy
				{
					public const float LANDPERCENTAGE = .3f;
					public const int BUILDRATE = 40;
					public const int TOPHIGHBUILDHEIGHT = 7;
					public const int BOTTOMHIGHBUILDHEIGHT = 5;
					public const int TOPLOWBUILDHEIGHT = 5;
					public const int BOTTOMLOWBUILDHEIGHT = 1;
					public const int HIGHBUILDCHANCE = 80;
				}

				public struct Medium
				{
					public const float LANDPERCENTAGE = .6f;
					public const int BUILDRATE = 30;
					public const int TOPHIGHBUILDHEIGHT = 4;
					public const int BOTTOMHIGHBUILDHEIGHT = 2;
					public const int TOPLOWBUILDHEIGHT = 2;
					public const int BOTTOMLOWBUILDHEIGHT = 1;
					public const int HIGHBUILDCHANCE = 40;
				}

				public struct Low
				{
					public const float LANDPERCENTAGE = .9f;
					public const int BUILDRATE = 20;
					public const int TOPHIGHBUILDHEIGHT = 2;
					public const int BOTTOMHIGHBUILDHEIGHT = 1;
					public const int TOPLOWBUILDHEIGHT = 1;
					public const int BOTTOMLOWBUILDHEIGHT = 1;
					public const int HIGHBUILDCHANCE = 50;
				}

				public struct Rural
				{
					public const float LANDPERCENTAGE = 1f;
					public const int BUILDRATE = 5;
					public const int TOPHIGHBUILDHEIGHT = 2;
					public const int BOTTOMHIGHBUILDHEIGHT = 1;
					public const int TOPLOWBUILDHEIGHT = 1;
					public const int BOTTOMLOWBUILDHEIGHT = 1;
					public const int HIGHBUILDCHANCE = 30;
				}
			}
		}

		private static Texture2D[] _tiles = new Texture2D[10];
		private static Texture2D[,] _blocks = new Texture2D[5,4]; 
		private static Texture2D[] _features = new Texture2D[9];


		public static void LoadContent(Game game)
		{
			//Add tiles
			_tiles[Tiles.BLANK] = game.Content.Load<Texture2D>(@"Floor/Grid");
			_tiles[Tiles.DIRT] = game.Content.Load<Texture2D>(@"Floor/Grid");
			_tiles[Tiles.GRASS] = game.Content.Load<Texture2D>(@"Floor/Grid");
			_tiles[Tiles.SAND] = game.Content.Load<Texture2D>(@"Floor/Grid");
			_tiles[Tiles.WATER] = game.Content.Load<Texture2D>(@"Floor/Grid");
			_tiles[Tiles.ROAD] = game.Content.Load<Texture2D>(@"Floor/Road");
			_tiles[Tiles.CORNER] = game.Content.Load<Texture2D>(@"Floor/Road2Way");
			_tiles[Tiles.CORNER2] = game.Content.Load<Texture2D>(@"Floor/Road2Way2");
			_tiles[Tiles.ROAD3WAY] = game.Content.Load<Texture2D>(@"Floor/Road3Way");
			_tiles[Tiles.ROAD4WAY] = game.Content.Load<Texture2D>(@"Floor/Road4Way");

			//Add Blocks
			_blocks[Blocks.BLOCK, Blocks.BASE] = game.Content.Load<Texture2D>(@"Building/Block/Block");
			_blocks[Blocks.BLOCK, Blocks.SHADING] = game.Content.Load<Texture2D>(@"Building/Block/Block-Shade");
			_blocks[Blocks.BLOCK, Blocks.LIGHTING] = game.Content.Load<Texture2D>(@"Building/Block/Block-Light");

			_blocks[Blocks.ROOF1, Blocks.BASE] = game.Content.Load<Texture2D>(@"Building/Roofs/Roof1/Roof1");

			_blocks[Blocks.ROOF2, Blocks.BASE] = game.Content.Load<Texture2D>(@"Building/Roofs/Roof2/Roof2");
			_blocks[Blocks.ROOF2, Blocks.SHADING] = game.Content.Load<Texture2D>(@"Building/Roofs/Roof2/Roof2-Shade");
			_blocks[Blocks.ROOF2, Blocks.LIGHTING] = game.Content.Load<Texture2D>(@"Building/Roofs/Roof2/Roof2-Light");
			_blocks[Blocks.ROOF2, Blocks.FEATURE] = game.Content.Load<Texture2D>(@"Building/Roofs/Roof2/Roof2-Feature");

			_blocks[Blocks.ROOF3, Blocks.BASE] = game.Content.Load<Texture2D>(@"Building/Roofs/Roof3/Roof3");
			_blocks[Blocks.ROOF3, Blocks.SHADING] = game.Content.Load<Texture2D>(@"Building/Roofs/Roof3/Roof3-Shade");

			_blocks[Blocks.ROOF4, Blocks.BASE] = game.Content.Load<Texture2D>(@"Building/Roofs/Roof4/Roof4");
			_blocks[Blocks.ROOF4, Blocks.SHADING] = game.Content.Load<Texture2D>(@"Building/Roofs/Roof4/Roof4-Shade");
			_blocks[Blocks.ROOF4, Blocks.LIGHTING] = game.Content.Load<Texture2D>(@"Building/Roofs/Roof4/Roof4-Light");

			//Add Features
			_features[Features.DOOR1] = game.Content.Load<Texture2D>(@"Features/Doors/Door1");
			_features[Features.DOOR2] = game.Content.Load<Texture2D>(@"Features/Doors/Door2");
			_features[Features.DOOR3] = game.Content.Load<Texture2D>(@"Features/Doors/Door3");
			_features[Features.DOOR4] = game.Content.Load<Texture2D>(@"Features/Doors/Door4");
			_features[Features.DOOR5] = game.Content.Load<Texture2D>(@"Features/Doors/Door5");

			_features[Features.WINDOW1] = game.Content.Load<Texture2D>(@"Features/Windows/Window1");
			_features[Features.WINDOW2] = game.Content.Load<Texture2D>(@"Features/Windows/Window2");
			_features[Features.WINDOW3] = game.Content.Load<Texture2D>(@"Features/Windows/Window3");
			_features[Features.WINDOW4] = game.Content.Load<Texture2D>(@"Features/Windows/Window4");

			//Add colors
			_blockcolors.Add(Color.Beige);
            _blockcolors.Add(Color.Bisque);
            _blockcolors.Add(Color.BlanchedAlmond);
            _blockcolors.Add(Color.Khaki);
            _blockcolors.Add(Color.Moccasin);
            _blockcolors.Add(Color.NavajoWhite);
            _blockcolors.Add(Color.Sienna);
            _blockcolors.Add(Color.Tan);
            _blockcolors.Add(Color.Gray);
            _blockcolors.Add(Color.DarkGray);

			//AddInfluences
			Influence.InitInfluences();
		}

		public static Texture2D GetBuildingTexture(int index, int subindex)
		{
			if(index <= Blocks.AIR && subindex < 0)
				throw new Exception("Air does not have a texture, or there is no entry at " + index);
			return _blocks[index, subindex]; 
		}

		public static bool CheckBuildingTexture(int index, int subindex)
		{
			if(index <= Blocks.AIR) return false;
			if(_blocks[index,subindex] == null)
				return false;
			return true;
		}

		public static Texture2D GetTileTexture (int index)
		{
			if(index <= Tiles.NONE)
				throw new Exception("No tile id exists at " + index);;
			return _tiles[index];
		}

		public static bool CheckTileTexture(int index)
		{
			if(index <= Tiles.NONE) return false;
			if(_tiles[index] == null)
				return false;
			return true;
		}

		public static Color GetRandomBlockColor()
		{
			return _blockcolors[Random.Next (0, _blockcolors.Count)];
		}

		public static Facing GetRandomFacing ()
		{
			return _facing[Random.Next(0,4)];
		}

		public static void RotateLeft (this Facing f)
		{
			for (int x = 0; x < 4; x++) {
				if(_facing[x] == f && x != 3)
					f = _facing[x+1];
			}
			f = _facing[0];
		}

		public static void RotateRight(this Facing f)
		{
			for (int x = 0; x < 4; x++) {
				if(_facing[x] == f && x != 0)
					f = _facing[x-1];
			}
			f = _facing[3];
		}

		public static bool RandomBool()
		{
			return (Random.Next(0,2) == 0);
		}

		public static Point FacingDirection(Facing f)
		{
			return _facingdirections[f];
		}

		public static Point Add(this Point a, Point b)
		{
			a.X += b.X;
			a.Y += b.Y;
			return a;
		}
	}
}

