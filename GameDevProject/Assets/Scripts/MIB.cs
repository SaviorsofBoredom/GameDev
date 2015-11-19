using UnityEngine;
using System.Collections;

public class MIB : MonoBehaviour {
	public float maxHealth;
	public float currentHealth;
	
	public Animator animator;
	
	
	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (currentHealth <= 0) {
			//Debug.Log("died");
			Destroy(this.gameObject);
		}
		
	}
	
	public void DecreaseHealth(float damage)
	{
		currentHealth = currentHealth-damage;
	}
	

}
