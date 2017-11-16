using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    public Sprite healthSprite;

    // Use this for initialization
    void Start () {
        // Generate Health Meter. This assumes the Player UI is the first child of the Player GameObject

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
	
	// Update is called once per frame
	void Update () {
		
	}
}
