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
		this.transform.position = new Vector3(x, y-Time.time*5 -4);
		float judgetime = y-Time.time*5;

		// 判定後だったら
		if (judgetime < -2){
			Debug.Log("poor");
			Destroy(this.gameObject);
		}
		// 判定前だったら
		else if (judgetime > 0.15){
		}

		else if (Input.GetKeyDown("s") || Input.GetKeyDown("l")){
			// perfect
			if (judgetime <= 0.02 && judgetime >= -0.02 ){
				Debug.Log("Perfect!");
			}
			else if (judgetime <= 0.04 && judgetime >= -0.04 ){
				Debug.Log("Great");
			}
			else if (judgetime <= 0.105 && judgetime >= -0.105 ){
				Debug.Log("good");
			}
			else if (judgetime <= 0.150 && judgetime >= -0.150 ){
				Debug.Log("bad...");
			}
			else{
				Debug.Log("poor");
			}
			Destroy(this.gameObject);

		}

	}
}
