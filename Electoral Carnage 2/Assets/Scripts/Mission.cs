using UnityEngine;
using System.Collections;

public class Mission : MonoBehaviour {
	public float delayTime = 15;
	
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds (delayTime);
		
		Application.LoadLevel (3);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
