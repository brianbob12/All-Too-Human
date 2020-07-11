using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour {

    public int index;//place of node in path

    private List<Enemy> here = new List<Enemy>();//stores a list of enemies that are at this node

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void fixedUpdate() {
        cleanup();
    }

    private void cleanup()
    {//removes null nodes from list
        if (here.Count > 0) {
            List<Enemy> temp = new List<Enemy>();
            foreach (Enemy en in here)
            {
                temp.Add(en);
            }
            foreach (Enemy en in temp)
            {
                if (en == null)
                {
                    here.Remove(en);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Enemy>() != null)
        {
            //collision with enemy
            //add enemy to here
            here.Add(collider.gameObject.GetComponent<Enemy>());

        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Enemy>() != null)
        {
            //collision with enemy
            //remove object from here
            here.Remove(collider.gameObject.GetComponent<Enemy>());

        }
    }

    public bool amIHere(Enemy sub) {//returns whether an enemy has arrived at a path node
        return here.Contains(sub);
    }
}
