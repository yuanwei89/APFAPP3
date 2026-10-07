using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Dictionary<string, int> drinks = new Dictionary<string, int>();
        Dictionary<string, int> orders = new Dictionary<string, int>();
        string resultMessage = "";
        string typeMessage = "內用";

        public MainWindow()
        {
            InitializeComponent();

            //讀取飲料品項    
            AddDrinksItems(drinks);


            //顯示所有飲料品項
            DisplayDrinksMenu(drinks);
        }
        private void DisplayDrinksMenu(Dictionary<string, int> drinks)
        {
            foreach (var drink in drinks)
            {
                StackPanel sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(2),
                    Height = 35,
                    VerticalAlignment = VerticalAlignment.Center,
                    Background = Brushes.AntiqueWhite,
                };

                CheckBox cb = new CheckBox {
                    Content = drink.Key,
                    FontFamily = new FontFamily("微軟正黑體"),
                    Width=200,
                    FontSize = 16,
                    Margin = new Thickness(10, 0, 20, 0),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.DarkBlue
                };

                Label lb_price = new Label
                {
                    Content = $"{drink.Value}元",
                    FontFamily = new FontFamily("微軟正黑體"),
                    FontSize = 16,
                    Margin = new Thickness(10, 0, 20, 0),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.Pink
                };
                Slider sl = new Slider
                {
                    Width = 150,
                    Minimum = 0,
                    Maximum = 10,
                    Value = 0,
                    IsSnapToTickEnabled = true,
                    VerticalContentAlignment = VerticalAlignment.Center,
                };

                Label lb_amount = new Label
                {
                    Content = "0",
                    FontFamily = new FontFamily("微軟正黑體"),
                    FontSize = 16,
                    Margin = new Thickness(10, 0, 20, 0),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.CadetBlue
                };

                Binding binding = new Binding("Value")
                {
                    Source = sl,
                    Mode = BindingMode.OneWay,
                };
                lb_amount.SetBinding(Label.ContentProperty, binding);

                sp.Children.Add(cb);
                sp.Children.Add(lb_price);
                sp.Children.Add(sl);
                sp.Children.Add(lb_amount);
                DrinkMenuStackPanel.Children.Add(sp);
            }

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
                    drinks.Add(drinkName, price);
                }
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            orders.Clear();
            resultMessage = "";

            double total = 0.0;
            string discountMessage = "沒有折扣";
            int index = 1;
            double sellPrice = 0.0;
            for (int i = 0; i < DrinkMenuStackPanel.Children.Count; i++)
            {
                var sp = DrinkMenuStackPanel.Children[i] as StackPanel;
                var cb = sp.Children[0] as CheckBox;
                var sl = sp.Children[2] as Slider;

                int quantity = (int)sl.Value;

                if (cb.IsChecked == true && quantity > 0)
                {
                    string drinkName = cb.Content.ToString();
                    orders.Add(drinkName, quantity);
                }
            } 



            // 檢視orders，把所有訂單細項內容計算出細項總和
            resultMessage += $"訂購方式：{typeMessage}，訂購清單如下：\n";
            foreach (var item in orders)
            {
                string drinkName = item.Key;
                if (drinks == null || !drinks.TryGetValue(drinkName, out int price))
                {
                    continue;
                }
                int quantity = item.Value;
                 
                int subTotal = price * quantity;
                total += subTotal;
                resultMessage += $"{index}. {drinkName}：{price}元 X {quantity}杯 = {subTotal}元\n";
                index++;
            }

            if (total >= 500)
            {
                discountMessage = "打8折";
                sellPrice = total * 0.8;
            }
            else if (total >= 300)
            {
                discountMessage = "打85折";
                sellPrice = total * 0.85;
            }
            else if (total >= 200)
            {
                discountMessage = "打9折";
                sellPrice = total * 0.9;
            }
            else
            {
                sellPrice = total;
            }
            resultMessage += $"總價{total}元，{discountMessage}，售價為：{sellPrice}元\n";
            ResultTextBlock.Text = resultMessage;

            // 儲存訂單明細
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "儲存訂單明細";   
            sfd.Filter = "文字檔案(*.txt)|*.txt|所有檔案|*.*";
            if (sfd.ShowDialog() == true)
            {
                string fileName = sfd.FileName;
                File.WriteAllText(fileName, resultMessage);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            typeMessage = rb.Content.ToString();
        }
    }
}