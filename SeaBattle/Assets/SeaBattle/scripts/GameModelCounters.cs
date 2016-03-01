using UnityEngine;
using System.Collections;

namespace SeaBattle
{

	[System.Serializable]
	public class GameModelCounters
	{


		public void ChangeCounter (BattleUnit battleUnit, int delta)
		{
			if (battleUnit.kind == BattleUnit.Kind.ship) {
				if (battleUnit.shape == BattleUnit.Shape.one)
					ships1 += delta;
				if (battleUnit.shape == BattleUnit.Shape.two)
					ships2 += delta;
				if (battleUnit.shape == BattleUnit.Shape.three)
					ships3 += delta;
				if (battleUnit.shape == BattleUnit.Shape.four)
					ships4 += delta;
			}

			if (battleUnit.kind == BattleUnit.Kind.tank) {
				if (battleUnit.shape == BattleUnit.Shape.two)
					tanks2 += delta;
				if (battleUnit.shape == BattleUnit.Shape.three)
					tanks3 += delta;
				if (battleUnit.shape == BattleUnit.Shape.four)
					tanks4 += delta;
			}

			if (battleUnit.kind == BattleUnit.Kind.plane) {
				planes += delta;
			}

			Game.instance.OnModelChange ();

		}



		public int ships1;
		public int ships2;
		public int ships3;
		public int ships4;

		public int tanks2;
		public int tanks3;
		public int tanks4;

		public int planes;

		public int UnitsToPlaceCount() {
			return ships1 + ships2 + ships3 + ships4 + tanks2 + tanks3 + tanks4 + planes; 
		}

		public int unitsTilesCount ;

		public void Clear ()
		{
			
			ships1 = 4;	
			ships2 = 3;	
			ships3 = 2;	
			ships4 = 1;	

			tanks2 = 3;
			tanks3 = 2;
			tanks4 = 1;

			planes = 2;

			unitsTilesCount = (ships1 * 1) + (ships2 * 2) + (ships3 * 3) + (ships4 * 4) + (tanks2 * 2) + (tanks3 * 3) + (tanks4 * 4) + (planes * 4);
		}

	}
}