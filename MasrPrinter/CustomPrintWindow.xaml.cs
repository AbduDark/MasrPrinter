using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MasrPrinter
{
    public partial class CustomPrintWindow : Window
    {
        private string currentBarcodeType = "Code128";

        public CustomPrintWindow()
        {
            InitializeComponent();
            UpdatePreview();
        }

        private void BarcodeTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentBarcodeType = BarcodeTypeComboBox.SelectedIndex == 1 ? "QR" : "Code128";
            UpdatePreview();
        }

        private void CustomTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePreview();
        }

        private void WithBarcodeCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (BarcodeTypeComboBox != null && BarcodeTypeLabel != null)
            {
                bool isChecked = WithBarcodeCheckBox.IsChecked == true;
                BarcodeTypeComboBox.IsEnabled = isChecked;
                BarcodeTypeLabel.Opacity = isChecked ? 1.0 : 0.5;
                UpdatePreview();
            }
        }

        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CustomTextBox?.Text))
                {
                    PreviewImage.Source = null;
                    PreviewPlaceholder.Visibility = Visibility.Visible;
                    return;
                }

                if (WithBarcodeCheckBox?.IsChecked == true)
                {
                    var barcodeImage = BarcodeGenerator.GenerateBarcodeImage(CustomTextBox.Text, currentBarcodeType);
                    PreviewImage.Source = barcodeImage;
                    PreviewPlaceholder.Visibility = Visibility.Collapsed;
                }
                else
                {
                    PreviewImage.Source = null;
                    PreviewPlaceholder.Text = "معاينة النص فقط (بدون باركود)";
                    PreviewPlaceholder.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                PreviewImage.Source = null;
                PreviewPlaceholder.Text = "خطأ في المعاينة";
                PreviewPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CustomTextBox.Text))
                {
                    MessageBox.Show("الرجاء إدخال نص أو رقم", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Directory.CreateDirectory("prints");
                string fileName;

                if (WithBarcodeCheckBox.IsChecked == true)
                {
                    fileName = Path.Combine("prints", $"custom_label_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    BarcodeGenerator.SaveLabel(CustomTextBox.Text, currentBarcodeType, fileName);
                }
                else
                {
                    fileName = Path.Combine("prints", $"custom_text_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    BarcodeGenerator.SaveTextOnly(CustomTextBox.Text, fileName);
                }

                MessageBox.Show($"تم الحفظ بنجاح!\n{fileName}", 
                    "نجح", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في الحفظ: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
