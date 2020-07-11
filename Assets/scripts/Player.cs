using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public LevelManager levelManager;//level manager handels level wide variables
    public GameObject store;//a location from where the player can buy things

    public CircleCollider2D fatCollider;//a collider that is only active when holding something

    public const float speed = 0.13f;

    //private variables
    private GameObject holding=null;//what the player is currently holding(as Prefab) null for nothing
    private const float distanceToActivate = 1.5f;//how far from the store the player needs to be to activate object
    private Rigidbody2D rb;

    public void setHolding(GameObject set) {
        holding = set;
        fatCollider.isTrigger = false;
    }


	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (!levelManager.getPause()) {
            rb.MovePosition((Vector2)transform.position + speed * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

            if (Input.GetButtonDown("Fire1")) {
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
                }
                else if (holding == null) {
                    //iterate over wandereres
                    Object[] roaming= Object.FindObjectsOfType(typeof(GameObject));
                    foreach (Object w in roaming)
                    {
                        GameObject temp = (GameObject)w;
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
                    if (holding.GetComponent<Tower>() != null)
                    {
                        //try to place robot
                        placeTower();
                    }
                }
            }
        }
    }

    private void placeTower() {
        Tower placed=Instantiate(holding, transform.position, Quaternion.identity).GetComponent<Tower>();
        //setup
        placed.levelManager = levelManager;
        noHolding();
    }

    private void noHolding() {//sets the player to not holding anything
        fatCollider.isTrigger = true;
        holding = null;
    }
}
