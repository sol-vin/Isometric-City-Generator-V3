using System;

namespace ICG
{
	public class Influence
	{
		public float LandCoverage;
		public int BuildRate;
		public int HighBuildChance;
		public int TopHighBuildingHeight;
		public int BottomHighBuildingHeight;
		public int TopLowBuildingHeight;
		public int BottomLowBuildingHeight;

		public Influence ()
		{
		}

		public int GetRandomHighBuildingHeight()
		{
			return Assets.Random.Next(BottomHighBuildingHeight, TopHighBuildingHeight + 1);
		}

		public int GetRandomLowBuildingHeight()
		{
			return Assets.Random.Next(BottomLowBuildingHeight, TopLowBuildingHeight + 1);
		}

		public int GetRandomBuildingHeight ()
		{
				if(Assets.Random.Next (0, 100) < HighBuildChance)
					return GetRandomHighBuildingHeight();
				return GetRandomLowBuildingHeight();
		}
	}

}

