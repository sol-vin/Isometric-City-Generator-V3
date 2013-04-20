using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
namespace ICG
{
	public class IsometricFactory
	{
		Block[,,] _blocks;
		Tile[,] _tiles;
		Point _influencepoint;
		int _largestinfluencedistance = 0;
		public bool InfluenceViewIsOn { get; private set; }

		public Facing Direction = Facing.North;

		public IsometricFactory ()
		{
			_tiles = new Tile[Game1.MAXX, Game1.MAXY];
			_blocks = new Block[Game1.MAXX, Game1.MAXY, Game1.MAXZ];
		}
		/// <summary>
		/// Generates the grid.
		/// </summary>
		public void GenerateGrid ()
		{
			_tiles = new Tile[Game1.MAXX, Game1.MAXY];
			Vector2 spacing = new Vector2 (Tiles.GRIDWIDTH/ 2f, Tiles.GRIDHEIGHT / 2f);
			spacing.X = (float)Math.Round (spacing.X, 0);
			spacing.Y = (float)Math.Round (spacing.Y, 0);

			//Our point of influence to make the center of the city.
			_influencepoint = new Point (Assets.Random.Next (0, Game1.MAXX), 
			                            Assets.Random.Next (0, Game1.MAXY));


			List<Point> corners = new List<Point> () {new Point(0,0), new Point(0, _tiles.GetUpperBound(1)), 
				new Point(_tiles.GetUpperBound(0), 0), new Point(_tiles.GetUpperBound(0), _tiles.GetUpperBound(1))}; 

			int largest = 0;
			foreach (Point c in corners) {
				int distance = GetInfluenceDistance(c);
				if(distance > largest)
					largest = distance;
			}
			_largestinfluencedistance = largest;

			for (int x = 0; x < Game1.MAXX; x++) {
				for (int y = 0; y < Game1.MAXY; y++) {
					Tile t = new Tile();
					t.SetIndex(Tiles.GRASS);
					t.DrawRect = GetTileDrawRect(x,y);
					t.ShowInfluence = InfluenceViewIsOn;
					t.InfluenceColor = new Color ( 1f * GetInfluencePercent(new Point(x,y)),1f - GetInfluencePercent (new Point(x, y)),1f - GetInfluencePercent (new Point(x, y)));
					_tiles[x,y] = t;
				}
			}


		}

		public void GenerateRoads ()
		{
			const int FLIPHOR = 0;
			const int FLIPVER = 1;

			//The frequency at which a newroad can generate
			//Point roadfreq = new Point (Game1.MAXX / 10, Game1.MAXY / 10);
			//TODO: Delete test value
			Point roadfreq = new Point (5, 5);

			//List of used points
			List<int> used = new List<int> ();
			for (int j = 0; j < roadfreq.X; j++) {
				//Grabs a random number and ensures it has no adjacent roads.
				int random;
				do{
					random = Assets.Random.Next(0, Game1.MAXX);
				}while (used.Contains (random) ||used.Contains (random+1) ||used.Contains (random-1)); 
				used.Add (random);

				//Whether the starting side will be x or y
				bool side = Assets.RandomBool();
				bool curves = false; //Assets.RandomBool();
				int curvespot = Assets.Random.Next(3,Game1.MAXY);
				int curvedirection = Assets.Random.Next(-Game1.MAXX, Game1.MAXX);

				for(int i = 0; i < (side ? Game1.MAXX : Game1.MAXY); i++)
				{
					int x,y;
					if(side){
						x = random;
						y = i;
					}
					else{
						x = i;
						y = random;
					}

					if(_tiles[x,y].Index == Tiles.ROAD && Assets.Random.Next(0,2) == 1){
 							break;
					}
					if(!curves){
						_tiles[x,y].SetIndex(Tiles.ROAD);
					}

					else{
						//Add a curve in the road.

					}

					CheckRoadSanity();
				}
			}
		}

		public void CheckRoadSanity ()
		{
			Facing[] neighbordirections;
			bool[] flip;
			for (int x = 0; x < Game1.MAXX; x++) {
				for(int y = 0; y < Game1.MAXY; y++){
					foreach(int r in Tiles.Roads){
						if(_tiles[x,y].Index == r){
							neighbordirections = FindTileNeighborDirections (x, y, Tiles.Roads);
							flip = GetRoadFlip (neighbordirections);

							_tiles [x, y].FlipHorizontal = flip [0];
							_tiles [x, y].FlipVectical = flip [1];
							_tiles [x, y].SetIndex (GetRoadIndex (neighbordirections));
							}
					}
				}
			}
		}

