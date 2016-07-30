using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace common
{
    public class Crypt
    {
        public static string encode(string str)
        {
            if (str == "") return "";
            //Random rnd1 = new Random(Environment.TickCount);
            //int rnd = rnd1.Next(0, 999);
            
            //Random rnd = new Random(1);
            //int randomNumber = rnd.Next(0,999999);
            
            string s = EncryptString( str , "pass");
            return s;
        }
        public static string dencode(string str)
        {
            if (str == "") return "";
            string s2 = DecryptString(str, "pass");
            return s2;
        }

        // パスワードから共有キーと初期化ベクタを作成
        private static void GenerateKeyFromPassword(string password, int keySize, out byte[] key, int blockSize, out byte[] iv)
        {
            byte[] salt = Encoding.UTF8.GetBytes("RandomByte");  // ８バイト以上の任意の文字列を指定
            Rfc2898DeriveBytes rdb = new Rfc2898DeriveBytes(password, salt);
            rdb.IterationCount = 999;  // 演算反復処理回数
            key = rdb.GetBytes(keySize / 8);
            iv = rdb.GetBytes(blockSize / 8);
        }

        // 文字列を暗号化
        private static string EncryptString(string sourceString, string password)
        {
            RijndaelManaged rm = new RijndaelManaged();
            byte[] key, iv;
            GenerateKeyFromPassword(password, rm.KeySize, out key, rm.BlockSize, out iv);
            rm.Key = key;
            rm.IV = iv;

            byte[] strBytes = Encoding.UTF8.GetBytes(sourceString);
            string strRet;
            using (ICryptoTransform ict = rm.CreateEncryptor())
            {
                byte[] encBytes = ict.TransformFinalBlock(strBytes, 0, strBytes.Length);
                strRet = Convert.ToBase64String(encBytes);
            }
            return strRet;
        }

        // 暗号化された文字列を復号化
        private static string DecryptString(string encryptString, string password)
        {
            RijndaelManaged rm = new RijndaelManaged();
            byte[] key, iv;
            GenerateKeyFromPassword(password, rm.KeySize, out key, rm.BlockSize, out iv);
            rm.Key = key;
            rm.IV = iv;

            byte[] strBytes = Convert.FromBase64String(encryptString);
            string strRet;
            using (ICryptoTransform ict = rm.CreateDecryptor())
            {
                byte[] decBytes = ict.TransformFinalBlock(strBytes, 0, strBytes.Length);
                strRet = Encoding.UTF8.GetString(decBytes);
            }
            return strRet;
        }
    }
}
