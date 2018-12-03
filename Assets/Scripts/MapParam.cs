using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class Note{
    public float time;
    public float lane; // Note:落ちる場所 BPM:bpmの値
    public int type; // Noteの種類
    public int option; // bit flag
    public string hitsound=""; // option&1 キー音
}
public class BPM{
    public float time;
    public float bpm;
    public int option;
    public int numer = 4; // 何分の何拍子
    public int denom = 4;

}

public class MapParam {
    public string Site;
    public string SiteId;
    public string Audio;
    public string Video;
    public string Title;
    public string Artist;
    public string Creator;
    public int CreaterId;
    public string Diffname;
    public int Length;
    public List<BPM> BPMs;
    public List<Note> Notes;

    public void AddMatadata(string key, string value){
        switch(key){
            case "Site":
                this.Site = value;
                break;
            case "SiteId":
                this.SiteId = value;
                break;
            case "Audio":
                this.Audio = value;
                break;
            case "Video":
                this.Video = value;
                break;
            case "Title":
                this.Title = value;
                break;
            case "Artist":
                this.Artist = value;
                break;
            case "Creator":
                this.Creator = value;
                break;
            case "CreaterId":
                this.CreaterId = int.Parse(value);
                break;
            case "Diffname":
                this.Diffname = value;
                break;
            default:
                break;
        }
        break;
    }

    public void AddBPM(string[] bpmStringList){
        BPM bpm = new BPM();
        int index=0;
        bpm.time = float.Parse(bpmStringList[index++]);
        bpm.bpm=float.Parse(bpmStringList[index++]);
        bpm.option=int.Parse(bpmStringList[index++]);
        if ((bpm.option&1<<0) != 0) bpm.numer = int.Parse(bpmStringList[index++]);
        if ((bpm.option&1<<0) != 0) bpm.denom = int.Parse(bpmStringList[index++]);
        this.BPMs.Add(bpm);
    }

    public void AddNotes(string[] noteStringList){
        Note note = new Note();
        int index=0;
        this.Length = (int)System.Math.Max(this.Length, float.Parse(noteStringList[0]));
        note.time   = float.Parse(noteStringList[index++]);
        note.lane   = float.Parse(noteStringList[index++]);
        note.type   = int.Parse(noteStringList[index++]);
        note.option = int.Parse(noteStringList[index++]);
        if ((note.option&1<<0) != 0) note.hitsound = noteStringList[index++];
        this.Notes.Add(note);
    }


    public void SetParam(string line, int option, bool noteLoad=true){
        switch(option){    
            case 0:
                break;
            case 1:
            // read metadata
                string key = line.Split(':')[0];
                string value = line.Split(':')[1]; 
                this.AddMatadata(key, value);
                break;
            case 2:
            // read bpms
                string[] bpmStringList = line.Split(',');
                this.AddBPM(bpmStringList);
                break;

            case 3:
            // read notes
                string[] noteStringList = line.Split(',');
                if (noteLoad){
                    this.AddNotes(noteStringList);
                }
                else{
                    this.Length = (int)System.Math.Max(this.Length, float.Parse(noteStringList[0]));
                }
                
                break;
        }
    }

    
    public static MapParam ReadWithoutNotes(string filename){
        MapParam readParam = new MapParam();
        readParam.BPMs = new List<BPM>();

        StringReader strReader = GetStringReader(filename);
        string line = null;
        int readOption = 0;
        readParam.Length = 0;

        while(true){
            line = strReader.ReadLine();
            if(line!=null){
                if (line.Equals("[metadata]")) readOption = 1;
                else if (line.Equals("[BPMs]")) readOption = 2;
                else if (line.Equals("[Notes]")) readOption = 3;
                else readParam.SetParam(line, readOption, false);
                }
            }
        return readParam;
    }

    public static MapParam ReadData(string filename,bool hasNotes=false){
        MapParam readParam = new MapParam();
        readParam.BPMs = new List<BPM>();
        if(hasNotes){
            readParam.Notes= new List<Note>();
        }
        else{
            readParam.Notes= null;
        }

        StringReader strReader = GetStringReader(filename);
        string line = null;
        int readOption = 0;
        readParam.Length = 0;

        while(true){
            line = strReader.ReadLine();
            if(line!=null){
                if (line.Equals("[metadata]")) readOption = 1;
                else if (line.Equals("[BPMs]")) readOption = 2;
                else if (line.Equals("[Notes]")) readOption = 3;
                else readParam.SetParam(line, readOption, hasNotes);
                }
            }
        return readParam;
    }
    

    public StringReader GetStringReader(string filename){
        string path = "file://" + Setting.SongsPath + "map/" + filename;
        Debug.Log("reading:"+path);
        WWW tmp = new WWW(path);
        return new StringReader(tmp.text);
    }


}