using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class RenderedAudioSource : MonoBehaviour {

	private bool started = false;
	private new AudioSource audio;
	private AudioListener listener;
	private bool destroyOnFinish = false;
	private SoundIcon icon;
	protected Sprite overrideIcon = null;

	//For measuring loudness
	private float timeSinceMeasurement = 0f;
	private float measureUpdateStep = 0.1f;
	private int sampleDataLength = 1024;

    void Awake()
    {
        if (SoundIconSpriteManager.instance == null)
        {
            LevelLoader.next_level = -1;
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
    }

	protected void Start () {
		if (started) return;
		audio = this.GetComponent<AudioSource>();
		listener = FindObjectOfType<AudioListener>();
		started = true;
	}

	void Update () {
		if (audio.isPlaying) {
			if (icon != null) {
				icon.orientation = listener.transform.InverseTransformPoint(audio.transform.position);
				timeSinceMeasurement += Time.deltaTime;
				if (timeSinceMeasurement > measureUpdateStep) {
					icon.loudness = GetLoudness();
				}
			} else {
				CreateIcon();
				timeSinceMeasurement = 0f;
			}
		} else if (icon != null) {
			DestroyIcon();
		}
	}

	void PlayClip(AudioClip clip) {
		Start();
		this.audio.clip = clip;
		CreateIcon();
	}

	void CreateIcon() {
		if (SoundCamera.soundCamera == null) return;
		Sprite iconImage = overrideIcon;
		if (iconImage == null) iconImage = SoundIconSpriteManager.instance.GetIconForClip(audio.clip);
		if (iconImage != null) {
			GameObject o = new GameObject("Icon", new System.Type[] {typeof(SoundIcon)});
			this.icon = o.GetComponent<SoundIcon>();
			this.icon.iconImage = iconImage;
		}
	}

	void DestroyIcon() {
		if (icon != null) icon.Remove();
	}
	
	public static Vector4 ToV4(Vector3 x) {
		return new Vector4(x.x, x.y, x.z, 1);
	}

	private float GetLoudness() {
		float[] clipSampleData = new float[sampleDataLength];
		audio.GetOutputData(clipSampleData, 0);
		float clipLoudness = 0f;
		foreach (var sample in clipSampleData) {
			clipLoudness += Mathf.Abs(sample);
		}
		return clipLoudness / sampleDataLength;
	}
}
