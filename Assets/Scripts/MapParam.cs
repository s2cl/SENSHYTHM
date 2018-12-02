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
    public float Length;
    public List<BPM> BPMs;
    public List<Note> Notes;
    
    public static MapParam ReadWithoutNotes(string filename){
        MapParam readParam = new MapParam();
        readParam.BPMs = new List<BPM>();
        readParam.Notes= new List<Note>();

        string path = Setting.SongsPath + "map/" + filename;
        Debug.Log("reading:"+path);
        WWW tmp = new WWW(path);
        StringReader strReader = new StringReader(tmp.text);
        string line = null;
        int readOption = 0;
        readParam.Length = 0f;

        Debug.Log(readParam.BPMs );
        Debug.Log(path);

        while(true){
            line = strReader.ReadLine();

            if(line!=null){
                if (line.Equals("[metadata]")) readOption = 1;
                else if (line.Equals("[BPMs]")) readOption = 2;
                else if (line.Equals("[Notes]")) readOption = 3;
                else{
                    int index=0;
                    switch(readOption){
                        
                        case 0:
                            break;
                        case 1:
                        // read metadata
                        string key = line.Split(':')[0];
                        string value = line.Split(':')[1]; 
                            switch(key){
                                case "Site":
                                    readParam.Site = value;
                                    break;
                                case "SiteId":
                                    readParam.SiteId = value;
                                    break;
                                case "Audio":
                                    readParam.Audio = value;
                                    break;
                                case "Video":
                                    readParam.Video = value;
                                    break;
                                case "Title":
                                    readParam.Title = value;
                                    break;
                                case "Artist":
                                    readParam.Artist = value;
                                    break;
                                case "Creator":
                                    readParam.Creator = value;
                                    break;
                                case "CreaterId":
                                    readParam.CreaterId = int.Parse(value);
                                    break;
                                case "Diffname":
                                    readParam.Diffname = value;
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case 2:
                            // read bpms
                            BPM bpm = new BPM();
                            string[] bpmtmp = line.Split(',');
                            bpm.time = float.Parse(bpmtmp[index++]);
                            bpm.bpm=float.Parse(bpmtmp[index++]);
                            bpm.option=int.Parse(bpmtmp[index++]);
                            if ((bpm.option&1<<0) !=0) bpm.numer = int.Parse(bpmtmp[index++]);
                            if ((bpm.option&1<<1) !=0) bpm.denom = int.Parse(bpmtmp[index++]);

                            readParam.BPMs.Add(bpm);
                            break;

                        case 3:
                            // read notes
                            string[] notetmp = line.Split(',');
                            readParam.Length = System.Math.Max(readParam.Length, float.Parse(notetmp[index]));
                            break;
                        }
                    }
                }
                else{
                    break;
                }
            }

        return readParam;
    }

    public static MapParam ReadWithNotes(string filename){
        MapParam readParam = new MapParam();
        readParam.BPMs = new List<BPM>();
        readParam.Notes= new List<Note>();
        
        string path = Setting.SongsPath + "map/" + filename;
        Debug.Log("reading:"+path);
        WWW tmp = new WWW(path);
        StringReader strReader = new StringReader(tmp.text);
        string line = null;
        int readOption = 0;

        while(true){
            line = strReader.ReadLine();

            if(line!=null){
                if (line.Equals("[metadata]")) readOption = 1;
                else if (line.Equals("[BPMs]")) readOption = 2;
                else if (line.Equals("[Notes]")) readOption = 3;
                else{
                    int index=0;
                    switch(readOption){
                        
                        case 0:
                            break;
                        case 1:
                        // read metadata
                        string key = line.Split(':')[0];
                        string value = line.Split(':')[1]; 
                            switch(key){
                                case "Site":
                                    readParam.Site = value;
                                    break;
                                case "SiteId":
                                    readParam.SiteId = value;
                                    break;
                                case "Audio":
                                    readParam.Audio = value;
                                    break;
                                case "Video":
                                    readParam.Video = value;
                                    break;
                                case "Title":
                                    readParam.Title = value;
                                    break;
                                case "Artist":
                                    readParam.Artist = value;
                                    break;
                                case "Creator":
                                    readParam.Creator = value;
                                    break;
                                case "CreaterId":
                                    readParam.CreaterId = int.Parse(value);
                                    break;
                                case "Diffname":
                                    readParam.Diffname = value;
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case 2:
                        // read bpms
                            BPM bpm = new BPM();
                            

                            string[] bpmtmp = line.Split(',');
                            bpm.time = float.Parse(bpmtmp[index++]);
                            bpm.bpm=float.Parse(bpmtmp[index++]);
                            bpm.option=int.Parse(bpmtmp[index++]);
                            if ((bpm.option&1<<0) != 0) bpm.numer = int.Parse(bpmtmp[index++]);
                            if ((bpm.option&1<<0) != 0) bpm.denom = int.Parse(bpmtmp[index++]);

                            readParam.BPMs.Add(bpm);
                            break;

                        case 3:
                        // read notes
                            Note note = new Note();
                            string[] notetmp = line.Split(',');
                            readParam.Length = System.Math.Max(readParam.Length, float.Parse(notetmp[index]));
                            note.time   = float.Parse(notetmp[index++]);
                            note.lane   = float.Parse(notetmp[index++]);
                            note.type   = int.Parse(notetmp[index++]);
                            note.option = int.Parse(notetmp[index++]);
                            if ((note.option&1<<0) != 0) note.hitsound = notetmp[index++];
                            
                            readParam.Notes.Add(note);
                            break;
                        }
                    }
                }
                else{
                    break;
                }
            }

        return readParam;
    }

}