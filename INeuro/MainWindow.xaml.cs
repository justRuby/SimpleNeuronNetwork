using Microsoft.Win32;
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

using INeuro.Core;
using INeuro.Neuron;
using INeuro.Model;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace INeuro
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Drawing.Image loadedImage;
        private NeuronNetwork net = new NeuronNetwork(10, 50, 50);
        public List<Answer> answers = new List<Answer>();

        public MainWindow()
        {
            InitializeComponent();
            HiddenObjects();

            ConsoleListBox.Items.Add(new Answer(ConsoleListBox.Items.Count, "Привет!"));
        }

        private void HiddenObjects()
        {
            answerGrid.Visibility = Visibility.Hidden;
            AcceptTrueAnswer.Visibility = Visibility.Hidden;
            neuronAnswerTextBox.Visibility = Visibility.Hidden;
        }

        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                loadedImage = new System.Drawing.Bitmap(openFileDialog.FileName);
            }

        }
        private void HandleImage_Click(object sender, RoutedEventArgs e)
        {
            ConsoleListBox.Items.Add(
                new Answer(ConsoleListBox.Items.Count, String.Format("Верный ли ответ - {0} ?",
                net.GetAnswer(loadedImage.ToInput(50, 50)).ToString())));

            answerGrid.Visibility = Visibility.Visible;
        }
        private void Handle_Click(object sender, RoutedEventArgs e)
        {
            if((sender as Button).Name == "HandleYes")
            {
                ConsoleListBox.Items.Add( new Answer(ConsoleListBox.Items.Count, String.Format("Я так и знал:)")));
            }
            else
            {
                ConsoleListBox.Items.Add(new Answer(ConsoleListBox.Items.Count, String.Format("А какой верный ответ?")));
                AcceptTrueAnswer.Visibility = Visibility.Visible;
                neuronAnswerTextBox.Visibility = Visibility.Visible;
            }

            answerGrid.Visibility = Visibility.Hidden;
        }
        private void AcceptTrueAnswer_Click(object sender, RoutedEventArgs e)
        {
            if(neuronAnswerTextBox.Text != "")
            {
                ConsoleListBox.Items.Add(new Answer(ConsoleListBox.Items.Count, String.Format("Я запомнил!")));
                net.Study(loadedImage.ToInput(50, 50), int.Parse(neuronAnswerTextBox.Text), 1);
            }
            else
            {
                ConsoleListBox.Items.Add(new Answer(ConsoleListBox.Items.Count, String.Format("Ты не написал, правильный ответ!")));
                return;
            }
            

            AcceptTrueAnswer.Visibility = Visibility.Hidden;
            neuronAnswerTextBox.Visibility = Visibility.Hidden;
        }
    }
}
