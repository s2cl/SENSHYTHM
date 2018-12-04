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

		// 押したキーを格納するset
		HashSet<string> keydownset = new HashSet<string>();

		// 一時停止
		
		if (Input.GetKeyDown(KeyCode.Escape)){
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

		if (!playFlag)return; // ポーズ中の判定停止
		// ハイスピ
		if (Input.GetKeyDown(KeyCode.F3)){
			Setting.Highspeed += 0.5f;
			Debug.Log("Highspeed : "+ Setting.Highspeed.ToString());
		}
		else if (Input.GetKeyDown(KeyCode.F4)){
			Setting.Highspeed -= 0.5f;
			Debug.Log("Highspeed : "+ Setting.Highspeed.ToString());
		}

		if (Input.GetKeyDown(KeyCode.F5)){
			Setting.UserOffset += 0.05f;
			Debug.Log("UserOffset : "+ Setting.UserOffset.ToString());
		}
		else if (Input.GetKeyDown(KeyCode.F6)){
			Setting.UserOffset -= 0.05f;
			Debug.Log("UserOffset : "+ Setting.UserOffset.ToString());
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
					note.disable(true);
					judgeflag = true;
				}
				else if (keydownset.Remove("right_note" + note.notetype.ToString())){
					note.disable(true);
					judgeflag = true;
				}

				if (judgeflag){

					float judgetime = note.y - (float)Music.time;

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
			}
		}
		SetScoreGUI();
	}

	public void SetScoreGUI(){
		perfectText.text = perfect.ToString();
		greatText.text = great.ToString();
		goodText.text = good.ToString();
		badText.text = bad.ToString();
		poorText.text = poor.ToString();
	}
}
