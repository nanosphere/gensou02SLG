

using Newtonsoft.Json;
using System;

namespace common
{
    public class JsonUtil
    {
        public static string serialize(object target)
        {

            // json.net を使用、.NET3.5以前いかunityは外部dll使えないらしい
            // 3.5もエラーがでる・・・ 2.0を使用でいけた
            try
            {
                string json = JsonConvert.SerializeObject(target);
                return json;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                Logger.error(e.ToString());
            }
            catch (Exception e)
            {
                Logger.error(e.ToString());
            }
            return "";
        }

        public static T deserialize<T>(string json)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                Logger.error(e.ToString());
            }
            catch (Exception e)
            {
                Logger.error(e.ToString());
            }
            return default(T);
        }
    }
}
