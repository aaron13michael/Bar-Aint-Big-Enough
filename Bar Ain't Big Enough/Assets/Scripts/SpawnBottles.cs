using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBottles : MonoBehaviour {

    public GameObject[] spawnLocations;
    public GameObject bottle;

    float timer;

	// Use this for initialization
	void Start () {
        timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        SpawnBottle();
	}

    void SpawnBottle()
    {
        if (timer > 2.0f)
        {
            int random = Random.Range(0, spawnLocations.Length);
            Instantiate(bottle, spawnLocations[random].GetComponent<Transform>().position, Quaternion.identity);
            timer = 0.0f;
        }
    }
}
