using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class LoadSongs : MonoBehaviour {
	public GameObject canvas;
	// Use this for initialization
	void Start () {
		// canvas 作成
	
		canvas = Instantiate((GameObject)Resources.Load("Prefabs/MapSelect/Canvas"), new Vector3(0,0,0), Quaternion.identity);

		canvas.GetComponent<Canvas>().worldCamera = Camera.main;
		GameObject songprefab = (GameObject)Resources.Load("Prefabs/MapSelect/SongText");
		DirectoryInfo dir = new DirectoryInfo(Application.dataPath+"/../Songs/map");
		FileInfo[] info = dir.GetFiles("*.json");
		float pos = 0f;
		foreach(FileInfo f in info){
			string mappath = "file://" + Application.dataPath + "/../Songs/map/" + f.Name;

			WWW tmp = new WWW(mappath);
			MapParam mapdata = MapParam.ReadFromJSON(tmp.text);

			Vector3 position = new Vector3(0,pos,0);
			GameObject obj = Instantiate(songprefab, position, Quaternion.identity);
			obj.name = f.Name;
			obj.GetComponent<Text>().text = mapdata.Title +"\n"+
											mapdata.Artist + " // " + mapdata.Creator + "\n" + 
											mapdata.Diffname;
			obj.transform.SetParent(canvas.transform, false);
			pos-= 180;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){
			Selected.Set("Natori Sana - Wakusei Loop [muzui].json");
			SceneManager.LoadScene("Game");
		}
		
	}
}
