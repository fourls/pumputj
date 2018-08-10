using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackObject : MonoBehaviour {
	public float duration = 0.3f;
	private float timeCreated;

	void Start() {
		timeCreated = Time.time;
	}

	void Update() {
		if(timeCreated + duration < Time.time) {
			enabled = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.CompareTag("Enemy")) {
			Destroy(other.gameObject);
		}
	}
}
