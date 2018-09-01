using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {
    public int NextLevel = 0;

	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Car c = other.GetComponent<Car>();
        if(c != null)
        {
            // Initiate winning routine
            LevelLoader.levelLoader.LoadLevel(NextLevel);
        }
    }
}
