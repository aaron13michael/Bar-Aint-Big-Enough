using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : Throwable {

	public Sprite fullBottle;
	public Sprite emptyBottle;
	public Sprite brokenBottle;
	public AudioClip[] bBreak;

	public enum BottleState {Full, Empty, Broken};
	public BottleState bState;

	SpriteRenderer bottleSprite;
	AudioSource audio;

	// Use this for initialization
	public override void Start () 
	{
		bottleSprite = this.gameObject.GetComponent<SpriteRenderer>();	
		audio = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		gameObject.transform.rotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f); // Keeps the bottle from rotation along the Z axis

		switch(bState)
		{
			case BottleState.Full:
				bottleSprite.sprite = fullBottle;
				break;
			case BottleState.Empty:
				bottleSprite.sprite = emptyBottle;
				break;
			case BottleState.Broken:
				bottleSprite.sprite = brokenBottle;
				break;
		}
	}

	public override void OnCollisionEnter2D(Collision2D other)
	{
		// if the bottle is thrown
		if (thrown)
		{
			// onGround is false
			onGround = false;

			// if the first object the thrown bottle hits is terrain
			if (other.gameObject.tag == "Terrain" || other.gameObject.tag == "Stairs" || other.gameObject.tag == "Wall")
			{
				thrown = false;

				if(bState == BottleState.Broken)
				{
					Destroy(this.gameObject); 
				}
				else
				{
					bState = BottleState.Broken;
					PlayBreak();
				}
			}

			// if the first object the thrown bottle hits is a person and kills the person
			if (other.gameObject.tag == "Player" && !onGround) 
			{
				PlayBreak();
				thrown = false;
				onGround = true;

				if(bState == BottleState.Broken)
				{
					Destroy(this.gameObject); 
				}
				else
				{
					bState = BottleState.Broken;
				}

				other.gameObject.GetComponent<Player>().applyDamage(50);
			}
		}
	}

	//Randomly plays one of the bottle break sounds
	private void PlayBreak()
	{
		int breakIndex = Random.Range(0, bBreak.Length);

		audio.clip = bBreak[breakIndex];
		audio.Play();
	}
}
