using UnityEditor;

public static class CMD
{
    public static void Download_music(string URL)
    {
        
        Call("/c youtube-dl -o \"Songs/music/%(id)s.%(ext)s\" -f 140 "+URL,false);
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