		/// <summary>
		/// Gets the tile index according to the number of specified neighbor roads
		/// </summary>
		/// <returns>
		/// The index.
		/// </returns>
		/// <param name='f'>
		/// The directions of neighboring roads.
		/// </param>
		public int GetRoadIndex (Facing[] f)
		{
			if(f.Count() > 4)
				throw new Exception("There cannot be more than 4 possible neighbors");
			else if(f.Count() == 4)
				return Tiles.ROAD4WAY;
			else if(f.Count() == 3)
				return Tiles.ROAD3WAY;
			else if(f.Count() == 2){
				if((f.Contains(Facing.North) && f.Contains(Facing.West)) || (f.Contains(Facing.South) && f.Contains(Facing.East)))
					return Tiles.CORNER;
				else if((f.Contains(Facing.South) && f.Contains(Facing.West)) || (f.Contains(Facing.North) && f.Contains(Facing.East)))
					return Tiles.CORNER2;
				else if((f.Contains(Facing.North) && f.Contains(Facing.South)) || (f.Contains(Facing.West) && f.Contains(Facing.East)))
					return Tiles.ROAD;
			}
			//If f.Count == 1 or 0
			return Tiles.ROAD;
		}

		//TODO: Fix the road flip values to reflect Facing North = x--
		public bool[] GetRoadFlip (Facing[] f)
		{
			if (f.Count() > 4)
				throw new Exception ("Cannot have more than 4 possible neighbors!");
			//If we know its a 4way.
			else if (f.Count() == 4)
				return new bool[]{false,false};

			//Check the 3ways
			else if (f.Count() == 3) {
				if (f.Contains (Facing.North) && f.Contains (Facing.West) && f.Contains (Facing.East))
					return new bool[]{true,true};
				else if (f.Contains (Facing.North) && f.Contains (Facing.West) && f.Contains (Facing.South))
					return new bool[]{true,false};
				else if (f.Contains (Facing.North) && f.Contains (Facing.South) && f.Contains (Facing.East))
					return new bool[]{false,true};
				else if (f.Contains (Facing.West) && f.Contains (Facing.South) && f.Contains (Facing.East))
					return new bool[]{false,false};
			}

			//Check the corners and roads
			else if (f.Count() == 2) {
				//Corner
				if (f.Contains (Facing.North) && f.Contains (Facing.West))
					return new bool[]{false,false};
				else if (f.Contains (Facing.South) && f.Contains (Facing.East))
					return new bool[]{false,true};

				//Corner2
				else if (f.Contains (Facing.North) && f.Contains (Facing.East))
					return new bool[]{false,false};
				else if (f.Contains (Facing.South) && f.Contains (Facing.West))
					return new bool[]{true,false};

				//Roads
				else if (f.Contains (Facing.North) && f.Contains (Facing.South))
					return new bool[]{false,false};
				else if (f.Contains (Facing.East) && f.Contains (Facing.West))
					return new bool[]{false,true};
			}
			else if (f.Count() == 1) {
				if (f.Contains (Facing.North) || f.Contains (Facing.South))
					return new bool[]{false,false};
				else if (f.Contains (Facing.West) || f.Contains (Facing.East))
					return new bool[]{false,true};
			}
			else if(f.Count () == 0)
				return new bool[]{false,false};
			throw new Exception("Array was malformed, contained duplicates, or just plain wrong.");
		}

		/// <summary>
		/// Gets the neighboring points of x,y.
		/// </summary>
		/// <returns>
		/// The neighboring points.
		/// </returns>
		/// <param name='x'>
		/// X.
		/// </param>
		/// <param name='y'>
		/// Y.
		/// </param>
		public List<Point> GetNeighboringPoints (int x, int y)
		{
			if (x < 0 || x >= Game1.MAXX || y < 0 || y >= Game1.MAXY)
				throw new Exception (x + "," + y + "is not valid point!");
			List<Point> neighbors = new List<Point> ();

			if (x != 0)
				neighbors.Add (new Point (x - 1, y));
			if (x != Game1.MAXX - 1)
				neighbors.Add (new Point (x + 1, y));

			if (y != 0)
				neighbors.Add (new Point (x, y - 1));
			if (y != Game1.MAXY - 1)
				neighbors.Add (new Point (x, y + 1));
			return neighbors;

		}

