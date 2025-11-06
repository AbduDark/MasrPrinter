using System.Windows;
using System.Windows.Controls;
using System.Drawing.Printing;
using System.Linq;

namespace MasrPrinter
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            LoadPrinters();
            LoadSettings();
            UpdateSizePreview();
        }

        private void LoadPrinters()
        {
            try
            {
                PrinterComboBox.Items.Clear();
                
                foreach (string printerName in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    PrinterComboBox.Items.Add(printerName);
                }

                if (PrinterComboBox.Items.Count == 0)
                {
                    PrinterComboBox.Items.Add("لا توجد طابعات متعرفة");
                    PrinterComboBox.IsEnabled = false;
                }
            }
            catch
            {
                PrinterComboBox.Items.Add("خطأ في تحميل الطابعات");
                PrinterComboBox.IsEnabled = false;
            }
        }

        private void LoadSettings()
        {
            var settings = PrinterSettings.Instance;
            PaperWidthTextBox.Text = settings.PaperWidth.ToString();
            PaperHeightTextBox.Text = settings.PaperHeight.ToString();
            ThermalLevelSlider.Value = settings.ThermalLevel;
            BarcodeQualitySlider.Value = settings.BarcodeQuality;
            CustomNumberTextBox.Text = settings.CustomNumber;
            
            if (!string.IsNullOrEmpty(settings.SelectedPrinter) && PrinterComboBox.Items.Contains(settings.SelectedPrinter))
            {
                PrinterComboBox.SelectedItem = settings.SelectedPrinter;
            }
            else if (PrinterComboBox.Items.Count > 0)
            {
                PrinterComboBox.SelectedIndex = 0;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateSizePreview();
        }

        private void PaperSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSizePreview();
        }

        private void UpdateSizePreview()
        {
            if (SizePreviewBorder == null) return;

            double widthMm = 80;
            double heightMm = 50;

            if (double.TryParse(PaperWidthTextBox.Text, out double width))
            {
                widthMm = width;
            }

            if (double.TryParse(PaperHeightTextBox.Text, out double height))
            {
                heightMm = height;
            }

            double scale = 2.0;
            SizePreviewBorder.Width = widthMm * scale;
            SizePreviewBorder.Height = heightMm * scale;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var settings = PrinterSettings.Instance;
            
            if (int.TryParse(PaperWidthTextBox.Text, out int width) && width >= 30 && width <= 150)
            {
                settings.PaperWidth = width;
            }
            else
            {
                MessageBox.Show("الرجاء إدخال عرض صحيح (30-150 مم)", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (int.TryParse(PaperHeightTextBox.Text, out int height) && height >= 20 && height <= 100)
            {
                settings.PaperHeight = height;
            }
            else
            {
                MessageBox.Show("الرجاء إدخال ارتفاع صحيح (20-100 مم)", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            settings.ThermalLevel = (int)ThermalLevelSlider.Value;
            settings.BarcodeQuality = (int)BarcodeQualitySlider.Value;
            settings.CustomNumber = CustomNumberTextBox.Text;
            
            if (PrinterComboBox.SelectedItem != null)
            {
                settings.SelectedPrinter = PrinterComboBox.SelectedItem.ToString() ?? "";
            }

            MessageBox.Show("تم حفظ الإعدادات بنجاح!", "نجح", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
