using System.IO;
using Newtonsoft.Json;

namespace Game.Script
{
    public class MyUtils
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
    }
}