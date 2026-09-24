using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 對應 XAML 裡的 TextChanged 事件
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var targetTextBox = sender as TextBox;
            var targetStackPanel = targetTextBox.Parent as StackPanel;
            var targetNameLabel = targetStackPanel.Children[0] as Label;
            var targetPriceLabel = targetStackPanel.Children[1] as Label;

            int amount;
            bool success = int.TryParse(targetTextBox.Text, out amount);
            if (!success)
            {
                MessageBox.Show("請輸入正確數字", "輸入錯誤");
                //targetTextBox.Text = "";
            }
            else
            {
                string drinKName = targetNameLabel.Content.ToString();
                int price = Convert.ToInt32(targetPriceLabel.Content.ToString().Substring(0, 2));
                //MessageBox.Show($"您選擇的飲料是 {drinKName}，數量是 {amount} 杯，總金額是 {price * amount} 元", "訂購資訊");
                ResultTextBlock.Text += $"您選擇的飲料是 {drinKName}，數量是 {amount} 杯，總金額是 {price * amount} 元\n";
            }
        }

        // 對應 XAML 裡按鈕的 Click 事件
        private void OrderButton_click(object sender, RoutedEventArgs e)
        {
            // 未來這裡可以寫按下訂購後要執行的計算邏輯
        }
    }
}