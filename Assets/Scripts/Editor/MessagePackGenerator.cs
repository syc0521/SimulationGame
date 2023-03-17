using System.Diagnostics;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MessagePackGenerator : MonoBehaviour
{
    [MenuItem("工具/生成MessagePack文件")]
    public static void Generate()
    {
        var projectPath = $"{System.Environment.CurrentDirectory}\\Game.Data.csproj";
        var outputPath = $"{Application.dataPath}/Scripts/Game/MessagePackGenerated.cs";
        var command = @$"dotnet mpc -i ""{projectPath}"" -o ""{outputPath}""";
        var thread = new Thread(() =>
        {
            StartCmd(command);
        });
        thread.Start();
    }
    
    private static void StartCmd(string Command)
    {
        Process p = new Process(); //创建进程对象
        p.StartInfo.FileName = @"C:\Windows\System32\cmd.exe"; //设定需要执行的命令
        
        p.StartInfo.UseShellExecute = false; //是否执行shell
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.Start();
        p.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
        p.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;
        p.StandardInput.WriteLine(Command);
        p.StandardInput.Close();
        p.WaitForExit(); //等待程序执行完退出进程
        Debug.Log(p.StandardOutput.ReadToEnd());
        p.Close();
    }
}