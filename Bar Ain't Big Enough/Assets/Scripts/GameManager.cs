using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		// determining winner in BattleScene
		if (SceneManager.GetActiveScene ().name == "BattleScene") 
		{
			// checks to see if there is one player alive
			if (players.Count <= 1) 
			{
				// set the player num as the winning player through PlayerPrefs
				string pNum = players [0].GetComponent<Player> ().playerNum.ToString ();
				PlayerPrefs.SetString ("Winner", "Player " + pNum + " Wins!");
				SceneManager.LoadScene ("GameOver");

			} 
		// if there are more than one player alive
			else 
			{
				// updates player list 
				for (int i = 0; i < players.Count; i++) 
				{
					// if the player is dead (i.e., null gameobject) remove from the list
					if (players [i] == null) 
					{
						players.RemoveAt (i);
					}
				}
			}
		}

		// Loading the GameOverScene
		if(SceneManager.GetActiveScene().name == "GameOver")
		{
			// retrieve the player pref for winner
			string winner = PlayerPrefs.GetString("Winner");
			Debug.Log (winner);

			// modify the text of GUI elment to display the player pref
		}
	}
}
