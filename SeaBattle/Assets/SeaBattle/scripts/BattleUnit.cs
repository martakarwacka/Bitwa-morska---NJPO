using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SeaBattle
{
	[System.Serializable]
	public class BattleUnit
	{

		public bool	TileKindOk (Tile.Kind tileKind)
		{

			if (tileKind == Tile.Kind.water)
				return (kind == Kind.ship) || (kind == Kind.plane);

			if (tileKind == Tile.Kind.land)
				return (kind == Kind.tank) || (kind == Kind.plane);



			// todomk
			return true;

		}

		public BattleUnit (BattleUnit.Kind kind, BattleUnit.Shape shape)
		{
			this.kind = kind;
			this.shape = shape;
			this.mask = BattleUnitMask.RandomMaskFor (shape);
		}

		public void InitMask (float rotationDegrees)
		{
			this.mask = BattleUnitMask.DefaultMaskFor (shape, rotationDegrees);
		}

		public Kind kind;
		public Shape shape;
		public BattleUnitMask mask;
		public Tile occupied;




		public enum Kind
		{
			none,
			tank,
			ship,
			plane
		}

		public static Kind ToKind (string kind ) {
			if ("ship".Equals (kind))
				return Kind.ship;
			if ("tank".Equals (kind))
				return Kind.tank;
			if ("plane".Equals (kind))
				return Kind.plane;

			return Kind.none;
		}

		public static string ToString (Kind kind)
		{
			switch (kind) {
			case Kind.ship:
				return "ship";
			case Kind.tank:
				return "tank";
			case Kind.plane:
				return "plane";
			}

			return "";
		}

		public  enum Shape
		{
			one,
			two,
			three,
			four,
			t,
			none
		}


		public static Shape ToShape (string shape ) {
		
			if ("one".Equals (shape))
				return Shape.one;
			
			if ("two".Equals (shape))
				return Shape.two;
			
			if ("three".Equals (shape))
				return Shape.three;
			
			if ("four".Equals (shape))
				return Shape.four;
			
			if ("t".Equals (shape))
				return Shape.t;

			return Shape.none;
			
		}

		public static string ToString (Shape shape)
		{
			switch (shape) {
			case Shape.one:
				return "one";
			case Shape.two:
				return "two";
			case Shape.three:
				return "three";
			case Shape.four:
				return "four";
			case Shape.t:
				return "t";
			}

			return "";
		}

	}
}