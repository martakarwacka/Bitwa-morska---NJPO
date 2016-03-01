using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SeaBattle
{
	public class BattleUnitMask
	{

		public BattleUnitMask (string code, Vector2 anchornDelta, List<Vector2> points, float viewRotation)
		{
			this.code = code;
			this.anchornDelta = anchornDelta;
			this.points = points;
			this.viewRotation = viewRotation;
		}

		public string code;
		public Vector2 anchornDelta;
		public float viewRotation;
		public List<Vector2> points;

		public static string mask1Code = "mask1";

		public static string mask2VerticalCode = "mask2Vertical";
		public static string mask2HorizontalCode = "mask2Horizontal";
		public static string mask3VerticalCode = "mask3Vertical";
		public static string mask3HorizontalCode = "mask3Horizontal";
		public static string mask4VerticalCode = "mask4Vertical";
		public static string mask4HorizontalCode = "mask4Horizontal";

		public static string maskTUpCode = "maskTUp";
		public static string maskTRightCode = "maskTRight";
		public static string maskTDownCode = "maskTDown";
		public static string maskTLeftCode = "maskTLeft";


		public static BattleUnitMask MaskFor( string code ) {

			if (mask1Code.Equals (code))
				return mask1;

			if (mask2HorizontalCode.Equals (code))
				return mask2Horizontal;

			if (mask2VerticalCode.Equals (code))
				return mask2Vertical;

			if (mask3HorizontalCode.Equals (code))
				return mask3Horizontal;

			if (mask3VerticalCode.Equals (code))
				return mask3Vertical;

			if (mask4HorizontalCode.Equals (code))
				return mask4Horizontal;

			if (mask4VerticalCode.Equals (code))
				return mask4Vertical;

			if (maskTLeftCode.Equals (code))
				return maskTLeft;

			if (maskTDownCode.Equals (code))
				return maskTDown;

			if (maskTRightCode.Equals (code))
				return maskTRight;

			if (maskTUpCode.Equals (code))
				return maskTUp;

			return null;
		}

		public BattleUnitMask RotatedMask ()
		{

			if (mask1Code.Equals (code))
				return mask1;

			if (mask2HorizontalCode.Equals (code))
				return mask2Vertical;

			if (mask2VerticalCode.Equals (code))
				return mask2Horizontal;

			if (mask3HorizontalCode.Equals (code))
				return mask3Vertical;

			if (mask3VerticalCode.Equals (code))
				return mask3Horizontal;

			if (mask4HorizontalCode.Equals (code))
				return mask4Vertical;

			if (mask4VerticalCode.Equals (code))
				return mask4Horizontal;

			if (maskTLeftCode.Equals (code))
				return maskTDown;

			if (maskTDownCode.Equals (code))
				return maskTRight;

			if (maskTRightCode.Equals (code))
				return maskTUp;

			if (maskTUpCode.Equals (code))
				return maskTLeft;
			
			return null;
		}

		public static BattleUnitMask DefaultMaskFor (BattleUnit.Shape kind, float rotationDegrees)
		{


			switch (kind) {

			case BattleUnit.Shape.one:
				return mask1;

			case BattleUnit.Shape.two:
				if (rotationDegrees == 0 || rotationDegrees == 180)
					return mask2Horizontal;
				else
					return mask2Vertical;
			
			case BattleUnit.Shape.three:
				if (rotationDegrees == 0 || rotationDegrees == 180)
					return mask3Horizontal;
				else
					return mask3Vertical;

			case BattleUnit.Shape.four:
				if (rotationDegrees == 0 || rotationDegrees == 180)
					return mask4Horizontal;
				else
					return mask4Vertical;

			case BattleUnit.Shape.t:

				if (rotationDegrees == 0 )
				  return maskTUp;

				if (rotationDegrees == 90 )
					return maskTLeft;

				if ((int)rotationDegrees == 180 )
					return maskTDown;

				if (rotationDegrees == 270 )
					return maskTRight;

				break;

			}

			return null;


		}

		public static BattleUnitMask RandomMaskFor (BattleUnit.Shape kind)
		{


			switch (kind) {
			case BattleUnit.Shape.one:
				return mask1;
			case BattleUnit.Shape.two:
				return Randomizer.FiftyFifty () ? mask2Vertical : mask2Horizontal;
			case BattleUnit.Shape.three:
				return Randomizer.FiftyFifty () ? mask3Vertical : mask3Horizontal;
			case BattleUnit.Shape.four:
				return Randomizer.FiftyFifty () ? mask4Vertical : mask4Horizontal;
			case BattleUnit.Shape.t:
				return tMasks [Random.Range (0, 4)];
			}

			return null;

		}

		public static BattleUnitMask mask1 = new BattleUnitMask (mask1Code, new Vector2 (0, 0), new List<Vector2> () { new Vector2 (0, 0) }, 0);

		public static BattleUnitMask mask2Vertical = new BattleUnitMask (mask2VerticalCode, new Vector2 (0, -0.5f), new List<Vector2> () {
			new Vector2 (0, 0),
			new Vector2 (0, 1)
		}, 90);
		public static BattleUnitMask mask2Horizontal = new BattleUnitMask (mask2HorizontalCode, new Vector2 (-0.5f, 0), new List<Vector2> () {
			new Vector2 (0, 0),
			new Vector2 (1, 0)
		}, 0);

		public static BattleUnitMask mask3Vertical = new BattleUnitMask (mask3VerticalCode, new Vector2 (0, -1.0f), new List<Vector2> () {
			new Vector2 (0, 0),
			new Vector2 (0, 1),
			new Vector2 (0, 2)
		}, 90);
		public static BattleUnitMask mask3Horizontal = new BattleUnitMask (mask3HorizontalCode, new Vector2 (-1.0f, 0), new List<Vector2> () {
			new Vector2 (0, 0),
			new Vector2 (1, 0),
			new Vector2 (2, 0)
		}, 0);

		public static BattleUnitMask mask4Vertical = new BattleUnitMask (mask4VerticalCode, new Vector2 (0, -1.5f), new List<Vector2> () {
			new Vector2 (0, 0),
			new Vector2 (0, 1),
			new Vector2 (0, 2),
			new Vector2 (0, 3)
		},  90);

		public static BattleUnitMask mask4Horizontal = new BattleUnitMask (mask4HorizontalCode, new Vector2 (-1.5f, 0), new List<Vector2> () {
			new Vector2 (0, 0),
			new Vector2 (1, 0),
			new Vector2 (2, 0),
			new Vector2 (3, 0)
		}, 0);

		public static BattleUnitMask maskTUp = new BattleUnitMask (maskTUpCode, new Vector2 (-1, -0.5f), new List<Vector2> () {
			new Vector2 (0, 0),
			new Vector2 (1, 0),
			new Vector2 (2, 0),
			new Vector2 (1, 1)
		}, 0);
		public static BattleUnitMask maskTRight = new BattleUnitMask (maskTRightCode, new Vector2 (-0.5f, -1), new List<Vector2> () {
			new Vector2 (0, 0),
			new Vector2 (0, 1),
			new Vector2 (0, 2),
			new Vector2 (1, 1)
		}, 270);
		public static BattleUnitMask maskTDown = new BattleUnitMask (maskTDownCode, new Vector2 (-1, -0.5f), new List<Vector2> () {
			new Vector2 (0, 1),
			new Vector2 (1, 0),
			new Vector2 (1, 1),
			new Vector2 (2, 1)
		}, 180.0f);
		public static BattleUnitMask maskTLeft = new BattleUnitMask (maskTLeftCode, new Vector2 (-0.5f, -1), new List<Vector2> () {
			new Vector2 (0, 1),
			new Vector2 (1, 0),
			new Vector2 (1, 1),
			new Vector2 (1, 2)
		}, 90);

		public static List<BattleUnitMask> tMasks = new List<BattleUnitMask> { maskTUp, maskTRight, maskTDown, maskTLeft  };
			


	}
}