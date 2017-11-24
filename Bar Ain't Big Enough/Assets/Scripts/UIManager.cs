using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Code from: https://www.sitepoint.com/adding-pause-main-menu-and-game-over-screens-in-unity/
public class UIManager : MonoBehaviour 
{
	// ***EVERYTHING COMMENTED OUT HAS TO DO WITH THE IMPLEMENTATION OF THE PAUSE MENU***

	GameObject pauseScreen;
    GameObject gameOverScreen;
    List<GameObject> sceneObjects;
	GameObject map; // added by Niko
	bool paused; // added by Niko
    bool gameover;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		pauseScreen = GameObject.FindGameObjectWithTag("ShowOnPause");
        gameOverScreen = GameObject.FindGameObjectWithTag("ShowGameOver");
		map = GameObject.FindGameObjectWithTag("Map"); // added by Niko
        sceneObjects = new List<GameObject>();
        sceneObjects.Add(GameObject.FindGameObjectWithTag("Player1"));
        sceneObjects.AddRange(GameObject.FindGameObjectsWithTag("Bottle"));
        sceneObjects.Add(map);
        hidePaused();
		paused = false; // added by Niko

        gameOverScreen.SetActive(false);
        gameover = false;
	}

	// Update is called once per frame
	void Update () {

		//uses the p button to pause and unpause the game
		// added by Niko
		
		if (Input.GetKeyDown (KeyCode.P) && !gameover) {
			if (!paused) {
				showPaused ();
				paused = true;
			} else {
				hidePaused ();
				paused = false;
			}
		}
        // Kill Switch until win state is implemented
        if (Input.GetKeyDown(KeyCode.K))
        {
            Text text;
            text = gameOverScreen.transform.GetChild(1).GetComponent<Text>();
            text.text = GameObject.FindGameObjectWithTag("Player1").transform.GetChild(1).gameObject.GetComponent<Player>().Label;
            showGameOverScreen();
            gameover = true;
        }
	}


	//Reloads the Level
	public void Reload()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	//controls the pausing of the scene
	public void pauseControl()
	{		
		// added by Niko
		hidePaused ();
		map.SetActive (true);
	}

	//shows objects with ShowOnPause tag
	public void showPaused(){
        foreach (GameObject sobj in sceneObjects)
        {
            sobj.SetActive(false);
        }
        pauseScreen.SetActive(true);
    }

	//hides objects with ShowOnPause tag
	public void hidePaused()
	{
        foreach (GameObject sobj in sceneObjects)
        {
            sobj.SetActive(true);
        }
        pauseScreen.SetActive(false);
    }
    public void showGameOverScreen()
    {
        foreach (GameObject sobj in sceneObjects)
        {
            sobj.SetActive(false);
        }
        gameOverScreen.SetActive(true);
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