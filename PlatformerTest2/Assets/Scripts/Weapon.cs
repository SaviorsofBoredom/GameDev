using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public float fireRate = 0;
    public float Damage = 10;
    public LayerMask WhatToHit; //Layer mask of what to hit (will exclude player and raymask)

    public Transform BulletTrailPrefab;

    float timeToSpawnEffect = 0;
    public float effectSpawnRate;
    float timeToFire = 0;
    Transform firePoint;

	void Awake ()
    {
        firePoint = transform.FindChild("FirePoint");
        if(firePoint == null)
        {
            Debug.LogError("No Firepoint");
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (fireRate == 0) //if single fire
        {
            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown ("x"))
            {
                Shoot();
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

        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition-firePointPosition, 100, WhatToHit);
        if (Time.time >= timeToSpawnEffect)
        {
            Effect();
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
        //Debug.DrawLine(firePointPosition, (mousePosition-firePointPosition)*100);

        if(hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Debug.Log("We hit " + hit.collider.name + " and did " + Damage + " damage.");
            if(hit.collider.tag == "Enemy")
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    void Effect()
    {
        Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
    }
}
