using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace Task1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (textBox != null)
            {
                string fontName = ((sender as ComboBox).SelectedItem as TextBlock).Text;
                textBox.FontFamily = new FontFamily(fontName);
            }
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (textBox != null)
            {
                double fontSize = Convert.ToDouble(((sender as ComboBox).SelectedItem as TextBlock).Text);
                textBox.FontSize = fontSize;
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.FontWeight == FontWeights.Bold)
                textBox.FontWeight = FontWeights.Normal;
            else
                textBox.FontWeight = FontWeights.Bold;
        }

        private void ToggleButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (textBox.FontStyle == FontStyles.Italic)
                textBox.FontStyle = FontStyles.Normal;
            else
                textBox.FontStyle = FontStyles.Italic;
        }

        private void ToggleButton_Click_2(object sender, RoutedEventArgs e)
        {
            if (textBox.TextDecorations.Any())
                textBox.TextDecorations.Clear();
            else
                textBox.TextDecorations.Add(TextDecorations.Underline);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (textBox != null)
                textBox.Foreground = Brushes.Black;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            if (textBox != null)
                textBox.Foreground = Brushes.Red;
        }
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChanged = true;
        }
        private bool isChanged;
        private string fileName = "";
        static void OpenFile(TextBox textBox, Window window, ref string fileName, ref bool isChanged)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                textBox.Text = File.ReadAllText(openFileDialog.FileName);
                fileName = openFileDialog.FileName;
                window.Title = openFileDialog.SafeFileName;
                isChanged = false;
            }
        }
        static void SaveFile(TextBox textBox, string fileName, ref bool isChanged)
        {
            File.WriteAllText(fileName, textBox.Text);
            isChanged = false;
        }
        static void SaveFileAs(TextBox textBox, Window window, ref string fileName, ref bool isChanged)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, textBox.Text);
                window.Title = saveFileDialog.SafeFileName;
                fileName = saveFileDialog.FileName;
                isChanged = false;
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (isChanged)
            {
                MessageBoxResult result = MessageBox.Show("Сохранить внесенные изменения?", "Сохранение данных", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes && fileName == "")
                {
                    SaveFileAs(textBox, window, ref fileName, ref isChanged);
                    OpenFile(textBox, window, ref fileName, ref isChanged);
                }
                else if(result == MessageBoxResult.Yes && fileName != "")
                {
                    SaveFile(textBox, fileName, ref isChanged);
                    OpenFile(textBox, window, ref fileName, ref isChanged);
                }
                else if (result == MessageBoxResult.No)
                {
                    OpenFile(textBox, window, ref fileName, ref isChanged);
                }
            }
            else
            {
                OpenFile(textBox,window, ref fileName, ref isChanged);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileAs(textBox, window, ref fileName, ref isChanged);
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (fileName == "")
            {
                SaveFileAs(textBox, window, ref fileName, ref isChanged);
            }
            else
            {
                SaveFile(textBox, fileName, ref isChanged);
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            if (isChanged)
            {
                MessageBoxResult result = MessageBox.Show("Сохранить внесенные изменения?", "Сохранение данных", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes && fileName == "")
                {
                    SaveFileAs(textBox, window, ref fileName, ref isChanged);
                    Application.Current.Shutdown();
                }
                else if (result == MessageBoxResult.Yes && fileName != "")
                {
                    SaveFile(textBox, fileName, ref isChanged);
                    Application.Current.Shutdown();
                }
                else if (result == MessageBoxResult.No)
                {
                    Application.Current.Shutdown();
                }
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
