using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : MonoBehaviour {
	public float x,y;
	public int notetype;
	public int bpmIndex;

	AudioClip song;
	AudioSource audioSource;
	public bool DisplayFlag=false;

	// Use this for initialization
	void Start () {
		x = this.transform.position.x;
		y = this.transform.position.y;
        song = Resources.Load<AudioClip>("Songs/hitclap");
        audioSource = this.gameObject.GetComponent<AudioSource>();
		audioSource.clip = song;
	}
	
	// Update is called once per frame
	void Update () {
		float nowtime = (float)LoadMap.Music.time;
        this.transform.position = new Vector3(x, y*Setting.Highspeed - nowtime*Setting.Highspeed - Setting.judgeLine + Setting.UserOffset);
		float judgetime = y - nowtime;

		
	}

	public void disable(bool playHitSound=false){
		this.tag = "judgeignore";
		this.enabled = false;
		this.GetComponent<SpriteRenderer>().enabled = false;
		if (playHitSound) audioSource.PlayOneShot(song, 1f);
	}

}
