﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    public Sprite healthSprite;
    public Sprite drunkSprite;

    // Use this for initialization
    void Start () {
        // Generate Health Meter. This assumes the Player UI is the first child of the Player GameObject
        createHealthMeter();

        // Generate Drunk Metter
        createDrunkMeter();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void createHealthMeter()
    {
        Transform healthMeter = gameObject.transform.GetChild(1);
        for (int i = 0; i < 100; i++)
        {
            GameObject healthChunk = new GameObject("health chunk", typeof(SpriteRenderer));
            healthChunk.GetComponent<SpriteRenderer>().sprite = healthSprite;
            healthChunk.transform.localScale = new Vector3(0.6f, 4.0f, 1.0f);
            healthChunk.transform.SetParent(healthMeter);
            healthChunk.transform.localPosition = new Vector3(i * 0.042f, 0.0f, 0.0f);
        }
    }

    void createDrunkMeter()
    {
        Transform drunkMeter = gameObject.transform.GetChild(2);
        for (int i = 0; i < 1000; i++)
        {
            GameObject drunkChunk = new GameObject("drunk chunk", typeof(SpriteRenderer));
            drunkChunk.GetComponent<SpriteRenderer>().sprite = drunkSprite;
            drunkChunk.transform.localScale = new Vector3(0.06f, 4.0f, 1.0f);
            drunkChunk.transform.SetParent(drunkMeter);
            drunkChunk.transform.localPosition = new Vector3(i * 0.0042f, 0.0f, 0.0f);
        }
    }


}
