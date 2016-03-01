using UnityEngine;
using System.Collections;

namespace SeaBattle
{
	public class BattleUnitView : MonoBehaviour
	{


		public BattleUnit battleUnit;
		public BattleUnit originalBattleUnit;
		private Vector3 deltaPosition;
		public Vector3 initPosition;
		public SpriteRenderer spriteRenderer;

		public bool onPalette = true;

		// Subscribe to events
		void OnEnable ()
		{
			EasyTouch.On_TouchStart += On_TouchStart;
			EasyTouch.On_Drag += On_Drag;
			EasyTouch.On_DragStart += On_DragStart;
			EasyTouch.On_DragEnd += On_DragEnd;
		}

		void OnDisable ()
		{
			UnsubscribeEvent ();
		}

		void OnDestroy ()
		{
			UnsubscribeEvent ();
		}

		void UnsubscribeEvent ()
		{
			EasyTouch.On_TouchStart -= On_TouchStart;
			EasyTouch.On_Drag -= On_Drag;
			EasyTouch.On_DragStart -= On_DragStart;
			EasyTouch.On_DragEnd -= On_DragEnd;
		}

		void On_TouchStart (Gesture gesture)
		{
			

			if (gesture.pickedObject != gameObject)
				return;

			Game.instance.OnBattleUnitTouchStart (this, gesture);

		}

		// At the drag beginning
		void On_DragStart (Gesture gesture)
		{

			// Verification that the action on the object
			if (gesture.pickedObject != gameObject)
				return;

			if (onPalette ) {

				GameObject paletteCopy = Object.Instantiate (gameObject);
				paletteCopy.transform.position = transform.position;
				paletteCopy.transform.SetParent ( Game.instance.view.battleUnitsPalette.transform );
				BattleUnitView paletteBattleUnitView = paletteCopy.GetComponent<BattleUnitView> ();
				paletteBattleUnitView.onPalette = true;
				this.onPalette = false;

				Game.instance.view.ReplacePaletteUnitRenderer (  paletteBattleUnitView );
				Game.instance.model.counters.ChangeCounter( battleUnit, -1 ) ;

			}

			// the world coordinate from touch
			//Vector3 position = gesture.GetTouchToWorldPoint (gesture.pickedObject.transform.position);
			//deltaPosition = position - transform.position;



		}

		// During the drag
		void On_Drag (Gesture gesture)
		{

			// Verification that the action on the object
			if (gesture.pickedObject == gameObject ) {

				// the world coordinate from touch
				Vector3 position = gesture.GetTouchToWorldPoint (gesture.pickedObject.transform.position);
				transform.position = position ;

				Vector2 coordinates = GameView.LocalPositionToCoordinates (transform.localPosition);

				Game.instance.Preplacedd (coordinates, battleUnit, false);

			}
		}

		// At the drag end
		void On_DragEnd (Gesture gesture)
		{

			// Verification that the action on the object
			if (gesture.pickedObject == gameObject) {
				TryMove ();
			}

		}

		public void ClearOccupied ()
		{
			if (battleUnit.occupied == null)
				return;
			
			Game.instance.model.leftBoard.SetClear (battleUnit.occupied, battleUnit);
			battleUnit.occupied = null;

			Game.instance.OnModelChange ();

		}

		public void TryMove ()
		{


			float anchornX = transform.localPosition.x + (battleUnit.mask.anchornDelta.x* GameView.size) ;
			float annchornY = transform.localPosition.y + (battleUnit.mask.anchornDelta.y *GameView.size);

			Vector2 coordinates = GameView.LocalPositionToCoordinates (new Vector2( anchornX, annchornY ));

			if (Game.instance.Preplacedd (coordinates, battleUnit, true)) {
				Game.instance.audioSource.PlayOneShot ( Game.instance.skin.sounds.correctPlaced );
				transform.localPosition = GameView.CoordinatesToLocalPosition ( coordinates) - ( battleUnit.mask.anchornDelta * GameView.size);

			} else {
				Game.instance.audioSource.PlayOneShot ( Game.instance.skin.sounds.incorrectPlaced );
				BackToPalette ();
			}

		}

		private void BackToPalette () {

			Game.instance.model.counters.ChangeCounter( battleUnit, 1 ) ;

			//ClearOccupied ();
			DestroyImmediate ( gameObject );
			//transform.position = initPosition;
		}


		// Use this for initialization
		void Start ()
		{
			initPosition = transform.position;
			battleUnit.InitMask ( transform.rotation.eulerAngles.z );
		}

		public void Rotate ()
		{
			transform.Rotate (0, 0, 90);
			battleUnit.mask = battleUnit.mask.RotatedMask ();

		}

	
		// Update is called once per frame
		void Update ()
		{
	
		}
	}
}