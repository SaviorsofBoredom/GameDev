using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public float fireRate = 0;
    public float Damage = 1;
    public LayerMask WhatToHit; //Layer mask of what to hit (will exclude player and raymask)

    public Transform BulletTrailPrefab;

    float timeToSpawnEffect = 0;
    public float effectSpawnRate;
    float timeToFire = 0;
    Transform firePoint;
	Vector3 velocity;

	float timeFired;
	float currentTime;

	Soldier soldier;
	MIB mib;
	Bulldozer dozer;
	GameObject enemyGameObj;

	float someScale;

	public GameObject shotPrefab;

	bool checkDirection;

	public AudioSource shotSound;
	AudioSource[] sounds;

	void Start()
	{
		timeFired = Time.time;
		someScale = transform.localScale.x;
		//shotPrefab = Resources.Load ("Bullets") as GameObject;

	}

	void Awake ()
    {
		sounds = GetComponents<AudioSource> ();
		shotSound = sounds [0];

        firePoint = transform.FindChild("FirePoint");
        if(firePoint == null)
        {
            Debug.LogError("No Firepoint");
        }
	}

    // Update is called once per frame
    void Update()
    {
		currentTime = Time.time;

		if (fireRate == 0) //if single fire
        {
			if (currentTime >= timeFired + 1) {

				if (Input.GetButtonDown("Fire1") || Input.GetKeyDown ("x"))
				{
					Shoot();
					timeFired = Time.time;
				}
			
			}
        }
        else //if not single fire
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

        //RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition-firePointPosition, 100, WhatToHit);
        if (Time.time >= timeToSpawnEffect)
        {
            Effect();
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;

			shotSound.Play ();
			GameObject shot = Instantiate(shotPrefab) as GameObject;
			shot.transform.position = firePointPosition;
			Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();
			rb.velocity = ((mousePosition-firePointPosition).normalized)*20;

			shot.transform.eulerAngles = firePoint.transform.eulerAngles;
			checkDirection = false;
			if(checkDirection == false)
			{
				if(mousePosition.x < firePointPosition.x)
				{
					Debug.Log("< 0");
					shot.transform.eulerAngles = new Vector3(firePoint.transform.eulerAngles.x, firePoint.transform.eulerAngles.y, -firePoint.transform.eulerAngles.z);
					checkDirection = true;
				}
			}



			/*
			if(shot.transform.eulerAngles.z < 0)
			{
				Debug.Log(shot.transform.eulerAngles.x);
				//shot.transform.localScale = new Vector2(-someScale, -transform.localScale.y);
			}
			*/

			//Debug.Log(shotTransform);
			/*
			moveScript move = shotTransform.gameObject.GetComponent<moveScript>();
			if (move != null)
			{
				Debug.Log("move");
				move.direction = firePointPosition; // towards in 2D space is the right of the sprite
			}
			*/
        }
        //Debug.DrawLine(firePointPosition, (mousePosition-firePointPosition)*100);

		/*
        if(hit.collider != null )
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Debug.Log("We hit " + hit.collider.name + " and did " + Damage + " damage.");

			if(hit.collider.tag == "Enemy")
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
				//Destroy(hit.collider.gameObject);
            }
            */
    }

    void Effect()
    {
        //Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
    }
}
