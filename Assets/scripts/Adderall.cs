using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adderall : MonoBehaviour {

    private float spawnTime;
    public float lifeTime=12;//lifetime of Adderall

	// Use this for initialization
	void Start () {
        spawnTime = getTime();
	}
	
	// Update is called once per frame
	void Update () {
        if (getTime() - spawnTime > lifeTime) {
            Destroy(this);//die
        }
	}

    private float getTime()
    {//returns a float of seconds since jan 1 1970

        float time = (float)((System.DateTime.Now - new System.DateTime(1970, 1, 1)).TotalSeconds - 1594000000);
        return time;
    }

    
}
