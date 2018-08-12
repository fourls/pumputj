using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : TileBasedObject {
	[Header("References")]
	public GameObject deathEffectPrefab;
	[Header("Values")]
	public int maxHealth = 10;
	public int beatsPerMove = 1;
	[Header("Attacks")]
	public EnemyRepertoire repertoire;
	protected int health;
	protected Player player;
	protected int numBeats;

	protected override void Start() {
		base.Start();
		health = maxHealth;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	public void GetAttacked(int h) {
		health -= h;

		if(health <= 0) {
			Die();
		}
	}

	protected virtual void Die() {
		if(deathEffectPrefab != null)
			Instantiate(deathEffectPrefab,transform.position,Quaternion.identity);
		Destroy(gameObject);
	}

	public override void OnEndBeat() {
		if(MusicManager.ins.BeatsSinceStart % beatsPerMove != 0)
			return;

		Vector2Int distance = (player.tilePosition - tilePosition);

		if(distance.magnitude == 1) {
			PerformAction(repertoire.attackAction);
		} else {
			bool xFirst = Random.value > 0.5f && distance.x != 0;

			if(xFirst) {
				if(distance.x > 0)
					PerformAction(repertoire.rightMovement);
				else if(distance.x < 0)
					PerformAction(repertoire.leftMovement);
			} else {
				if(distance.y > 0)
					PerformAction(repertoire.upMovement);
				else if(distance.y < 0)
					PerformAction(repertoire.downMovement);
				else if(distance.x > 0)
					PerformAction(repertoire.rightMovement);
				else if(distance.x < 0)
					PerformAction(repertoire.leftMovement);
			}
		}

	}

	void PerformAction(ObjectAction action) {
		if(!IsObjectAtPosition(tilePosition + action.relocation)) {
			TryChangePosition(tilePosition + action.relocation);
			if(action.prefab != null) {
				foreach(Vector2Int affLoc in action.affectedLocations) {
					Instantiate(action.prefab,tilePosition + (Vector2)affLoc,Quaternion.identity);
				}
			}
		}
	}
}
