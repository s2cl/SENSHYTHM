using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour {
	private GameObject note;
	float x,y,z;
	
	// Use this for initialization
	void Start () {
		//note = GameObject.Find("/prehab/note.prehab");
		x = this.transform.position.x;
		y = this.transform.position.y;
		//z = 0;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3(x, y-Time.time*5);
	}
}
