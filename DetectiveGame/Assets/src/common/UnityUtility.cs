using System;

using UnityEngine;
//using UnityEditor;



//============================================
// Unity専用の機能を使う場所を抜き出し
// unity以外の場合はここを書き換えて代用
//============================================
public class UnityUtility
{
	public UnityUtility ()
	{
	}

	public static void DebugLog(string s){
		Debug.Log (s);
	}
	public static void MessageBox(string s){
		//EditorUtility.DisplayDialog("Information", s, "OK");
		Debug.LogError(s);
	}

}


