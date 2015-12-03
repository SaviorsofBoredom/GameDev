using UnityEngine;
using System.Collections;

public class Medipack : MonoBehaviour {

	Player player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			player = collider.gameObject.GetComponent<Player>();

			player.currentHealth = player.currentHealth + 5f;

			if(player.currentHealth > player.maxHealth)
			{
				player.currentHealth = player.maxHealth;
			}

			Destroy(gameObject);
		}
	}
}
