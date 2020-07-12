using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerWatch : MonoBehaviour {
    //wander rate
    public int wanderRate = 100;

    private SpriteRenderer spr;
    private Tower tow;
    private bool playingAnimation = false;
    private int animationIndex = 0;
    private float lastTime;

    //animation stuff
    public float animationTime = 0.3f;//time between frames in animation
    public Sprite[] anim;

    // Use this for initialization
    void Start () {
        tow = GetComponent<Tower>();
        spr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (playingAnimation)
        {
            if (getTime() - lastTime > animationTime)
            {
                animationIndex += 1;
                lastTime = getTime();
                if (animationIndex >= anim.Length)
                {
                    wake();
                }
                else
                {
                    spr.sprite = anim[animationIndex];

                }
            }
        }
	}

    private void FixedUpdate()
    {
        if (Random.Range(0, wanderRate) < 1)
        {
            distract();
        }
    }

    public void wake() {
        playingAnimation = false;
        tow.setActive(true);
    }

    private void distract()
    {
        playingAnimation = true;
        tow.setActive(false);
        lastTime = getTime();
        animationIndex = 0;
        spr.sprite = anim[animationIndex];
    }

    private float getTime()
    {//returns a float of seconds since jan 1 1970

        float time = (float)((System.DateTime.Now - new System.DateTime(1970, 1, 1)).TotalSeconds - 1594000000);
        return time;
    }
}
