using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NapierMessageFilter
{
    public class FileMessage
    {
        public int ID { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
    }

    public class LoadFromFile : MainWindow
    {
        public List<FileMessage> fileMessages = new List<FileMessage>();
        public bool closeAll = false;
        public bool saveAll = false;
        public int numOfMsgs = 0;

        public FileMessage current;

        public LoadFromFile(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);

            numOfMsgs = CountMessages(lines);

            ProcessMessage(lines);

            foreach (FileMessage message in fileMessages)
            {
                Message = new Message();
                Format = new Format();
                Filter = new Filter();
                current = message;

                Format.Start(message.Header, message.Text, Message);

                if (Filter.Start(Message))
                {
                    if(!closeAll && !saveAll)
                    {
                        Output output = new Output(Message, this, false);
                        output.MoreOptions();
                        output.ShowDialog();
                    }
                    else if(saveAll)
                    {
                        SaveAll();
                        break;
                    }
                }
            }
        }

        public void SaveAll()
        {
            foreach (var message in fileMessages)
            {
                Message = new Message();
                Format = new Format();
                Filter = new Filter();

                Format.Start(message.Header, message.Text, Message);

                if (Filter.Start(Message))
                {
                   Output output = new Output(Message, this, true);
                }
            }

            MessageBox.Show("All messages saved!");
        }

        public int CountMessages(string[] lines)
        {
            bool startFound = false;
            int count = 0;

            foreach (var item in lines)
            {
                if (item == "-")
                {
                    startFound = true;
                }

                if (item == "--" && startFound)
                {
                    count++;
                    startFound = false;
                }
            }

            return count;
        }

        public void ProcessMessage(string[] lines)
        {
            int pointer = 0;
            int msgLength = 0;
            int msgStart = 0;
            int msgEnd = 0;
            int msgNum = -1;
            int idCount = 1;
            Random rng = new Random();
            bool startFound = false;

            foreach (var item in lines)
            {
                pointer++;

                if (item == "-")
                {
                    startFound = true;
                    msgNum++;
                    msgStart = pointer;
                    fileMessages.Add(new FileMessage { ID = idCount, Header = (lines[msgStart] + rng.Next(100000000, 999999999)) });
                }

                if (item == "--" && startFound)
                {
                    msgEnd = pointer;
                    msgLength = Math.Abs(msgStart - msgEnd);
                    idCount++;
                    startFound = false;

                    for (int i = msgStart + 1; i < msgEnd-1; i++)
                    {
                        fileMessages[msgNum].Text += lines[i] + "\n";
                    }
                }
            }
        }
    }
}
