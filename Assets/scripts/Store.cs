using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour {

    private SpriteRenderer spr;

    public LevelManager levelManager;
    public float animWait = 1f;

    public Sprite[] sequence;

    private float lastTime;
    private int index=0;


    // Use this for initialization
    void Start () {
        spr = GetComponent<SpriteRenderer>();
        lastTime = getTime();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (levelManager.getWaveOn())
        {
            if ( getTime()- lastTime > animWait) { 
                index += 1;
                index %= sequence.Length;
                spr.sprite = sequence[index];
                lastTime = getTime();
            }
        }
        else
        {
            index = 0;
            spr.sprite = sequence[index];
            lastTime = getTime();
        }
        
    }

    private float getTime()
    {//returns a float of seconds since jan 1 1970

        float time = (float)((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds - 1594000000);
        return time;
    }
}
