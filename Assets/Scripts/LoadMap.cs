using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadMap : MonoBehaviour {
	TextAsset csvFile; // CSVファイル
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
        float offset = 2;
        float speed = 0.5f;
		for (int i=0;i<csvDatas.Count;i++){
			
			Debug.Log(csvDatas[i][0]);
			// プレハブからインスタンスを生成
			Vector3 position = new Vector3(float.Parse(csvDatas[i][1])*2, float.Parse(csvDatas[i][0])*speed + offset);
			GameObject obj = Instantiate (prefab, position, Quaternion.identity);
			obj.GetComponent<SpriteRenderer>().sprite = spriteImages[int.Parse(csvDatas[i][2])];
		}
        AudioClip song = Resources.Load<AudioClip>("Songs/audio");
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(song);


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
