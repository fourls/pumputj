using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalBehaviour : MonoBehaviour {
	[Header("Synchronising With Music")]
	public Animator animator;

	protected void Start() {
		MusicManager.ins.musicalBehaviours.Add(this);
		OnSongChange();
	}

	public virtual void OnSongChange() {
		if(animator != null) {
			animator.speed = MusicManager.ins.BPM/60;
			animator.enabled = false;
			Invoke("StartAnimator",MusicManager.ins.music.offset);
		}
	}

	public virtual void OnEndBeat() {

	}

	void OnDestroy() {
		MusicManager.ins.musicalBehaviours.Remove(this);
	}

	void StartAnimator() {
		animator.enabled = true;
	}
}
