using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public GameObject popup;
    public Text popupText;
    public GameObject add;

    private LevelManager levelManager;
    private int mesageIndex = 0;
    private string[] messages = new string[] {
        "Welcome to  All Too Human! \n use W, A, S, and D to move around.",
        "Next, walk up to the store (pressing ENTER to go in) and purchase a Whacker Robot (the one in the middle). Then press next.",
        "Great, now that you are holding a Wacker go and place it down (ENTER) anywhere next to the road.",
        "Now you should see that when an alien comes our Wacker will make short work of it",
        "Uh Oh, it looks like your Wacker has fallen asleep, better go grab a flashlight to wake it up....",
        "Go get this from the flashlight vending machine on the right (ENTER).",
        "To use the flashlight on the Wacker just walk up to him and press ENTER",
        "Cool, now you should be able to afford a Wanderer. Why don’t you go pick one up from the store.",
        "Now place it down anywhere along the road.",
        "Uh Oh, it looks like he decided to go for a bit of a wander, better grab him and put him back in his place",
        "Finally, you should be able to complete your arsenal with a sniper...",
        "why don’t you go pick one up from the shop and place it down anywhere in the map",
        "Aww, look he’s enjoying nature. Better get him focused again by grabbing a...",
        "Holding Tactical Portable Focus Procurement Mechanism(TPFPM) from the vending machine on the left",
        "Then just use the TPFPM on the Sniper the same way you woke up the Wacker",
        "What is that that just fell from the heavens with no explanation? Is that some adderall?",
        "Why don’t you go use that on one of the robots to keep them focused for 30 seconds.",
        " Final Tip: if you pick up an item you don’t want to be holding, you can always throw it away...",
        "in the recycling bin to the right of the vending machines.",

        "Great, now you have all the tools to start defending yourself from the alien invasion.",        "Enjoy!"

    };


	// Use this for initialization
	void Start () {
        levelManager = GetComponent<LevelManager>();
        popupText.text = messages[mesageIndex];
        
    }
	
	// Update is called once per frame
	void Update () {
        Cursor.lockState = CursorLockMode.None;
    }

    public void nextStep()
    {
        mesageIndex += 1;
        if (mesageIndex >= messages.Length)
        {
            popup.active = false;
        }
        else
        {
            popupText.text = messages[mesageIndex];
        }
        if (mesageIndex == 3)
        {
            levelManager.spawn(levelManager.enemy1);
        }
        if (mesageIndex == 4)
        {
            Object[] roaming = Object.FindObjectsOfType(typeof(GameObject));
            foreach (Object w in roaming)
            {
                GameObject temp = (GameObject)w;
                if (temp.GetComponent<Sleeper>() != null)//check for distracted
                {
                    temp.GetComponent<Sleeper>().goToSleep();
                }
            }
        }
        if (mesageIndex == 5)
        {
            Object[] roaming = Object.FindObjectsOfType(typeof(GameObject));
            foreach (Object w in roaming)
            {
                GameObject temp = (GameObject)w;
                Debug.Log("1");
                if (temp.GetComponent<Wanderer>() != null)//check for distracted
                {
                    Debug.Log("2");
                    temp.GetComponent<Wanderer>().startDistraction();
                }
            }
        }
        if (mesageIndex == 7)
        {
            levelManager.addMoney(100);
        }
        if (mesageIndex == 10)
        {
            levelManager.addMoney(200);
        }
        if (mesageIndex == 12)
        {
            Object[] roaming = Object.FindObjectsOfType(typeof(GameObject));
            foreach (Object w in roaming)
            {
                GameObject temp = (GameObject)w;
                if (temp.GetComponent<FlowerWatch>() != null)//check for distracted
                {
                    temp.GetComponent<FlowerWatch>().distract();
                }
            }
        }
        if (mesageIndex == 15)
        {
            add.active = true;
        }
    }
}
