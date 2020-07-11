using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class LevelManager : MonoBehaviour {

    //public variables
    public int difficulty = 1;
    public int startingLives = 20;
    public int startingMoney = 150;
    public int priceOfWanderer = 100;
    public int priceOfSniper = 100;
    public int priceOfWhacker = 100;
    public int priceOfTeen = 100;
    public float waveCooldown=30;
    public int waveLimit = 3;//number of waves in the system

    //public gameObjects
    public Player player;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public Path path;

    //robot prefabs
    public GameObject wandererPrefab;
    public GameObject sniperPrefab;
    public GameObject whackerPrefab;
    public GameObject teenPrefab;

    //UI objects
    public Text livesText;
    public Text moneyText;
    public Image DeathScreen;
    public GameObject GameOver;
    public GameObject storeMenu;

    //private variables
    private int score = 0;
    private int lives;
    private int money = 0;
    private bool wifi = true;//wifi satus true is on
    private bool storeOpen = false;//stores if the store UI is open
    private bool pause = false;//stores whether the level is progressing

    //wave stuff
    private int wave = 0;//wave count
    private int intraWaveIndex = 0;//position in wave
    private bool waveOn = false;//whether or not a wave is currently active
    private string wavePath = "assets/waves";//path to wave files
    private List<int> types;//list of aliens
    private List<float> delays;//delays in seconds between each enemy
    private float lastTime;//last time an enemy was relased
    private int enemiesInWave;

    //getters and setters
    public int getScore() { return this.score; }
    public int getMoney() { return this.money; }
    public int getLives() { return this.lives; }
    public int getWave() { return this.wave; }
    public bool getWifi() { return this.wifi; }
    public bool getWaveOn() { return this.waveOn; }
    public bool getStoreOpen() { return this.storeOpen; }
    public bool getPause() { return this.pause; }

    public void addScore(int tad) { this.score += tad; }
    public void addMoney(int tad) { this.money += tad; }
    public void setWifi(bool status) { this.wifi = status; }
    //public void setStoreOpen(bool status) { this.storeOpen = status; }REDUNDANT
    //public void setPause(bool status) { this.pause = status; }REDUNDANT

    public void removeLife() {
        lives -= 1;
        Color temp = DeathScreen.color;
        temp.a= (100- ((float)lives / (float)startingLives)*100)/255;
        DeathScreen.color = temp;
        if (lives == 0) {
            //endgame
            GameOver.SetActive(true);
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
        Cursor.lockState = CursorLockMode.Locked;
        lastTime = getTime();
    }
	
	// Update is called once per frame
	void Update () {
        livesText.text = lives.ToString() + " Lives";

        moneyText.text = "$" + money.ToString();
	}

    // called at equal intervals indipendant of frame rate
    private void FixedUpdate()
    {
        //check for menu presses
        if (storeOpen&&Input.GetKeyDown(KeyCode.Escape)) {
            closeStore();
        }

        //waves
        if (waveOn) {
            if (getTime() - lastTime > delays[intraWaveIndex]) {
                //new enemy
                int tempType = types[intraWaveIndex];
                switch (tempType) {
                    case 1:
                        spawn(enemy1);
                        break;
                    case 2:
                        spawn(enemy2);
                        break;
                    case 3:
                        spawn(enemy3);
                        break;

                }
                lastTime = getTime();//very important
                intraWaveIndex += 1;
                if (intraWaveIndex >= enemiesInWave) {
                    waveOn = false;
                    //end of wave
                }
            }
        }
        else if(getTime()-lastTime>waveCooldown){//time for new wave
            wave += 1;
            if (wave > waveLimit)
            {
                //TODO do something at wave limit
                wave -= 1;
                lastTime = getTime();
            }
            else {//start new wave
                //clear viarables
                Debug.Log("starting new wave");
                intraWaveIndex = 0;
                types = new List<int>();
                delays = new List<float>();
                enemiesInWave = 0;
                //read from file
                StreamReader inputStream = new StreamReader(wavePath+"/WAVE"+wave.ToString()+".txt");

                while (!inputStream.EndOfStream)
                {
                    string[] temp = inputStream.ReadLine().Split(',');
                    enemiesInWave += 1;
                    types.Add(int.Parse(temp[0]));
                    delays.Add(float.Parse(temp[1]));
                }
                inputStream.Close();
                //let's go!
                waveOn = true;
                lastTime = getTime();
            }
        }
    }

    public void openStore() {//opens store
        storeOpen = true;
        pause = true;
        storeMenu.SetActive(true);
        //unclock cursor
        Cursor.lockState = CursorLockMode.None;
    }

    public void closeStore() {

        storeOpen = false;
        pause = false;
        storeMenu.SetActive(false);
        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    //buying functions
    public void buyWanderer() {
        if (enoughMoney(priceOfWanderer))
        {
            money -= priceOfWanderer;
            player.setHolding(wandererPrefab);
            closeStore();
        }
        else {
            //indicate to the player not enough money
        }
    }
    public void buyWhacekr()
    {
        if (enoughMoney(priceOfWhacker))
        {
            money -= priceOfWhacker;
            player.setHolding(whackerPrefab);
            closeStore();
        }
        else
        {
            //indicate to the player not enough money
        }
    }
    public void buySniper()
    {
        if (enoughMoney(priceOfSniper))
        {
            money -= priceOfSniper;
            player.setHolding(sniperPrefab);
            closeStore();
        }
        else
        {
            //indicate to the player not enough money
        }
    }

    private float getTime(){//returns a float of seconds since jan 1 1970

        float time = (float)((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds-1594000000);
        return time;
    }

    private void spawn(GameObject enemy) {//spawns at first node in path
        Enemy placed = Instantiate(enemy, path.getPathNode(0).transform.position, Quaternion.identity).GetComponent<Enemy>();
        //setup
        placed.levelManager = this;
        placed.path = path;
    }
}
