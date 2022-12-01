using System;
using System.IO;
using Newtonsoft.Json;

namespace Game.Script
{
    public static class MyUtils
    {
        public static void ReadFile(string path, ref int[,] lsMap, ref int[,] lsNumber)
        {
            StreamReader reader = new StreamReader(path);
            var str1 = reader.ReadLine();
            var str2 = reader.ReadLine();
            if (str1 != null) lsMap = JsonConvert.DeserializeObject<int[,]>(str1);
            if (str2 != null) lsNumber = JsonConvert.DeserializeObject<int[,]>(str2);
            reader.Close();
        }
        
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp);
            return dateTime;
        }

        public static long DateTimeToTimeStamp(DateTime dateTime)
        {
            var span = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return (long)span.TotalSeconds;
        }
    }
}