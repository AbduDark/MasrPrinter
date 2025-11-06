using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MasrPrinter
{
    public partial class BatchPrintWindow : Window
    {
        private string currentBarcodeType = "Code128";

        public BatchPrintWindow()
        {
            InitializeComponent();
        }

        private void BarcodeTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentBarcodeType = BarcodeTypeComboBox.SelectedIndex == 1 ? "QR" : "Code128";
        }

        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(StartNumberTextBox.Text, out int start))
                {
                    MessageBox.Show("الرجاء إدخال رقم بداية صحيح", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var barcodeImage = BarcodeGenerator.GenerateBarcodeImage(start.ToString(), currentBarcodeType);
                PreviewImage.Source = barcodeImage;
                PreviewInfoText.Text = $"معاينة الملصق رقم {start}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في المعاينة: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(StartNumberTextBox.Text, out int start) || 
                    !int.TryParse(EndNumberTextBox.Text, out int end))
                {
                    MessageBox.Show("الرجاء إدخال أرقام صحيحة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (start > end)
                {
                    MessageBox.Show("رقم البداية يجب أن يكون أصغر من رقم النهاية", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                PrintProgressBar.Maximum = end - start + 1;
                PrintProgressBar.Value = 0;

                Directory.CreateDirectory("prints");

                for (int i = start; i <= end; i++)
                {
                    string fileName = Path.Combine("prints", $"label_{i}.png");
                    BarcodeGenerator.SaveLabel(i.ToString(), currentBarcodeType, fileName);
                    
                    PrintProgressBar.Value = i - start + 1;
                    ProgressText.Text = $"تم طباعة {PrintProgressBar.Value} من {PrintProgressBar.Maximum}";
                    
                    await System.Threading.Tasks.Task.Delay(10);
                }

                MessageBox.Show($"تم طباعة {end - start + 1} ملصق بنجاح في مجلد prints", 
                    "نجح", MessageBoxButton.OK, MessageBoxImage.Information);
                
                ProgressText.Text = "اكتملت الطباعة بنجاح! ✓";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في الطباعة: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
