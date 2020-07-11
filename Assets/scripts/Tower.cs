using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    //public variables
    public CircleCollider2D rangeCollider;
    public float maxHealth = 100;
    public int fireingRate=10;//number of fixedUpdates between shots
    public int damage = 0;//damage delt per shot

    //private variables
    private bool active=true;//whether or not this Tower is active
    private Rigidbody2D rb;
    private Enemy target;//currently selected target
    private float fireingTimer = 0;//logs fixedUPdates since last fired

    //getters and setters}

    public bool getActive() { return this.active; }
    public void setActive(bool set) { this.active = set; }

    private List<Enemy> enemies = new List<Enemy>();

    //attack setting
    public enum attackSetting{
        CLOSE,
        FAR,
        STRONG,
        WEAK,
        AHEAD,
        BEHIND
    }

    public attackSetting setting = attackSetting.CLOSE;

    // Use this for initialization
    void Start () {
		rb=this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void FixedUpdate()
    {
        //select target
        target = null;

        //this part is for removing dead enemies
        List<Enemy> enemiesCopy = new List<Enemy>();
        foreach (Enemy en in enemies) {
            enemiesCopy.Add(en);
        }

        if (enemies.Count != 0) {
            foreach (Enemy en in enemiesCopy) {
                if (en == null)
                {//en has died
                    //remove en from master list
                    enemies.Remove(en);
                    continue;
                }

                if (target == null) {
                    target = en;
                    continue;
                }
                if (this.setting == attackSetting.AHEAD)
                {
                    if (en.aheadScore() > target.aheadScore()) {
                        target = en;
                    }
                }
                else if (this.setting == attackSetting.BEHIND)
                {
                    if (en.aheadScore() < target.aheadScore())
                    {
                        target = en;
                    }
                }
                else if (this.setting == attackSetting.STRONG)
                {
                    if (en.strengthScore() > target.strengthScore())
                    {
                        target = en;
                    }
                }
                else if (this.setting == attackSetting.WEAK)
                {
                    if (en.strengthScore() < target.strengthScore())
                    {
                        target = en;
                    }
                }
                else if (this.setting == attackSetting.FAR)
                {
                    if (en.closeScore(this.gameObject) > target.closeScore(this.gameObject))
                    {
                        target = en;
                    }
                }
                else if (this.setting == attackSetting.CLOSE)
                {
                    if (en.closeScore(this.gameObject) < target.closeScore(this.gameObject))
                    {
                        target = en;
                    }
                }
            }
        }
        //point towards target

        //attack timer
        if (fireingTimer >= fireingRate) {

            if (target != null) {
                fireingTimer = 0;
                //fire
                target.damage(damage);
                //TODO
                //play fireing animation
                //dispatch lazer
            }
        }
    }

    //manage enemeis list
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Enemy>()!=null) {
            //collision with enemy
            //add object to enemies
            enemies.Add(collider.gameObject.GetComponent<Enemy>());
            
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Enemy>() != null)
        {
            //collision with enemy
            //add object to enemies
            enemies.Remove(collider.gameObject.GetComponent<Enemy>());
        }
    }

}
