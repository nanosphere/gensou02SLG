using System;
using System.Collections.Generic;
using System.IO;

//============================================
// ファイル名をあいまい検索する
//============================================
public class NearFileFind : ADirUtils
{
	public NearFileFind ()
	{
	}

	private string filename;
	private string result_file;

	//===================================
	// あいまい検索して一致したファイルパスを返す
	// unity用ファイルパス
	//===================================
	public string unity_find(string basepath,string filename)
	{
		this.filename = filename.ToLower();
		result_file = "";
		base.SearchFiles(basepath);

		return result_file;
	}

    //===================================
    // ファイル
    //===================================
    protected override bool checkFile(string fullpath)
	{
        string file = base.getFileName(fullpath);
        
        //大文字小文字区別せずいファイル名に含まれていればそれ
        file = file.ToLower();
		if (file.IndexOf (filename) >= 0) {
			result_file = fullpath;
			return false;
		}
		return true;
	}
    
}

