using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleeper : Tower {
    private bool attacking=false;
    private int attackFrame=0;
    private int attackingFrames = 8;
    private float timePerFame = 0.1f;
 
    private float lastTime;
    public Sprite[] attackSequence;
    //sleeping
    private bool sleeping = false;
    private int sleepFrame = 0;
    private int sleepingFrames = 5;
    public Sprite[] sleepSequence;

    protected override void Start()
    {
        base.Start();
        spr.sprite = stand1;
    }

    protected override void fixedUpdateCall()
    {
        if (!levelManager.getPause())
        {
            targetSelect();

            //attack timer
            checkFire();

            if (attacking) {
                if (getTime() - lastTime > timePerFame) {
                    attackFrame += 1;
                    if (attackFrame >= attackingFrames)
                    {
                        attacking = false;
                        spr.sprite = stand1;
                    }
                    else
                    {
                        spr.sprite = attackSequence[attackFrame];
                        lastTime = getTime();
                    }
                }
            }
        }
    }

    protected override void inActiveUpdate()
    {
        base.inActiveUpdate();
        if (sleeping)
        {
            if (getTime() - lastTime > timePerFame)
            {
                sleepFrame += 1;
                sleepFrame %= sleepingFrames;
                spr.sprite = sleepSequence[attackFrame];
                lastTime = getTime();
            }
        }
    }

    protected override void fire()
    {
        target.damage(damage);
        if (!attacking) {
            attacking = true;
            attackFrame = 0;
            lastTime = getTime();
            spr.sprite = attackSequence[0];
        }
    }

    private float getTime()
    {//returns a float of seconds since jan 1 1970

        float time = (float)((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds - 1594000000);
        return time;
    }

    public void goToSleep() {
        setActive(false);

    }

    public void wake() {//rudly awakens robot from gentle slumber
        sleeping = false;
        spr.sprite = stand1;
    }

}
