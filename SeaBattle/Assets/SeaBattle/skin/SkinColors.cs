using UnityEngine;
using System.Collections;

namespace SeaBattle
{
	public class SkinColors : MonoBehaviour
	{

		public Color land;
		public Color water;

		public Color plane;
		public Color tank;
		public Color ship;

		public Color activeTile;
		public Color preocuppiedTile;

		public Color activeBattleUnit;

		public Color ColorFor (BattleUnit unit)
		{
			switch (unit.kind) {
			case BattleUnit.Kind.tank:
				return tank;
			case BattleUnit.Kind.ship:
				return ship;
			case BattleUnit.Kind.plane:
				return plane;
			}

			return ship;

		}


	}
}