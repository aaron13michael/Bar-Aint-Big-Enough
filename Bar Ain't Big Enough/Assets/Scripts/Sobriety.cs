using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sobriety : MonoBehaviour {

    // Use this for initialization
    public Vector3 startPosition;

    void Start () {
        startPosition = new Vector3(2.8f, -0.2f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.localScale = gameObject.transform.localScale.x > 0 ? 
            new Vector3(gameObject.transform.localScale.x - 0.1f, gameObject.transform.localScale.y) : new Vector3(60.0f, 4.0f);

        if(gameObject.transform.localScale.x > 0)
        {
            gameObject.transform.position += new Vector3(-0.0035f, 0.0f);
        }
        else
        {
            gameObject.transform.localPosition = startPosition;
        }
          
	}
}
