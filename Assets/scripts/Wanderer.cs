using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour {

    public float wanderingSpeed;
    public int wanderRate = 100;

    public Sprite[] walkingAnimation;

    private float lastTime;
    public float animationTime=1.3f;
    private int animationIndex;

    private Tower tower;
    private Rigidbody2D rb;
    private SpriteRenderer spr;

    // Use this for initialization
    void Start () {
        tower = GetComponent<Tower>();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        spr = GetComponentInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!tower.getActive()) {
            if (getTime() - lastTime > animationTime)
            {
                lastTime = getTime();
                animationIndex += 1;
                animationIndex %= walkingAnimation.Length;
                spr.sprite = walkingAnimation[animationIndex];
            }
        }
	}

    private void FixedUpdate()
    {
        if (tower.getActive())
        {
            if (Random.Range(0, wanderRate)<1&&!tower.getFocused())
            {
                tower.setActive(false);
                distract();
                lastTime = getTime();
                animationIndex = -1;
                rb.constraints = RigidbodyConstraints2D.None|RigidbodyConstraints2D.FreezeRotation;
            }
            if (tower.getTarget() == null)
            {
                if (tower.getFocused())
                {
                    spr.sprite = tower.focused7;
                }
                else
                {
                    spr.sprite = tower.stand7;
                }
            }
        }
        else {
            if (Random.Range(0, wanderRate/6) < 1)
            {
                distract();
            }
            
        }
    }

    protected void distract() {
        rb.AddForce(new Vector2(Random.Range(-wanderingSpeed, wanderingSpeed), Random.Range(-wanderingSpeed, wanderingSpeed)));
    }

    private float getTime()
    {//returns a float of seconds since jan 1 1970

        float time = (float)((System.DateTime.Now - new System.DateTime(1970, 1, 1)).TotalSeconds - 1594000000);
        return time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!tower.getActive()) {
            distract();
        }
    }
}
