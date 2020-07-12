using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleeping : MonoBehaviour {

    public int wanderRate = 100;

    private Sleeper sleeper;

    // Use this for initialization
    void Start()
    {
        sleeper = GetComponent<Sleeper>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (sleeper.getActive())
        {
            if (Random.Range(0, wanderRate) < 1)
            {
                sleeper.goToSleep();
            }
        }
        else
        {
            
        }
    }
}
