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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace NapierMessageFilter
{
    public partial class MainWindow : Window
    {

        public Format Format { get; set; }
        public Filter Filter { get; set; }
        public Message Message { get; set; }
        Random rng = new Random();
        OpenFileDialog ofd = new OpenFileDialog();
        

        public MainWindow()
        {
            InitializeComponent();

            Lable_BodyText2.Visibility = Visibility.Hidden;
            Lable_Subject.Visibility = Visibility.Hidden;

            Lable_BodyText1.Visibility = Visibility.Hidden;
            Lable_Sender.Visibility = Visibility.Hidden;
        }


        private void BTN_Process_Click(object sender, RoutedEventArgs e)
        {
            Message = new Message();
            Format = new Format();
            Filter = new Filter();

            if(Format.Start(TextBox_Header.Text, TextBox_Text.Text, Message))
            {
                if (Filter.Start(Message))
                {
                    Output output = new Output(Message, null, false);
                    output.Show();
                }
            }
        } 

        private void Button_GenH(object sender, RoutedEventArgs e)
        {
            Lable_Sender.Visibility = Visibility.Visible;

            Button genButton = sender as Button;

            if(genButton == Button_S) TextBox_Header.Text = string.Format("S{0}", rng.Next(100000000, 999999999));
            else if (genButton == Button_E) TextBox_Header.Text = string.Format("E{0}", rng.Next(100000000, 999999999));
            else if (genButton == Button_T) TextBox_Header.Text = string.Format("T{0}", rng.Next(100000000, 999999999));

            ClearBodyText();

            if(genButton == Button_E)
            {
                Lable_BodyText2.Visibility = Visibility.Visible;
                Lable_Subject.Visibility = Visibility.Visible;

                Lable_BodyText1.Visibility = Visibility.Hidden;
            }
            else
            {
                Lable_BodyText2.Visibility = Visibility.Hidden;
                Lable_Subject.Visibility = Visibility.Hidden;

                Lable_BodyText1.Visibility = Visibility.Visible;
            }
           
        }

        private void ClearBodyText()
        {
            TextBox_Text.Text = "";
        }

        private void Button_ViewAllMsg_Click(object sender, RoutedEventArgs e)
        {
            ViewMessages view = new ViewMessages();
            view.Show();
        }

        private void Button_LoadMsgFile_Click(object sender, RoutedEventArgs e)
        {
            if(ofd.ShowDialog() == true)
            {
                LoadFromFile loadFromFile = new LoadFromFile(ofd.FileName);
            }
        }
    }
}
