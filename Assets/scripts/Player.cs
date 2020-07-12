using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public LevelManager levelManager;//level manager handels level wide variables
    public GameObject store;//a location from where the player can buy things
    public GameObject torches;//a location for the retrival of torches
    public GameObject airHorns;

    //thease are just templates with releavant components
    public GameObject torch;
    public GameObject airHorn;

    public CircleCollider2D fatCollider;//a collider that is only active when holding something

    //images
    public Sprite stand1;
    public Sprite stand2;
    public Sprite stand3;
    public Sprite stand4;
    public Sprite stand5;
    public Sprite stand6;
    public Sprite stand7;
    public Sprite stand8;

    //holding
    public Sprite hold1;
    public Sprite hold2;
    public Sprite hold3;
    public Sprite hold4;
    public Sprite hold5;
    public Sprite hold6;
    public Sprite hold7;
    public Sprite hold8;

    public const float speed = 0.13f;

    //private variables
    private GameObject holding=null;//what the player is currently holding(as Prefab) null for nothing
    private const float distanceToActivate = 1.7f;//how far from the store the player needs to be to activate object
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private bool mobile = true;//wheter the player can move 
    private bool playingWakeAnimation = false;
    private bool playingDistractAnimation = false;
    private int animationIndex = 0;
    private float lastTime;

    //animation stuff
    public float animationTime = 0.3f;//time between frames in animation
    public Sprite[] wakeAnimation;
    public Sprite[] distractAnimation;

    public void setHolding(GameObject set) {
        holding = set;
        //fatCollider.isTrigger = false;
    }


	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        spr = this.GetComponent<SpriteRenderer>();
        lastTime = getTime();
	}
	
	// Update is called once per frame
	void Update () {
        //check for animations
        if (playingWakeAnimation)
        {
            if (getTime() - lastTime > animationTime)
            {
                lastTime = getTime();
                animationIndex += 1;
                if (animationIndex >= wakeAnimation.Length)
                {
                    mobile = true;
                    playingWakeAnimation = false;
                }
                else
                {
                    spr.sprite = wakeAnimation[animationIndex];
                }
            }
        }
        else if (playingDistractAnimation)
        {
            if (getTime() - lastTime > animationTime)
            {
                lastTime = getTime();
                animationIndex += 1;
                if (animationIndex >= distractAnimation.Length)
                {
                    mobile = true;
                    playingDistractAnimation = false;
                }
                else
                {
                    spr.sprite = distractAnimation[animationIndex];
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!levelManager.getPause()&&mobile) {
            Vector2 toMove =  new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (toMove.magnitude > 0) { pointToVec(toMove); }//point sprite to direction of movement
            rb.MovePosition((Vector2)transform.position + speed * toMove);

            if (Input.GetButtonDown("Fire1")||Input.GetKeyDown(KeyCode.Return)) {
                //check distance to store
                if (((Vector2)(this.transform.position - store.transform.position)).magnitude < distanceToActivate)
                {
                    //at store
                    if (holding == null)
                    {
                        levelManager.openStore();
                    }
                    else
                    {
                        //indicate to the player that they can't buy robots while holding an object
                    }
                }//check for torches
                else if (((Vector2)(this.transform.position - torches.transform.position)).magnitude < distanceToActivate)
                {
                    //at store
                    if (holding == null)
                    {
                        holding=torch;
                    }
                    else
                    {
                        //indicate to the player that they can't buy robots while holding an object
                    }
                }//check for air horns
                else if (((Vector2)(this.transform.position - airHorns.transform.position)).magnitude < distanceToActivate)
                {
                    //at store
                    if (holding == null)
                    {
                        holding = airHorn;
                    }
                    else
                    {
                        //indicate to the player that they can't buy robots while holding an object
                    }
                }
                else if (holding == null) {
                    //iterate over wandereres
                    Object[] roaming = Object.FindObjectsOfType(typeof(GameObject));
                    foreach (Object w in roaming)
                    {
                        GameObject temp = (GameObject)w;
                        //check for wanderer
                        if (temp.GetComponent<Wanderer>() != null) {
                            if (!temp.GetComponent<Tower>().getActive())
                            {
                                if (((Vector2)(this.transform.position - temp.transform.position)).magnitude < distanceToActivate) {
                                    //pick up wanderer
                                    holding = levelManager.wandererPrefab;//hold wanderer
                                    Destroy(temp);//poosible foreach issues
                                }
                            }
                        }
                    }
                }
                else if (holding != null)
                {
                    if (holding.GetComponent<Tower>() != null)//if holding tower
                    {
                        //try to place robot
                        placeTower();
                    }
                    else if (holding.GetComponent<WakeUpKit>() != null) {//if holding wakeupKit
                        Debug.Log("wakeUpKit");
                        Object[] roaming = Object.FindObjectsOfType(typeof(GameObject));
                        foreach (Object w in roaming)
                        {
                            GameObject temp = (GameObject)w;
                            if (temp.GetComponent<Sleeper>() != null)//check for sleeper
                            {
                                Debug.Log("sleeper detected");
                                if (!temp.GetComponent<Tower>().getActive())//check if sleeping
                                {
                                    Debug.Log("sleeping detected");
                                    if (((Vector2)(this.transform.position - temp.transform.position)).magnitude < distanceToActivate)//check if in range
                                    {
                                        //wake up tower
                                        holding = null;//delete holding
                                        temp.GetComponent<Sleeper>().wake();
                                        playWakeAnimation();
                                    }
                                }
                            }
                        }
                    }
                    else if (holding.GetComponent<Airhorn>() != null)
                    {//if holding wakeupKit
                        Object[] roaming = Object.FindObjectsOfType(typeof(GameObject));
                        foreach (Object w in roaming)
                        {
                            GameObject temp = (GameObject)w;
                            if (temp.GetComponent<FlowerWatch>() != null)//check for sleeper
                            {
                                if (!temp.GetComponent<Tower>().getActive())//check if sleeping
                                {
                                    if (((Vector2)(this.transform.position - temp.transform.position)).magnitude < distanceToActivate)//check if in range
                                    {
                                        //wake up tower
                                        holding = null;//delete holding
                                        temp.GetComponent<FlowerWatch>().wake();
                                        playWakeAnimation();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
    }

    private void placeTower() {
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, 3);//for camera layering
        Tower placed=Instantiate(holding, newPos, Quaternion.identity).GetComponent<Tower>();
        //setup
        placed.levelManager = levelManager;
        noHolding();
    }

    private void noHolding() {//sets the player to not holding anything
        fatCollider.isTrigger = true;
        holding = null;
    }

    private void pointToVec(Vector2 target)//points the player sprite to face the direction closest to target
    {
        //targetting
        float angle = Vector2.SignedAngle(transform.up, target);
        if (angle < -157.5)
        {
            if (holding == null)
            {
                spr.sprite = stand5;
            }
            else
            {
                spr.sprite = hold5;
            }
        }
        else if (angle < -112.5)
        {
            if (holding == null)
            {
                spr.sprite = stand6;
            }
            else
            {
                spr.sprite = hold6;
            }
        }
        else if (angle < -67.5)
        {
            if (holding == null)
            {
                spr.sprite = stand7;
            }
            else
            {
                spr.sprite = hold7;
            }
        }
        else if (angle < -22.5)
        {
            if (holding == null)
            {
                spr.sprite = stand8;
            }
            else
            {
                spr.sprite = hold8;
            }
        }
        else if (angle < 22.5)
        {
            if (holding == null)
            {
                spr.sprite = stand1;
            }
            else
            {
                spr.sprite = hold1;
            }
        }
        else if (angle < 67.5)
        {
            if (holding == null)
            {
                spr.sprite = stand2;
            }
            else
            {
                spr.sprite = hold2;
            }
        }
        else if (angle < 112.5)
        {
            if (holding == null)
            {
                spr.sprite = stand3;
            }
            else
            {
                spr.sprite = hold3;
            }
        }
        else if (angle < 157.5)
        {
            if (holding == null)
            {
                spr.sprite = stand4;
            }
            else
            {
                spr.sprite = hold4;
            }
        }
        else
        {
            if (holding == null)
            {
                spr.sprite = stand5;
            }
            else
            {
                spr.sprite = hold5;
            }
        }
    }

    private void playWakeAnimation() {
        //TODO finish this
        mobile = false;
        playingWakeAnimation = true;
        lastTime = getTime();
        animationIndex = 0;
        spr.sprite = wakeAnimation[animationIndex];
    }
    private void playDistractAnimation()
    {
        //TODO finish this
        mobile = false;
        playingDistractAnimation = true;
        lastTime = getTime();
        animationIndex = 0;
        spr.sprite = distractAnimation[animationIndex];
    }

    private float getTime()
    {//returns a float of seconds since jan 1 1970

        float time = (float)((System.DateTime.Now - new System.DateTime(1970, 1, 1)).TotalSeconds - 1594000000);
        return time;
    }
}
