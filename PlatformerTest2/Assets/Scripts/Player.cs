using UnityEngine;
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

	int maxHealth;
	public int currentHealth;

	float timeHit;
	float currentTime;
	float invincibilityTime;


	Controller2D controller;
	Animator animator;

	void Start() {
		controller = GetComponent<Controller2D> ();
		animator = GetComponent<Animator> ();

		maxHealth = 5;
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
		print ("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity);
	}

	void Update() {
		currentTime = Time.time;

		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		int wallDirX = (controller.collisions.left) ? -1 : 1;

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);

		if (velocity.x >= 0)
		{
			//Debug.Log("Moving right " + velocity.x);
			transform.localScale = new Vector2(someScale, transform.localScale.y);		}

		else
		{
			//Debug.Log("Moving left " + velocity.x);
			transform.localScale = new Vector2(-someScale, transform.localScale.y);
		}		


		if (currentHealth == 0) {
			controller.damaged = false;
			transform.position = new Vector3(-18, -11, 1);
			currentHealth = maxHealth;
		}

		if (controller.damaged == true && currentTime >= timeHit + invincibilityTime) {

			timeHit = Time.time;

			currentHealth = currentHealth-1;

			controller.damaged = false;

			Debug.Log("Current Health: " + currentHealth);
		}
		controller.damaged = false;


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
}
