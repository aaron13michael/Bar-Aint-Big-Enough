﻿using System.Collections;
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
			// if the first object the thrown bottle hits is terrain
			if (other.gameObject.tag == "Terrain" || other.gameObject.tag == "Wall" || other.gameObject.tag == "Throwable")
			{
				thrown = false;
				PlayBreak();

				if(bState == BottleState.Broken)
				{
					DestroyThrowable();
				}
				else
				{
					bState = BottleState.Broken;
				}
			}

			// if the first object the thrown bottle hits is a person and kills the person
			if (other.gameObject.tag == "Player") 
			{
				PlayBreak();
				thrown = false;

				if(bState == BottleState.Full)
				{
                    other.gameObject.GetComponent<Player>().applyDrunk(15);
				}
                DestroyThrowable();
                other.gameObject.GetComponent<Player>().applyDamage((int)(15 * modifier));
			}
		}
	}

	//Randomly plays one of the bottle break sounds
	private void PlayBreak()
	{
		int breakIndex = Random.Range(0, bBreak.Length);

		float pitchAmount = Random.Range(-0.4f, 0.4f);

		audio.clip = bBreak[breakIndex];
		audio.pitch = 1.0f + pitchAmount;
		audio.Play();
	}

	protected override void DestroyThrowable()
	{
		if(audio.isPlaying)
		{
			this.GetComponent<SpriteRenderer>().enabled = false;
			this.GetComponent<PolygonCollider2D>().enabled = false;
			Destroy(this.GetComponent<Rigidbody2D>());
			Destroy (gameObject, audio.clip.length);	
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
