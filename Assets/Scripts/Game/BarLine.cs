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
		float judgeLine = LoadMap.judgeLine;
        float speed = LoadMap.speed;
		float nowtime = LoadMap.Music.time;

		this.transform.position = new Vector3(0, y*speed - nowtime*speed - judgeLine, 1);
		if (y - nowtime < -2){
			Destroy(this.gameObject);
		}
	}
}
