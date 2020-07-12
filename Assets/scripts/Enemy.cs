using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public LevelManager levelManager;//level manager handels level wide variables
    public Path path;//the path that the enemy will follow
    public float speed = 0.1f;//if the speed is too fast it will lead to issues
    public int startingHealth;
    public int strength;
    public Sprite im1;
    public Sprite im2;

    //private variables
    private int health;
    private int node = 0;
    private PathNode target=null;
    private bool finished = false;//is true when enemy has finished path
    private Rigidbody2D rb;
    private Vector2 going;//the direction that the enemy is travelling in
    private SpriteRenderer spr;
    private bool im1Sel = true;//used for switching images

    public int getHealth() { return this.health; }
    public int imageChangeInverseFrequency = 30;
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
        health = startingHealth;
        spr = gameObject.GetComponent<SpriteRenderer>();
	}

    // Update is called once per frame
    void Update()
    {
        if (!levelManager.getPause()) {
            if (Random.Range(0, imageChangeInverseFrequency) < 1)
            {
                if (im1Sel)
                {
                    spr.sprite = im1;
                }
                else
                {
                    spr.sprite = im2;
                }
                im1Sel = !im1Sel;
            }
        }
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
        return strength;
    }
    public float aheadScore()//returns a value for the distance of the enemy along the track
    {
        float distanceToNextNode = (transform.position - target.transform.position).magnitude;
        if (node > 0) {
           
            distanceToNextNode /= (target.transform.position - path.getPathNode(node - 1).transform.position).magnitude;//as a fraction of total distance
            return node + distanceToNextNode;
        }
        //else
        return 1/distanceToNextNode;
    }
}
