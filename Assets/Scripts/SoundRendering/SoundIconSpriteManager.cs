using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundIconSpriteManager : MonoBehaviour {

	public static SoundIconSpriteManager instance;
	public SoundIconSpriteBook data;
	private Dictionary<int, Sprite> iconDictionary = new Dictionary<int, Sprite>();

	// Use this for initialization
	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(this);
		}
		for (int i = 0; i < data.iconCatalog.Length; i++) {
			iconDictionary.Add(data.iconCatalog[i].clip.samples, data.iconCatalog[i].icon);
			Debug.Log(data.iconCatalog[i].clip.samples);
		}
	}

	public Sprite GetIconForClip(AudioClip clip) {
		Sprite icon = null;
		iconDictionary.TryGetValue(clip.samples, out icon);
		
		Debug.Log("aaa"+clip.samples);
		return icon; 
	}
}

[System.Serializable]
 public struct IconClip {
     public AudioClip clip;
     public Sprite icon;
 }
