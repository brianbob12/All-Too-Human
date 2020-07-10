using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    //public variables
    public int difficulty = 1;

    //private variables
    private int score = 0;
    private int money = 0;
    private int wave = 0;//wave count
    private bool wifi = true;//wifi satus true is on
    private bool waveOn = false;//whether or not a wave is currently active

    //getters and setters
    int getScore() { return this.score; }
    int getMoney() { return this.money; }
    int getWave() { return this.wave; }
    bool getWifi() { return this.wifi; }
    bool getWaveOn() { return this.waveOn; }

    void addScore(int tad) { this.score += tad; }
    void addMoney(int tad) { this.money += tad; }
    void setWifi(bool status) { this.wifi = status; }
    void takeMoney(int ttk) {//removes given value from money stopping at zero
        this.money -= ttk;
        if (this.money < 0) {
            this.money = 0;
        }
    }

    bool enoughMoney(int price) {//checks if there is enough money to pay for price
        if (price > this.money) {
            return false;
        }
        return true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // called at equal intervals indipendant of frame rate
    private void FixedUpdate()
    {
        
    }
}
