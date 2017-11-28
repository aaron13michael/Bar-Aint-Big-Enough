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

    // True if drunkeness is increasing. False otherwise.
    bool drunkApply = false;

	public string Label;

	// gameobject to store the held item
    public GameObject heldItem;
    public GameObject footCollider;
	// item force and direction
    public float itemForce = 0.0f;
    public int direction = 0; 

	// Is this player 1, 2, 3, or 4?
	public int playerNum;

	private float pickupTime;

	private float jumpCD ; // cooldown timer on jumps
	public float prevJumpTime; // the last time that jump was used

	private Rigidbody2D rb;
	private Animator animator;

	public float maxVel;


    // Use this for initialization
    void Start () 
	{
        health = 100;
        drunkeness = 1000;
		if(playerNum > 4 || playerNum <= 0)
		{
			playerNum = 1;
		}

		pickupTime = 0.0f;
		jumpCD = 1.5f;
		prevJumpTime = -2.0f;

		rb = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (health <= 0)
        {
            Death();
        }
        else
        {
            ProcessInput();

            //Slowly decrease drunkeness level
            //Transform drunkMeter = gameObject.transform.parent.GetChild(0).GetChild(2);
            //if (drunkMeter.transform.childCount > 0 && !drunkApply)
                //Destroy(drunkMeter.transform.GetChild(drunkeness - 1).gameObject);

            drunkeness = drunkeness > 0 ? drunkeness - 1 : 0;
        }
	}

    void ProcessInput()
    {
		float xAxis = Input.GetAxis("LeftX" + playerNum);
		float yAxis = Input.GetAxis("LeftY" + playerNum);
		bool jumpBtn = Input.GetButton("Jump" + playerNum);
		bool throwBtn = Input.GetButton("Throw" + playerNum);


		if(xAxis > 0) 
		{
			rb.AddForce (new Vector2(20.0f, 0.0f));
			animator.SetFloat ("xVelocity", rb.velocity.x);
		}
		if(xAxis < 0)
		{
			rb.AddForce (new Vector2(-20.0f, 0.0f));
			animator.SetFloat ("xVelocity", rb.velocity.x);
		}
		else if(xAxis == 0)
		{
			Vector2 currentVel = rb.velocity;
			currentVel.x *= 0.0f;
			rb.velocity = currentVel;
			animator.SetFloat ("xVelocity", rb.velocity.x);
		}

		Vector2 velocity = new Vector2(maxVel, 0.0f); 
		if (Mathf.Abs(rb.velocity.x) > maxVel) 
		{
			if (rb.velocity.x > 0.0f) 
			{
				rb.velocity = velocity;
			}
			else 
			{
				rb.velocity = -velocity;
			}
		}
		animator.SetFloat ("xVelocity", rb.velocity.x);

		if(throwBtn && hasPickup)
		{
			hasPickup = false;
			heldItem.transform.parent = null;
			heldItem.transform.position = this.transform.position + new Vector3(2.5f * (Mathf.Round(xAxis)) * (Mathf.Cos(xAxis)), 2.5f * (Mathf.Sin(yAxis)), 0.0f);
			heldItem.AddComponent<Rigidbody2D>();
            heldItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(itemForce * xAxis, itemForce * yAxis));
            heldItem.GetComponent<Throwable>().thrown = true;
        }

		if(jumpBtn && (Time.time - prevJumpTime) >= jumpCD)
		{
			prevJumpTime = Time.time;
			rb.AddForce (new Vector2(0.0f, 2000.0f));
			animator.SetTrigger ("Jump");
		}
		if (yAxis < 0) // drop faster by holding down on the joystick's yaxis
		{
			rb.AddForce (new Vector2(0.0f, -20.0f));
		}
		
		animator.SetFloat ("yVelocity", rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
		bool pickupBtn = Input.GetButton ("Pickup" + playerNum);

        if (health >= 1)
        {
            if (other.gameObject.tag == "Bottle")
            {
				// If the player holds down the pickup button (X) for a second, they will pick up a bottle
				if (pickupBtn) 
				{
					if (!hasPickup)
					{
						hasPickup = true;
						Destroy(other.rigidbody);
						//other.gameObject.AddComponent<HingeJoint2D> ();
						//other.gameObject.GetComponent<HingeJoint2D> ().connectedBody = GameObject.FindGameObjectWithTag ("ThrowingHand").GetComponent<Rigidbody2D>();
						//other.gameObject.GetComponent<HingeJoint2D> ().enableCollision = false;
						other.gameObject.transform.parent = this.transform;
						other.transform.position = this.transform.position + new Vector3(1.0f, 0.1f);
						heldItem = other.gameObject;
					}
				}

				/*
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
                */

            }
		}
        //Check if the stairs are below the player's feet.
        if (other.gameObject.tag == "Stairs")
        {
            //If they aren't, allow player to walk 
            if (!(gameObject.transform.position.y - gameObject.GetComponent<Collider2D>().bounds.size.y / 2 > other.gameObject.transform.position.y + other.gameObject.GetComponent<Collider2D>().bounds.size.y / 2))
            {
                other.gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Stairs")
        {
            if (!(gameObject.transform.position.y - gameObject.GetComponent<Collider2D>().bounds.size.y / 2 > other.gameObject.transform.position.y + other.gameObject.GetComponent<Collider2D>().bounds.size.y / 2))
            {
                other.gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
            Debug.Log("Within trigger");
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Stairs")
        {
            Debug.Log("Exited trigger");
            other.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Stairs")
        {
            if (gameObject.transform.position.y - gameObject.GetComponent<Collider2D>().bounds.size.y/2 > other.gameObject.transform.position.y + other.bounds.size.y/2)
            {
                other.isTrigger = false;
            }
            else
            {
                other.isTrigger = true;
            }
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Stairs")
        {
            other.isTrigger = false;
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

    /// <summary>
    /// Set the level of drunkeness for a player
    /// </summary>
    /// <param name="amount">New value of the player's drunkeness out of 1000</param>
    void applyDrunk(int amount)
    {
        drunkeness = drunkeness + amount < 1000 ? drunkeness + amount : 1000;
        drunkApply = true;
        gameObject.transform.parent.GetChild(0).gameObject.GetComponent<PlayerUI>().createDrunkMeter(drunkeness);
        drunkApply = false;
    }

    void Death()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
    }
}
