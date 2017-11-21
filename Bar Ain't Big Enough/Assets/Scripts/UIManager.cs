using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Code from: https://www.sitepoint.com/adding-pause-main-menu-and-game-over-screens-in-unity/
public class UIManager : MonoBehaviour 
{
	// ***EVERYTHING COMMENTED OUT HAS TO DO WITH THE IMPLEMENTATION OF THE PAUSE MENU***

	//GameObject[] pauseObjects;
	//GameObject quad; // added by Niko
	//bool paused; // added by Niko

	// Use this for initialization
	void Start () {
		//Time.timeScale = 1;
		//pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
		//quad = GameObject.FindGameObjectWithTag ("Map"); // added by Niko
		//hidePaused();
		//paused = false; // added by Niko
	}

	// Update is called once per frame
	void Update () {

		//uses the p button to pause and unpause the game
		// added by Niko

		/*
		if (Input.GetKeyDown (KeyCode.P)) {
			if (!paused) {
				quad.SetActive (false);
				showPaused ();
				paused = true;
			} else {
				hidePaused ();
				quad.SetActive (true);
				paused = false;
			}
		}
		*/


		/*
			 * Original Code
			if(Time.timeScale == 1)
			{
				Time.timeScale = 0;
				showPaused();
			} else if (Time.timeScale == 0){
				Debug.Log ("high");
				Time.timeScale = 1;
				hidePaused();
			}
			*/
	}


	//Reloads the Level
	public void Reload()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	//controls the pausing of the scene
	public void pauseControl()
	{
		/*
		// added by Niko
		hidePaused ();
		quad.SetActive (true);
		*/

		/*
		 * Original code
		if(Time.timeScale == 1)
		{
			Time.timeScale = 0;
			showPaused();
		} else if (Time.timeScale == 0){
			Time.timeScale = 1;
			hidePaused();
		}
		*/
	}

	//shows objects with ShowOnPause tag
	public void showPaused(){
		/*
		foreach(GameObject g in pauseObjects){
			g.SetActive(true);
		}
		*/
	}

	//hides objects with ShowOnPause tag
	public void hidePaused()
	{
		/*
		foreach(GameObject g in pauseObjects){
			g.SetActive(false);
		}
		*/
	}

	//loads inputted level
	public void LoadLevel(string level)
	{
		SceneManager.LoadScene (level);
	}

	//loads inputted level
	public void ExitLevel()
	{
		Application.Quit();
	}
}