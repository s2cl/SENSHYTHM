using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour {

	public float x,y;
	public int notetype;
	public int bpmIndex;

    AudioClip song;
	AudioSource audioSource;

	ParticleSystem particle;
	public bool DisplayFlag=false;


    // Use this for initialization
    void Start () {
		x = this.transform.position.x;
		y = this.transform.position.y;
        song = Resources.Load<AudioClip>("Songs/hitclap");
        audioSource = this.gameObject.GetComponent<AudioSource>();
		audioSource.clip = song;
		particle = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update () {
		float nowtime = (float)LoadMap.Music.time;
        this.transform.position = new Vector3(x, y*Setting.Highspeed - nowtime*Setting.Highspeed - Setting.judgeLine + Setting.UserOffset);
		float judgetime = y - nowtime;

		// 判定後だったら
		if (judgetime < -0.2){
			this.tag = "judgeignore";
			this.enabled = false;
			this.GetComponent<SpriteRenderer>().enabled = false;
			++LoadMap.poor;

		}
		// 判定前だったら
		else if (judgetime < 0.2){			
			this.tag = "judgenotes";
		}
		if (!DisplayFlag){
			if (judgetime < 2){
				this.GetComponent<SpriteRenderer>().enabled = true;
				DisplayFlag = true;
			}
		}


	}

	public void disable(bool playHitSound=false){
		this.tag = "judgeignore";
		this.enabled = false;
		this.GetComponent<SpriteRenderer>().enabled = false;
		if (playHitSound) {
		audioSource.PlayOneShot(song, 0.5f);
		particle.Play();
		}
	}


    IEnumerator DestroyNote(){
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }

}
