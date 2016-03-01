using UnityEngine;
using System.Collections;

namespace SeaBattle
{
	public class GameModel: MonoBehaviour
	{

		public int cols = 14;
		public int rows = 22;

		public GameState gameState;
		public GameResult gameResult;

		public BoardModel leftBoard;
		public BoardModel rightBoard;

		public GameModelCounters counters;

		public bool editorMode;
		public bool playerOneMoving;

		public enum GameState
		{
			playing,
			ended
		}

		public enum GameResult
		{
			inProgress,
			playerOneWon,
			playerTwoWon
		}


		public void ShotAtTile (Tile tile)
		{
			
			if (tile.IsOccupied ())
				Game.instance.audioSource.PlayOneShot (Game.instance.skin.sounds.correctShot);
			else
				Game.instance.audioSource.PlayOneShot (Game.instance.skin.sounds.incorrectShot);

			tile.shot = true;
			tile.board.AfterShotAtTile (tile);

			ChangeState (tile.board);

			OnChange ();	



		}

		public static int computerThinkingTime = 1;

		public void ChangeState (BoardModel board)
		{

			// koniec gry
			if (board.correctCount == counters.unitsTilesCount) {
				gameState = GameState.ended;
				if (board.playerOneBoard)
					gameResult = GameResult.playerTwoWon;
				else
					gameResult = GameResult.playerOneWon;

				return;
			}

			if (!board.correctLastShot)
				SwitchPlayerMoving ();

			if (!playerOneMoving)
				Invoke ("ComputerShot", computerThinkingTime);
			
		}

		private void SwitchPlayerMoving ()
		{
			playerOneMoving = !playerOneMoving;
		}

		public void ComputerShot ()
		{

			if (PlayerTwoCanShot ())
				leftBoard.Autoshot ();
		}

		public Tile.Kind TileKind (int col, int row)
		{

			// pierwsza polowa wierszy to ziemia
			if ((row < Game.instance.model.rows / 2))
				return Tile.Kind.land;

			// reszta to woda
			return  Tile.Kind.water;

		}

		public void Awake ()
		{
			leftBoard = new BoardModel ();
			rightBoard = new BoardModel ();
		}

		public void Init ()
		{


			leftBoard.Init (true);
			rightBoard.Init (false);

			Clear ();

			OnChange ();

		}

		public void Clear ()
		{
			gameState = GameState.playing;
			gameResult = GameResult.inProgress;
			playerOneMoving = true;
			counters.Clear ();
			leftBoard.Clear ();
			rightBoard.Clear ();

		}

		public bool PlayerOneCanShot ()
		{
			return !editorMode && (gameState == GameState.playing && playerOneMoving);
		}

		public bool  PlayerTwoCanShot ()
		{
			return !editorMode && (gameState == GameState.playing && (!playerOneMoving));
		}


		public bool AllUnitsArePlacedCorectly ()
		{
			return counters.UnitsToPlaceCount () == 0;
		
		}

		public bool CanStartPlaying ()
		{

			if (!editorMode)
				return false;

			return AllUnitsArePlacedCorectly ();
		}

		public void StopPlaying ()
		{
			editorMode = true;
			OnChange ();
		}

		public void StartPlaying ()
		{
			leftBoard.ClearShots ();
			rightBoard.Shuffle ();
			editorMode = false;
			playerOneMoving = true;
			OnChange ();
		}

		public void ShufflePlayer ()
		{
			leftBoard.Shuffle ();
			OnChange ();
		}


		public void OnChange ()
		{
			Game.instance.OnModelChange ();
		}

		public string GetStatusText ()
		{

			if (editorMode)
				return GetEditorStatusText ();
			else
				return GetPlayStatusText ();


		}

		public string GetEditorStatusText ()
		{
			if (AllUnitsArePlacedCorectly ())
				return "zacznij grę lub przestaw jednostki";
			else
				return "ustaw jednostki";
		}

		public string GetPlayStatusText ()
		{
			
			if (gameState == GameState.ended) {
				if (gameResult == GameResult.playerOneWon)
					return "wygrałeś";
				else
					return "przegrałeś";
			} else {

				if (playerOneMoving)
				if (rightBoard.correctLastShot)
					return "trafiony - dodatkowy ruch";
				else
					return "twój ruch";
				else if (leftBoard.correctLastShot)
					return "trafił - dodatkowy ruch przeciwnika";
				else
					return "ruch przeciwnika";
			}
		}

	}
}
