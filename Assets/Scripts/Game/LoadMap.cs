using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Video;

public class LoadMap : MonoBehaviour {
	// Map Info
	public VideoPlayer vp;
	public static AudioSource Music;
	public static float mapBPM; //mapparam


	public static int perfect=0, great=0, good=0, bad=0, poor=0;


	// User Setting

	//List<string> leftKeybind = new List<string>(Setting.LeftKeybind);
	//List<string> rightKeybind = new List<string>(Setting.RightKeybind);

	public bool playFlag = false;

	private float startDelayTime = 5.0f;

	GameObject circle;

	
	// Use this for initialization
	async void Start () {

		// map情報を読み込み
		mapBPM = Selected.BPM;

		// 動画設定
		vp = LoadVideo(Selected.Video);
		
		// 音楽設定
		Music = await LoadMusic(Selected.Audio);

		CreateNotes(Selected.Notes);

		CreateBarLine(Selected.Length,mapBPM);

		//circle
		circle = Instantiate((GameObject)Resources.Load("Prefabs/Circle"), new Vector3(0, -Setting.judgeLine), Quaternion.identity);
		Sprite circleImage = Resources.Load("Skins/default/circle0", typeof(Sprite)) as Sprite;
		circle.GetComponent<SpriteRenderer>().sprite = circleImage;

		playFlag = true;
		vp.Play();
		Music.Play();
	}

	// notesを生成
	void CreateNotes(List<Note> notes){
		// プレハブを取得
		GameObject prefab = (GameObject)Resources.Load("Prefabs/note");
		Sprite[] spriteImages = new Sprite[9];
		for (int i=0;i<9;i++){
			spriteImages[i] = Resources.Load("Skins/default/note" + i.ToString() , typeof(Sprite)) as Sprite;
		}
		foreach(Note i in notes){
			// プレハブからインスタンスを生成
			Vector3 position = new Vector3(i.lane, i.time + startDelayTime);
			GameObject obj = Instantiate(prefab, position, Quaternion.identity);
			obj.GetComponent<SpriteRenderer>().sprite = spriteImages[i.type];
			obj.GetComponent<Notes>().typeset(i.type);
			obj.name = "note " + i.time.ToString();
		}	
	}

	// lineを生成
	void CreateBarLine(float length, float bpm){
		GameObject barline = (GameObject)Resources.Load("Prefabs/BarLine");
		for (int i=1; i<length + startDelayTime; i+=4){
			Vector3 position = new Vector3(0, 60 * i / bpm);
			GameObject obj = Instantiate(barline, position, Quaternion.identity);
			obj.name = "bar " + (60 * i / bpm).ToString();
		}
	}

	VideoPlayer LoadVideo(string file_name){
		VideoPlayer Video_Player = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
		Video_Player.url = Application.persistentDataPath + "/Songs/music/" + file_name;
		return Video_Player;
	}


	async Task<AudioSource> LoadMusic(string file_name){
		AudioSource audioSource = this.gameObject.GetComponent<AudioSource> ();
		WWW www = await new WWW("file://" + Application.persistentDataPath + "/Songs/music/" + file_name);
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

		// 押したキーを格納するset
		HashSet<string> keydownset = new HashSet<string>();

		// 一時停止
		
		if (Input.GetKeyDown(KeyCode.Escape)){
			if (playFlag){
				Music.Pause();
				vp.Pause();
				playFlag = false;
			}
			else {
				vp.time = Music.time;
				Music.Play();
				vp.Play();
				playFlag = true;
			}
		}

		// ハイスピ
		if (Input.GetKeyDown(KeyCode.F3)){
			Setting.Highspeed += 0.2f;
			Debug.Log("speed : "+ Setting.Highspeed.ToString());
		}
		else if (Input.GetKeyDown(KeyCode.F4)){
			Setting.Highspeed -= 0.2f;
			Debug.Log("speed : "+ Setting.Highspeed.ToString());
		}

		if (Input.GetKeyDown(KeyCode.F5)){
			Setting.UserOffset += 0.05f;
			Debug.Log("speed : "+ Setting.UserOffset.ToString());
		}
		else if (Input.GetKeyDown(KeyCode.F6)){
			Setting.UserOffset -= 0.05f;
			Debug.Log("speed : "+ Setting.UserOffset.ToString());
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

					float judgetime = note.y - (float)Music.time;

					if (judgetime <= 0.02 && judgetime >= -0.02){
						Debug.Log("Perfect!");
						perfect++;
					}
					else if (judgetime <= 0.04 && judgetime >= -0.04){
						Debug.Log("Great");
						great++;
					}
					else if (judgetime <= 0.105 && judgetime >= -0.105){
						Debug.Log("good");
						good++;
					}
					else if (judgetime <= 0.15 && judgetime >= -0.15){
						Debug.Log("bad...");
						bad++;
					}
					else{
						Debug.Log("poor");
						poor++;
					}
					
					Destroy(i);
				}
			}
		}
	}
}
