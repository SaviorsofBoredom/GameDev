using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

	Player player;
	GameObject playerGameObj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Player")
		{
			playerGameObj = GameObject.Find("Player");
			player = playerGameObj.GetComponent<Player>();
			player.spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		}
	}
}