		/// <summary>
		/// Finds the number of neighbors to cell x,y with index.
		/// </summary>
		/// <returns>
		/// The neighbors.
		/// </returns>
		/// <param name='x'>
		/// X.
		/// </param>
		/// <param name='y'>
		/// Y.
		/// </param>
		/// <param name='index'>
		/// Index.
		/// </param>
		public int FindTileNeighborsCount (int x, int y, int index)
		{
			int answer = 0;
			List<Point> neighbors = GetNeighboringPoints(x,y);
			foreach (Point n in neighbors) 
				if(_tiles[n.X,n.Y].Index == index)
					answer++;
			return answer;
		}

		public int FindTileNeighbors (int x, int y, int[] index)
		{
			int answer = 0;
			List<Point> neighbors = GetNeighboringPoints(x,y);
			foreach(int i in index)
				foreach (Point n in neighbors) 
					if(_tiles[n.X,n.Y].Index == i)
						answer++;
			return answer;
		}

		/// <summary>
		/// Finds the neighbor directions in relation to x,y.
		/// </summary>
		/// <returns>
		/// The neighbor directions.
		/// </returns>
		/// <param name='x'>
		/// X.
		/// </param>
		/// <param name='y'>
		/// Y.
		/// </param>
		/// <param name='index'>
		/// Index.
		/// </param>
		public Facing[] FindTileNeighborDirections (Point p, int index)
		{
			List<Facing> answer = new List<Facing> ();
			List<Point> neighbors = GetNeighboringPoints (p.X, p.Y);
			Facing f;
			foreach (Point n in neighbors) 
				if (_tiles [n.X, n.Y].Index == index) {
					if(n == p.Add(Assets.FacingDirection(Facing.North))) answer.Add (Facing.North);
					else if(n == p.Add(Assets.FacingDirection(Facing.South))) answer.Add (Facing.South);
					else if(n == p.Add(Assets.FacingDirection(Facing.East))) answer.Add (Facing.East);
					else if(n == p.Add (Assets.FacingDirection(Facing.West))) answer.Add (Facing.West);
				}
			return answer.ToArray();;
		}

		public Facing[] FindTileNeighborDirections (int x, int y, int[] index)
		{
			List<Facing> answer = new List<Facing>();
			List<Point> neighbors = GetNeighboringPoints(x,y);
			foreach(int i in index)
				foreach (Point n in neighbors) 
					if (_tiles [n.X, n.Y].Index == i) {
						if(n.Y == y-1) answer.Add (Facing.North);
						else if(n.Y == y+1) answer.Add (Facing.South);
						else if(n.X == x+1) answer.Add (Facing.East);
						else if(n.X == x-1) answer.Add (Facing.West);
				}
			return answer.ToArray();
		}

		/// <summary>
		/// Generates the blocks.
		/// </summary>
		public void GenerateBlocks ()
		{
			//Clear the blocks
			_blocks = new Block[Game1.MAXX,Game1.MAXY,Game1.MAXZ];
			for (int x = 0; x < Game1.MAXX; x++) {
				for (int y = 0; y < Game1.MAXY; y++) {
					for (int z = 0; z < Game1.MAXZ; z++) {
						var b = new Block();
						b.SetIndex(Blocks.AIR);
						b.ShowInfluence = InfluenceViewIsOn;
						b.InfluenceColor = new Color ( 1f * GetInfluencePercent(new Point(x,y)),1f - GetInfluencePercent (new Point(x, y)),1f - GetInfluencePercent (new Point(x, y)));
						_blocks[x,y,z] = b;
					}
				}
			}

			//Generate buildings
			for (int y = 0; y < Game1.MAXY; y++) {
				for (int x = 0; x < Game1.MAXX; x++) {
					float influencepercent = GetInfluencePercent(new Point(x,y));
					Influence influence = Assets.Influence.Rural;
					foreach(Influence i in Assets.Influence.Influences)
					{
						//Go through the list of influences and compare the land coverage percent to the 
						//distance from the influence point. The lowest one that fits in our number is 
						//one 
						if(influencepercent < i.LandCoverage && i.LandCoverage < influence.LandCoverage)
							influence = i;
					}

					//Now we will try to build our building
					if(_tiles[x,y].Buildable)
						if(Assets.Random.Next(0,100) < influence.BuildRate)
							GenerateBuilding(x,y, influence);						
				}
			}
		}

