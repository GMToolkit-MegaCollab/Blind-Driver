using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {
    public int NextLevel = 0;

    void OnTriggerEnter2D(Collider2D other) {
        Car c = other.GetComponent<Car>();
        if (c != null) {

            if (LevelLoader.levelLoader != null)
                LevelLoader.levelLoader.LoadLevel(NextLevel);

            var yha = GameObject.Find("You Have Arrived");
            if (yha != null) {
                var vl = yha.GetComponent<VoiceLine>();
                if (vl != null)
                    vl.Trigger();
            }
        }
    }
}