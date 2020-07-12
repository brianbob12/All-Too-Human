using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    //public variables
    public LevelManager levelManager;
    public CircleCollider2D rangeCollider;
    
    public float bulletSpeed;
    public int fireingRate=10;//number of fixedUpdates between shots
    public int damage = 0;//damage delt per shot
    public GameObject projectile;//prefab of bullet

    //images
    public Sprite stand1;
    public Sprite stand2;
    public Sprite stand3;
    public Sprite stand4;
    public Sprite stand5;
    public Sprite stand6;
    public Sprite stand7;
    public Sprite stand8;

    //firePoints
    public GameObject firePoint1;
    public GameObject firePoint2;
    public GameObject firePoint3;
    public GameObject firePoint4;
    public GameObject firePoint5;
    public GameObject firePoint6;
    public GameObject firePoint7;
    public GameObject firePoint8;

    //private variables
    private bool active=true;//whether or not this Tower is active
    private Rigidbody2D rb;
    protected Enemy target;//currently selected target
    private float fireingTimer = 0;//logs fixedUPdates since last fired
    protected SpriteRenderer spr;
    private GameObject activeFirePoint;//the source of bullets

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
    protected virtual void Start () {
		rb=this.GetComponent<Rigidbody2D>();
        spr = GetComponentInChildren<SpriteRenderer>();
	}



    private void FixedUpdate()
    {
        if (active)
        {
            fixedUpdateCall();//for overrideing
        }
        else {
            inActiveUpdate();
        }
    }

    protected virtual void fixedUpdateCall() {
        if (!levelManager.getPause())
        {
            targetSelect();
            //point towards target

            //sprites and firepoints
            if (target != null)
            {
                pointToTarget();
            }

            //attack timer
            checkFire();


        }
    }

    protected virtual void inActiveUpdate() {//to be overridden

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

    protected void checkFire()
    {
        if (fireingTimer >= fireingRate)
        {

            if (target != null)
            {
                fireingTimer = 0;
                //fire
                fire();
            }
            else
            {
                fireingTimer += 1;
            }
        }
        else
        {
            fireingTimer += 1;
        }
    }
    protected virtual void fire()
    {
        target.damage(damage);
        //TODO
        //play fireing animation
        //dispatch weapon

        //old system

        Vector2 fireVector = ((Vector2)target.transform.position + target.getGoing() * 35) - (Vector2)activeFirePoint.transform.position;
        fireVector /= fireVector.magnitude;//normalize


        //new system
        /*
        Vector2 alpha = (Vector2)activeFirePoint.transform.position - (Vector2)target.transform.position;
        alpha /= alpha.magnitude;
        alpha *= bulletSpeed;
        alpha *= ((Vector2)target.transform.position).magnitude / target.getGoing().magnitude;
        //convergance
        for (int i = 0; i < 50;i++) {
            Vector2 newAlpha= (Vector2)activeFirePoint.transform.position - alpha;
            newAlpha /= newAlpha.magnitude;
            newAlpha*= bulletSpeed;
            newAlpha *= alpha.magnitude / target.getGoing().magnitude;
            alpha = newAlpha;
        }

        Vector2 fireVector = alpha;
        */

        Bullet placed = Instantiate(projectile, activeFirePoint.transform.position, Quaternion.identity).GetComponent<Bullet>();
        //setup
        placed.target = target.gameObject;
        placed.motionVector = fireVector;
        placed.speed = bulletSpeed;
    }
    protected void targetSelect() {
        //select target
        target = null;

        //this part is for removing dead enemies
        List<Enemy> enemiesCopy = new List<Enemy>();
        foreach (Enemy en in enemies)
        {
            enemiesCopy.Add(en);
        }

        if (enemies.Count != 0)
        {
            foreach (Enemy en in enemiesCopy)
            {
                if (en == null)
                {//en has died
                 //remove en from master list
                    enemies.Remove(en);
                    continue;
                }

                if (target == null)
                {
                    target = en;
                    continue;
                }
                if (this.setting == attackSetting.AHEAD)
                {
                    if (en.aheadScore() > target.aheadScore())
                    {
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
    }

    private void pointToTarget() {
        //targetting
        float angle = Vector2.SignedAngle(transform.up, (Vector2)(target.transform.position - transform.position));
        if (angle < -157.5)
        {
            spr.sprite = stand5;
            activeFirePoint = firePoint5;
        }
        else if (angle < -112.5)
        {
            spr.sprite = stand6;
            activeFirePoint = firePoint6;
        }
        else if (angle < -67.5)
        {
            spr.sprite = stand7;
            activeFirePoint = firePoint7;
        }
        else if (angle < -22.5)
        {
            spr.sprite = stand8;
            activeFirePoint = firePoint8;
        }
        else if (angle < 22.5)
        {
            spr.sprite = stand1;
            activeFirePoint = firePoint1;
        }
        else if (angle < 67.5)
        {
            spr.sprite = stand2;
            activeFirePoint = firePoint2;
        }
        else if (angle < 112.5)
        {
            spr.sprite = stand3;
            activeFirePoint = firePoint3;
        }
        else if (angle < 157.5)
        {
            spr.sprite = stand4;
            activeFirePoint = firePoint4;
        }
        else
        {
            spr.sprite = stand5;
            activeFirePoint = firePoint5;
        }
    }
}
