using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Config {
	public string Username;
    public float Highspeed;
    public float UserOffset;
    public float judgeLine;
    public List<string> LeftKeybind;
    public List<string> RightKeybind;

	public static Config ReadFromJSON(string jsonString){
        return JsonUtility.FromJson<Config>(jsonString);
    }

    public string MapParamToJSON(){
        return JsonUtility.ToJson(this);
    }


}
