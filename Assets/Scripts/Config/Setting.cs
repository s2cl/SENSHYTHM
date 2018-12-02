using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Setting{
    public static string Username;
    public static float Highspeed;
    public static float UserOffset;
    public static float judgeLine;
    public static List<string> LeftKeybind;
	public static List<string> RightKeybind;
	public static string SongsPath;

	[RuntimeInitializeOnLoadMethod()]
	static void Init()
	{
		string configpath = "file://" + Application.dataPath + "/../config.json";
		WWW tmp = new WWW(configpath);
		Config config = Config.ReadFromJSON(tmp.text);

		Username	= config.Username;
		Highspeed	= config.Highspeed;
		UserOffset	= config.UserOffset;
		judgeLine	= config.judgeLine;
		LeftKeybind	= config.LeftKeybind;
		RightKeybind= config.RightKeybind;
		SongsPath = Application.persistentDataPath + "/Songs/";
	}

	public void Reload(){
		string configpath = "file://" + Application.dataPath + "/../config.json";
		WWW tmp = new WWW(configpath);
		Config config = Config.ReadFromJSON(tmp.text);

		Username	= config.Username;
		Highspeed	= config.Highspeed;
		UserOffset	= config.UserOffset;
		judgeLine	= config.judgeLine;
		LeftKeybind	= config.LeftKeybind;
		RightKeybind= config.RightKeybind;
		SongsPath = Application.persistentDataPath + "/Songs/";
	}
}