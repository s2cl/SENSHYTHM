using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour {
	private GameObject note;
	float x,y,z;

    AudioClip song;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {
		//note = GameObject.Find("/prehab/note.prehab");
		x = this.transform.position.x;
		y = this.transform.position.y;
        song = Resources.Load<AudioClip>("Songs/hitclap");
        audioSource = gameObject.GetComponent<AudioSource>();


        //z = 0;
    }

    // Update is called once per frame
    void Update () {
        float judgeLine = 4;
        float speed = 0.5f;
        this.transform.position = new Vector3(x, y-Time.time*speed - judgeLine);
		float judgetime = y-Time.time*speed;

		// 判定後だったら
		if (judgetime < -2){
			Debug.Log("poor");
			Destroy(this.gameObject);
		}
		// 判定前だったら
		else if (judgetime > 0.15 * speed){
		}

		else if (Input.GetKeyDown("s") || Input.GetKeyDown("l")){
            audioSource.PlayOneShot(song);
            // perfect
            if (judgetime <= 0.02 * speed && judgetime >= -0.02*speed ){
				Debug.Log("Perfect!");
			}
			else if (judgetime <= 0.04 * speed && judgetime >= -0.04*speed ){
				Debug.Log("Great");
			}
			else if (judgetime <= 0.105 * speed && judgetime >= -0.105 * speed ){
				Debug.Log("good");
			}
			else if (judgetime <= 0.15 * speed && judgetime >= -0.15 * speed ){
				Debug.Log("bad...");
			}
			else{
				Debug.Log("poor");
			}
            StartCoroutine("DestroyNote");

		}

	}

    IEnumerator DestroyNote(){
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }

}
