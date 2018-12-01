using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selected{

	public static string path;
	public static string Site;
	public static string Audio;
	public static string Video;
	public static string Title;
	public static string Artist;
	public static string Creator;
	public static int CreaterId;
	public static string Diffname;
	public static float Length;
	public static float BPM;
	public static float Offset;
	public static List<Note> Notes;

	public static void Set(string filename){
		path = "file://" + Application.persistentDataPath + "/Songs/map/" + filename;
		WWW tmp = new WWW(path);
		MapParam param = MapParam.ReadFromJSON(tmp.text);
		Audio  = param.Audio;
		Video = param.Video;
		Title	= param.Title;
		Artist	= param.Artist;
		Creator	= param.Creator;
		CreaterId	= param.CreaterId;
		Diffname	= param.Diffname;
		Length	= param.Length;
		BPM		= param.BPM;
		Offset	= param.Offset;
		Notes	= param.Notes;

	}
}