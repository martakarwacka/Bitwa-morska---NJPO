using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace SeaBattle
{

	public class GameView : MonoBehaviour
	{


		public static float size = 100;
		public static float halfSize = size / 2.0f;

		public bool inited = false;
		public List<TileView> leftTilesViews;
		public List<TileView> rightTilesViews;

		public Text modeText;
		public Text statusText;

		public GameObject leftBoardGo;
		public GameObject rightBoardGo;
		public GameObject leftEditorPanel;
		public GameObject battleUnitsPalette;

		public GameObject editorModeButton ;
		public GameObject playingModeButton ;

		public TextMesh leftBoardStatusText;
		public TextMesh rightBoardStatusText;

		public TextMesh ships1CountText;
		public TextMesh ships2CountText;
		public TextMesh ships3CountText;
		public TextMesh ships4CountText;
		public TextMesh tanks2CountText;
		public TextMesh tanks3CountText;
		public TextMesh tanks4CountText;
		public TextMesh planesCountText;

		public SpriteRenderer paletteShip1SpriteRenderer;
		public SpriteRenderer paletteShip2SpriteRenderer;
		public SpriteRenderer paletteShip3SpriteRenderer;
		public SpriteRenderer paletteShip4SpriteRenderer;

		public SpriteRenderer paletteTank2SpriteRenderer;
		public SpriteRenderer paletteTank3SpriteRenderer;
		public SpriteRenderer paletteTank4SpriteRenderer;

		public SpriteRenderer palettePlaneSpriteRenderer;

	
		public static Vector2 IntToCoordintates (int tileNo)
		{
			Vector2 result = new Vector2 (tileNo % Game.instance.model.cols, tileNo / Game.instance.model.cols);
			return result; 
		}

		public static int CoordinatesToInt (Vector2 coordinates)
		{
			return (int)(coordinates.x + coordinates.y * Game.instance.model.cols);
		}

		public static Vector2 LocalPositionToCoordinates (Vector3 pos)
		{
			return new Vector2 (Mathf.Floor (pos.x / GameView.size), Mathf.Floor (pos.y / GameView.size));
		}


		public static Vector2 		CoordinatesToLocalPosition (Vector2 pos)
		{
			return new Vector2 (pos.x * GameView.size + halfSize, pos.y * GameView.size + halfSize);
		}


		public static void ChangeColorAlpha (SpriteRenderer spriteRenderer, float alpha)
		{
			spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
		}

		public bool isEditorMode;

		public void Validate ()
		{

			CheckInit ();

			if (isEditorMode != Game.instance.model.editorMode)
				ValidateMode ();

			foreach (TileView tileView in leftTilesViews)
				tileView.Validate ();

			foreach (TileView tileView in rightTilesViews)
				tileView.Validate ();

			CountersViewsValidate ();


			playingModeButton.SetActive(isEditorMode && Game.instance.model.AllUnitsArePlacedCorectly())   ;
			Info ();



		}

		public void ValidateMode (){
			isEditorMode = Game.instance.model.editorMode;

			leftEditorPanel.SetActive(isEditorMode) ;
			battleUnitsPalette.SetActive(isEditorMode) ;

			editorModeButton.SetActive(!isEditorMode)  ;


			foreach (BattleUnitView buv in GameObject.FindObjectsOfType<BattleUnitView>())
				buv.GetComponent<Collider2D> ().enabled = isEditorMode;

			rightBoardGo.SetActive ( !isEditorMode );

			if (isEditorMode) {
				
				modeText.text = "tryb edycji";

			} else {
				modeText.text = "tryb grania";
			}

		}

		public void Info() {
	
		
			statusText.text = Game.instance.model.GetStatusText();
			leftBoardStatusText.text = Game.instance.model.leftBoard.GetStatusText ();
			rightBoardStatusText.text = Game.instance.model.rightBoard.GetStatusText ();

		}

		public SpriteRenderer GetFor (BattleUnit unit)
		{

			if (unit.kind == BattleUnit.Kind.ship) {
				if (unit.shape == BattleUnit.Shape.one)
					return paletteShip1SpriteRenderer;
				if (unit.shape == BattleUnit.Shape.two)
					return paletteShip2SpriteRenderer;
				if (unit.shape == BattleUnit.Shape.three)
					return paletteShip3SpriteRenderer;
				if (unit.shape == BattleUnit.Shape.four)
					return paletteShip4SpriteRenderer;
			}

			if (unit.kind == BattleUnit.Kind.tank) {
				if (unit.shape == BattleUnit.Shape.two)
					return paletteTank2SpriteRenderer;
				if (unit.shape == BattleUnit.Shape.three)
					return paletteTank3SpriteRenderer;
				if (unit.shape == BattleUnit.Shape.four)
					return paletteTank4SpriteRenderer;
			}

			if (unit.kind == BattleUnit.Kind.plane) {
				return palettePlaneSpriteRenderer;
			}

			return null;

		}

		public void Clear ()
		{
			foreach (BattleUnitView view in GameObject.FindObjectsOfType<BattleUnitView>()) {
				if (!view.onPalette)
					DestroyImmediate (view.gameObject);
			}
		}

		public void TakeBattleUnitFor (BattleUnit unit)
		{
			SpriteRenderer sr = GetFor (unit);

			GameObject copyGo = Object.Instantiate (sr.gameObject);
			copyGo.transform.SetParent (leftBoardGo.transform);
			copyGo.transform.localPosition = CoordinatesToLocalPosition (unit.occupied.coordinates) - (unit.mask.anchornDelta * GameView.size);
			copyGo.transform.rotation = Quaternion.Euler (0, 0, unit.mask.viewRotation);

			BattleUnitView copyView = copyGo.GetComponent<BattleUnitView> ();
			copyView.onPalette = false;
			copyView.battleUnit = unit;

			Game.instance.model.counters.ChangeCounter (unit, -1);

			
		}

		private void ValidateCounterText (SpriteRenderer sr, TextMesh textMesh, int value)
		{

			textMesh.text = value.ToString ();

			if (value == 0) {
				ChangeColorAlpha (sr, 0.2f);
				SetColliderEnable (sr, false);
			} else {
				ChangeColorAlpha (sr, 1.0f);
				SetColliderEnable (sr, true);
			}

		}

		public static void SetColliderEnable (Component c, bool value)
		{
			Collider2D collider = c.GetComponent<Collider2D> ();
			collider.enabled = value;
		}

		//sr.GetComponent<BattleUnitView> ().enabled = true;
		//sr.GetComponent<BattleUnitView> ().enabled = false;

		public void ReplacePaletteUnitRenderer (BattleUnitView battleUnitView)
		{
		
			SpriteRenderer sr = battleUnitView.GetComponent<SpriteRenderer> ();

			if (battleUnitView.battleUnit.kind == BattleUnit.Kind.ship) {
				if (battleUnitView.battleUnit.shape == BattleUnit.Shape.one)
					paletteShip1SpriteRenderer = sr;
				if (battleUnitView.battleUnit.shape == BattleUnit.Shape.two)
					paletteShip2SpriteRenderer = sr;
				if (battleUnitView.battleUnit.shape == BattleUnit.Shape.three)
					paletteShip3SpriteRenderer = sr;
				if (battleUnitView.battleUnit.shape == BattleUnit.Shape.four)
					paletteShip4SpriteRenderer = sr;
			}

			if (battleUnitView.battleUnit.kind == BattleUnit.Kind.tank) {
				if (battleUnitView.battleUnit.shape == BattleUnit.Shape.two)
					paletteTank2SpriteRenderer = sr;
				if (battleUnitView.battleUnit.shape == BattleUnit.Shape.three)
					paletteTank3SpriteRenderer = sr;
				if (battleUnitView.battleUnit.shape == BattleUnit.Shape.four)
					paletteTank4SpriteRenderer = sr;
			}

			if (battleUnitView.battleUnit.kind == BattleUnit.Kind.plane) {
				palettePlaneSpriteRenderer = sr;
			}


		}

		public void CountersViewsValidate ()
		{
			
			ValidateCounterText (paletteShip1SpriteRenderer, ships1CountText, Game.instance.model.counters.ships1);
			ValidateCounterText (paletteShip2SpriteRenderer, ships2CountText, Game.instance.model.counters.ships2);
			ValidateCounterText (paletteShip3SpriteRenderer, ships3CountText, Game.instance.model.counters.ships3);
			ValidateCounterText (paletteShip4SpriteRenderer, ships4CountText, Game.instance.model.counters.ships4);

			ValidateCounterText (paletteTank2SpriteRenderer, tanks2CountText, Game.instance.model.counters.tanks2);
			ValidateCounterText (paletteTank3SpriteRenderer, tanks3CountText, Game.instance.model.counters.tanks3);
			ValidateCounterText (paletteTank4SpriteRenderer, tanks4CountText, Game.instance.model.counters.tanks4);

			ValidateCounterText (palettePlaneSpriteRenderer, planesCountText, Game.instance.model.counters.planes);

		}


		public void CheckInit ()
		{

			if (leftTilesViews.Count > 0)
				return;


			leftBoardGo = GameObject.Find ("leftBoard");
			rightBoardGo = GameObject.Find ("rightBoard");

			InitBoardTiles (Game.instance.model.leftBoard, out leftTilesViews, leftBoardGo.transform.FindChild ("tiles"), false);
			InitBoardTiles (Game.instance.model.rightBoard, out rightTilesViews, rightBoardGo.transform.FindChild ("tiles"), true);



		}

		public void SetEditorMode ()
		{
			statusText.text = "tryb edycji - ustaw statki";
			rightBoardGo.SetActive ( false );
		}


		public void InitBoardTiles (BoardModel boardModel, out List<TileView> tilesViews, Transform parentTransform, bool touchable )
		{

			tilesViews = new List<TileView> ();

			for (int col = 0; col < Game.instance.model.cols; col++)
				for (int row = 0; row < Game.instance.model.rows; row++) {

					GameObject go = GameObject.Instantiate (Game.instance.skin.tileView) as GameObject;
					go.transform.SetParent (parentTransform);
					go.name = "tile-" + col.ToString ("00") + "-" + row.ToString ("00");
					TileView tileView = go.GetComponent<TileView> ();
					tileView.SetTile (boardModel.tiles [col, row]);
					tileView.GetComponent<Collider2D> ().enabled = touchable;

					tilesViews.Add (tileView);


				}
		}

		// Use this for initialization
		void Start ()
		{
		}
	
	}
}