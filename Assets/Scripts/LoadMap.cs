using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadMap : MonoBehaviour {
	// Map Info
	TextAsset csvFile; // CSVファイル
	public static AudioSource Music;
	public static float speed = 5.0f;
	public static float bpm = 180;
	public static float offset = 2;

	// User Setting
	public static float judgeLine = 4f;


	public bool playFlag = false;
   	List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

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
			Vector3 position = new Vector3(float.Parse(csvDatas[i][1])*2, float.Parse(csvDatas[i][0]) + offset);
			GameObject obj = Instantiate(prefab, position, Quaternion.identity);
			obj.GetComponent<SpriteRenderer>().sprite = spriteImages[int.Parse(csvDatas[i][2])];
		}

		// lineを生成
		GameObject barline = (GameObject)Resources.Load("Prefabs/BarLine");
		for (int i=1;i<202;i+=4){
			Vector3 position = new Vector3(0, 60/bpm*i + offset);
			GameObject obj = Instantiate(barline, position, Quaternion.identity);
		}

		// line barを生成

        AudioClip song = Resources.Load<AudioClip>("Songs/audio");
        Music = gameObject.GetComponent<AudioSource>();
		Music.clip = song;
        Music.Play();
		playFlag = true;

	}
	
	// Update is called once per frame
	void Update () {

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
		if (Input.GetKeyDown(KeyCode.F3)){
			speed += 0.2f;
		}
		else if (Input.GetKeyDown(KeyCode.F4)){
			speed -= 0.2f;
		}


	}
}
