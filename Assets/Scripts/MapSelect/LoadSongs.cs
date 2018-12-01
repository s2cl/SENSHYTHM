using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class LoadSongs : MonoBehaviour {
	public GameObject songlist;
	public Button btn;

	// Use this for initialization
	void Start () {
		// canvas 作成
	
		songlist = GameObject.Find("SongButton");

		GameObject button = (GameObject)Resources.Load("Prefabs/MapSelect/Button");
		DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/Songs/map");
		FileInfo[] info = dir.GetFiles("*.json");

		foreach(FileInfo f in info){
			string mappath = "file://" + Application.persistentDataPath + "/Songs/map/" + f.Name;
			WWW tmp = new WWW(mappath);
			MapParam mapdata = MapParam.ReadFromJSON(tmp.text);

			GameObject ButtonObj = Instantiate(button, new Vector3(), Quaternion.identity);
			ButtonObj.transform.SetParent(songlist.transform, false);
			ButtonObj.name = f.Name;
			
			btn = ButtonObj.GetComponent<Button>();
			btn.onClick.AddListener(btn.GetComponent<GameStart>().OnClick);

			GameObject title = ButtonObj.transform.Find("Title").gameObject;
			GameObject artist = ButtonObj.transform.Find("Artist").gameObject;
			title.GetComponent<Text>().text = mapdata.Title;
			artist.GetComponent<Text>().text= mapdata.Artist;

        }
	}

}
