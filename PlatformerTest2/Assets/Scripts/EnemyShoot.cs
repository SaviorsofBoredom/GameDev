﻿using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour {

	public float fireRate = 0;
	public float Damage = 1;
	public LayerMask WhatToHit; //Layer mask of what to hit (will exclude player and raymask)
	
	public Transform BulletTrailPrefab;
	
	float timeToSpawnEffect = 0;
	public float effectSpawnRate;
	float timeToFire = 0;
	Transform firePoint;
	
	float timeFired;
	float currentTime;
	Collider trigger;

	Player player;
	GameObject playerGameObj;
	
	void Start()
	{
		timeFired = Time.time;
		//trigger = GetComponent<CircleCollider2D>;
	}
	
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
		currentTime = Time.time;
	}
	

	void OnTriggerStay2D(Collider2D collider){


		if (collider.tag == "Player") {
			//Debug.Log(collider.name + " triggered me");

			if (Time.time > timeToFire)
			{
				timeToFire = Time.time + 1 / fireRate;
				Shoot();
			}
		}
	}


	void Shoot()
	{
		//Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
		
		RaycastHit2D hit = Physics2D.Raycast(firePointPosition, Vector2.left, 100, WhatToHit);
		if (Time.time >= timeToSpawnEffect)
		{
			Effect();
			timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
		}
		//Debug.DrawLine(firePointPosition, (Vector2.left)*100);
		
		if(hit.collider != null )
		{
			Debug.DrawLine(firePointPosition, hit.point, Color.red);
			Debug.Log("We hit " + hit.collider.name + " and did " + Damage + " damage.");
			if(hit.collider.tag == "Player")
			{
				//Destroy(hit.collider.gameObject);
				playerGameObj = GameObject.Find("Player");
				player = playerGameObj.GetComponent<Player>();
				player.DecreaseHealth(Damage);
			}
		}
	}
	
	void Effect()
	{
		Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
	}
}
