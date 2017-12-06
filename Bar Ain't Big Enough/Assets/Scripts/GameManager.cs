using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	// list of players
	public List<GameObject> players;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		// checks to see if there is one player alive
		if (players.Count <= 1) 
		{
			// set the player num as the winning player through PlayerPrefs
			string pNum = players [0].GetComponent<Player> ().playerNum.ToString();
			PlayerPrefs.SetString (pNum, "Player " + pNum + " Wins!");
		} 
		// if there are more than one player alive
		else 
		{
			/*
			// loop through the players
			for (int i = 0; i < players.Count; i++) 
			{
				// if a player died remove them from the list
				if (players [i].GetComponent<Player>().health <= 0) 
				{
					players.RemoveAt (i);
				}
			}
			*/
		}
	}
}
