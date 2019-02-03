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
using Newtonsoft.Json;
using System.IO;
using System.Data;
using System.ComponentModel;

namespace NapierMessageFilter
{
    public class Hashtag
    {
        public string Text { get; set; }
        public int Count { get; set; }
    }

    public class Mention
    {
        public string Handle { get; set; }
        public int Count { get; set; }
    }

    public class SIR
    {
        public string MessageID { get; set; }
        public string Sortcode { get; set; }
        public string Incident { get; set; }
    }

    public partial class ViewMessages : Window
    {
        public string MessageType { get; set; }

        public List<Hashtag> HashtagList { get; set; }
        public List<Mention> MentionsList { get; set; }
        public List<SIR> SIRList { get; set; }
        public List<Message> MessageList { get; set; }

        public Message SelectedMessage { get; set; }
        public ICollectionView ItemList { get; set; }

        public string SearchContent { get; set; }

        public ViewMessages()
        {
            InitializeComponent();

            Datagrid_Msg.UnselectAll();

            Button_Delete.IsEnabled = false;

            SearchContent = "";

            HashtagList = new List<Hashtag>();
            MentionsList = new List<Mention>();
            SIRList = new List<SIR>();
            MessageList = new List<Message>();
            UpdateListView();
        }

        public void Load()
        {
            LoadFile();

            Datagrid_Msg.UnselectAll();

            TextBox_FullMsg.Text = "";

            var filterType = new Predicate<object>(item => ((Message)item).Type.Contains(MessageType));

            ItemList.Filter = filterType;

            Datagrid_Msg.ItemsSource = ItemList;

            FormatDatagrid();

            HashtagList.Clear();
            MentionsList.Clear();
            SIRList.Clear();

            CreateSIRList();
            CountHashtags();
            CountMentions();

            ListBox_Trending.Items.Clear();
            ListBox_Mentions.Items.Clear();
            ListBox_SIR.Items.Clear();

            UpdateListView();

            if (MessageType == "tweet")
            {
                SortTweetLists();

                foreach (var item in HashtagList)
                {
                    ListBox_Trending.Items.Add(item.Text + " - " + item.Count);
                }

                foreach (var item in MentionsList)
                {
                    ListBox_Mentions.Items.Add(item.Handle + " - " + item.Count);
                }
            }
            else if (MessageType == "email")
            {
                foreach (var item in SIRList)
                {
                    ListBox_SIR.Items.Add(item.Sortcode + " : " + item.Incident);
                }
            }
        }

        private void LoadFile()
        {
            string path = GetFilePath("text.txt");

            string jsonData = File.ReadAllText(path);

            MessageList = JsonConvert.DeserializeObject<List<Message>>(jsonData);

            var itemSourceList = new CollectionViewSource() { Source = MessageList };

            ItemList = itemSourceList.View;
        }

        private void SortTweetLists()
        {
            HashtagList.Sort(delegate (Hashtag x, Hashtag y)
            {
                return y.Count.CompareTo(x.Count);
            });

            MentionsList.Sort(delegate (Mention x, Mention y)
            {
                return y.Count.CompareTo(x.Count);
            });
        }

        private void CreateSIRList()
        {
            foreach (var message in MessageList)
            {
                if(message.Type == "email" && message.IsSIR)
                {
                    SIRList.Add(new SIR { MessageID = message.Header, Incident = message.SIReport.Incident, Sortcode = message.SIReport.Sortcode});
                }
            }
        }

        private void CountHashtags()
        {
            foreach (var message in MessageList)
            {
                if(message.Type == "tweet" && message.Hashtags != null)
                {
                    foreach (var hashtag in message.Hashtags)
                    {
                        if(CheckHashtag(hashtag))
                        {
                            continue;
                        }
                        else
                        {
                            HashtagList.Add(new Hashtag { Text = hashtag, Count = 1 });
                        }
                    }  
                }
            }
        }

