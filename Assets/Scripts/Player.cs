using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MusicalGameObject {
	[Header("References")]
	public GameObject playerAttackPrefab;
	// 'bopping' one that gets reset every beat
	public DynamicCircle innerCircle;
	// indicates the outer edge of the attack radius
	public DynamicCircle boundaryCircle;
	[Header("Values")]
	public float speed = 6f;
	// cooldown before the next key is registered
	public float keyCooldown = 0.1f;
	[Header("Data")]
	public List<AttackCombo> attacks;
	private Rigidbody2D rb2d;

	private int maxRadius = 0;
	private float lastTimeComboPressed;
	private List<ComboKeys> keysPressed = new List<ComboKeys>();

	new protected void Start() {
		base.Start();
		rb2d = GetComponent<Rigidbody2D>();
	}

	void Update() {
		innerCircle.radius += (1 / MusicManager.ins.secsPerBeat) * (maxRadius+1) * Time.deltaTime;
		innerCircle.DoRenderer();

		foreach(ComboKeys key in (ComboKeys[])System.Enum.GetValues(typeof(ComboKeys))) {
			KeyCode keyCode = ComboKeysUtil.ToKeyCode(key);
			if(Input.GetKeyDown(keyCode) && lastTimeComboPressed + keyCooldown < Time.time) {
				if(MusicManager.ins.IsBeat()) {
					keysPressed.Add(key);
				} else {
					maxRadius = 0;
				}
				lastTimeComboPressed = Time.time;
				boundaryCircle.DoRenderer();
			}
		}

	}

	void FixedUpdate() {
		Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")).normalized;

		if(movement.x != 0)
			GetComponent<SpriteRenderer>().flipX = movement.x < 0;
		GetComponent<Animator>().SetBool("moving",movement.magnitude > 0);

		rb2d.velocity = movement * speed;
	}

	public override void OnEndBeat() {
		if(keysPressed.Count == 1) {
			maxRadius ++;
			foreach(AttackCombo ac in attacks) {
				if(ac.HandleKey(keysPressed[0])) {
					AttackWith(ac);
					continue;
				}
			}
		} else {
			maxRadius = 0;
		}
		innerCircle.radius = 0;
		keysPressed.Clear();
		boundaryCircle.radius = maxRadius;
		boundaryCircle.DoRenderer();
	}

	void AttackWith(AttackCombo attack) {
		keysPressed.Clear();
		foreach(AttackCombo ac in attacks) {
			ac.consecutive = 0;
		}
		Debug.Log("attacking with " + attack.comboKeys.ToString());
		GameObject go = Instantiate(playerAttackPrefab,transform.position,Quaternion.identity);
		go.transform.localScale = new Vector3(maxRadius,maxRadius,1);
	}
}