		/// <summary>
		/// Generates the test pattern.
		/// </summary>
		public void GenerateTestPattern ()
		{
			//Clear the array
			ClearBlocks();
			List<Point> points = new List<Point> ();
			points.Add(new Point(2,2));
			points.Add(new Point(2,2).Add(Assets.FacingDirection(Facing.North)));
			points.Add(new Point(2,2).Add(Assets.FacingDirection(Facing.East)));
			points.Add(new Point(2,2).Add(Assets.FacingDirection(Facing.West)));
			points.Add(new Point(2,2).Add(Assets.FacingDirection(Facing.South)));

			Dictionary<Point, Color> colors = new Dictionary<Point, Color>(){
				{ points[0], Color.White }, //Center Point
				{ points[1], Color.Red }, //North
				{ points[2], Color.Orange }, //East
				{ points[3], Color.Green }, //West
				{ points[4], Color.Yellow }  //South
			};

			//To get a color
			int i = 0;
			foreach (Point p in points) {
				Color c = colors[p];
				for(int z = 0; z < 6; z++){
					Block b = new Block();
					b.SetIndex(Blocks.BLOCK);
					b.Color = c;
					b.InfluenceColor = c;
					b.DrawRect = 
						new Rectangle(_tiles[p.X,p.Y].DrawRect.X, 					                                                         
						              _tiles[p.X,p.Y].DrawRect.Y - (Blocks.BLOCKHEIGHT*(z+1)), 
						              31,31);
					_blocks[p.X,p.Y,z] = b;
				}
				i++;
			}
		}

		/// <summary>
		/// Generates the building.
		/// </summary>
		/// <param name='x'>
		/// X.
		/// </param>
		/// <param name='y'>
		/// Y.
		/// </param>
		/// <param name='i'>
		/// The influence of the area
		/// </param>
		public void GenerateBuilding (int x, int y, Influence i)
		{
			//Get a random color
			Color c = Assets.GetRandomBlockColor();

			//Find max building height
			int buildheight = i.GetRandomBuildingHeight ();
			for (int z = 0; z < buildheight; z++) {
				_blocks[x,y,z].SetIndex(Blocks.BLOCK);
				_blocks[x,y,z].Color = c;
				_blocks[x,y,z].DrawRect = new Rectangle(_tiles[x,y].DrawRect.X, _tiles[x,y].DrawRect.Y - (Blocks.BLOCKHEIGHT*(z+1)), 31,31);
			}
		}

		/// <summary>
		/// Draws the grid.
		/// </summary>
		/// <param name='sb'>
		/// Sb.
		/// </param>
		/// <param name='c'>
		/// C.
		/// </param>
		public void DrawGrid (SpriteBatch sb, Camera c)
		{
			for (int x = 0; x < Game1.MAXX; x++) {
				for (int y = 0; y < Game1.MAXY; y++) {
					_tiles [x, y].Draw (sb, c);
				}
			}
		}

		/// <summary>
		/// Draws the blocks.
		/// </summary>
		/// <param name='sb'>
		/// Sb.
		/// </param>
		/// <param name='c'>
		/// C.
		/// </param>
		public void DrawBlocks (SpriteBatch sb, Camera c)
		{
			for (int x = 0; x < Game1.MAXX; x++) {
				for (int y = 0; y < Game1.MAXY; y++) {
					for (int z = 0; z < Game1.MAXZ; z++) {
						_blocks [x, y, z].Draw (sb, c);
					}
				}
			}
		}

		/// <summary>
		/// Gets the influence distance.
		/// </summary>
		/// <returns>
		/// The influence distance.
		/// </returns>
		/// <param name='p'>
		/// P.
		/// </param>
		public int GetInfluenceDistance(Point p)
		{
			int x = Math.Abs(p.X - _influencepoint.X);
			int y = Math.Abs(p.Y - _influencepoint.Y);

			int d = (int)Math.Sqrt(x+y);
			return d;
		}

		/// <summary>
		/// Gets the percent of the land covered of a specific point to the influence point.
		/// </summary>
		/// <returns>
		/// The influence percent.
		/// </returns>
		/// <param name='p'>
		/// P.
		/// </param>
		public float GetInfluencePercent(Point p)
		{
			return GetInfluenceDistance(p) / (float)_largestinfluencedistance;
		}

		//TODO: Flip tiles and blocks to match rotations.

