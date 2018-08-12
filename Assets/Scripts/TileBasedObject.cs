using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D),typeof(Rigidbody2D))]
public class TileBasedObject : MusicalBehaviour {
	public float lerp = 5f;
	public GameObject graphics;
	public Vector2Int tilePosition;
	protected Rigidbody2D rb2d;

	protected override void Start() {
		base.Start();
		rb2d = GetComponent<Rigidbody2D>();
		tilePosition = Vector2Int.RoundToInt(rb2d.position);
	}

	protected virtual void Update() {
		bool moving = Vector2.Distance(graphics.transform.localPosition,Vector2.zero) > 1f/16f;
		if(moving) {
			if(Vector2.Distance(graphics.transform.localPosition,Vector2.zero) < 1f/16f) {
				graphics.transform.localPosition = Vector3.zero;
			} else {
				graphics.transform.localPosition = Vector2.Lerp(graphics.transform.localPosition,Vector2.zero,lerp * Time.deltaTime);
			}
		}

		if(animator != null) {
			bool lastMoving = animator.GetBool("moving");
			if(moving != lastMoving) {
				animator.SetBool("moving",moving);
				animator.SetFloat("offset",MusicManager.ins.GetNormalizedTimeSinceCurrentBeat());
			}
		}
	}

	protected bool IsObjectAtPosition(Vector2Int position) {
		RaycastHit2D hit = Physics2D.Raycast(position,Vector2.zero,0.01f,1 << LayerMask.NameToLayer("TileBasedObjects"));

		if(hit.collider != null && hit.collider.gameObject == gameObject)
			return false;
		return (hit.collider != null);
	}

	protected void TryChangePosition(Vector2Int newPosition) {

		if(!IsObjectAtPosition(newPosition)) {
			ChangePosition(newPosition);
		}
	}

	protected void ForceChangePosition(Vector2Int newPosition) {
		RaycastHit2D hit = Physics2D.Raycast(newPosition,Vector2.zero,0.01f,1 << LayerMask.NameToLayer("TileBasedObjects"));

		if(hit.collider != null) {
			Destroy(hit.collider.gameObject);
		}

		ChangePosition(newPosition);
	}
	
	protected void ChangePosition(Vector2Int newPosition) {
		Vector2 lastTilePosition = tilePosition;
		tilePosition = newPosition;

		transform.position = (Vector2)tilePosition;
		graphics.transform.localPosition = (lastTilePosition - newPosition);
	}
}