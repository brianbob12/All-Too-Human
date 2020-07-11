using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a line that kills itself after some time and moves in a specified direction
public class Bullet : MonoBehaviour {

    public int life = 6;
    public Vector2 motionVector;
    public float speed = 1;
    public float length=0.5f;
    public float dieDistance = 0.9f;
    public GameObject target;
    private int timer;
    private LineRenderer line;
    

	// Use this for initialization
	void Start () {
        timer = 0;
        //setup line
        line = GetComponent<LineRenderer>();
    }
	
	void FixedUpdate () {
        
        timer += 1;
        if (target != null) {
            if (((Vector2)(transform.position - target.transform.position)).magnitude <= dieDistance){
                Destroy(this.gameObject);
            }
        }
        if (timer >= life)
        {
            Destroy(this.gameObject);
        }
        else {
            //move
            transform.position += (Vector3) motionVector * speed;
        }
        line.SetPosition(0, (Vector2)transform.position + (motionVector) * length);
        line.SetPosition(1, transform.position);
    }
}
