using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TileBasedObject {
	[Header("References")]
	public UnityEngine.UI.Slider jamSlider;

	[Header("Values")]
	public int maxJam = 60;
	public int jamLossOnBadMove = 10;

	[Header("Data")]
	public Repertoire repertoire;

	private int currentJam = 60;
	private List<ComboKeys> keysPressed = new List<ComboKeys>();
	private bool fuckedItUp = false;
	private bool madeMove = false;

	protected override void Start() {
		base.Start();
		currentJam = maxJam/2;
	}

	protected override void Update() {
		base.Update();

		foreach(ComboKeys key in (ComboKeys[])System.Enum.GetValues(typeof(ComboKeys))) {
			KeyCode keyCode = ComboKeysUtil.ToKeyCode(key);
			if(Input.GetKeyDown(keyCode)) {
				if(MusicManager.ins.IsBeat() && !madeMove && !fuckedItUp) {
					MakeMove(key);
				} else {
					fuckedItUp = true;
				}
			}
		}
		

		jamSlider.value = Mathf.Lerp(jamSlider.value,(float)currentJam/(float)maxJam,5*Time.deltaTime);

	}

	void ResetCombo() {
		foreach(PlayerAction ac in repertoire.actions) {
			ac.consecutive = 0;
		}
	}

	void MakeMove(ComboKeys key) {
		madeMove = true;
		
		foreach(PlayerAction ac in repertoire.actions) {
			if(ac.HandleKey(key)) {
				PerformAction(ac);
				continue;
			}
		}
	}

	public override void OnEndBeat() {
		if(fuckedItUp) {
			ResetCombo();
			TakeDamage(jamLossOnBadMove);
		}

		madeMove = false;
		fuckedItUp = false;
		keysPressed.Clear();
	}

	void PerformAction(PlayerAction action) {
		ResetCombo();
		
		if(!IsObjectAtPosition(tilePosition + action.relocation)) {
			TryChangePosition(tilePosition + action.relocation);
			if(action.prefab != null) {
				foreach(Vector2Int affLoc in action.affectedLocations) {
					Instantiate(action.prefab,(Vector2)(tilePosition + affLoc),Quaternion.identity);
				}
			}
		}

		currentJam -= action.jamCost;
		if(currentJam <= 0) {
			Die();
		}
	}

	void Die() {
		// Destroy(gameObject);
		MusicManager.ins.GetComponent<AudioSource>().pitch = 0.4f;
	}

	public void AddJam(int amount) {
		currentJam = Mathf.Clamp(currentJam + amount,0,maxJam);
	}

	public void TakeDamage(int amount) {
		currentJam -= amount;
		if(currentJam <= 0) {
			Die();
		}
	}
}
