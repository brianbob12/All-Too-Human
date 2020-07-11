using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class priceIndicator : MonoBehaviour {

    public LevelManager levelManager;
    public int price;

    private Image image;
    private Color yes;
    private Color no;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        yes = new Color(255, 0, 0,255);
        no = new Color(193, 193, 193,255);
	}
	
	// Update is called once per frame
	void Update () {
        if (this.isActiveAndEnabled) {
            if (!levelManager.enoughMoney(price))
            {
                image.color = yes;
            }
            else {
                image.color = no;
            }
        }
	}
}
