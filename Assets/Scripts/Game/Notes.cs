using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Notes : MonoBehaviour {
	private GameObject note;
	public float x,y;
	public int notetype;
	public int bpmIndex;

    public AudioClip song;
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

		// 判定後だったら
		if (judgetime < -0.2){
			this.tag = "judgeignore";
			this.GetComponent<Notes>().enabled = false;
			this.GetComponent<SpriteRenderer>().enabled = false;
			Debug.Log("poor");

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

	public void disable(){
		this.tag = "judgeignore";
		this.GetComponent<Notes>().enabled = false;
		this.GetComponent<SpriteRenderer>().enabled = false;
		audioSource.PlayOneShot(song, 1f);

	}

	public void typeset(int ntype){
		notetype = ntype;
	}
	

    IEnumerator DestroyNote(){
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }

}
