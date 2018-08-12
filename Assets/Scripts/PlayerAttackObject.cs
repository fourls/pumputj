using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackObject : MonoBehaviour {
	public float duration = 0.3f;
	public int damage = 10;

	private float timeCreated;
	private List<Enemy> alreadyAttacked = new List<Enemy>();

	void Start() {
		timeCreated = Time.time;
	}

	void Update() {
		if(timeCreated + duration < Time.time) {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.CompareTag("Enemy")) {
			Enemy enemy = other.gameObject.GetComponent<Enemy>();
			if(!alreadyAttacked.Contains(enemy)) {
				other.gameObject.GetComponent<Enemy>().GetAttacked(damage);
				alreadyAttacked.Add(enemy);
			}
		}
	}
}
