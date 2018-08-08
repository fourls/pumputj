using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalBehaviour : MonoBehaviour {
	[Header("Synchronising With Music")]
	public Animator animator;

	private void Start() {
		animator.speed = MusicManager.ins.BPM/60;
		animator.enabled = false;
		Invoke("StartAnimator",MusicManager.ins.music.offset);
	}

	void StartAnimator() {
		animator.enabled = true;
	}
}
