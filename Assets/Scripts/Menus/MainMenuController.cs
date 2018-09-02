using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public int SceneToLoad = 1;

	// Use this for initialization
	void Start () {
		
	}
	
    void Update()
    {
        if (Input.anyKeyDown)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneToLoad);
    }

}
