using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SeaBattle
{
	public class GameFile
	{

		public static char separator = '-';

		public static void LoadFromFile (string fileName)
		{
			string fileContent = System.IO.File.ReadAllText (fileName);

			if ("".Equals (fileContent))
				return;

			string[] units = fileContent.Split (separator);


			int number = 0;
			string kind = "";
			string shape = "";
			string maskCode = "";

				
			for (int i = 0; i < units.Length; i++) {

				if (i % 4 == 0)
					number = int.Parse (units [i]);
				else if (i % 4 == 1)
					kind = units [i];
				else if (i % 4 == 2)
					shape = units [i];
				else { // 3
					maskCode = units [i];	
					AddBattleUnit (number, kind, shape, maskCode);
				}
			}


		}


		public static void AddBattleUnit (int tileNo, string kind, string shape, string maskCode)
		{

			Vector2 coordinates = GameView.IntToCoordintates (tileNo);
			//Debug.Log ( "in file " + coords + " " + maskCode );

			BattleUnit unit = new BattleUnit (BattleUnit.ToKind( kind ), BattleUnit.ToShape( shape ));
			unit.mask = BattleUnitMask.MaskFor ( maskCode );

			Tile tile = Game.instance.model.leftBoard.tiles [(int)coordinates.x, (int)coordinates.y];

			Game.instance.model.leftBoard.PlaceBattleUnit (tile, unit, true);

		}

		public static void SaveToFile (string fileName)
		{

			string fileContent = "";
			foreach (BattleUnitView battleUnitView in GameObject.FindObjectsOfType<BattleUnitView>()) {

				// nie zapisujemy niepołożonych
				if (battleUnitView.battleUnit.occupied == null)
					continue;

				if( "".Equals(fileContent) )
				   fileContent += FileLineFrom (battleUnitView);
				else 
					fileContent += separator + FileLineFrom (battleUnitView);
				}

			System.IO.File.WriteAllText (fileName, fileContent);


		}

		private static void SaveToFile (string fileName, List<string> fileLines)
		{
			Debug.Log ("zapis do pliku " + fileName);
			foreach (string s in fileLines)
				Debug.Log (s);
		}

		private static string FileLineFrom (BattleUnitView battleUnitView)
		{

			int tileNo = GameView.CoordinatesToInt (battleUnitView.battleUnit.occupied.coordinates);
			return tileNo.ToString () + separator + BattleUnit.ToString (battleUnitView.battleUnit.kind) + separator + BattleUnit.ToString (battleUnitView.battleUnit.shape) + separator + battleUnitView.battleUnit.mask.code;

		}


	}
}