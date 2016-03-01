using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomList<T>
{
	public void CutTo (int differentShellsMax)
	{
		while (ready.Count > differentShellsMax)
			ready.RemoveAt (ready.Count - 1);
	}

	public int Count ()
	{
		return used.Count + ready.Count;
	}

	private IList<T> used;
	public IList<T> ready;
	public T lastUsed;
	public bool real ;

	public RandomList ()
	{
		ready = new List<T> ();
		used = new List<T> ();
	}

	public void Add (T value)
	{
		ready.Add (value);
	}

	public T RandomBut (T but)
	{
		T result = Random ();

		// return next random if b
		if (result.Equals (but))
			return RandomBut (but);

		return result;
	}

	public T RealRandom ()
	{
		if (ready.Count == 0)
			Reincarnate (true);

		int ix = UnityEngine.Random.Range (0, ready.Count);
		return ready [ix];

	}

	public T Random ()
	{
	
		if (real) {
			return RealRandom();
		}

		if (ready.Count == 0)
			Reincarnate (true);

		lastUsed = ready [0];
		ready.RemoveAt (0);
		used.Add (lastUsed);

		return lastUsed;

	}

	public void Shuffle ()
	{
		Randomizer.ShuffleList (ref ready);
	}

	public void ShuffleAndCutTo ( int i )
	{

		if (i <= 0)
			Debug.Log ( "cant cut to " + i  );
		
		Randomizer.ShuffleList (ref ready);

		while( ready.Count > i )
		  ready.RemoveAt ( 0 );
	}

	public void Reincarnate (bool withoutLastUsed)
	{
		IList<T> temp = ready;
		ready = used;
		used = temp;

		while (used.Count > 0) {
			ready.Add (used [0]);
			used.RemoveAt (0);
		}

		// lastUsed to used
		if (withoutLastUsed)
		if (ready.Count > 1) {
			ready.Remove (lastUsed);
			used.Add (lastUsed);
		}

		Shuffle ();
	}

	public void Clear ()
	{
		ready.Clear ();
		used.Clear ();
	}

	public void Add (List<T> list)
	{
		foreach (T ac in list) {
			Add (ac);
		}
	}
}
