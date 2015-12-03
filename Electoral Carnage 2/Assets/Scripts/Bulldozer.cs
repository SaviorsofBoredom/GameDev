using UnityEngine;
using System.Collections;

public class Bulldozer : MonoBehaviour {

	public float maxHealth;
	public float currentHealth;

	Rigidbody2D rb;
	Animator animator;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth <= 0) {
			Debug.Log("died");
			Destroy(this.gameObject);
		}

		if (rb.velocity.x > 0.05 || rb.velocity.x < -0.05) {
			animator.SetBool ("Moving", true);
		} 
		else {
			animator.SetBool ("Moving", false);
		}
	}

	void OnTriggerStay2D(Collider2D collider){
		if (collider.tag == "Player") {
			Debug.Log("player");
			if(collider.gameObject.transform.position.x-transform.position.x <= 0)
			{
				Debug.Log("Moving left");
				rb.AddForce(new Vector2(-6, 0));
				transform.eulerAngles = new Vector3(0, 0, 0);
			}

			if(collider.gameObject.transform.position.x-transform.position.x > 0)
			{
				rb.AddForce(new Vector2(6, 0));
				transform.eulerAngles = new Vector3(0, 180, 0);
			}
		}
	}

	public void DecreaseHealth(float damage)
	{
		currentHealth = currentHealth-damage;
	}
}
