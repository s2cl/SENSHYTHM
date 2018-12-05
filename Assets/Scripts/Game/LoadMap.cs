using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LoadMap : MonoBehaviour {
	// Map Info
	public VideoPlayer vp;
	public static AudioSource Music;

	public static int perfect=0, great=0, good=0, bad=0, poor=0;
	Text perfectText, greatText, goodText, badText, poorText;

	public bool playFlag = false;

	private float startDelayTime = 3.0f;
	private bool start = false;

	GameObject circle;

	List<bool> keyDownList = new List<bool>(){false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};

	
	// Use this for initialization
	async void Start () {
		// ScoreGUI
		perfectText = GameObject.Find("perfect").GetComponent<Text>();
		greatText = GameObject.Find("great").GetComponent<Text>();
		goodText = GameObject.Find("good").GetComponent<Text>();
		badText = GameObject.Find("bad").GetComponent<Text>();
		poorText = GameObject.Find("poor").GetComponent<Text>();

		// 動画設定
		vp = LoadVideo(Selected.Song.Video);
		vp.Play();
		vp.Pause();
		
		// 音楽設定
		enabled = false; // Startのコルーチン完了後にUpdate処理を開始するために必要
		Music = await LoadMusic(Selected.Song.Audio);
		enabled = true;

		CreateNotes(Selected.Song.Notes);

		CreateBarLine(Selected.Song.Length, Selected.Song.BPMs);

		//circle
		circle = Instantiate((GameObject)Resources.Load("Prefabs/Circle"), new Vector3(0, -Setting.judgeLine), Quaternion.identity);
		Sprite circleImage = Resources.Load("Skins/default/circle0", typeof(Sprite)) as Sprite;
		circle.GetComponent<SpriteRenderer>().sprite = circleImage;

		playFlag = true;
		Music.Play();
	}

	// notesを生成
	void CreateNotes(List<Note> notes){
		// プレハブを取得
		GameObject prefab = (GameObject)Resources.Load("Prefabs/note");
		//GameObject longPrefab = (GameObject)Resources.Load("Prefabs/LongNote");

		Sprite[] spriteImages = new Sprite[9];
		for (int i=0;i<9;i++){
			spriteImages[i] = Resources.Load("Skins/default/note" + i.ToString() , typeof(Sprite)) as Sprite;
		}
		foreach(Note i in notes){
			// プレハブからインスタンスを生成
			Vector3 position = new Vector3(i.lane/2, i.time + startDelayTime);
			GameObject obj = Instantiate(prefab, position, Quaternion.identity);
			obj.GetComponent<SpriteRenderer>().sprite = spriteImages[i.type];
			obj.GetComponent<Notes>().notetype = i.type;
			obj.name = "note " + i.time.ToString();
		}	
	}

	// lineを生成
	void CreateBarLine(float length, List<BPM> bpms){
		int index = 0;
		int bpmsCount = bpms.Count();
		
		GameObject barline = (GameObject)Resources.Load("Prefabs/BarLine");

		// first bar
		GameObject fbar = Instantiate(barline, new Vector3(0,bpms[0].time + startDelayTime), Quaternion.identity);
		fbar.name = "first bar";
		fbar.GetComponent<BarLine>().bpmIndex = index;

		for (float bartime=bpms[0].time+ startDelayTime; bartime<(length + startDelayTime);){
			if ((index+1)<bpmsCount){
				if ((bartime+(60 * 4 / bpms[index].bpm)) > bpms[index+1].time){
					bartime = bpms[++index].time;

					GameObject tmp = Instantiate(barline, new Vector3(0, bartime), Quaternion.identity);
					tmp.name = "bar " + bartime.ToString();
					tmp.GetComponent<BarLine>().bpmIndex = index;
				}
			}
			
			bartime += 60 * 4 / bpms[index].bpm;

			Vector3 position = new Vector3(0, bartime);
			GameObject obj = Instantiate(barline, position, Quaternion.identity);
			obj.name = "bar " + bartime.ToString();
			obj.GetComponent<BarLine>().bpmIndex = index;
		}
	}

	VideoPlayer LoadVideo(string file_name){
		VideoPlayer Video_Player = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
		Video_Player.url = Setting.SongsPath + "music/" + file_name;
		return Video_Player;
	}


	async Task<AudioSource> LoadMusic(string file_name){
		AudioSource audioSource = this.gameObject.GetComponent<AudioSource> ();
		WWW www = await new WWW("file://" + Setting.SongsPath + "music/" + file_name);
		audioSource.clip = AddSilentTime(www.GetAudioClip(false,false));
		return audioSource;
	}


    AudioClip AddSilentTime(AudioClip clip)
    {
        if (clip == null)
            return null;

        int length = 0;
        length += (clip.samples + (int)(clip.frequency * startDelayTime)) * clip.channels;
        if (length == 0)
            return null;

        float[] data = new float[length];
		float[] buffer = new float[clip.samples * clip.channels];
		clip.GetData(buffer, 0);
		buffer.CopyTo(data, (int)(clip.frequency * startDelayTime) * clip.channels);

        AudioClip result = AudioClip.Create("Combine", length / clip.channels, clip.channels, clip.frequency, false);
        result.SetData(data, 0);

        return result;
    }
	



    // Update is called once per frame
    void Update () {
		if(!start && Music.time >= startDelayTime){
			start = true;
			vp.Play();
		}

		if (Input.anyKeyDown){
			GameObject[] notes = GameObject.FindGameObjectsWithTag("judgenotes");
			// 判定
			DownKeyAction(notes);
		}
		SetScoreGUI();
		

	}

	
	public void DownKeyAction(GameObject[] notes){

		// 一時停止
		if (Input.GetButtonDown("Cancel")){
			if (playFlag){
				Music.Pause();
				if(start) vp.Pause();
				playFlag = false;
			}
			else {
				if(start){
					vp.time = Music.time - startDelayTime;
					vp.Play();
				}
				Music.Play();
				playFlag = true;
			}
		}

		// ハイスピ変更
		if (Input.GetButtonDown("HighSpeedUp")) Setting.Highspeed += 1f;
		else if (Input.GetButtonDown("HighSpeedDown")) Setting.Highspeed -= 1f;

		// オフセット変更
		if (Input.GetButtonDown("OffsetPlus")) Setting.UserOffset += 0.05f;
		else if (Input.GetButtonDown("OffsetMinus")) Setting.UserOffset -= 0.05f;

		// 判定
		// 押したキーを格納するlist keyDownList

		for (int i=0;i<9;i++){
			if (Input.GetButtonDown("LeftNote" +i.ToString())) keyDownList[i*2] = true;
			else keyDownList[i*2] = false;

			if (Input.GetButtonDown("RightNote"+i.ToString())) keyDownList[i*2+1] = true;
			else keyDownList[i*2+1] = false;
		}


		foreach(GameObject i in notes){
			Notes note = i.GetComponent<Notes>();
			if (IsDown(note.notetype)){
				note.disable(true);
				circle.transform.position = new Vector3(notes[0].transform.position.x, -Setting.judgeLine, 0.5f);
				float judgetime = note.y - Music.time;
				JudgeCount(judgetime);
			}
		}

	}

	public bool IsDown(int notetype){
		if (keyDownList[notetype*2]){
			keyDownList[notetype*2] = false;
			return true;
			}
		if (keyDownList[notetype*2+1]){
			keyDownList[notetype*2+1] = false;
			return true;
			}
		return false;
	}


	public void JudgeCount(float judgetime){
		if (judgetime <= 0.02 && judgetime >= -0.02){
			perfect++;
		}
		else if (judgetime <= 0.04 && judgetime >= -0.04){
			great++;
		}
		else if (judgetime <= 0.105 && judgetime >= -0.105){
			good++;
		}
		else if (judgetime <= 0.15 && judgetime >= -0.15){
			bad++;
		}
		else{
			poor++;
		}
	}


	public void SetScoreGUI(){
		perfectText.text = perfect.ToString();
		greatText.text = great.ToString();
		goodText.text = good.ToString();
		badText.text = bad.ToString();
		poorText.text = poor.ToString();
	}
}
