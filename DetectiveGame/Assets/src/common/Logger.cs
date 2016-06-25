
using System;
using System.IO;
using System.Text;

//============================================
// Logger
// unityとかVSなど、複数環境で統一ができるかわからないので自作
// ログはすべてここから出力
//============================================
public class Logger {
	public Logger(){
	}
    public static bool funity = true;

    //---------------------------------------------------
    // print
    // ここを切り替えれば表示先が変わる予定
    //---------------------------------------------------
    private static void print(string severiry,string str)
    {
        string s = "[" + severiry + "]" + str;

		Console.WriteLine(s);   //一般C#用
        if(funity)UnityUtility.DebugLog(s);

    }

    //---------------------------------------------------
    // info
    //---------------------------------------------------
    public static void info(string str)
    {
        print("INFO",str);
    }

    //---------------------------------------------------
    // error
    //---------------------------------------------------
    public static void error(string str)
    {
        print("ERROR", str);
    }

	//---------------------------------------------------
	// msgbox
	//---------------------------------------------------
	public static void MessageBox(string str)
	{
		UnityUtility.MessageBox (str);
	}
    
}
