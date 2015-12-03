using UnityEngine;
using System.Collections;

public class MIB : MonoBehaviour {
	public float maxHealth;
	public float currentHealth;
	
	public Animator animator;

	public AudioSource gruntSound;
	public AudioSource deathSound;
	AudioSource[] sounds;
	
	// Use this for initialization
	void Start () {
		sounds = GetComponents<AudioSource> ();
		gruntSound = sounds [0];
		deathSound = sounds [2];
		currentHealth = maxHealth;

		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (currentHealth <= 0) {
			//Debug.Log("died");
			deathSound.Play ();
			Destroy(this.gameObject);
		}
		
	}
	
	public void DecreaseHealth(float damage)
	{
		gruntSound.Play ();
		currentHealth = currentHealth-damage;
		if (currentHealth <= 0)
		{
			deathSound.Play ();
		}
	}

}
