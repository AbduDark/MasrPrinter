using System.Windows;

namespace MasrPrinter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BatchPrintButton_Click(object sender, RoutedEventArgs e)
        {
            var batchWindow = new BatchPrintWindow();
            batchWindow.Owner = this;
            batchWindow.ShowDialog();
        }

        private void CustomPrintButton_Click(object sender, RoutedEventArgs e)
        {
            var customWindow = new CustomPrintWindow();
            customWindow.Owner = this;
            customWindow.ShowDialog();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }
    }
}
