using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdderallSpawner : MonoBehaviour {

    public float timeDelayUpper;
    public float timeDelayLower;
    public GameObject template;
    public float lifeTime = 12;//lifetime of adderall in seconds
    public int onlySpawnAfterWave = 1;

    private float lastTime;
    private float timeGoal;

    // Use this for initialization
    void Start () {
        lastTime = getTime();
        timeGoal = Random.Range(timeDelayLower, timeDelayUpper);
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<LevelManager>().getWave() > onlySpawnAfterWave)
        {
            if (getTime() - lastTime > timeGoal)
            {
                newAdderall();
                lastTime = getTime();
                timeGoal = Random.Range(timeDelayLower, timeDelayUpper);
            }
        }
	}
    /*
    private void newAdderall()
    {
        Vector3 location = new Vector3(Random.Range(-12, 6.45f), Random.Range(-4.46f, 4.6f), 9);//perspective location

        Object[] roaming = Object.FindObjectsOfType(typeof(GameObject));
        foreach (Object w in roaming)
        {
            GameObject temp = (GameObject)w;
            //check if in collider
            Collider2D[] colliders = temp.GetComponents<Collider2D>();
            if (colliders == null)
            {
                continue;
            }
            else {
                foreach (Collider2D col in colliders) {
                    if (col.bounds.Contains((Vector2)location)) {
                        //failed location
                        Debug.Log("failed attempt");
                        newAdderall();//try agiain
                        //then proply close
                        return;
                    }
                }
            }
        }
        //reached the end without failing

        GameObject placed = Instantiate(template, location, Quaternion.identity);
        if (placed.GetComponent<Adderall>() != null)
        {
            placed.GetComponent<Adderall>().lifeTime = lifeTime;
        }
    }
    */

    private void newAdderall() {
        GameObject placed = null;
        Vector3 location;
        location = new Vector3(Random.Range(-12, 6.45f), Random.Range(-4.46f, 4.6f), 9);//perspective location
        placed = Instantiate(template, location, Quaternion.identity);
        placed.GetComponent<Adderall>().lifeTime = lifeTime;
        while (true)//check for collisions
        {
            Collider2D[] colliders = new Collider2D[1];
            ContactFilter2D contactFilter = new ContactFilter2D();
            //contactFilter.useTriggers = false;
            //https://docs.unity3d.com/ScriptReference/ContactFilter2D.html
            int colliderCount = placed.GetComponent<Collider2D>().OverlapCollider(contactFilter, colliders);
            if (colliderCount != 0)
            {
                location = new Vector3(Random.Range(-12, 6.45f), Random.Range(-4.46f, 4.6f), 9);//perspective location
                placed.transform.position = location;
            }
            else {
                //sucess
                break;
            }

        }
    }
    private float getTime()
    {//returns a float of seconds since jan 1 1970

        float time = (float)((System.DateTime.Now - new System.DateTime(1970, 1, 1)).TotalSeconds - 1594000000);
        return time;
    }
}
