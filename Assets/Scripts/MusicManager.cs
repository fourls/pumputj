using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
	public static MusicManager ins = null;
	[Header("Temp")]
	public GameObject indicator;
	public Music music;
	public float threshold = 0.05f;
	public int BPM { get { return music.bpm; }}
	public float secsPerBeat { get { return 60f / (float)BPM; }}

	private AudioSource source;

	void Awake() {
		if(ins == null)
			ins = this;
		else if (ins != this)
			Destroy(gameObject);
		
		source = GetComponent<AudioSource>();
		source.clip = music.clip;
		source.Play();
	}

	void Update() {
		indicator.SetActive(IsBeat());

		if(source.time + music.restartOffset > music.clip.length) {
			source.Stop();
			source.Play();
		}
		
	}

	public bool IsBeat() {
		float closenessToBeat = (source.time+music.offset) % secsPerBeat;
		return closenessToBeat <= threshold;
	}
}
