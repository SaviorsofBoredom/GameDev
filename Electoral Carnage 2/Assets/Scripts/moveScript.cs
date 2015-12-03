using UnityEngine;

/// <summary>
/// Simply moves the current game object
/// </summary>
public class moveScript : MonoBehaviour
{
	// 1 - Designer variables
	
	/// <summary>
	/// Object speed
	/// </summary>
	public Vector2 speed = new Vector2(10, 10);

	Player player;
	GameObject playerGameObj;
	public float Damage = 1;

	Soldier soldier;
	MIB mib;
	Bulldozer dozer;
	GameObject enemyGameObj;

	/// <summary>
	/// Moving direction
	/// </summary>
	/// 

	public Vector2 direction = new Vector2(-1, 0);
	
	private Vector2 movement;

	void Start () {

		//var playerDir = GameObject.Find("Player").transform.localScale.x;
		//var gunRot = GameObject.Find("firePoint").transform.localRotation.y;

		//direction = new Vector2(playerDir,gunRot);
	}

	void Update()
	{

		// 2 - Movement
		transform.Translate(Vector3.right * Time.deltaTime * 35);
		Destroy(gameObject, 1);

	}
	
	void FixedUpdate()
	{

		// Apply movement to the rigidbody
		GetComponent<Rigidbody2D>().velocity = movement;

	}

	void OnCollisionEnter2D(Collision2D hit)
	{
		if (hit.gameObject.tag != "Player") 
		{
			if (hit.collider.isTrigger == false) 
			{
				//Debug.Log ("We hit " + other.name + " and did Test damage.");
				if(transform.gameObject.tag != "Player"){
					if (hit.collider.tag == "Enemy")
					{
						soldier = hit.collider.gameObject.GetComponent<Soldier>();
						mib = hit.collider.gameObject.GetComponent<MIB>();
						dozer = hit.collider.gameObject.GetComponent<Bulldozer>();

						if(soldier != null)
						{
							Debug.Log("hit");
							soldier.DecreaseHealth(Damage);
						}
							
						if(mib != null)
						{
							Debug.Log("hit");
							mib.DecreaseHealth(Damage);
						}
							
						if(dozer != null)
						{
							Debug.Log("hit");
							dozer.DecreaseHealth(Damage);
						}

					}
				}


			}
		}


		/*
		else if (other.tag != "Player") 
		{
			Destroy (gameObject);
			Debug.Log ("You were hit");
			playerGameObj = GameObject.Find("Player");
			player = playerGameObj.GetComponent<Player>();
			player.DecreaseHealth(Damage);
		}
		*/


	}

}