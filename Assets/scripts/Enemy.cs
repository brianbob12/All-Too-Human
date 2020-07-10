using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //private variables
    private int health = 100;

    public int getHealth() { return this.health; }
    public void setHealth(int toSet) { this.health = toSet; }
    public bool damage(int damage) {//decraeses health and returns whether this enemy has died
        this.health -= damage;
        //check death
        return this.checkDie();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool checkDie() {//checks if enmy is dead returns if the enemy is dead
        if (this.health <= 0) {
            //manage death
            Destroy(this.gameObject);//deletes itself
            return true;
        }
        return false;
    }

    //functions for target selection
    public float closeScore(GameObject targetter)//distance between this and the targetter
    {
        return Mathf.Abs((targetter.transform.position-this.transform.position).magnitude);
    }
    public float strengthScore()//returns a value for the strenght of enemy
    {
        return 0f;//TODO finish
    }
    public float aheadScore()//returns a value for the distance of the enemy along the track
    {
        return 0f;//TODO finsih
    }
}
