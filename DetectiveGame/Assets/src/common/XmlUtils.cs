using System;
using System.Collections.Generic;
using System.IO;

//============================================
// XMLを読み込む
//============================================
public class XmlUtils
{
    public XmlUtils()
    {
    }
    
    //===================================
    // 情報をロード
    //===================================
    public static T load<T>(string file)
    {
        if (!File.Exists(file))
        {
            // ファイル見つからね
            Logger.error("XmlUtils.load():file is not found. file="+file);
            return default(T);
        }
        if (file.IndexOf(".xml") <= 0)
        {
            // 拡張子が違う
            Logger.error("XmlUtils.load():file is not XML. file=" + file);
            return default(T);
        }

        FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);

        // デシリアライズ
        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        T t = (T)serializer.Deserialize(fs);

        fs.Close();

        Logger.info("XmlUtils.load():file is loaded. file=" + file);
        return t;
    }

    //===================================
    // 情報をセーブ
    //===================================
    public static void save<T>( T t,string file)
    {

        //書き込むファイルを開く（UTF-8 BOM無し）
        System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, new System.Text.UTF8Encoding(false));

        // デシリアライズ
        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

        //シリアル化し、XMLファイルに保存する
        serializer.Serialize(sw, t);
        //ファイルを閉じる
        sw.Close();
        
        Logger.info("XmlUtils.load():file is saved. file=" + file);
    }

}

