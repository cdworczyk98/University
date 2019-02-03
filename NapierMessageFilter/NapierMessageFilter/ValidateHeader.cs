using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace NapierMessageFilter
{
    public class ValidateHeader
    {
        public string MsgType { get; set; }

        Regex numberRegex = new Regex("^[0-9]{9}$");

        public string CheckHeader(string header)
        {
            if (header.Length == 10)
            {
                string firstChar = header.Substring(0, 1);
                string numSet = header.Substring(1, 9);

                if (numberRegex.IsMatch(numSet) && (firstChar == "S" || firstChar == "E" || firstChar == "T"))
                {
                    switch (firstChar)
                    {
                        case "S":
                            MsgType =  "SMS";
                            break;
                        case "E":
                            MsgType = "email";
                            break;
                        case "T":
                            MsgType = "tweet";
                            break;
                        default:
                            MsgType = "invalid";
                            break;
                    }

                    return MsgType; ;
                }
                else MessageBox.Show("The header has inccorect format. First letter is made up of S, E or T followed by 9 numbers. (e.g S123456789)");
            }
            else MessageBox.Show("The header is inccorect length, must be 10 characters.");

            return MsgType;
        }
    }
}
