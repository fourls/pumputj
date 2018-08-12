using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackObject : MonoBehaviour {
	public float duration = 0.3f;
	public int damage = 10;

	private float timeCreated;
	private bool alreadyAttacked = false;

	void Start() {
		timeCreated = Time.time;
	}

	void Update() {
		if(timeCreated + duration < Time.time) {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.CompareTag("Player")) {
			Player player = other.gameObject.GetComponent<Player>();
			if(!alreadyAttacked) {
				player.TakeDamage(damage);
				alreadyAttacked = true;
			}
		}
	}
}
