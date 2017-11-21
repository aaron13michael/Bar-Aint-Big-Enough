using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour {

    //Weight of object for damage calculation
    public int weight;
    //Alcohol requirement for the object to be picked up.
    public int drunkCheck;
    public bool thrown = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (thrown)
        {
            if (other.gameObject.tag == "Terrain")
            {
                thrown = false;
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-gameObject.GetComponent<Rigidbody2D>().velocity.x * .8f, -gameObject.GetComponent<Rigidbody2D>().velocity.y * .8f));
            }
        }
        else
        {
       
        }
    }
}
