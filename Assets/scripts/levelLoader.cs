using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelLoader : MonoBehaviour {
    public Canvas canv;

    public void loadLevel(string name) {
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
        canv.gameObject.active = false;
    }
}
