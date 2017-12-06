using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	// player health
    public int health;

	bool dead = false;

	// checks if player is holding a pick up
    bool hasPickup = false;

    // True if drunkeness is increasing. False otherwise.
    bool drunkApply = false;

	public string Label;

	// gameobject to store the held item
    public GameObject heldItem;
	// item force and direction
    public float itemForce = 0.0f;
    public int direction = 0; 

	// Is this player 1, 2, 3, or 4?
	public int playerNum;

	public float prevJumpTime; // the last time that jump was used

	private AudioSource audio;
	public AudioClip gulpSound;
	public AudioClip[] punchSounds;

	private Rigidbody2D rb;
	private Animator animator;

	private bool grounded;
	private float drunkDecreaseCD; //time since the drunk meter last went down
	private int drunkeness;
    private float modifier; //value for modifying throw strength and movement speed for being drunk

	//Punch variables
	public float punchCooldown;
	public float punchLength;
	private float currPCooldown;
	private bool isPunching;
	private bool canPunch;
	Transform punchArm;

    //UI Meters
    Transform drunkMeter;
    Transform healthMeter;
    PlayerUI UI;

    // Use this for initialization
    void Start () 
	{
        health = 100;
        drunkeness = 50;
		if(playerNum > 4 || playerNum <= 0)
		{
			playerNum = 1;
		}
			
		prevJumpTime = -2.0f;
		grounded = true;
		rb = GetComponent<Rigidbody2D> ();
		audio = GetComponent<AudioSource>();
		animator = GetComponent<Animator> ();
		animator.SetBool ("Right", true);

		GameObject uiParent = GameObject.Find ("Player" + playerNum + "UI");

		drunkMeter = uiParent.transform.GetChild(2);
		healthMeter = uiParent.transform.GetChild(1);
		UI = uiParent.GetComponent<PlayerUI>();

		punchArm = transform.GetChild(5).GetChild(0).GetChild(0);

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

			// UI Meter
            //Slowly decrease drunkeness level
            if (drunkMeter.transform.childCount > 0 && !drunkApply && drunkDecreaseCD >= 0.5f)
            {
                Destroy(drunkMeter.transform.GetChild(drunkeness - 1).gameObject);
                drunkDecreaseCD = 0;
                drunkeness = drunkeness > 0 ? drunkeness - 1 : 0;
            }
            else
            {
                drunkDecreaseCD += Time.deltaTime;
            }
            
            if (drunkeness > 0)
            {
                modifier = 1.0f + (drunkeness / 100.0f);
            }
            else
            {
                modifier = 1.0f;
            }
        }

		// Apply extra gravity to the player
		rb.AddForce(new Vector2(0.0f, -8.0f));

		if (!grounded) 
		{
			if (animator.GetBool("Right")) 
			{
				rb.AddForce (new Vector2 (-20.0f, 0.0f));
			}
			else 
			{
				rb.AddForce (new Vector2 (20.0f, 0.0f));
			}
		}

		//Update punch timer if currently punching
		if (currPCooldown < punchCooldown) 
		{
			currPCooldown += Time.deltaTime;
		}
		else 
		{
			canPunch = true;
		}

		if (currPCooldown >= punchLength) 
		{
			isPunching = false;
		}
	}

    void ProcessInput()
    {
		if(dead) {return;}
		float xAxis = Input.GetAxis("LeftX" + playerNum);
		float yAxis = Input.GetAxis("LeftY" + playerNum);
		bool jumpBtn = Input.GetButton("Jump" + playerNum);
		bool throwBtn = Input.GetButton("Throw" + playerNum);
        bool useBtn = Input.GetButton("Use" + playerNum);
		float punch = Input.GetAxis ("Punch" + playerNum);

		// Run
		if(xAxis > 0) 
		{
			rb.AddForce (new Vector2 (30.0f, 0.0f));
			animator.SetBool ("Right", true);
		}
		else if(xAxis < 0)
		{
			rb.AddForce (new Vector2 (-30.0f, 0.0f));
			animator.SetBool ("Right", false);
		}
		else if(xAxis == 0)
		{
			rb.velocity = new Vector2 (0.0f, rb.velocity.y);
		}

		// Make sure the player is not going too fast
		if (rb.velocity.x > 7.0f) 
		{
			rb.velocity = new Vector2 (7.0f, rb.velocity.y);
		}
		if (rb.velocity.x < -7.0f) 
		{
			rb.velocity = new Vector2 (-7.0f, rb.velocity.y);
		}

		// Throw
		if(throwBtn && hasPickup)
		{
			hasPickup = false;
			heldItem.transform.parent = null;

			if(xAxis == 0 && yAxis == 0)
			{
				heldItem.AddComponent<Rigidbody2D>(); //Drop

				if(heldItem.transform.position.x > this.gameObject.transform.position.x)
				{
					heldItem.transform.position = this.transform.position + new Vector3(0.7f, -0.7f, 0.0f);		
				}
				else
				{
					heldItem.transform.position = this.transform.position + new Vector3(-0.7f, -0.7f, 0.0f);	
				}
			}
			else
			{
				if(xAxis > 0)
				{
					heldItem.transform.position = this.transform.position + new Vector3(2.5f * (Mathf.Cos(xAxis)), 2.5f * (Mathf.Sin(yAxis)), 0.0f);
				}
				else if(xAxis < 0)
				{
					heldItem.transform.position = this.transform.position + new Vector3(-2.5f * (Mathf.Cos(xAxis)), 2.5f * (Mathf.Sin(yAxis)), 0.0f);
				}
				else
				{
					heldItem.transform.position = this.transform.position + new Vector3(0.0f, 2.5f * (Mathf.Sin(yAxis)), 0.0f);	
				}

				heldItem.AddComponent<Rigidbody2D>();
				heldItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(itemForce * modifier * xAxis, itemForce * modifier * yAxis));
				heldItem.GetComponent<Throwable>().thrown = true;	
			}
				
        }
        // Drink bottle
        if(useBtn && hasPickup)
        {
            if (heldItem.GetComponent<Bottle>().bState == Bottle.BottleState.Full)
            {
                heldItem.GetComponent<Bottle>().bState = Bottle.BottleState.Empty;
				applyDrunk(20);
				PlaySound(gulpSound, 0.5f);
                if (drunkeness >= 100)
                {
					Death();
                }
            }
        }

        // Jump
        if(jumpBtn && grounded)
		{
			prevJumpTime = Time.time;
			rb.velocity = new Vector2 (rb.velocity.x, 8.0f);
			animator.SetTrigger ("Jump");
		}

		//Punch
		if (punch > 0.3f && canPunch && !hasPickup) 
		{
			//Animator stuff goes here
			punchArm.tag = "PunchArm";
			isPunching = true;
			canPunch = false;
			currPCooldown = 0;

			int pSoundIndex = Random.Range (0, 1);

			PlaySound (punchSounds[pSoundIndex], 0.8f);
			animator.SetTrigger ("Punch");
		}

		animator.SetFloat ("xVelocity", rb.velocity.x);
		animator.SetFloat ("yVelocity", rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
		if (!hasPickup) 
		{
			if (other.gameObject.tag == "Bottle") 
			{
				hasPickup = true;
				Destroy (other.rigidbody);
				other.gameObject.transform.parent = GameObject.Find ("Hand" + playerNum).transform;
				other.transform.position = GameObject.Find ("Hand" + playerNum).transform.position;
				heldItem = other.gameObject;
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

		// check if player is being punched
		if (other.gameObject.tag == "Player" && isPunching) 
		{
			other.gameObject.GetComponent<Player>().applyDamage(Mathf.RoundToInt(15 * modifier));
			currPCooldown = 0;
			isPunching = false;
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
        }

		if (other.gameObject.tag == "Terrain" || other.gameObject.tag == "Stairs") 
		{
			grounded = true;
			animator.SetBool ("Grounded", true);
		}
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Stairs")
        {
            Debug.Log("Exited trigger");
            other.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }

		if (other.gameObject.tag == "Terrain" || other.gameObject.tag == "Stairs") 
		{
			grounded = false;
			animator.SetBool ("Grounded", false);
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
    public void applyDamage(int damage)
    {
        //Used to update health meter
        int oldHealth = health;

        //Check that health does not go below 0
        health = health - damage > 0 ? health - damage : 0;
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
        drunkeness = drunkeness + amount < 100 ? drunkeness + amount : 100;
        drunkApply = true;
        UI.createDrunkMeter(drunkeness);
        drunkApply = false;
    }

	//Plays a clip at a certain volume. Randomizes pitch slightly.
	private void PlaySound(AudioClip clip, float volume)
	{
		float pitchAmount = Random.Range(-0.4f, 0.4f);

		audio.clip = clip;
		audio.volume = volume;
		audio.pitch = 1.0f + pitchAmount;
		audio.Play();
	}

    void Death()
    {
		dead = true;

		/*
		if(audio.isPlaying)
		{
			Destroy (gameObject, audio.clip.length);	
		}
		else
		{
			Destroy(gameObject);
		}*/

		Destroy (gameObject);
			
    }
}
