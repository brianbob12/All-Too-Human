using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    //public variables
    public int difficulty = 1;
    public int startingLives = 20;
    public int startingMoney = 150;

    //UI objects
    public Text livesText;
    public Text moneyText;
    public Image DeathScreen;
    public Canvas GameOver;

    //private variables
    private int score = 0;
    private int lives;
    private int money = 0;
    private int wave = 0;//wave count
    private bool wifi = true;//wifi satus true is on
    private bool waveOn = false;//whether or not a wave is currently active

    //getters and setters
    public int getScore() { return this.score; }
    public int getMoney() { return this.money; }
    public int getLives() { return this.lives; }
    public int getWave() { return this.wave; }
    public bool getWifi() { return this.wifi; }
    public bool getWaveOn() { return this.waveOn; }

    public void addScore(int tad) { this.score += tad; }
    public void addMoney(int tad) { this.money += tad; }
    public void setWifi(bool status) { this.wifi = status; }

    public void removeLife() {
        lives -= 1;
        Color temp = DeathScreen.color;
        temp.a= (100- (lives / startingLives)*100)/255;
        DeathScreen.color = temp;
        if (lives == 0) {
            //endgame
            GameOver.enabled = true;
        }
    }

    public void takeMoney(int ttk) {//removes given value from money stopping at zero
        this.money -= ttk;
        if (this.money < 0) {
            this.money = 0;
        }
    }

    public bool enoughMoney(int price) {//checks if there is enough money to pay for price
        if (price > this.money) {
            return false;
        }
        return true;
    }

    // Use this for initialization
    void Start () {
        lives = startingLives;
        money = startingMoney;
	}
	
	// Update is called once per frame
	void Update () {
        livesText.text = lives.ToString() + " Lives";

        moneyText.text = "$" + money.ToString();
	}

    // called at equal intervals indipendant of frame rate
    private void FixedUpdate()
    {
        
    }
}
