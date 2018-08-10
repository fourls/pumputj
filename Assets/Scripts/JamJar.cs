using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JamJar : MusicalGameObject {
	public int jamAmount = 30;
	public GameObject onDestroy;

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.CompareTag("Player")) {
			other.gameObject.GetComponent<Player>().AddJam(jamAmount);
			Destroy(gameObject);
			if(onDestroy != null) {
				Instantiate(onDestroy,transform.position,Quaternion.identity);
			}
		}
	}
}
