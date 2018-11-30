using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Note
{
    public double time;
    public int lane;
    public int type;
}

[System.Serializable]
public class MapParam {
    public string Site;
    public string Audio;
    public string Title;
    public string Artist;
    public string Creator;
    public int CreaterId;
    public string Diffname;
    public int Length;
    public int BPM;
    public double Offset;
    public List<Note> Notes;

    public static MapParam ReadFromJSON(string jsonString){
        return JsonUtility.FromJson<MapParam>(jsonString);
    }

    public string MapParamToJSON(){
        return JsonUtility.ToJson(this);
    }

}