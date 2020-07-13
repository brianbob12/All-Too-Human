using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public LevelManager levelManager;//level manager handels level wide variables
    public GameObject store;//a location from where the player can buy things
    public GameObject torches;//a location for the retrival of torches
    public GameObject airHorns;
    public GameObject trashCan;


    //thease are just templates with releavant components
    public GameObject torch;
    public GameObject airHorn;
    public GameObject adderall;

    public CircleCollider2D fatCollider;//a collider that is only active when holding something

    //images
    //thase won't show up in inspetor
    public Sprite[][] walkingNone=new Sprite[8][];//a list of 8 sprite lists consisting of walk animations for each direction
    public Sprite[][] walkingCase= new Sprite[8][];
    public Sprite[][] wakeAnimation = new Sprite[8][];//stores animations for each direction
    public Sprite[][] distractAnimation = new Sprite[8][];
    //thease will show in inpector
    public Sprite[] walkingNone1;
    public Sprite[] walkingNone2;
    public Sprite[] walkingNone3;
    public Sprite[] walkingNone4;
    public Sprite[] walkingNone5;
    public Sprite[] walkingNone6;
    public Sprite[] walkingNone7;
    public Sprite[] walkingNone8;
    public Sprite[] walkingCase1;
    public Sprite[] walkingCase2;
    public Sprite[] walkingCase3;
    public Sprite[] walkingCase4;
    public Sprite[] walkingCase5;
    public Sprite[] walkingCase6;
    public Sprite[] walkingCase7;
    public Sprite[] walkingCase8;

    public Sprite[] wake1;
    public Sprite[] wake2;
    public Sprite[] wake3;
    public Sprite[] wake4;
    public Sprite[] wake5;
    public Sprite[] wake6;
    public Sprite[] wake7;
    public Sprite[] wake8;
    public Sprite[] distract1;
    public Sprite[] distract2;
    public Sprite[] distract3;
    public Sprite[] distract4;
    public Sprite[] distract5;
    public Sprite[] distract6;
    public Sprite[] distract7;
    public Sprite[] distract8;

    public Sprite[] stillNone;
    public Sprite[] stillCase;
    //holding

    public const float speed = 0.13f;

    //private variables
    private GameObject holding=null;//what the player is currently holding(as Prefab) null for nothing
    private const float distanceToActivate = 1.7f;//how far from the store the player needs to be to activate object
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private bool mobile = true;//wheter the player can move 
    private bool moving = false;//if player is walking
    private bool playingWakeAnimation = false;
    private bool playingDistractAnimation = false;
    private int animationIndex = 0;
    private float lastTime;
    private int direction = 0;//the direction that the player is pointing
    private int smallDirection = 0;//the direction of the player limited to four directions

    //animation stuff
    public float animationTime = 0.3f;//time between frames in animation
    

    public void setHolding(GameObject set) {
        holding = set;
        //fatCollider.isTrigger = false;
    }
    public GameObject getHolding() { return holding; }

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        spr = this.GetComponent<SpriteRenderer>();
        lastTime = getTime();

        //initialise 2d arrays
        //walkingNone = new Sprite[][8] { };
        walkingNone[0] = walkingNone1;
        walkingNone[1] = walkingNone2;
        walkingNone[2] = walkingNone3;
        walkingNone[3] = walkingNone4;
        walkingNone[4] = walkingNone5;
        walkingNone[5] = walkingNone6;
        walkingNone[6] = walkingNone7;
        walkingNone[7] = walkingNone8;
        walkingCase[0] = walkingCase1;
        walkingCase[1] = walkingCase2;
        walkingCase[2] = walkingCase3;
        walkingCase[3] = walkingCase4;
        walkingCase[4] = walkingCase5;
        walkingCase[5] = walkingCase6;
        walkingCase[6] = walkingCase7;
        walkingCase[7] = walkingCase8;

        wakeAnimation[0] = wake1;
        wakeAnimation[1] = wake2;
        wakeAnimation[2] = wake3;
        wakeAnimation[3] = wake4;
        wakeAnimation[4] = wake5;
        wakeAnimation[5] = wake6;
        wakeAnimation[6] = wake7;
        wakeAnimation[7] = wake8;
        distractAnimation[0] = distract1;
        distractAnimation[1] = distract2;
        distractAnimation[2] = distract3;
        distractAnimation[3] = distract4;
        distractAnimation[4] = distract5;
        distractAnimation[5] = distract6;
        distractAnimation[6] = distract7;
        distractAnimation[7] = distract8;
    }

    // Update is called once per frame
    void Update()
    {
        //check for interactions
        if (!levelManager.getPause() && mobile)
        {
            Vector2 toMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (toMove.magnitude > 0)
            {
                moving = true;
                pointToVec(toMove);
            }//point sprite to direction of movement
            else
            {
                moving = false;
                animationIndex = 0;
            }
            rb.MovePosition((Vector2)transform.position + speed * toMove);

            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Return))
            {
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
                        Debug.Log("already holding item");
                        //indicate to the player that they can't buy robots while holding an object
                    }
                }//check for torches
                else if (((Vector2)(this.transform.position - torches.transform.position)).magnitude < distanceToActivate)
                {
                    //at store
                    if (holding == null)
                    {
                        holding = torch;
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
                else if (((Vector2)(this.transform.position - trashCan.transform.position)).magnitude < distanceToActivate)
                {
                    //at trashCan
                    holding = null;
                }
                else if (holding == null)
                {
                    //iterate over wandereres
                    Object[] roaming = Object.FindObjectsOfType(typeof(GameObject));
                    foreach (Object w in roaming)
                    {
                        GameObject temp = (GameObject)w;
                        //check for wanderer
                        if (temp.GetComponent<Wanderer>() != null)
                        {
                            if (!temp.GetComponent<Tower>().getActive())
                            {
                                if (((Vector2)(this.transform.position - temp.transform.position)).magnitude < distanceToActivate)
                                {
                                    //pick up wanderer
                                    holding = levelManager.wandererPrefab;//hold wanderer
                                    Destroy(temp);
                                    break;
                                }
                            }
                        }
                        else if (temp.GetComponent<Adderall>() != null)
                        {
                            if (((Vector2)(this.transform.position - temp.transform.position)).magnitude < distanceToActivate)
                            {
                                holding = adderall;
                                Destroy(temp);
                                break;
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
                    else if (holding.GetComponent<WakeUpKit>() != null)
                    {//if holding wakeupKit
                        Object[] roaming = Object.FindObjectsOfType(typeof(GameObject));
                        foreach (Object w in roaming)
                        {
                            GameObject temp = (GameObject)w;
                            if (temp.GetComponent<Sleeper>() != null)//check for sleeper
                            {
                                if (!temp.GetComponent<Tower>().getActive())//check if sleeping
                                {
                                    if (((Vector2)(this.transform.position - temp.transform.position)).magnitude < distanceToActivate)//check if in range
                                    {
                                        //wake up tower
                                        holding = null;//delete holding
                                        animationIndex = -1;
                                        temp.GetComponent<Sleeper>().wake();
                                        playWakeAnimation();
                                        break;
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
                            if (temp.GetComponent<FlowerWatch>() != null)//check for distracted
                            {
                                if (!temp.GetComponent<Tower>().getActive())//check if sleeping
                                {
                                    if (((Vector2)(this.transform.position - temp.transform.position)).magnitude < distanceToActivate)//check if in range
                                    {
                                        //wake up tower
                                        holding = null;//delete holding
                                        temp.GetComponent<FlowerWatch>().wake();
                                        animationIndex = -1;
                                        playDistractAnimation();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (holding.GetComponent<Adderall>() != null)
                    {
                        Object[] roaming = Object.FindObjectsOfType(typeof(GameObject));
                        foreach (Object w in roaming)
                        {
                            GameObject temp = (GameObject)w;
                            if (temp.GetComponent<Tower>() != null)
                            {//detected tower
                                //check range
                                if (((Vector2)(this.transform.position - temp.transform.position)).magnitude < distanceToActivate && temp.GetComponent<Tower>().getActive())//check if in range and active
                                {
                                    //give adderolll
                                    holding = null;
                                    temp.GetComponent<Tower>().focus();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
       

        //check for animations
        if (playingWakeAnimation)
        {
            if (getTime() - lastTime > animationTime)
            {
                lastTime = getTime();
                animationIndex += 1;
                if (animationIndex >= wakeAnimation[smallDirection].Length)
                {
                    mobile = true;
                    playingWakeAnimation = false;
                }
                else
                {
                    Debug.Log(smallDirection.ToString()+" "+animationIndex);
                    spr.sprite = wakeAnimation[smallDirection][animationIndex];
                }
            }
        }
        else if (playingDistractAnimation)
        {
            if (getTime() - lastTime > animationTime)
            {
                lastTime = getTime();
                animationIndex += 1;
                if (animationIndex >= distractAnimation[direction].Length)
                {
                    mobile = true;
                    playingDistractAnimation = false;
                }
                else
                {
                    spr.sprite = distractAnimation[direction][animationIndex];
                }
            }
        }
        else if (moving)
        {
            if (getTime() - lastTime > animationTime)
            {
                lastTime = getTime();
                animationIndex += 1;
                //branch
                if (holding == null)//holding nothing
                {
                    animationIndex %= walkingNone[direction].Length;
                    spr.sprite = walkingNone[direction][animationIndex];
                }
                else//if holding tower air horn or torch
                {
                    animationIndex %= walkingCase[direction].Length;
                    spr.sprite = walkingCase[direction][animationIndex];
                }
                
            }
        }
        else//still
        {
            if (holding == null)//holding nothing
            {
                spr.sprite = stillNone[direction];
            }
            else//if holding tower air horn or torch
            {
                spr.sprite = stillCase[direction];
            }
        }
    }
    private void FixedUpdate()
    {
        
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
            direction = 4;
        }
        else if (angle < -112.5)
        {
            direction = 5;
        }
        else if (angle < -67.5)
        {
            direction = 6;
        }
        else if (angle < -22.5)
        {
            direction = 7;
        }
        else if (angle < 22.5)
        {
            direction = 0;
        }
        else if (angle < 67.5)
        {
            direction = 1;
        }
        else if (angle < 112.5)
        {
            direction = 2;
        }
        else if (angle < 157.5)
        {
            direction = 3;
        }
        else
        {
            direction = 4;
        }
        //find small direction
        if (angle < -135)
        {
            smallDirection = 4;
        }
        else if (angle < -75)
        {
            smallDirection = 6;
        }
        else if (angle < 45)
        {
            smallDirection = 0;
        }
        else if (angle < 135)
        {
            smallDirection = 2;
        }
        else
        {
            smallDirection = 4;
        }
    }

    private void playWakeAnimation() {
        //TODO finish this
        mobile = false;
        playingWakeAnimation = true;
        lastTime = getTime();
        animationIndex = 0;
        spr.sprite = wakeAnimation[direction][animationIndex];
    }
    private void playDistractAnimation()
    {
        //TODO finish this
        mobile = false;
        playingDistractAnimation = true;
        lastTime = getTime();
        animationIndex = 0;
        spr.sprite = distractAnimation[direction][animationIndex];
    }

    private float getTime()
    {//returns a float of seconds since jan 1 1970

        float time = (float)((System.DateTime.Now - new System.DateTime(1970, 1, 1)).TotalSeconds - 1594000000);
        return time;
    }
}
