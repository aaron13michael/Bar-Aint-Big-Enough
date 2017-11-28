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
			if (other.gameObject.tag == "Terrain" || other.gameObject.tag == "Stairs")
			{
				thrown = false;
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-gameObject.GetComponent<Rigidbody2D>().velocity.x * .8f, -gameObject.GetComponent<Rigidbody2D>().velocity.y * .8f));

				if(bState == BottleState.Broken)
				{
					//Destroy(this.gameObject); Will destroy once bottles spawn
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

				if(bState == BottleState.Broken)
				{
					//Destroy(this.gameObject); Will destroy once bottles spawn
				}
				else
				{
					bState = BottleState.Broken;
					PlayBreak();
				}

				Destroy (other.gameObject);
				thrown = false;
				onGround = true;
			}
		}
		else
		{


		}
	}

	//Randomly plays one of the bottle break sounds
	private void PlayBreak()
	{
		int breakIndex = Random.Range(0, bBreak.Length - 1);

		audio.clip = bBreak[breakIndex];
		audio.Play();
	}
}
