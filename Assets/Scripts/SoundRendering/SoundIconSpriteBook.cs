using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundIconSpriteBook", menuName = "SoundIconSpriteBook", order = 1)]
public class SoundIconSpriteBook : ScriptableObject {
	public Sprite Arrow;
	public Sprite Brackets;
	
	public Sprite GPS;
	public Sprite Passenger;
	public Sprite Crash;

    public IconClip[] iconCatalog;
}