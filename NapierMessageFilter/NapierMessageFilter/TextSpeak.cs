using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NapierMessageFilter
{
    public class TextSpeak
    {
        public string[] entries;
        public string[,] abbrWords = new string[2, 256];

        public TextSpeak()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

            ReadFileIn(path + "\\Resources\\textwords.csv");
        }

        public void ReadFileIn(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.Default);

            List<string> lines = File.ReadAllLines(filePath).ToList();

            while (!sr.EndOfStream)
            {
                int y = 0;

                foreach (var line in lines)
                {
                    entries = line.Split(',');
                    abbrWords[0, y] = entries[0];
                    abbrWords[1, y] = entries[1];
                    y++;
                }
                break;
            }
        }
    }
}
