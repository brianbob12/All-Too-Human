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
    private bool intoSleeping = false;
    private int intoSleepFrame = 0;
    public float intoSleepTime = 1;
    public Sprite[] intoSleepSequence;

    private bool sleeping = false;
    private int sleepFrame = 0;
    public float sleepTime=1;
    public Sprite[] sleepSequence;

    private bool wakeing;

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
            if (intoSleeping)
            {
                if (getTime() - lastTime > intoSleepTime)
                {
                    intoSleepFrame += 1;
                    if (intoSleepFrame >= intoSleepSequence.Length)
                    {
                        intoSleeping = false;
                    }
                    else
                    {
                        spr.sprite = intoSleepSequence[intoSleepFrame];
                    }
                    lastTime = getTime();
                }
            }
            else if (wakeing)
            {
                intoSleepFrame -= 1;
                if (intoSleepFrame <= 0)
                {
                    wakeing = false;
                    sleeping = false;
                    setActive(true);
                }
                else
                {
                    spr.sprite = intoSleepSequence[intoSleepFrame];
                }

            }
            else if (getTime() - lastTime > sleepTime)
            {
                sleepFrame += 1;
                sleepFrame %= sleepSequence.Length;
                spr.sprite = sleepSequence[sleepFrame];
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
        sleeping = true;
        intoSleeping = true;
        intoSleepFrame = -1;
        sleepFrame = -1;
    }

    public void wake() {//rudly awakens robot from gentle slumber
        wakeing= true;
        intoSleepFrame = intoSleepSequence.Length;
    }

}
