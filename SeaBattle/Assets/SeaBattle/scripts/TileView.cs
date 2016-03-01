using UnityEngine;
using System.Collections;


namespace SeaBattle
{
	public class TileView : MonoBehaviour
	{


		public Tile tile;

		public GameObject dot;
		public GameObject cross;
		public SpriteRenderer spriteRenderer;


		public void SetTile (Tile tile)
		{

			this.tile = tile;
			Validate ();

		}

		public Color GetColor() {

			if (tile.kind == Tile.Kind.water)
				return Game.instance.skin.colors.water;

			return Game.instance.skin.colors.land;
		}


		public void Validate (  )
		{
	
			float x = (tile.coordinates.x ) * GameView.size + GameView.halfSize; 
			float y = (tile.coordinates.y ) * GameView.size + GameView.halfSize; 
			gameObject.transform.localPosition = new Vector3 (x, y, gameObject.transform.localPosition.z);


			spriteRenderer.color = GetColor ();

			// cienie dla planszy gracza nr 1
			if (tile.board.playerOneBoard) {
				float alpha = tile.shadows == 0 ? 1 : 0.9f;
				GameView.ChangeColorAlpha (spriteRenderer, alpha);
			}

			dot.SetActive ( tile.shot && !tile.IsOccupied() );

			bool crossVisible = tile.IsOccupied () && (tile.shot || tile.board.visibility);
			cross.SetActive ( crossVisible );

		}


		// Subscribe to events
		void OnEnable(){
			EasyTouch.On_TouchStart += On_TouchStart;
		}

		void OnDisable(){
			UnsubscribeEvent();
		}

		void OnDestroy(){
			UnsubscribeEvent();
		}

		void UnsubscribeEvent(){
			EasyTouch.On_TouchStart -= On_TouchStart;
		}

		// At the touch beginning 
		private void On_TouchStart(Gesture gesture){
			if (gesture.pickedObject == gameObject){
				Game.instance.OnTileTouchStart ( tile, gesture );


			}
		}


		// Use this for initialization
		void Start ()
		{
		}
	

	}

}
