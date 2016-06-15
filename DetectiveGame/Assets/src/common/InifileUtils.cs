using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace common
{
    /// <summary>
    /// iniファイル取り扱いのためのユーティリティクラス
    /// </summary>
    public class InifileUtils
    {
        /// <summary>
        /// iniファイルのパスを保持
        /// </summary>
        private string filePath { get; set; }
        private bool ffile=false;
        public bool isload()
        {
            return ffile;
        }

        public Dictionary<string, Dictionary<string, string>> data = new Dictionary<string, Dictionary<string, string>>();
        

        // ==========================================================
        [DllImport("KERNEL32.DLL")]
        public static extern uint
            GetPrivateProfileString(string lpAppName,
            string lpKeyName, string lpDefault,
            StringBuilder lpReturnedString, uint nSize,
		string lpFileName);

        [DllImport("KERNEL32.DLL")]
        public static extern uint
            GetPrivateProfileInt(string lpAppName,
		string lpKeyName, int nDefault, string lpFileName);

        [DllImport("KERNEL32.dll")]
        private static extern int WritePrivateProfileString(
            string lpApplicationName,
            string lpKeyName,
            string lpstring,
		string lpFileName);
        
        [DllImport("KERNEL32.dll")]
	private static extern int GetPrivateProfileSectionNames(byte[] lpszReturnBuffer, int nSize, string lpFileName);

        [DllImport("KERNEL32.dll")]
		private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringA")]
            public static extern uint
        GetPrivateProfileStringByByteArray(string lpAppName,
        string lpKeyName, string lpDefault,
        byte[] lpReturnedString, uint nSize,
        string lpFileName);
        // ==========================================================

        /// <summary>
        /// コンストラクタ(fileパスを指定する場合)
        /// </summary>
        /// <param name="filePath">iniファイルパス</param>
        public InifileUtils(string file)
        {
            filePath = System.IO.Path.GetFullPath(file);
            // ファイルが開けるかどうかだけ確認
            if (!System.IO.File.Exists(filePath))
            {
                Logger.error("file not found/load.=" + filePath);
                return;
            }
            
            ffile = true;

        }
        
        /// <summary>
        /// iniファイル中のセクションのキーを指定して、整数値を返す
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string getValueString(string section, string key)
        {
            // まず、辞書にあるか見る
            if (data.ContainsKey(section))
            {
                if (data[section].ContainsKey(key)) {
                    // 登録されている
                    return data[section][key];
                }
            }
            // 登録されていない場合は、ファイルから読んでみる
            StringBuilder sb = new StringBuilder(1024);
			GetPrivateProfileString(section,key,"",sb,Convert.ToUInt32(sb.Capacity),filePath);
            if(sb.Equals(""))
            {
                Logger.error("NotFound.section=" + section + " key=" + key );
                return "";
            }
            // 読み込んだ値を辞書に登録
            setDictionary(section,key,sb.ToString());

            return sb.ToString();
        }
        public int getValueInt(string section, string key)
        {
			string s = getValueString (section, key);
			if (s == "") {
				return 0;
			}
			return int.Parse(s);
        }
        

        /// <summary>
        /// 指定したセクション、キーに数値を書き込む
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public bool setValue(string section, string key, int val)
        {
			return setValue (section,key,""+val);
        }
		//文字列バージョン追加
		public bool setValue(string section, string key, string str)
		{
			// ファイルに書き込み
			if (WritePrivateProfileString (section, key, str, filePath) == 0) {
				//書き込みに失敗
				Logger.error("SetValue save fail.section=" + section + " key=" + key + " str=" + str + " file=" + filePath);
				return false;
			}
			
			// 辞書に登録
			setDictionary(section, key, str);

			Logger.info("SetValue and save.section=" + section + " key=" + key + " str=" + str + " file=" + filePath);
			return true;
		}
        
        // 辞書に登録する
        private void setDictionary(string section, string key, string val)
        {
            if (data.ContainsKey(section))
            {
                if (data[section].ContainsKey(key))
                {
                    data[section][key] = val;
                }
                else
                {
                    data[section].Add(key, val);
                }
            }
            else
            {
                Dictionary<string, string> d = new Dictionary<string, string>();
                d.Add(key, "" + val);
                data.Add(section, d);
            }

        }


        //-------------------------------
        // すべてのkeyを読み込む
        //-------------------------------
        public bool allLoad()
        {
            if (!ffile)
            {
                Logger.error("iniファイルの読み込みに失敗しました。file not found="+filePath);
                return false;
            }

            byte[] ar2 = new byte[1024];
            uint resultSize2
                  = GetPrivateProfileStringByByteArray(
                        null, null, "default", ar2,
                        (uint)ar2.Length, filePath);
            string result2 = Encoding.Default.GetString(
                                    ar2, 0, (int)resultSize2 - 1);
            string[] sections = result2.Split('\0');
            foreach (string section in sections)
            {
                loadKeys(section);
            }
            
            return true;
        }
        private void loadKeys(string section)
        {
            byte[] buffer = new byte[2048];

            // Windows API でキー名を取得
			GetPrivateProfileSection(section, buffer, 20480, filePath);

            // バイト型配列（ASCII文字コード）をstring型配列に変換
            string[] tmp = Encoding.ASCII.GetString(buffer).Trim('\0').Split('\0');

            Dictionary<string, string> result = new Dictionary<string, string>();

            // string型配列をリストに変換
            foreach (string entry in tmp)
            {
                Logger.info(entry);
                char[] del = { '=' };
                string[] s = entry.Split(del);
                result.Add(s[0], s[1]);
            }
            data.Add(section, result);
        }

        //----------------------------------------------
        // debug
        //----------------------------------------------
        public override string ToString()
        {
            string str = "";
            foreach (string sec in data.Keys)
            {
                str += "[" + sec + "]\r\n";
                foreach (KeyValuePair<string, string> item in data[sec])
                {
                    str += item.Key+"="+item.Value+"\r\n";
                }
            }
            return str;
        }
    }
}
