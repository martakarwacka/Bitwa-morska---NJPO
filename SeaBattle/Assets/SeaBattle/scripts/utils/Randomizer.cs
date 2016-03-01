using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Randomizer
{

	public static bool FiftyFifty() {
		return UnityEngine.Random.Range (0, 2) == 1; 
	}


	public static IList<int> Random(int howMuch, int min, int max, int repeats2)
    {

        IList<int> result = new List<int>();

        for (int i = min; i <= max; i++)
        {
            result.Add(i);
        }

        for (int i = 0; i < repeats2 - 1; i++)
            result.Add(UnityEngine.Random.Range(min, max + 1));

        ShuffleList(ref result);

        while (result.Count > howMuch)
            result.RemoveAt(0);

        //		Debug.Log ( result );

        return result;

    }

    public static int OneInt(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }

    public static Vector3 RandomDirection()
    {
        return new Vector3(UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(-50, 50), 0);
    }

    public static float ZeroToOne()
    {
        return UnityEngine.Random.Range(0, 100) / 100.0f;
    }



	public static void ShuffleList<E>(ref IList<E> list)
    {

		//System.Random rand = new System.Random( );

		if (list.Count <= 1 )
			return ;

		for (int i = list.Count - 1; i >= 0; i--)
            {
                E tmp = list[i];
			int randomIndex = UnityEngine.Random.Range ( 0, i + 1);

                //Swap elements
                list[i] = list[randomIndex];
                list[randomIndex] = tmp;
            }
    }

	public static void ShuffleList<E>(ref List<E> list)
	{
		System.Random rand = new System.Random( );
		if (list.Count <= 1)
			return;

			for (int i = list.Count - 1; i >= 0; i--)
			{
				E tmp = list[i];
	//			int randomIndex = UnityEngine.Random.Range(0, i + 1);
			int randomIndex = rand.Next( 0, i + 1);

				//Swap elements
				list[i] = list[randomIndex];
				list[randomIndex] = tmp;
			}
	}



	public static Vector3 LeftOrRight()
    {

		if (FiftyFifty())
            return Vector3.left;

        else
            return Vector3.right;
    }



    internal static Vector3 GetPointOnRing(float rMin, float rMax)
    {
        float r = UnityEngine.Random.Range( rMin, rMax );
        float angle = UnityEngine.Random.Range(0, Mathf.PI * 2);
        return new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r, 0);
    }
}
