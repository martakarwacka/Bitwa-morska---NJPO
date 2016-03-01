using UnityEngine;
using UnityEditor;
using System.Collections;

namespace GF
{
	public class BuildContexMenu : MonoBehaviour
	{



		public static void SetDirty (Object c)
		{
			#if UNITY_EDITOR
			EditorUtility.SetDirty (c);
			#endif
		}

		[MenuItem ("CONTEXT/Tile/validate")]
		static void StaticLevelsModel (MenuCommand commandRight)
		{

			//TileView tile = ((TileView)commandRight.context);

//			foreach (Tile t in GameObject.FindObjectsOfType<Tile>()) {
//				t.Validate ();
//				BuildContexMenu .SetDirty (t);
	//		}



		}

	}
}