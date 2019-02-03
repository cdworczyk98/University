using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace NapierMessageFilter
{
    public class Format
    {
        ValidateHeader vHeader = new ValidateHeader();
        Regex sirRegex = new Regex(@"^SIR ([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$");

        public bool Start(string header, string text, Message message)
        {
            //--------------Get all the message detials we need like type, header and sender as well as if it's an SIR---------------//

            message.Type = vHeader.CheckHeader(header);

            if (message.Type != "invalid")
            {
                message.Header = header;
                message.Sender = text.Split(new string[] { "\r\n", "\r", "\n", }, StringSplitOptions.None)[0];
                if (message.Sender.Substring(message.Sender.Length - 3, message.Sender.Length - 1) == "\r")
                {
                    message.Sender = message.Sender.Substring(0, message.Sender.Length - 3);
                }
                

                if (message.Type == "email")
                {
                    message.Subject = text.Split('\n')[1];
                    if (sirRegex.IsMatch(message.Subject))
                    {
                        message.IsSIR = true;
                    }
                }

                //--------------Prepare the text by seperating words into a list we can work with---------------//

                string[] lines = text.Split(new string[] { "\r\n", "\r", "\n", }, StringSplitOptions.RemoveEmptyEntries);

                lines[0] = "";
                if (message.Type == "email")
                {
                    lines[1] = "";
                    if (message.IsSIR)
                    {
                        message.SIReport.Sortcode = lines[2].Substring(11);
                        message.SIReport.Incident = lines[3].Substring(20);

                        lines[2] = "";
                        lines[3] = "";
                    }
                }

                List<string> words = new List<string>();

                for (int i = 0; i < lines.Length; i++)
                {
                    words = new List<string>(lines[i].Split(new string[] { "\n", "\r", " " }, StringSplitOptions.RemoveEmptyEntries));
                    foreach (var item in words)
                    {
                        message.Words.Add(item);
                    }
                }

                message.Words = message.Words.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                if (!ValidateMessage(message)) return false;
            }
            else
            {
                MessageBox.Show("The header was entered inccorectly, please try again.");
                return false;
            }

            return true;
        }

        public bool ValidateMessage(Message message)
        {
            string sender = CheckSender(message.Sender, message.Type);
            string body = CheckCharCount(message.Words, message.Type);

            if (sender != "")
            {
                MessageBox.Show("Error with sender: " + sender);
                return false;
            }

            if (message.Type == "email")
            {
                string subject = CheckSubject(message.Subject);

                if(subject != "")
                {
                    MessageBox.Show("Error with subject: " + subject);
                    return false;
                }

                if (message.IsSIR)
                {
                    string sir = CheckIncidnetReport(message.SIReport.Incident, message.SIReport.Sortcode);
                    if (sir != "")
                    {
                        MessageBox.Show("Error with Formatting SIR: " + sir);
                        return false;
                    }
                } 
            }

            if (body != "")
            {
                MessageBox.Show(body);
                return false;
            }

            return true;
        }

        public string CheckIncidnetReport(string i, string s)
        {
            Regex incidnetRegex = new Regex(@"^[a-zA-Z]+$");
            Regex sortcodeRegex = new Regex(@"^\d{2}\-\d{2}\-\d{2}$");

            if (!incidnetRegex.IsMatch(i))
            {
                return "Incident can not be left empty";
            }

            if (!sortcodeRegex.IsMatch(s))
            {
                return "Sortcode needs to be in format XX-XX-XX";
            }

            return "";
        }

        public string CheckCharCount(List<string> words, string type)
        {
            int count = 0;

            foreach (var word in words)
            {
                count += word.Length;
            }

            if (count < 1) return "Body text can not be left empty.";

            if (type == "email")
            {
                if(count > 1028) return "Too many characters, Charcter Count: " + count;
            }
            else if (count > 140)
            {
                return "Too many characters, Charcter Count: " + count;
            }

            return "";
        }

        public string CheckSender(string sender, string type)
        {

            if (sender.Length < 1) return "Sender can not be empty.";

            if (type == "SMS")
            {
                if (sender.Length > 15)
                {
                    return "Phone number is too long!";
                }
            }
            else if (type == "email")
            {
                try
                {
                    var addr = new MailAddress(sender);
                    if (addr.Address == sender) return "";
                }
                catch (Exception)
                {
                    return "Email is invalid.";
                }
            }
            else if (type == "tweet")
            {
                if (sender.Substring(0, 1) != "@")
                {
                    return "Sender needs to begin with '@' symbol";
                }
                else if (sender.Length > 16)
                {
                    return "Too many characters in twitter handle.";
                }
            }

            return "";
        }

        public string CheckSubject(string subject)
        {
            if (subject.Length > 20) return "Subject line is too long.";
            else if (subject.Length < 1) return "Subject line can not be left empty";

            return "";
        }
    }
}
