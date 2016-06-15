using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace common
{
    public class FileIO
    {
        private static bool _write(string file, string str, FileMode mode)
        {
            FileStream wfs = null;
            StreamWriter sw = null;
            try
            {
                //書き込み
                wfs = new FileStream(file, mode, FileAccess.Write);
                sw = new StreamWriter(wfs);
                sw.Write(str);
                return true;
            }
            catch (FileNotFoundException e)
            {
                Logger.info("FileIO.write():" + e.Message);
            }
            finally
            {
                if (sw != null) { sw.Close(); sw = null; }
                if (wfs != null) { wfs.Close(); wfs = null; }
            }
            return false;

        }
        public static bool write(string file, string str)
        {
            return _write(file, str, FileMode.Create);
        }
        public static bool writeAdd(string file, string str)
        {
            return _write(file, str, FileMode.Append);
        }
        
        public static string read(string file)
        {
            FileStream rfs = null;
            StreamReader sr = null;
            try
            {
                rfs = new FileStream(file, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(rfs);
                var s = sr.ReadToEnd();
                return s;
            }
            catch (FileNotFoundException e)
            {
                Logger.info("FileIO.read():" + e.Message);
            }
            finally
            {
                if (sr != null) sr.Close();
                if (rfs != null) rfs.Close();
            }
            return "";
        }
        public static List<string> readLines(string file)
        {
            List<string> lines = new List<string>();
            FileStream rfs = null;
            StreamReader sr = null;
            try
            {
                rfs = new FileStream(file, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(rfs);
                while (sr.Peek() > -1)
                {
                    var s = sr.ReadLine();
                    lines.Add(s);
                }
                return lines;
            }
            catch (FileNotFoundException e)
            {
                Logger.info("FileIO.read():" + e.Message);
            }
            finally
            {
                if (sr != null) sr.Close();
                if (rfs != null) rfs.Close();
            }
            return lines;
        }
        
            
    }
}
