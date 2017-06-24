using UnityEngine;
using System.Collections;

public class ActorAudioManager : MonoBehaviour {

	public static ActorAudioManager I;

	AudioSource[] audioClips;

	void Awake() {
		I = this;

		audioClips = GetComponents<AudioSource>();
	}

	public void Play(int index) {
		if (index < 0 || index >= audioClips.Length) {
			Debug.Log("Play(int index): invalid index");
			return;
		}

		audioClips[index].Play();
	}

	public void Stop(int index) {
		if (index < 0 || index >= audioClips.Length) {
			Debug.Log("Stop(int index): invalid index");
			return;
		}

		audioClips[index].Stop();
	}

	public float GetClipLength(int index) {
		if (index < 0 || index >= audioClips.Length) {
			return 0;
		}

		return audioClips[index].clip.length;
	}
}