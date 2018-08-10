using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalBehaviour : MonoBehaviour {
	[Header("Synchronising With Music")]
	public Animator animator;
	public string startingAnim;

	protected void Start() {
		MusicManager.ins.musicalBehaviours.Add(this);
		OnSongChange();
	}

	public virtual void OnSongChange() {
		if(animator != null) {
			animator.speed = MusicManager.ins.BPM/60;
			animator.Play(startingAnim,0,MusicManager.ins.GetNormalizedTimeSinceCurrentBeat());
		}
	}

	public virtual void OnEndBeat() {

	}

	void OnDestroy() {
		MusicManager.ins.musicalBehaviours.Remove(this);
	}

}
