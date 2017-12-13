using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBottles : MonoBehaviour {

    public GameObject[] spawnLocations;
    public GameObject bottle;
	public float spawnTime;

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
		if (timer > spawnTime)
        {
            int random = Random.Range(0, spawnLocations.Length);
            GameObject newBottle = Instantiate(bottle, spawnLocations[random].GetComponent<Transform>().position, Quaternion.identity);
            if (random == 0)
            {
                newBottle.GetComponent<Rigidbody2D>().AddForce(new Vector2(-300.0f, 0.0f));
            }
            else if (random == 1)
            {
                newBottle.GetComponent<Rigidbody2D>().AddForce(new Vector2(350.0f, 0.0f));
            }
            else
            {
                newBottle.GetComponent<Bottle>().bState = Bottle.BottleState.Empty;
            }
            timer = 0.0f;
        }
    }
}
