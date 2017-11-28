﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour {

    //Weight of object for damage calculation
    public int weight;
    //Alcohol requirement for the object to be picked up.
    public int drunkCheck;
    public bool thrown = false;

	// the bottles start on the ground
	public bool onGround = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D other)
    {
		// if the bottle is thrown
        if (thrown)
        {
			// onGround is false
			onGround = false;

			// if the first object the thrown bottle hits is terrain
            if (other.gameObject.tag == "Terrain")
            {
                thrown = false;
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-gameObject.GetComponent<Rigidbody2D>().velocity.x * .8f, -gameObject.GetComponent<Rigidbody2D>().velocity.y * .8f));
            }

			// if the first object the thrown bottle hits is a person and kills the person
			if (other.gameObject.tag == "Player" && !onGround) 
			{
				Destroy (other.gameObject);
				thrown = false;
				onGround = true;
			}
        }
        else
        {
			
       
        }
    }
}
