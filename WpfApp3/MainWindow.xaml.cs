using System.Windows;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Dictionary<string, int> drinks = new Dictionary<string, int>();  
        public MainWindow()
        {
            InitializeComponent();

            //讀取飲料品項    
            AddDrinksItems(drinks);
        }

         private void AddDrinksItems(Dictionary<string, int> drinks)
        {
            OpenFileDialog ofd  = new OpenFileDialog();
            ofd.Title = "選擇飲料品項檔案";
            ofd.Filter = "CSV檔案|*.csv|所有檔案|*.*";
            if (ofd.ShowDialog() == true)
            {
                string fileName = ofd.FileName;
                string[] lines = File.ReadAllLines(fileName);   


                foreach (var line in lines) 
                {
                    string[] tokens = line.Split(',');
                    string drinkName = tokens[0];
                    int price = int.Parse(tokens[1]); 
                }
            }
        }


    }
}