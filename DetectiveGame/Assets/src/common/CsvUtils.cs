//using Microsoft.VisualBasic.FileIO; //参照追加する必要あり
using System;
using System.Collections.Generic;
using System.IO;

//============================================
// CSVを読み込む
//TextFieldParser 
//============================================
public class CsvUtils
{
    public CsvUtils()
    {
    }

    //===================================
    // 情報をロード
    //===================================
    /*
    // MS libraryが使える場合
    public static List<string[]> load(string file)
    {
        if (!File.Exists(file))
        {
            // ファイル見つからね
            Logger.error("CsvUtils.load():file is not found. file=" + file);
            return null;
        }
        if (file.IndexOf(".csv") <= 0)
        {
            // 拡張子が違う
            Logger.info("CsvUtils.load():file is not CSV. file=" + file);
            return null;
        }

		List<string[]> list = new List<string[]>();

        TextFieldParser parser = new TextFieldParser(file, System.Text.Encoding.GetEncoding("Shift_JIS"));
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");// ","区切り
        
        while (parser.EndOfData == false)
        {
            list.Add(parser.ReadFields());
        }

        Console.WriteLine("ロード終了 file=" + file);
        return list;
    }
    */

    public static List<string[]> load(string file)
    {
        if (!File.Exists(file))
        {
            // ファイル見つからね
            Logger.error("CsvUtils.load():file is not found. file=" + file);
            return null;
        }
        if (file.IndexOf(".csv") <= 0)
        {
            // 拡張子が違う
            //Logger.info("CsvUtils.load():file is not CSV. file=" + file);
            return null;
        }

        List<string[]> list = new List<string[]>();

        // -----------------------------------

        FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
        StreamReader objReader = new StreamReader(fs);
        string sLine = ""; // ←　一時格納用

        // ファイルから1行ずつ読み込む
        while (sLine != null)
        {
            // ファイルから1行ずつ読み込むReadToEndでもいい
            sLine = objReader.ReadLine();
            if (sLine != null)
            {
                list.Add( parseLine(sLine)); // addメソッドで追記
            }
        }
        objReader.Close();
        fs.Close();
        
        // -----------------------------------
        return list;

    }
    private static string [] parseLine(string line)
    {
        string[] temp2 = line.Split(',');
        return temp2;
    }
}

