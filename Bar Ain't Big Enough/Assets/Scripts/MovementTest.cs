﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.DownArrow))
        {
            //gameObject.GetComponent<Rigidbody2D>().MovePosition(gameObject.GetComponent<Rigidbody2D>().position + new Vector2(0.0f, -0.1f));
            gameObject.transform.position = gameObject.transform.position + new Vector3(0.0f, -0.1f);
        }
		if (Input.GetKey(KeyCode.UpArrow))
        {
            //gameObject.GetComponent<Rigidbody2D>().MovePosition(gameObject.GetComponent<Rigidbody2D>().position + new Vector2(0.0f, 0.1f)) ;
            gameObject.transform.position = gameObject.transform.position + new Vector3(0.0f, 0.14f);
        }
		if (Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(-0.1f, 0.0f);
            //gameObject.GetComponent<Rigidbody2D>().MovePosition(gameObject.GetComponent<Rigidbody2D>().position + new Vector2(-0.1f, 0.0f));
        }
		if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0.1f, 0.0f);
            //gameObject.GetComponent<Rigidbody2D>().MovePosition(gameObject.GetComponent<Rigidbody2D>().position + new Vector2(0.1f, 0.0f));
        }
    }

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Bottle") 
		{
			Destroy (other.rigidbody);
			other.gameObject.transform.parent = this.transform;
			other.transform.position = this.transform.position + new Vector3 (0.0f, 0.1f);
		}
	}
}
