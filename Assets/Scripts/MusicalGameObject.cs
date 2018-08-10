using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalGameObject : MusicalBehaviour {

	private void FixedUpdate() {
		transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.y);
	}
}
