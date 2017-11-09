using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour 
{
	bool hasPickup = false;
	GameObject heldBottle;
	public float bottleForce = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.DownArrow))
        {
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, -75.0f));
            //gameObject.transform.position = gameObject.transform.position + new Vector3(0.0f, -0.1f);
        }
		if (Input.GetKey(KeyCode.UpArrow))
        {
			//gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, 175.0f));
            gameObject.GetComponent<Rigidbody2D>().MovePosition(gameObject.GetComponent<Rigidbody2D>().position + new Vector2(0.0f, 0.1f)) ;
            //gameObject.transform.position = gameObject.transform.position + new Vector3(0.0f, 0.14f);
        }
		if (Input.GetKey(KeyCode.LeftArrow))
        {
			//gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-75.0f, 0.0f));
            gameObject.transform.position = gameObject.transform.position + new Vector3(-0.1f, 0.0f);
            //gameObject.GetComponent<Rigidbody2D>().MovePosition(gameObject.GetComponent<Rigidbody2D>().position + new Vector2(-0.1f, 0.0f));
        }
		if (Input.GetKey(KeyCode.RightArrow))
        {
			//gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(75.0f, 0.0f));
            gameObject.transform.position = gameObject.transform.position + new Vector3(0.1f, 0.0f);
            //gameObject.GetComponent<Rigidbody2D>().MovePosition(gameObject.GetComponent<Rigidbody2D>().position + new Vector2(0.1f, 0.0f));
        }

		if (Input.GetMouseButtonDown(0)) {
			if (hasPickup) {
				hasPickup = false;
				heldBottle.transform.parent = null;
				heldBottle.transform.position = this.transform.position + new Vector3 (2.5f * (Mathf.Cos(this.transform.rotation.z)), 0.2f * (Mathf.Sin(this.transform.rotation.z)), 0.0f);
				heldBottle.AddComponent<Rigidbody2D> ();
				heldBottle.GetComponent<Rigidbody2D> ().AddForce (new Vector2(bottleForce, bottleForce));
			}
		}
    }

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Bottle") 
		{
			if (!hasPickup) {
				hasPickup = true;
				Destroy (other.rigidbody);
				other.gameObject.transform.parent = this.transform;
				other.transform.position = this.transform.position + new Vector3 (0.0f, 0.1f);
				heldBottle = other.gameObject;
			}

		}
	}
}
