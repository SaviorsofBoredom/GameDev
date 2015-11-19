using UnityEngine;
using System.Collections;

public class Soldier : MonoBehaviour {

	public float maxHealth;
	public float currentHealth;

	Animator animator;


	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;

		animator = GetComponent<Animator> ();
		animator.SetBool ("Idle", true);
		animator.SetBool ("Sighted", false);
	}
	
	// Update is called once per frame
	void Update () {
	
		if (currentHealth <= 0) {
			Debug.Log("died");
			Destroy(this.gameObject);
		}

	}

	void OnTriggerEnter2D(Collider2D collider)
	{

		if (collider.tag == "Player") {
			animator.SetBool ("Idle", false);
			animator.SetBool ("Sighted", true);
		}
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			animator.SetBool ("Sighted", false);
		}
	}

	public void DecreaseHealth(float damage)
	{
		currentHealth = currentHealth-damage;
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			animator.SetBool ("Idle", true);
		}
	}
}
