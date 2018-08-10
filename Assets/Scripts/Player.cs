using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MusicalGameObject {
	[Header("References")]
	// 'bopping' one that gets reset every beat
	public DynamicCircle innerCircle;
	// indicates the outer edge of the attack radius
	public DynamicCircle boundaryCircle;
	public UnityEngine.UI.Slider jamSlider;

	[Header("Values")]
	public float speed = 6f;
	// cooldown before the next key is registered
	public float keyCooldown = 0.1f;
	public int maxJam = 60;

	[Header("Data")]
	public List<AttackCombo> attacks;


	private Rigidbody2D rb2d;
	private int maxRadius = 0;
	private int currentJam = 30;
	private float lastTimeComboPressed;
	private List<ComboKeys> keysPressed = new List<ComboKeys>();

	new protected void Start() {
		base.Start();
		rb2d = GetComponent<Rigidbody2D>();
		currentJam = maxJam/2;
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
					ResetCombo();
				}
				lastTimeComboPressed = Time.time;
				boundaryCircle.DoRenderer();
			}
		}

		jamSlider.value = Mathf.Lerp(jamSlider.value,(float)currentJam/(float)maxJam,5*Time.deltaTime);

	}

	new protected void FixedUpdate() {
		base.FixedUpdate();

		Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")).normalized;

		if(movement.x != 0)
			GetComponent<SpriteRenderer>().flipX = movement.x < 0;
		// GetComponent<Animator>().SetBool("moving",movement.magnitude > 0);
		string currentAnim = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
		if(movement.magnitude > 0 && currentAnim != "Walk")
			animator.Play("Walk",0,MusicManager.ins.GetNormalizedTimeSinceCurrentBeat());
		else if(movement.magnitude == 0 && currentAnim != "Bop")
			animator.Play("Bop",0,MusicManager.ins.GetNormalizedTimeSinceCurrentBeat());

		rb2d.velocity = movement * speed;
	}

	void ResetCombo() {
		maxRadius = 0;
		foreach(AttackCombo ac in attacks) {
			ac.consecutive = 0;
		}
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
			ResetCombo();
		}
		innerCircle.radius = 0;
		keysPressed.Clear();
		boundaryCircle.radius = maxRadius;
		boundaryCircle.DoRenderer();
	}

	void AttackWith(AttackCombo attack) {
		int tempMax = maxRadius;
		ResetCombo();
		GameObject go = Instantiate(attack.prefab,transform.position,Quaternion.identity);
		currentJam -= attack.jamCost;
		if(currentJam <= 0) {
			Die();
		}
		// go.transform.localScale = new Vector3(tempMax*2,tempMax*2,1);
	}

	void Die() {
		transform.position = Vector2.up*10000;
		MusicManager.ins.GetComponent<AudioSource>().pitch = 0.4f;
	}

	public void AddJam(int amount) {
		currentJam = Mathf.Clamp(currentJam + amount,0,maxJam);
	}
}
