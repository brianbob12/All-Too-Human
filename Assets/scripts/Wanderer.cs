using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour {

    public float wanderingSpeed;
    public int wanderRate = 100;

    private Tower tower;
    private Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        tower = GetComponent<Tower>();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (tower.getActive())
        {
            if (Random.Range(0, wanderRate)<1)
            {
                tower.setActive(false);
                distract();
                rb.constraints = RigidbodyConstraints2D.None|RigidbodyConstraints2D.FreezeRotation;
            }
        }
        else {
            //TODO walking animation
            if (Random.Range(0, wanderRate/4) < 1)
            {
                distract();
            }
        }
    }

    protected void distract() {
        rb.AddForce(new Vector2(Random.Range(-wanderingSpeed, wanderingSpeed), Random.Range(-wanderingSpeed, wanderingSpeed)));
    }
}
