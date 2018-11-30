using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour {
	private GameObject note;
	public float x,y;
	public int notetype;

    public AudioClip song;
    public AudioSource audioSource;

    // Use this for initialization
    void Start () {
		x = this.transform.position.x;
		y = this.transform.position.y;
        song = Resources.Load<AudioClip>("Songs/hitclap");
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
		float speed = Setting.Highspeed;

        float judgeLine = Setting.judgeLine;
		float nowtime = LoadMap.Music.time;
        this.transform.position = new Vector3(x, y*speed - nowtime*speed - judgeLine);
		float judgetime = y - nowtime;

		// 判定後だったら
		if (judgetime < -0.2){
			Debug.Log("poor");
			Destroy(this.gameObject);
		}
		// 判定前だったら
		else if (judgetime < 0.2){
			this.tag = "judgenotes";
		}

	} 

	public void typeset(int ntype){
		notetype = ntype;
	}
	

    IEnumerator DestroyNote(){
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }

}
