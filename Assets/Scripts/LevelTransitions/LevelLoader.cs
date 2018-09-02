using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public static LevelLoader levelLoader;
    public SpriteRenderer mask;

    public static float transition = 0f;
    public static int next_level = 2;
    public float transition_time = 0.5f;

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(this.gameObject);
        levelLoader = this;
	}
	
	// Update is called once per frame
	void Update () {
        // Transition
        transition += Time.deltaTime / (next_level >= 0 ? transition_time : -transition_time);

        if(next_level >= 0 && transition >= 1)
        {
            // Go to next level
            SceneManager.LoadScene(next_level);
            next_level = -1;
        }

        // Clamp the transition
        transition = Mathf.Clamp01(transition);

        // Color the mask
        mask.color = new Color(0, 0, 0, transition);
	}

    public void LoadLevel(int next_level)
    {
        LevelLoader.next_level = next_level;
    }
}
