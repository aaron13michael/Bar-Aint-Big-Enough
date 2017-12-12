using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : Throwable 
{
	public int maxHits;
	int currHits;

	AudioSource audio;
	public AudioClip chairBreak;
	public AudioClip chairHit;


	// Use this for initialization
	void Start () 
	{
		audio = this.GetComponent<AudioSource>();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnCollisionEnter2D(Collision2D other)
	{
		// if the object is thrown
		if (thrown)
		{
			// if the first object the thrown chair hits is terrain
			if (other.gameObject.tag == "Terrain" || other.gameObject.tag == "Wall" || other.gameObject.tag == "Throwable")
			{
				thrown = false;
				Hit();
			}

			// if the first object the thrown bottle hits is a person and kills the person
			if (other.gameObject.tag == "Player") 
			{
				thrown = false;
				Hit();
				other.gameObject.GetComponent<Player>().applyDamage(30);
			}
		}
	}

	//Randomly plays one of the bottle break sounds
	private void PlaySound(AudioClip clip)
	{
		float pitchAmount = Random.Range(-0.4f, 0.4f);

		audio.clip = clip;
		audio.pitch = 1.0f + pitchAmount;
		audio.Play();
	}

	private void Hit()
	{
		currHits++;

		if(currHits >= maxHits)
		{
			PlaySound(chairBreak);
			DestroyThrowable();
		}
		else
		{
			PlaySound(chairHit);
		}
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
