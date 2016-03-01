using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SeaBattle
{

	public class BoardModel
	{

		public bool visibility;
		public Tile[,] tiles;
		public Tile lastActiveTile;
		public bool playerOneBoard;
		public int shotCount;
		public int correctCount;
		public bool correctLastShot ;

		int cols;
		int rows;

		public void Init (bool playerOneBoard)
		{

			this.playerOneBoard = playerOneBoard;
			this.cols = Game.instance.model.cols;
			this.rows = Game.instance.model.rows;
			this.tiles = new Tile[cols, rows];

			for (int col = 0; col < cols; col++)
				for (int row = 0; row < rows; row++) {
					Tile.Kind kind = Game.instance.model.TileKind (col, row); 
					tiles [col, row] = new Tile (this, new Vector2 (col, row), kind);
				}

			Clear ();

		}

		public void ClearShots () {
			for (int col = 0; col < cols; col++)
				for (int row = 0; row < rows; row++) {
					tiles [col, row].shot = false ;
				}
		}

		public void Clear ()
		{
			for (int col = 0; col < cols; col++)
				for (int row = 0; row < rows; row++) {
					tiles [col, row].Clear ();
				}

			lastActiveTile = null;
			shotCount = 0;
			correctCount = 0; 
		}

		public void Shuffle ()
		{

			Clear ();
			PlaceBattleUnits ();

		}


		private RandomList<Tile> shuffledTiles;

		public void Autoshot () {


			// przygotuj potasowaną listę tiles
			if (shuffledTiles == null) {
				shuffledTiles = new RandomList<Tile> ();
				for (int col = 0; col < cols; col++)
					for (int row = 0; row < rows; row++) {
						shuffledTiles.Add( tiles [col, row]);
					}
				shuffledTiles.Shuffle ();
			}

			Game.instance.model.ShotAtTile ( shuffledTiles.Random()  );

		}

		public void AfterShotAtTile ( Tile tile ) {
			shotCount++;
			correctLastShot = tile.occupiedBy != BattleUnit.Kind.none;

			if ( correctLastShot ) 
				correctCount++;
			
		}

		public void PlaceOneKind (List<BattleUnit>  units)
		{



			if (units.Count == 0)
				return;

			RandomList<Tile> shuffledTiles = ShuffledFreeTiles (units [0].TileKindOk (Tile.Kind.land), units [0].TileKindOk (Tile.Kind.water));


			// todomk
			int antiInfinityLoopCounter = 0; 

			while (units.Count > 0 && (antiInfinityLoopCounter < 5000)) {

				BattleUnit placingUnit = units [0];

				Tile tileForBattleUnit = shuffledTiles.Random ();
				if (TryPlaceUnit (tileForBattleUnit, placingUnit, playerOneBoard))
					units.Remove (placingUnit);
				else
					antiInfinityLoopCounter++;

			}
		}

		public void PlaceBattleUnits ()
		{
		
			List<BattleUnit> unitsToPlaced = UnitsToPlaced (BattleUnit.Kind.ship);
			PlaceOneKind (unitsToPlaced);

			unitsToPlaced = UnitsToPlaced (BattleUnit.Kind.tank);
			PlaceOneKind (unitsToPlaced);

			unitsToPlaced = UnitsToPlaced (BattleUnit.Kind.plane);
			PlaceOneKind (unitsToPlaced);

		}

		public RandomList<Tile>  ShuffledFreeTiles (bool waterOd, bool landOk)
		{
			RandomList<Tile> shuffledTiles = new RandomList<Tile> ();
			for (int col = 0; col < cols; col++)
				for (int row = 0; row < rows; row++) {

					if (!tiles [col, row].CouldBeOccupied ())
						continue;
					
					shuffledTiles.ready.Add (tiles [col, row]);

				}
			shuffledTiles.Shuffle ();
			return shuffledTiles;
		}

		public static List<BattleUnit> UnitsToPlaced (BattleUnit.Kind kind)
		{
			
			List<BattleUnit> units = new List<BattleUnit> ();

			switch (kind) {
			case BattleUnit.Kind.ship: 
				AddUnits (units, BattleUnit.Kind.ship, BattleUnit.Shape.one, 4);
				AddUnits (units, BattleUnit.Kind.ship, BattleUnit.Shape.two, 3);
				AddUnits (units, BattleUnit.Kind.ship, BattleUnit.Shape.three, 2);
				AddUnits (units, BattleUnit.Kind.ship, BattleUnit.Shape.four, 1);
				break;

			case BattleUnit.Kind.tank: 
				AddUnits (units, BattleUnit.Kind.tank, BattleUnit.Shape.two, 3);
				AddUnits (units, BattleUnit.Kind.tank, BattleUnit.Shape.three, 2);
				AddUnits (units, BattleUnit.Kind.tank, BattleUnit.Shape.four, 1);

				break;

			case BattleUnit.Kind.plane: 
				AddUnits (units, BattleUnit.Kind.plane, BattleUnit.Shape.t, 2);
				break;

			}

			return units;
		}

		public static void AddUnits (List<BattleUnit> unitsToPlaced, BattleUnit.Kind kind, BattleUnit.Shape shape, int howMany)
		{

			for (int i = 0; i < howMany; i++)
				unitsToPlaced.Add (new BattleUnit (kind, shape));

		}

		public bool TryPlaceUnit (Tile tile, BattleUnit unit, bool withView)
		{

			if (!CouldPlaceUnit (tile, unit))
				return false;

			PlaceBattleUnit (tile, unit, withView);
			return true;

		}

		public void SetClear (Tile tile, BattleUnit unit)
		{
			foreach (Vector2 point in unit.mask.points)
				SetKind (tile, point, BattleUnit.Kind.none);
		}

		public void PlaceBattleUnit (Tile tile, BattleUnit unit, bool withView)
		{

			unit.occupied = tile;

			foreach (Vector2 point in unit.mask.points)
				SetKind (tile, point, unit.kind);

			if (withView) {
				Game.instance.view.TakeBattleUnitFor (unit);
			}
		}

		public bool CouldPlaceUnit (Tile tile, BattleUnit unit)
		{
			foreach (Vector2 point in unit.mask.points) {
				if (!CouldBeOccupied (tile, point, unit))
					return false;
			}

			return true;


		}

		public bool CouldBeOccupied (Tile tile, Vector2 maskPoint, BattleUnit unit)
		{
		
			int x = (int)(tile.coordinates.x + maskPoint.x);
			int y = (int)(tile.coordinates.y + maskPoint.y);

			if (OutOfBounds (x, y))
				return false;

			return tiles [x, y].CouldBeOccupied (unit);

		}


		public void SetKind (Tile tile, Vector2 offsetFromMask, BattleUnit.Kind kind)
		{

			int col = (int)(tile.coordinates.x + offsetFromMask.x);
			int row = (int)(tile.coordinates.y + offsetFromMask.y);

			if (OutOfBounds (col, row))
				return;

			if (kind == BattleUnit.Kind.none)
				ChangeShadowAround (col, row, -1);
			else
				ChangeShadowAround (col, row, 1);

			tiles [col, row].shot = false;
			tiles [col, row].occupiedBy = kind;
			if (kind != BattleUnit.Kind.none) {
				tiles [col, row].shadows = 0;
			}



		}

		public void ChangeShadowAround (int col, int row, int delta)
		{

			// right 
			TryAddShadow (col + 1, row, delta);

			// left
			TryAddShadow (col - 1, row, delta);

			// down
			TryAddShadow (col, row - 1, delta);

			// up
			TryAddShadow (col, row + 1, delta);

			// ritht-up
			TryAddShadow (col + 1, row + 1, delta);

			// right-down
			TryAddShadow (col + 1, row - 1, delta);

			// left-up
			TryAddShadow (col - 1, row + 1, delta);

			// left-down
			TryAddShadow (col - 1, row - 1, delta);

		}

		public void TryAddShadow (int col, int row, int delta)
		{

			// no shadows outside the board 
			if (OutOfBounds (col, row))
				return;

			// no shadows when tile is ocuppied by unitBattle - other tile of the same battle unit possible
			if (tiles [col, row].IsOccupied ())
				return;
			
			tiles [col, row].shadows += delta;
			if (tiles [col, row].shadows < 0)
				tiles [col, row].shadows = 0;

		}

		public bool OutOfBounds (int col, int row)
		{
			return (col < 0) || (col >= cols) || (row < 0) || (row >= rows);
		}

		public bool InOfBounds (int col, int row)
		{
			return !OutOfBounds (col, row);
		}

		public string GetStatusText ()
		{
			string result = "";
			if (playerOneBoard)
				result = "gracz";
			else
					result ="przeciwnik";


			if (!Game.instance.model.editorMode) 
				result += " - " + StatsInfo ();

			return result;
		}

		public string StatsInfo () {
			return "strzałów: " + shotCount + " trafień: " + correctCount;
		}

	}
}