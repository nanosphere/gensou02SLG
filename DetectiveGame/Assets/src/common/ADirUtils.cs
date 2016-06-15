using System;
using System.Collections.Generic;
using System.IO;

//============================================
// dirの探索を実装するクラス
//============================================
public abstract class ADirUtils
{

    //===================================
    // 探索の結果、ファイルに対しての処理を記載する
    // 戻り値：探索をつづけるかどうか true:続ける
    //===================================
    protected abstract bool checkFile(string fullpath);

    //===================================
    // ファイル
    // 戻り値：探索をつづけるかどうか true:続ける
    //===================================
    protected string getFileName(string fullpath)
    {
        return Path.GetFileName(fullpath);
    }

    //===================================
    // ディレクトリ内を全サーチ
    // このメソッドを外部から呼ぶ
    //===================================
    public bool SearchFiles(string dir)
    {
        if (!Directory.Exists(dir))
        {
            return true;
        }

        // 階層が上のほうが優先度高く
        string[] files = Directory.GetFiles(dir);
        foreach (string fullpath in files)
        {
            if (!checkFile(fullpath))
            {
                return false;
            }
        }

        string[] dirs = Directory.GetDirectories(dir);
        foreach (string s in dirs)
        {
            if (!SearchFiles(s))
            {
                return false;
            }
        }
        return true;
    }


    
    
    
}

