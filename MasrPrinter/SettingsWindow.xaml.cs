using System.Windows;
using System.Windows.Controls;

namespace MasrPrinter
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            LoadSettings();
            UpdateSizePreview();
        }

        private void LoadSettings()
        {
            var settings = PrinterSettings.Instance;
            PaperWidthSlider.Value = settings.PaperWidth;
            PaperHeightSlider.Value = settings.PaperHeight;
            ThermalLevelSlider.Value = settings.ThermalLevel;
            BarcodeQualitySlider.Value = settings.BarcodeQuality;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateSizePreview();
        }

        private void UpdateSizePreview()
        {
            if (SizePreviewBorder == null) return;

            double widthMm = PaperWidthSlider.Value;
            double heightMm = PaperHeightSlider.Value;

            double scale = 2.0;
            SizePreviewBorder.Width = widthMm * scale;
            SizePreviewBorder.Height = heightMm * scale;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var settings = PrinterSettings.Instance;
            settings.PaperWidth = (int)PaperWidthSlider.Value;
            settings.PaperHeight = (int)PaperHeightSlider.Value;
            settings.ThermalLevel = (int)ThermalLevelSlider.Value;
            settings.BarcodeQuality = (int)BarcodeQualitySlider.Value;

            MessageBox.Show("تم حفظ الإعدادات بنجاح!", "نجح", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
