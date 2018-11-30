using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarLine : MonoBehaviour {
	float y;
	// Use this for initialization
	void Start () {
		y = this.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		float nowtime = LoadMap.Music.time;

		this.transform.position = new Vector3(0, y*Setting.Highspeed - nowtime*Setting.Highspeed - Setting.judgeLine, 1);
		if (y - nowtime < -2){
			Destroy(this.gameObject);
		}
	}
}
