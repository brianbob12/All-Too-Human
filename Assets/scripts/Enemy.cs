using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public LevelManager levelManager;//level manager handels level wide variables
    public Path path;//the path that the enemy will follow
    public float speed = 0.1f;//if the speed is too fast it will lead to issues

    //private variables
    private int health = 100;
    private int node = 0;
    private PathNode target=null;
    private bool finished = false;//is true when enemy has finished path
    private Rigidbody2D rb;
    private Vector2 going;//the direction that the enemy is travelling in

    public int getHealth() { return this.health; }
    public void setHealth(int toSet) { this.health = toSet; }
    public Vector2 getGoing() { return going; }
    public bool damage(int damage) {//decraeses health and returns whether this enemy has died
        this.health -= damage;
        //check death
        return this.checkDie();
    }

	// Use this for initialization
	void Start () {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
    private void FixedUpdate()
    {
        if (!levelManager.getPause())
        {
            if (finished)
            {
                //remove life
                levelManager.removeLife();
                //die
                Destroy(this.gameObject);
                return;
            }
            //if not finished
            if (target == null)
            {//if first frame
                target = path.getPathNode(0);
            }
            else
            {
                //check if arrived at current target
                if (target.amIHere(this))
                {
                    node += 1;
                    if (node >= path.finalNode())
                    {
                        finished = true;
                    }
                    else
                    {
                        //update target
                        target = path.getPathNode(node);
                    }
                }
                else
                {
                    //move towards node
                    Vector2 toMove = target.transform.position - this.transform.position;
                    toMove /= toMove.magnitude;//normalize
                    toMove *= speed;
                    going = toMove;//pointers
                    rb.MovePosition((Vector2)this.transform.position + toMove);
                }
            }
        }
    }

    public bool checkDie() {//checks if enmy is dead returns if the enemy is dead
        if (this.health <= 0) {
            //manage death
            //add money
            levelManager.addMoney((int)strengthScore());
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
        return 1f;//TODO finish
    }
    public float aheadScore()//returns a value for the distance of the enemy along the track
    {
        return 0f;//TODO finsih
    }
}
