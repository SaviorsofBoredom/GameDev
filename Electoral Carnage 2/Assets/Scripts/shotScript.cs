using UnityEngine;
using System.Collections;

public class shotScript : MonoBehaviour {

	public int damage = 10;
	public bool isEnemyShot = false;


	// Use this for initialization
	void Start () 
	{
		Destroy (gameObject, 5);

	}
	

}
