using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float moveSpeed = 6;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	int direction;
	float _posX;
	float someScale;

	public float maxHealth;
	public float currentHealth;

	float timeHit;
	float currentTime;
	float invincibilityTime;


	Controller2D controller;
	Animator animator;

	public GameObject healthBar;

	public Text currentHealthText;
	public Text maxHealthText;
	public Vector3 spawnPoint;

	public Texture2D cursorTexture;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = new Vector2(24, -24);

	public AudioSource gruntSound;

	AudioSource[] sounds;

	GameObject pivot;
	//GameObject arm;

	void Start() {
		sounds = GetComponents<AudioSource> ();
		gruntSound = sounds [0];

		controller = GetComponent<Controller2D> ();
		animator = GetComponent<Animator> ();

		Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

		currentHealth = maxHealth;
		Debug.Log("Current Health: " + currentHealth);

		timeHit = Time.time;
		currentTime = Time.time;
		invincibilityTime = 1;

		direction = 1;
		_posX = transform.position.x;
		someScale = transform.localScale.x;

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
		//print ("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity);

		pivot = transform.FindChild("Pivot").gameObject;
	}

	void Update() {
		currentHealthText.text = "Current Health: " + currentHealth.ToString();
		maxHealthText.text = "Max Health: " + maxHealth.ToString();

		currentTime = Time.time;

		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		int wallDirX = (controller.collisions.left) ? -1 : 1;

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);

		Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

		if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x >= transform.position.x)
		{
			//Debug.Log("Moving right " + velocity.x);
			transform.localScale = new Vector2(someScale, transform.localScale.y);
			pivot.transform.localScale = new Vector2(someScale, transform.localScale.y);
		}

		else
		{
			//Debug.Log("Moving left " + velocity.x);
			transform.localScale = new Vector2(-someScale, transform.localScale.y);
			pivot.transform.localScale = new Vector2(-someScale, -transform.localScale.y);

		}		


		if (currentHealth <= 0) {
			transform.position = spawnPoint;
			currentHealth = maxHealth;
			SetHealthBar(currentHealth/maxHealth);
		}

		/*
		if (controller.damaged == true && currentTime >= timeHit + invincibilityTime) {

			timeHit = Time.time;

			currentHealth = currentHealth-1;

			controller.damaged = false;

			Debug.Log("Current Health: " + currentHealth);
		}
		controller.damaged = false;
		*/


		if (velocity.x > 0.05 || velocity.x < -0.05) {
			animator.SetBool ("Moving", true);
		} 
		else {
			animator.SetBool ("Moving", false);
		}

		bool wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (input.x != wallDirX && input.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				}
				else {
					timeToWallUnstick = wallStickTime;
				}
			}
			else {
				timeToWallUnstick = wallStickTime;
			}

		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			if (wallSliding) {
				if (wallDirX == input.x) {
					velocity.x = -wallDirX * wallJumpClimb.x;
					velocity.y = wallJumpClimb.y;
				}
				else if (input.x == 0) {
					velocity.x = -wallDirX * wallJumpOff.x;
					velocity.y = wallJumpOff.y;
				}
				else {
					velocity.x = -wallDirX * wallLeap.x;
					velocity.y = wallLeap.y;
				}
			}
			if (controller.collisions.below) {
				velocity.y = maxJumpVelocity;
				animator.SetBool ("Jumping", true);
			}
		}
		else {
			animator.SetBool ("Jumping", false);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			if (velocity.y > minJumpVelocity) {
				velocity.y = minJumpVelocity;
				//animator.SetBool ("Jumping", true);
			}
		}
		else {
			//animator.SetBool ("Jumping", false);
		}
	
		velocity.y += gravity * Time.deltaTime;

		controller.Move (velocity * Time.deltaTime, input);

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

	}

	public void DecreaseHealth(float damage)
	{
		gruntSound.Play ();
		if (currentTime >= timeHit + invincibilityTime) {
			
			animator.SetBool ("Hit", true);

			timeHit = Time.time;
			
			currentHealth = currentHealth-damage;

			float calcHealth = currentHealth/maxHealth;
			
			//Debug.Log("Current Health: " + currentHealth);

			SetHealthBar(calcHealth);

			animator.SetBool ("Hit", false);
		}

	}

	public void SetHealthBar(float healthPercent)
	{
		healthBar.transform.localScale = new Vector3 (healthPercent, healthBar.transform.localScale.y, healthBar.transform.localScale.y);
	}
}
