using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song : MonoBehaviour {
	public bool songFind;

	// Use this for initialization
	void Start () {
		// 曲があったらsongFindをtrue,無かったらFalseにする
		songFind = true;
	}
	
	// Update is called once per frame
	void Update () {
		// 曲が無かったら
		if (!songFind){
			// DLするボタンを表示

		}

	}
	// DLボタンが押されたら
	public void MyPointerDownUI(){
		//DLする
		CMD.Download_music("https://www.youtube.com/watch?v=AeB6SOvka44");
		CMD.Download_video("https://www.youtube.com/watch?v=AeB6SOvka44");
		songFind = true;
	}
}
