using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    int health;
    bool hasPickup = false;
    GameObject heldItem;
    public float itemForce = 0.0f;

    //UI Assets

    // Use this for initialization
    void Start () {
        health = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0)
        {
            Death();
        }
        else
        {
            ProcessInput();
        }
	}

    void ProcessInput()
    {
        if (gameObject.tag == "Player1")
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, -75.0f));
                //gameObject.transform.position = gameObject.transform.position + new Vector3(0.0f, -0.1f);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, 175.0f));
                gameObject.GetComponent<Rigidbody2D>().MovePosition(gameObject.GetComponent<Rigidbody2D>().position + new Vector2(0.0f, 0.1f));
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

            if (Input.GetMouseButtonDown(0))
            {
                if (hasPickup)
                {
                    hasPickup = false;
                    heldItem.transform.parent = null;
                    heldItem.transform.position = this.transform.position + new Vector3(2.5f * (Mathf.Cos(this.transform.rotation.z)), 0.2f * (Mathf.Sin(this.transform.rotation.z)), 0.0f);
                    heldItem.AddComponent<Rigidbody2D>();
                    heldItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(itemForce, itemForce));
                }
            }
        }
        else
        {

        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (health >= 1)
        {
            if (other.gameObject.tag == "Bottle")
            {
                if (!hasPickup)
                {
                    hasPickup = true;
                    Destroy(other.rigidbody);
                    other.gameObject.transform.parent = this.transform;
                    other.transform.position = this.transform.position + new Vector3(0.0f, 0.1f);
                    heldItem = other.gameObject;
                }
                else
                {
                    applyDamage(10);
                    Destroy(other.gameObject);
                    Debug.Log("Player's current health: " + health);
                }
            }
        }
    }
    /// <summary>
    /// Applies damage to the player and reflects damage taken through health meter
    /// </summary>
    /// <param name="damage">How much damage the player has taken (out of 100)</param>
    void applyDamage(int damage)
    {
        //Used to update health meter
        int oldHealth = health;

        //Check that health does not go below 0
        health = health - damage > 0 ? health - damage : 0;
        Transform healthMeter = gameObject.transform.parent.GetChild(0).GetChild(1);
        for(int h = oldHealth; h > health; h--)
        {
            Destroy(healthMeter.transform.GetChild(h - 1).gameObject);
        }
    }

    void Death()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
    }
}
