using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	Player player;
	GameObject playerGameObj;
	public float Damage = 1;
	
	Soldier soldier;
	MIB mib;
	Bulldozer dozer;
	GameObject enemyGameObj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D hit)
	{

		if (hit.gameObject.tag != "Player") {
			if (hit.collider.isTrigger == false && hit.collider.tag != "Bullet") {
				//Debug.Log ("We hit " + other.name + " and did Test damage.");
				if (transform.gameObject.tag != "Player") {
					if (hit.collider.tag == "Enemy") {
						soldier = hit.collider.gameObject.GetComponent<Soldier> ();
						mib = hit.collider.gameObject.GetComponent<MIB> ();
						dozer = hit.collider.gameObject.GetComponent<Bulldozer> ();
						
						if (soldier != null) {
							//Debug.Log ("hit");
							soldier.DecreaseHealth (Damage);
						}
						
						if (mib != null) {
							//Debug.Log ("hit");
							mib.DecreaseHealth (Damage);
						}
						
						if (dozer != null) {
							//Debug.Log ("hit");
							dozer.DecreaseHealth (Damage);
						}
					}
				}
			}
		} 
		else {
			playerGameObj = GameObject.Find("Player");
			player = playerGameObj.GetComponent<Player>();
			player.DecreaseHealth(Damage);
		}

		Destroy(gameObject);
	}
}