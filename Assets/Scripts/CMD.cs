using UnityEditor;
using System.Collections;
using System.Collections.Generic;
public static class CMD
{
    public static void Download_music(string URL, string id)
    {
        string path = Setting.SongsPath;
        Call($"/c youtube-dl -o \"{path}/music/%(id)s.%(ext)s\" -f 140 "+URL, false);
        Call($"/c ffmpeg -i \" {path}/music/{id}.m4a\" -vn -ac 2 -ar 44100 -ab 128k -acodec libvorbis -f ogg \" {path}/music/{id}.ogg\"");
        Call($"/c del {path}/music/{id}.m4a");
    }
    public static void Download_video(string URL)
    {
        Call("/c youtube-dl -o \"Songs/video/%(id)s.%(ext)s\" -f 136 "+URL,false);
    }


    public static void Call(string cmd, bool isHide = true)
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        if(isHide) startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = cmd;
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();
    }
}