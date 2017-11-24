using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	// player health
    int health;
    int drunkeness;
	// checks if player is holding a pick up
    bool hasPickup = false;

	// gameobject to store the held item
    GameObject heldItem;

	// item force and direction
    public float itemForce = 0.0f;
    public int direction = 0; 

	// Is this player 1, 2, 3, or 4?
	public int playerNum;

    // String representing player in UI
    public string Label;

	// how fast the character is moving
	public float moveSpeed;

    // Use this for initialization
    void Start () 
	{
        health = 100;
        drunkeness = 1000;
		if(playerNum > 4 || playerNum <= 0)
		{
			playerNum = 1;
		}
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
            Transform drunkMeter = gameObject.transform.parent.GetChild(0).GetChild(2);
            if (drunkMeter.transform.childCount > 0)
                Destroy(drunkMeter.transform.GetChild(drunkeness - 1).gameObject);

            drunkeness = drunkeness > 0 ? drunkeness - 1 : 0;
        }
	}

    void ProcessInput()
    {

		float xAxis = Input.GetAxis("LeftX" + playerNum);
		float yAxis = Input.GetAxis("LeftY" + playerNum);
		bool jumpBtn = Input.GetButton("Jump" + playerNum);
		bool throwBtn = Input.GetButton("Throw" + playerNum);

		gameObject.transform.position = gameObject.transform.position + new Vector3(moveSpeed * xAxis, 0.0f);

		if(throwBtn && hasPickup)
		{
			hasPickup = false;
			heldItem.transform.parent = null;
			heldItem.transform.position = this.transform.position + new Vector3(2.5f * (Mathf.Round(xAxis)) * (Mathf.Cos(xAxis)), 2.5f * (Mathf.Sin(yAxis)), 0.0f);
			heldItem.AddComponent<Rigidbody2D>();
            heldItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(itemForce * xAxis, itemForce * yAxis));
            heldItem.GetComponent<Throwable>().thrown = true;
        }

		if(jumpBtn)
		{
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + moveSpeed, 0.0f);
		}

        if (gameObject.tag == "Player1")
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, -75.0f));

				// cannot go below the floor
				if (gameObject.transform.position.y > -4) {
					gameObject.transform.position = gameObject.transform.position + new Vector3 (0.0f, -0.1f);
				}
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, 175.0f));
                //gameObject.GetComponent<Rigidbody2D>().MovePosition(gameObject.GetComponent<Rigidbody2D>().position + new Vector2(0.0f, 0.1f));

				// cannot fly
				if (gameObject.transform.position.y < gameObject.transform.position.y + 4.0f) {
					gameObject.transform.position += new Vector3 (0.0f, 0.14f);
				}
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-75.0f, 0.0f));
                gameObject.transform.position = gameObject.transform.position + new Vector3(-0.1f, 0.0f);
                //gameObject.GetComponent<Rigidbody2D>().MovePosition(gameObject.GetComponent<Rigidbody2D>().position + new Vector2(-0.1f, 0.0f));
                direction = 0;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(75.0f, 0.0f));
                gameObject.transform.position = gameObject.transform.position + new Vector3(0.1f, 0.0f);
                //gameObject.GetComponent<Rigidbody2D>().MovePosition(gameObject.GetComponent<Rigidbody2D>().position + new Vector2(0.1f, 0.0f));
                direction = 1;
            }
				
			//attempt to keep upright
			//gameObject.GetComponent<Rigidbody2D> ().MoveRotation (180.0f);

            if (Input.GetMouseButtonDown(0))
            {
                if (hasPickup)
                {
                    hasPickup = false;
                    heldItem.transform.parent = null;
                    heldItem.transform.position = this.transform.position + new Vector3(2.5f * (Mathf.Cos(this.transform.rotation.z)), 0.2f * (Mathf.Sin(this.transform.rotation.z)), 0.0f);
                    heldItem.AddComponent<Rigidbody2D>();
                    if (direction == 0)
                    {
                        heldItem.transform.position = this.transform.position + new Vector3(-2.5f * (Mathf.Cos(this.transform.rotation.z)), 0.2f * (Mathf.Sin(this.transform.rotation.z)), 0.0f);
                        heldItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(-itemForce, itemForce));
                    }
                    else
                    {
                        heldItem.transform.position = this.transform.position + new Vector3(2.5f * (Mathf.Cos(this.transform.rotation.z)), 0.2f * (Mathf.Sin(this.transform.rotation.z)), 0.0f);
                        heldItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(itemForce, itemForce));
                    }
                    heldItem.GetComponent<Throwable>().thrown = true;
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
                if (other.gameObject.GetComponent<Throwable>().thrown)
                {
                    applyDamage(other.gameObject.GetComponent<Throwable>().weight);
                    Destroy(other.gameObject);
                    Debug.Log("Player's current health: " + health);
                }
                else
                {
                    if (!hasPickup)
                    {
                        hasPickup = true;
                        Destroy(other.rigidbody);
                        other.gameObject.transform.parent = this.transform;
                        other.transform.position = this.transform.position + new Vector3(0.0f, 0.1f);
                        heldItem = other.gameObject;
                    }
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
