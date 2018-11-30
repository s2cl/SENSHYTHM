/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadMap : MonoBehaviour {
	// Map Info
	TextAsset csvFile; // CSVファイル
	public static AudioSource Music;
	
	public static float bpm = 180; //mapparam
	public static float offset = 0.68f; //mapparam.offset

	// User Setting
	float speed = Setting.Highspeed;

	//List<string> leftKeybind = new List<string>(Setting.LeftKeybind);
	//List<string> rightKeybind = new List<string>(Setting.RightKeybind);

	public bool playFlag = false;
   	List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

	GameObject circle;

	
	// Use this for initialization
	void Start () {
		csvFile = Resources.Load("test") as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }

        // csvDatas[行][列]を指定して値を自由に取り出せる
        
		// プレハブを取得
		GameObject prefab = (GameObject)Resources.Load("Prefabs/note");
		Sprite[] spriteImages = new Sprite[9];
		for (int i=0;i<9;i++){
			spriteImages[i] = Resources.Load("Skins/default/note" + i.ToString() , typeof(Sprite)) as Sprite;
		}

		// notesを生成
		for (int i=0;i<csvDatas.Count;i++){
			// プレハブからインスタンスを生成
			Vector3 position = new Vector3(float.Parse(csvDatas[i][1]), float.Parse(csvDatas[i][0]) + offset);
			GameObject obj = Instantiate(prefab, position, Quaternion.identity);
			obj.GetComponent<SpriteRenderer>().sprite = spriteImages[int.Parse(csvDatas[i][2])];
			obj.GetComponent<Notes>().typeset(int.Parse(csvDatas[i][2]));
			obj.name = "note " + csvDatas[i][0];

		}

		// lineを生成
		GameObject barline = (GameObject)Resources.Load("Prefabs/BarLine");
		for (int i=1;i<202;i+=4){
			Vector3 position = new Vector3(0, 60/bpm*i + offset);
			GameObject obj = Instantiate(barline, position, Quaternion.identity);
			obj.name = "bar " + (60/bpm*i).ToString();
		}

		// 曲を再生
        AudioClip song = Resources.Load<AudioClip>("Songs/audio");
        Music = gameObject.GetComponent<AudioSource>();
		Music.clip = song;
        
		circle = Instantiate((GameObject)Resources.Load("Prefabs/Circle"), new Vector3(0, -Setting.judgeLine), Quaternion.identity);
		Sprite circleImage = Resources.Load("Skins/default/circle0", typeof(Sprite)) as Sprite;
		circle.GetComponent<SpriteRenderer>().sprite = circleImage;

		playFlag = true;
		Music.Play();
	}


	// Update is called once per frame
	void Update () {

		// 押したキーを格納するset
		HashSet<string> keydownset = new HashSet<string>();

		// 一時停止
		if (Input.GetKeyDown(KeyCode.Escape)){
			if (playFlag){
				Music.Pause();
				playFlag = false;
			}
			else {
				Music.UnPause();
				playFlag = true;
			}
		}

		// ハイスピ
		if (Input.GetKeyDown(KeyCode.F3)){
			Setting.Highspeed += 0.2f;
		}
		else if (Input.GetKeyDown(KeyCode.F4)){
			Setting.Highspeed -= 0.2f;
		}

		// 判定
		GameObject[] notes = GameObject.FindGameObjectsWithTag("judgenotes");

		if (notes.Length!=0){
			circle.transform.position = new Vector3(notes[0].transform.position.x, -Setting.judgeLine, 0.5f);

			for (int i=0;i<9;i++){
				if (Input.GetKeyDown(Setting.LeftKeybind[i])) keydownset.Add("left_note"+i.ToString());
				if (Input.GetKeyDown(Setting.RightKeybind[i])) keydownset.Add("right_note"+i.ToString());
			}

			foreach(GameObject i in notes){
				Notes note = i.GetComponent<Notes>();
				bool judgeflag = false;
				if (keydownset.Remove("left_note" + note.notetype.ToString())){
					note.audioSource.PlayOneShot(note.song,1f);
					judgeflag = true;
				}
				else if (keydownset.Remove("right_note" + note.notetype.ToString())){
					note.audioSource.PlayOneShot(note.song,1f);
					judgeflag = true;
				}

				if (judgeflag){

					float judgetime = note.y - Music.time;

					if (judgetime <= 0.02 && judgetime >= -0.02){
					Debug.Log("Perfect!");
					}
					else if (judgetime <= 0.04 && judgetime >= -0.04){
						Debug.Log("Great");
					}
					else if (judgetime <= 0.105 && judgetime >= -0.105){
						Debug.Log("good");
					}
					else if (judgetime <= 0.15 && judgetime >= -0.15){
						Debug.Log("bad...");
					}
					else{
						Debug.Log("poor");
					}
					
					Destroy(i);
				}
			}
		}

		
		


	}
}

*/