        private void CountMentions()
        {
            foreach (var message in MessageList)
            {
                if (message.Type == "tweet" && message.Hashtags != null)
                {
                    foreach (var handle in message.Mentions)
                    {
                        if (CheckHandle(handle))
                        {
                            continue;
                        }
                        else
                        {
                            MentionsList.Add(new Mention { Handle = handle, Count = 1 });
                        }
                    }
                }
            }
        }

        public bool CheckHashtag(string hashtag)
        {
            foreach (var item in HashtagList)
            {
                if(item.Text == hashtag)
                {
                    item.Count++;
                    return true;
                }
            }

            return false;
        }

        public bool CheckHandle(string handle)
        {
            foreach (var mention in MentionsList)
            {
                if (mention.Handle == handle)
                {
                    mention.Count++;
                    return true;
                }
            }

            return false;
        }

        public void UpdateListView()
        {
            Lable_Mentions.Visibility = Visibility.Hidden;
            Lable_Trending.Visibility = Visibility.Hidden;
            ListBox_Mentions.Visibility = Visibility.Hidden;
            ListBox_Trending.Visibility = Visibility.Hidden;

            Lable_SIR.Visibility = Visibility.Hidden;
            ListBox_SIR.Visibility = Visibility.Hidden;

            Width = 560;

            if (MessageType == "email")
            {
                Lable_SIR.Visibility = Visibility.Visible;
                ListBox_SIR.Visibility = Visibility.Visible;
                Width = 700;
            }
            else if (MessageType == "tweet")
            {
                Lable_Mentions.Visibility = Visibility.Visible;
                Lable_Trending.Visibility = Visibility.Visible;
                ListBox_Mentions.Visibility = Visibility.Visible;
                ListBox_Trending.Visibility = Visibility.Visible;
                Width = 700;
            }
            else if(MessageType == "SMS")
            {
                this.Width = 560;
            }
        }

        static string GetFilePath(string filename)
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));

            return path + "\\bin\\Debug\\" + filename;
        }

        public void FormatDatagrid()
        {
            int offset = 10;

            if (MessageType =="email") offset = 9;


            Datagrid_Msg.Columns[0].Visibility = Visibility.Collapsed;
            Datagrid_Msg.Columns[3].Visibility = Visibility.Collapsed;
            for (int i = 4; i < offset; i++) Datagrid_Msg.Columns[i].Visibility = Visibility.Collapsed;
            Datagrid_Msg.CanUserAddRows = false;
            Datagrid_Msg.IsReadOnly = true;
            Datagrid_Msg.CanUserResizeColumns = false;

            Datagrid_Msg.Columns[1].Width = 110;
            Datagrid_Msg.Columns[2].Width = 110;

        }

        private void Datagrid_Msg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedMessage = (Message)Datagrid_Msg.SelectedItem;

            if (SelectedMessage == null) Button_Delete.IsEnabled = false;

            if(SelectedMessage != null)
            {
                Button_Delete.IsEnabled = true;

                TextBox_FullMsg.Text = "";
                TextBox_FullMsg.Text += (SelectedMessage.Header + "\n");
                TextBox_FullMsg.Text += (SelectedMessage.Sender + "\n");
                foreach (var item in SelectedMessage.Words)
                {
                    TextBox_FullMsg.Text += (item + " ");
                }
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            string path = GetFilePath("text.txt");

            string jsonData = File.ReadAllText(path);

            var messageList = JsonConvert.DeserializeObject<List<Message>>(jsonData);

            var selectedMessage = JsonConvert.SerializeObject(messageList.Where(i => i.Header != SelectedMessage.Header), Formatting.Indented);

            File.WriteAllText(path, selectedMessage);

            MessageBox.Show("Message Deleted.");

            Load();
        }

        private void RadioButton_SET_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if (radioButton == RadioButton_S)
            {
                Datagrid_Msg.UnselectAll();
                MessageType = "SMS";
            }
            else if (radioButton == RadioButton_E)
            {
                Datagrid_Msg.UnselectAll();
                MessageType = "email";

            }
            else if (radioButton == RadioButton_T)
            {
                Datagrid_Msg.UnselectAll();
                MessageType = "tweet";
            }

            Load();
        } 

        private void Button_Reload_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }
    }
}
