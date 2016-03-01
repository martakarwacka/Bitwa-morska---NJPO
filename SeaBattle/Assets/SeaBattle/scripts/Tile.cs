using UnityEngine;
using System.Collections;

namespace SeaBattle
{
	public class Tile
	{


		public BoardModel board;

		public BattleUnit.Kind occupiedBy;
		public int shadows ;
		public bool shot = false;
		public Kind kind;
		public Vector2 coordinates;


		public enum Kind
		{
			land,
			water
		}

		public Tile (BoardModel board, Vector2 coordinates, Kind kind)
		{

			this.board = board;
			this.coordinates = coordinates;
			this.kind = kind;
			this.Clear ();

		}


		public bool CouldBeOccupied() {
			return !IsOccupied () && (shadows == 0);	
		}

		public bool CouldBeOccupied( BattleUnit unit ) {
			return CouldBeOccupied () && unit.TileKindOk ( kind );

		}


		public bool IsOccupied() {
			return occupiedBy != BattleUnit.Kind.none;
		}

		public void Clear ()
		{
			this.occupiedBy = BattleUnit.Kind.none;
			this.shot = false;
			this.shadows = 0;
		}


	}
}