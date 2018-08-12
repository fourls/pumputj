using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
	public static MusicManager ins = null;
	public List<MusicalBehaviour> musicalBehaviours;
	public Music music;
	public float threshold = 0.1f;
	public int BPM { get { return music.bpm; }}
	public float SecsPerBeat { get { return 60f / (float)BPM; }}
	public int BeatsSinceStart { get { return Mathf.FloorToInt((source.time+music.offset) / SecsPerBeat); }}

	private AudioSource source;
	private int lastBeat = 0;

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

		int beatsPassed = Mathf.FloorToInt(((source.time+music.offset) / SecsPerBeat) - threshold);
		if(beatsPassed > lastBeat) {
			OnEndBeat();
		}
		lastBeat = beatsPassed;

		if(Input.GetKeyDown(KeyCode.F)) {
			float oldBeat = (BPM*source.time)/60;
			float beatDiff = Mathf.Floor(oldBeat) - oldBeat;

			float recOffset = (60*beatDiff)/BPM;
			Debug.Log("recommended offset is " + recOffset.ToString());
		}
	}

	void OnEndBeat() {
		foreach(MusicalBehaviour mb in musicalBehaviours) {
			mb.OnEndBeat();
		}
	}

	public bool IsBeat() {
		// float closenessToBeat = (source.time+music.offset) % secsPerBeat;
		float beatsPassed = (source.time+music.offset) / SecsPerBeat;
		float distanceThroughBeat = beatsPassed - Mathf.Floor(beatsPassed);

		return distanceThroughBeat > 1 - threshold || distanceThroughBeat < threshold;
	}

	public float GetNormalizedTimeSinceCurrentBeat() {
		float beatsPassed = (source.time+music.offset) / SecsPerBeat;
		float distanceThroughBeat = beatsPassed - Mathf.Floor(beatsPassed);

		return distanceThroughBeat;
	}
}
