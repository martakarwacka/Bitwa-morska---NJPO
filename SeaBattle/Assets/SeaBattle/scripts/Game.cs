using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SeaBattle
{


	public class Game : MonoBehaviour
	{

		public static Game instance;
		public AudioSource audioSource;
		public GameModel model;
		public GameView view;
		public Skin skin;

		public bool isEditorMode ;

		// Use this for initialization
		void Start ()
		{
			instance = this;
			model.Init ();
			SetAsEditor ();
		}

		public void OnModelChange ()
		{
			view.Validate ();
		}

		public BattleUnitView activeBattleUnitView ;

	    

		public bool Preplacedd (Vector2 coordinates, BattleUnit battleUnit, bool placeIfPossible = false )  {

			if (model.leftBoard.OutOfBounds ((int)coordinates.x, (int)coordinates.y)) {
				//Debug.Log ( "no placed - out of bounds" );
				return false;
			}

			Tile tile = model.leftBoard.tiles [(int)coordinates.x, (int)coordinates.y];
			if (!model.leftBoard.CouldPlaceUnit (tile, battleUnit)) {
				//Debug.Log ( "no placed - occupied already" );
				return false;
			}

			model.leftBoard.lastActiveTile = tile;

			if (placeIfPossible)
				model.leftBoard.TryPlaceUnit (tile, battleUnit, false );
			
			OnModelChange ();

			return true;

		}


		public void RotatePalette() {

			audioSource.PlayOneShot (skin.sounds.rotatePalette);

			foreach (BattleUnitView unitView in GameObject.FindObjectsOfType<BattleUnitView>()) {
				if( unitView.onPalette )
				   unitView.Rotate ();
			}


		}

		public void ClearGame() {
		    view.Clear ();
			model.Clear ();
			OnModelChange ();
		}

		public void SetActiveBattleUnit ( BattleUnitView battleUnitView )  {

			activeBattleUnitView = battleUnitView;
			activeBattleUnitView.transform.SetParent ( view.leftBoardGo.transform );

		}

		public void OnBattleUnitTouchStart ( BattleUnitView battleUnitView, Gesture gesture ) {

			battleUnitView.ClearOccupied ();
			SetActiveBattleUnit ( battleUnitView ) ;
		}


		public void SetAsEditor () {
			model.StopPlaying ();
		}

		public void SetAsPlaying () {
			if (model.CanStartPlaying ()) {
				model.StartPlaying ();
			} else {
				
			}
				
		}

		public void SaveToFile() {

			string fileName = GameObject.Find ( "fileNameEditTextGo" ).GetComponent<Text>().text ;
			GameFile.SaveToFile ( fileName );

		}

		public void LoadFromFile() {
			ClearGame ();

			string fileName = GameObject.Find ( "fileNameEditTextGo" ).GetComponent<Text>().text ;
			GameFile.LoadFromFile (  fileName );
		}

		public void OnTileTouchStart (Tile tile, Gesture gesture)
		{

			if (!model.PlayerOneCanShot ())
				return;
			

			if (tile.shot) {
				audioSource.PlayOneShot (skin.sounds.againShot);
				return;
			}
			

			model.ShotAtTile ( tile );

		}


		public void ShufflePlayer ()
		{
			ClearGame ();
			model.ShufflePlayer ();
		}

	}
}