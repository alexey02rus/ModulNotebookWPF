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

        //private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (textBox != null)
        //    {
        //        string fontName = (sender as ComboBox).SelectedItem as string;
        //        textBox.FontFamily = new FontFamily(fontName);
        //    }
        //}

        //private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    if (textBox != null)
        //    {
        //        double fontSize = Convert.ToDouble((sender as ComboBox).SelectedItem);
        //        textBox.FontSize = fontSize;
        //    }
        //}

        private void ComboBox_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            if (textBox != null && textBox.Foreground != Brushes.Red)
            {
                if (themes.SelectedIndex == 0)
                    textBox.Foreground = Brushes.Black;
                else
                    textBox.Foreground = Brushes.WhiteSmoke;
            }
            Uri theme = new Uri(themes.SelectedIndex == 0 ? "Light.xaml" : "Dark.xaml", UriKind.Relative);
            ResourceDictionary themeDictionary = Application.LoadComponent(theme) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Add(themeDictionary);
            Application.Current.Resources.MergedDictionaries.RemoveAt(1);
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
            {
                if (themes.SelectedIndex == 0)
                    textBox.Foreground = Brushes.Black;
                else
                    textBox.Foreground = Brushes.WhiteSmoke;
            }
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

        static void CloseWindow(TextBox textBox, Window window, ref string fileName, ref bool isChanged)
        {
            if (isChanged)
            {
                MessageBoxResult result = MessageBox.Show("Сохранить внесенные изменения?", "Сохранение данных", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFile(textBox, window, ref fileName, ref isChanged);
                    Exit();
                }
                else if (result == MessageBoxResult.No)
                {
                    Exit();
                }
            }
            else
            {
                Exit();
            }

            void Exit()
            {
                window.Close();
            }
        }

        static void OpenFile(TextBox textBox, Window window, ref string fileName, ref bool isChanged)
        {
            if (isChanged)
            {
                MessageBoxResult result = MessageBox.Show("Сохранить внесенные изменения?", "Сохранение данных", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFile(textBox, window, ref fileName, ref isChanged);
                    Open();
                }
                else if (result == MessageBoxResult.No)
                {
                    Open();
                }
            }
            else
            {
                Open();
            }

            string fN = fileName;
            bool iC = isChanged;
            void Open()
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    textBox.Text = File.ReadAllText(openFileDialog.FileName);
                    fN = openFileDialog.FileName;
                    window.Title = openFileDialog.SafeFileName;
                    iC = false;
                }
            }
        }
        static void SaveFile(TextBox textBox, Window window, ref string fileName, ref bool isChanged)
        {
            if (fileName == "")
            {
                SaveFileAs(textBox, window, ref fileName, ref isChanged);
            }
            else
            {
                File.WriteAllText(fileName, textBox.Text);
                isChanged = false;
            }
            
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
        private void MenuItem_Open(object sender, RoutedEventArgs e)
        {
            OpenFile(textBox, (MainWindow)sender, ref fileName, ref isChanged);
        }
        private void MenuItem_Save(object sender, RoutedEventArgs e)
        {
            SaveFile(textBox, (MainWindow)sender, ref fileName, ref isChanged);
        }

        private void MenuItem_SaveAs(object sender, RoutedEventArgs e)
        {
            SaveFileAs(textBox, (MainWindow)sender, ref fileName, ref isChanged);
        }
        private void MenuItem_NewWindow(object sender, RoutedEventArgs e)
        {
            //int themeIndex = themes.SelectedIndex;
            MainWindow mainWindow = new MainWindow();
            //mainWindow.themes.SelectedIndex = themeIndex;
            mainWindow.Show();

        }
        private void MenuItem_CloseWindow(object sender, RoutedEventArgs e)
        {
            CloseWindow(textBox, (MainWindow)sender, ref fileName, ref isChanged);
        }

        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isChanged)
            {
                MessageBoxResult result = MessageBox.Show("Сохранить внесенные изменения?", "Сохранение данных", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFile(textBox, window, ref fileName, ref isChanged);
                    e.Cancel = false;
                }
                else if (result == MessageBoxResult.No)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = false;
            }
        }


    }
}
