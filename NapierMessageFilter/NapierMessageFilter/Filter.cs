using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NapierMessageFilter
{
    public class Filter
    {
        public bool Start(Message message)
        {
            Regex hashRegex = new Regex(@"^#\w+$");
            Regex handleRegex = new Regex(@"^@\w+$");
            TextSpeak abbrFile = new TextSpeak();

            if (message.Type == "SMS")
            {
                ExpandTextSpeak(message, abbrFile);

                return true;
            }
            else if (message.Type == "tweet")
            {
                ExpandTextSpeak(message, abbrFile);
                TweetCheck(message, hashRegex, handleRegex);

                return true;
            }
            else if (message.Type == "email")
            {
                QuarantineURL(message);

                return true;
            }

            return false;
        }

        private void TweetCheck(Message message, Regex hashRegex, Regex handleRegex)
        {
            for (int i = 0; i < message.Words.Count; i++)
            {
                if (hashRegex.IsMatch(message.Words[i]))
                {
                    message.Hashtags.Add(message.Words[i]);
                }
                else if (handleRegex.IsMatch(message.Words[i]))
                {
                    message.Mentions.Add(message.Words[i]);
                }
            }
        }

        private void ExpandTextSpeak(Message message, TextSpeak abbrFile)
        {
            for (int i = 0; i < 255; i++)
            {
                if (message.Words.Contains(abbrFile.abbrWords[0, i]))
                {

                    int index = message.Words.IndexOf(abbrFile.abbrWords[0, i]);

                    message.Words[index] = message.Words[index] + " <" + abbrFile.abbrWords[1, i] + ">";
                }
            }
        }

        private void QuarantineURL(Message message)
        {
            Regex obj = new Regex(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$");

            for (int i = 0; i < message.Words.Count; i++)
            {
                if (obj.IsMatch(message.Words[i]))
                {
                    message.URLs.Add(message.Words[i]);
                    message.Words[i] = "<URL Quarantined>";
                }
            }
        }
    }
}
