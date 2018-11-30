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
		Debug.Log(Highspeed);
		Debug.Log(RightKeybind);
	}
}