		/// <summary>
		/// Rotates the city to the right.
		/// </summary>
		public void RotateRight ()
		{
			//To find the distance each piece of the grid should be to connect it together
			Vector2 spacing = new Vector2 (Tiles.GRIDWIDTH / 2f, Tiles.GRIDHEIGHT / 2f);
			spacing.X = (float)Math.Round (spacing.X, 0);
			spacing.Y = (float)Math.Round (spacing.Y, 0);

			//Direction for the orientation of the city. Is used to draw features correctly.
			Direction.RotateRight();
			Camera.Direction.RotateRight();
			Block[,,] blockresult = new Block[Game1.MAXX, Game1.MAXY, Game1.MAXZ];
			Tile[,] tileresult = new Tile[Game1.MAXX, Game1.MAXY];

			for (int y = 0; y < Game1.MAXY; y++)
				for (int x = 0; x < Game1.MAXX; x++) {
					//Rotate the tiles
					tileresult [x, y] = _tiles [Game1.MAXY - 1 - y, x];
					//Recalculate the drawrect.
					tileresult [x, y].DrawRect = GetTileDrawRect(x,y);

					//Rotate the blocks
					for (int z = 0; z < Game1.MAXZ; z++) {
						blockresult [x, y, z] = _blocks [Game1.MAXY - 1 - y, x, z];

					//Recalc block position
					blockresult[x,y,z].DrawRect = new Rectangle(
						tileresult[x,y].DrawRect.X, 
					    tileresult[x,y].DrawRect.Y - (Blocks.BLOCKHEIGHT*(z+1)), 31,31);
					}
				}
			_blocks =  (Block[,,])blockresult.Clone();
			_tiles = (Tile[,])tileresult.Clone();
			CheckRoadSanity();
	    }	 

		/// <summary>
		/// Rotates the city to the left.
		/// </summary>
	    public void RotateLeft()
	    {
			Vector2 spacing = new Vector2 (Tiles.GRIDWIDTH / 2f, Tiles.GRIDHEIGHT / 2f);
			spacing.X = (float)Math.Round (spacing.X, 0);
			spacing.Y = (float)Math.Round (spacing.Y, 0);
			Direction.RotateLeft();
			Camera.Direction.RotateRight();
			Block[,,] blockresult = new Block[Game1.MAXX, Game1.MAXY, Game1.MAXZ];
			Tile[,] tileresult = new Tile[Game1.MAXX, Game1.MAXY];

			for (int y = 0; y < Game1.MAXY; y++)
				for (int x = 0; x < Game1.MAXX; x++) {
					//Rotate the tiles
					tileresult [x, y] = _tiles [y, Game1.MAXX - 1 - x];
					//Recalculate the drawrect.
					tileresult [x, y].DrawRect = GetTileDrawRect(x,y);

					//Rotate the blocks
					for (int z = 0; z < Game1.MAXZ; z++) {
					blockresult [x, y, z] = _blocks [y, Game1.MAXX - 1 - x, z];

					//Recalc block position
					blockresult[x,y,z].DrawRect = new Rectangle(
						tileresult[x,y].DrawRect.X, 
					    tileresult[x,y].DrawRect.Y - (Blocks.BLOCKHEIGHT*(z+1)), 31,31);
					}
				}
			_blocks =  (Block[,,])blockresult.Clone();
			_tiles = (Tile[,])tileresult.Clone();
			CheckRoadSanity();
	    }

		/// <summary>
		/// Shows the influence map
		/// </summary>
		public void ShowInfluence ()
		{
			InfluenceViewIsOn = true;
			foreach (Tile t in _tiles)		
				t.ShowInfluence = true;
			
			foreach(Block b in _blocks)
				b.ShowInfluence = true;
		}

		/// <summary>
		/// Shows the influence map
		/// </summary>
		public void HideInfluence()
		{
			InfluenceViewIsOn = false;
			foreach (Tile t in _tiles)		
				t.ShowInfluence = false;
			
			foreach(Block b in _blocks)
				b.ShowInfluence = false;
		}

		public Rectangle GetTileDrawRect (int x, int y)
		{
			Vector2 spacing = new Vector2 (Tiles.GRIDWIDTH/ 2f, Tiles.GRIDHEIGHT / 2f);
			spacing.X = (float)Math.Round (spacing.X, 0);
			spacing.Y = (float)Math.Round (spacing.Y, 0);
			return new Rectangle((int)((-x * spacing.X) + (y*spacing.X) - y + x + 5),
					                           (int)((x * spacing.Y) + (y*spacing.Y) - y - x + 250),
					                           Tiles.GRIDWIDTH,
					                           Tiles.GRIDHEIGHT);
		}

		/// <summary>
		/// Clears the blocks.
		/// </summary>
		public void ClearBlocks()
		{
			//Clear the blocks
			_blocks = new Block[Game1.MAXX,Game1.MAXY,Game1.MAXZ];
			for (int x = 0; x < Game1.MAXX; x++) {
				for (int y = 0; y < Game1.MAXY; y++) {
					for (int z = 0; z < Game1.MAXZ; z++) {
						var b = new Block();
						b.SetIndex(Blocks.AIR);
						_blocks[x,y,z] = b;
					}
				}
			}
		}

	}
}

