using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json;

namespace NapierMessageFilter
{
    public partial class Output : Window
    {
        public Message Message { get; set; }
        public LoadFromFile LoadFromFile { get; set; }
        public bool SaveAll { get; set; }

        public Output(Message message, LoadFromFile loadFromFile, bool saveAll)
        {
            InitializeComponent();

            SaveAll = saveAll;

            Button_CloseAll.Visibility = Visibility.Hidden;
            Button_SaveAll.Visibility = Visibility.Hidden;

            LoadFromFile = loadFromFile;
            Message = message;

            Lable_MsgType.Content = Message.Type;

            if(loadFromFile != null)
            {
                Title = loadFromFile.current.ID + " of " + loadFromFile.numOfMsgs;
            }

            if (SaveAll)
            {
                Button_Save.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                Close();
            }

            if (Message.Type != "email")
            {
                TextBox_Output.Text += Message.Header + "\n";
                TextBox_Output.Text += Message.Sender + "\n";
                foreach (var item in Message.Words)
                {
                    TextBox_Output.Text += item + " ";
                }

                if (Message.Type == "tweet")
                {
                    foreach (var item in Message.Hashtags)
                    {
                        Textbox_Tending.Text += item + "\n";
                    }

                    foreach (var item in Message.Mentions)
                    {
                        TextBox_Mentions.Text += item + "\n";
                    }
                }
            }
            else
            {
                TextBox_Output.Text += Message.Header + "\n";
                TextBox_Output.Text += Message.Sender + "\n";
                TextBox_Output.Text += Message.Subject + "\n";
                if (Message.IsSIR)
                {
                    TextBox_Output.Text += "Nature Of Incident: "+Message.SIReport.Incident + "\n";
                    TextBox_Output.Text += "Sort Code: "+Message.SIReport.Sortcode + "\n";
                }
                foreach (var item in Message.Words)
                {
                    TextBox_Output.Text += item + " ";
                }
                foreach (var item in Message.URLs)
                {
                    TextBox_URL.Text += item + "\n";
                }
                Lable_Incident.Content = message.SIReport.Incident;
                Lable_Sortcode.Content = message.SIReport.Sortcode;
            }
        }

        public void MoreOptions()
        {
            Button_CloseAll.Visibility = Visibility.Visible;
            Button_SaveAll.Visibility = Visibility.Visible;

            Button_Close.Content = "Next";
        }

        static string GetFilePath(string filename)
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));

            return path + "\\bin\\Debug\\" + filename;
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            string path = GetFilePath("text.txt");

            if (!File.Exists(path))
            {
                var file = File.Create(path);
                file.Close();
            }

            string jsonData = File.ReadAllText(path);

            var messageList = JsonConvert.DeserializeObject<List<Message>>(jsonData) ?? new List<Message>();

            messageList.Add(Message);

            jsonData = JsonConvert.SerializeObject(messageList, Formatting.Indented);

            File.WriteAllText(path, jsonData);

            if(!SaveAll) MessageBox.Show("Message Saved to JSON file.");
        }

        private void Button_CloseAll_Click(object sender, RoutedEventArgs e)
        {
            LoadFromFile.closeAll = true;
            Close();
        }

        private void Button_SaveAll_Click(object sender, RoutedEventArgs e)
        {
            LoadFromFile.saveAll = true;
            Close();
        }
    }
}
