using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PassengerController2 : MonoBehaviour {
    public new LinkedList<AudioSource> audio = new LinkedList<AudioSource>();
    public VoiceLine lastVoiceLineStarted;

	void Awake() {
		audio.AddLast(this.GetComponent<AudioSource>());
	}

	protected void Update() {
		if (!audio.Last.Value.isPlaying && audio.Count > 1) {
			audio.RemoveLast();
			audio.Last.Value.UnPause();
		}
	}

	public void Play(AudioClip clip) {
		AudioSource source = audio.Last.Value;
		if (source.isPlaying) {
			source.Pause();
			audio.AddLast(this.gameObject.AddComponent<AudioSource>());
			source = audio.Last.Value;
		}
		source.clip = clip;
		source.Play();
	}

	public static AudioClip Combine(params AudioClip[] clips) {
        if (clips == null || clips.Length == 0)
            return null;

        int length = 0;
        for (int i = 0; i < clips.Length; i++) {
            if (clips[i] == null)
                continue;

            length += clips[i].samples * clips[i].channels;
        }

        float[] data = new float[length];
        length = 0;
        for (int i = 0; i < clips.Length; i++) {
            if (clips[i] == null)
                continue;

            float[] buffer = new float[clips[i].samples * clips[i].channels];
            clips[i].GetData(buffer, 0);
            buffer.CopyTo(data, length);
            length += buffer.Length;
        }

        if (length == 0)
            return null;

        //AudioClip result = AudioClip.Create("Combine", length / 2, 2, clips[0].frequency, false); //stereo
        AudioClip result = AudioClip.Create("Combine", length, 1, clips[0].frequency, false); //mono
        result.SetData(data, 0);

        return result;
    }
}
