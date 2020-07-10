using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public LevelManager levelManager;//level manager handels level wide variables

    public float speed = 0.13f;

    //private variables
    private GameObject holding=null;//what the player is currently holding null for nothing

    private Rigidbody2D rb;



	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        rb.MovePosition((Vector2)transform.position+speed * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
    }
}
