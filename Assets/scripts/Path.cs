using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

    private PathNode[] nodes;
	// Use this for initialization
	void Start () {
        nodes = this.gameObject.GetComponentsInChildren<PathNode>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int finalNode() {//returns the index of the final node in the path
        return nodes.Length;
    }
    public PathNode getPathNode(int index) {//throws index out of range
        return nodes[index];
    }
}
