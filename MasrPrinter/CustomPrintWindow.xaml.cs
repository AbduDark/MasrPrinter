using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Media.Imaging;

namespace MasrPrinter
{
    public partial class CustomPrintWindow : Window
    {
        private string currentBarcodeType = "Code128";

        public CustomPrintWindow()
        {
            InitializeComponent();
            CustomTextBox.Text = PrinterSettings.Instance.CustomNumber;
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
            if (BarcodeTypeComboBox == null || BarcodeTypeLabel == null)
                return;

            bool isChecked = WithBarcodeCheckBox.IsChecked == true;
            BarcodeTypeComboBox.IsEnabled = isChecked;
            BarcodeTypeLabel.Opacity = isChecked ? 1.0 : 0.5;
            if (BarcodeSizeSlider != null)
                BarcodeSizeSlider.IsEnabled = isChecked;

            UpdatePreview();
        }

        private void TextSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PrinterSettings.Instance.TextSize = (int)e.NewValue;
            UpdatePreview();
        }

        private void BarcodeSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PrinterSettings.Instance.BarcodeSize = (int)e.NewValue;
            UpdatePreview();
        }

        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            try
            {
                if (PreviewImage == null || PreviewPlaceholder == null)
                    return;

                if (string.IsNullOrWhiteSpace(CustomTextBox?.Text))
                {
                    PreviewImage.Source = null;
                    PreviewPlaceholder.Text = "أدخل النص لعرض المعاينة";
                    PreviewPlaceholder.Visibility = Visibility.Visible;
                    return;
                }

                bool withBarcode = WithBarcodeCheckBox?.IsChecked == true;

                if (withBarcode)
                {
                    var barcodeImage = BarcodeGenerator.GenerateBarcodeImage(CustomTextBox.Text, currentBarcodeType);
                    PreviewImage.Source = barcodeImage;
                    PreviewPlaceholder.Visibility = Visibility.Collapsed;
                }
                else
                {
                    PreviewImage.Source = GenerateTextOnlyPreview(CustomTextBox.Text);
                    PreviewPlaceholder.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                if (PreviewImage != null && PreviewPlaceholder != null)
                {
                    PreviewImage.Source = null;
                    PreviewPlaceholder.Text = $"خطأ في المعاينة: {ex.Message}";
                    PreviewPlaceholder.Visibility = Visibility.Visible;
                }
            }
        }

        private BitmapImage GenerateTextOnlyPreview(string text)
        {
            var settings = PrinterSettings.Instance;
            int dpi = (int)settings.BarcodeQuality;

            int width = (int)(settings.PaperWidth * dpi / 25.4);
            int height = (int)(settings.PaperHeight * dpi / 25.4);

            using var bitmap = new Bitmap(width, height);
            bitmap.SetResolution(dpi, dpi);

            using var g = Graphics.FromImage(bitmap);
            g.Clear(System.Drawing.Color.White);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            int fontSize = settings.TextSize;
            using var font = new System.Drawing.Font("Tahoma", fontSize, System.Drawing.FontStyle.Bold, GraphicsUnit.Point);
            var textSize = g.MeasureString(text, font);
            float textX = (width - textSize.Width) / 2;
            float textY = (height - textSize.Height) / 2;

            g.DrawString(text, font, System.Drawing.Brushes.Black, textX, textY);

            using var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;

            var bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            bmpImage.StreamSource = ms;
            bmpImage.CacheOption = BitmapCacheOption.OnLoad;
            bmpImage.EndInit();
            bmpImage.Freeze();

            return bmpImage;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CustomTextBox.Text))
                {
                    MessageBox.Show("الرجاء إدخال نص أو رقم قبل الحفظ.", "تحذير", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Directory.CreateDirectory("prints");
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string filePath;

                if (WithBarcodeCheckBox.IsChecked == true)
                {
                    filePath = Path.Combine("prints", $"label_{timestamp}.png");
                    BarcodeGenerator.SaveLabel(CustomTextBox.Text, currentBarcodeType, filePath);
                }
                else
                {
                    filePath = Path.Combine("prints", $"text_{timestamp}.png");
                    BarcodeGenerator.SaveTextOnly(CustomTextBox.Text, filePath);
                }

                PrintLabel(filePath);
                MessageBox.Show("تم الحفظ والطباعة بنجاح ✅", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء الحفظ أو الطباعة:\n{ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PrintLabel(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("ملف الطباعة غير موجود.", filePath);

                var settings = PrinterSettings.Instance;
                using var printDoc = new PrintDocument();

                if (!string.IsNullOrEmpty(settings.SelectedPrinter))
                    printDoc.PrinterSettings.PrinterName = settings.SelectedPrinter;

                int widthHundredths = (int)(settings.PaperWidth / 25.4 * 100);
                int heightHundredths = (int)(settings.PaperHeight / 25.4 * 100);
                printDoc.DefaultPageSettings.PaperSize = new PaperSize("Custom", widthHundredths, heightHundredths);

                printDoc.PrintPage += (s, e) =>
                {
                    using var img = System.Drawing.Image.FromFile(filePath);
                    var destRect = new Rectangle(0, 0, e.PageBounds.Width, e.PageBounds.Height);
                    e.Graphics?.DrawImage(img, destRect);
                };

                printDoc.Print();
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
