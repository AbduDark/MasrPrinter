
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using QRCoder;
using BarcodeLib;
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Avalonia.Threading;

namespace MasrPrinterAvalonia
{
    public partial class MainWindow : Window
    {
        private PrinterSettings _settings;

        public MainWindow()
        {
            InitializeComponent();
            _settings = new PrinterSettings();
            LoadSettings();
        }

        private void BtnBatchPrint_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtStartNumber.Text, out int start) || 
                    !int.TryParse(txtEndNumber.Text, out int end))
                {
                    txtBatchStatus.Text = "خطأ: أدخل أرقام صحيحة";
                    return;
                }

                if (start > end)
                {
                    txtBatchStatus.Text = "خطأ: رقم البداية أكبر من رقم النهاية";
                    return;
                }

                string barcodeType = cboBarcodeType.SelectedIndex == 0 ? "Code128" : "QR";
                
                for (int i = start; i <= end; i++)
                {
                    PrintLabel(i.ToString(), barcodeType);
                }

                txtBatchStatus.Text = $"تم طباعة {end - start + 1} ملصق بنجاح!";
            }
            catch (Exception ex)
            {
                txtBatchStatus.Text = $"خطأ: {ex.Message}";
            }
        }

        private void BtnSinglePrint_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                string number = txtSingleNumber.Text;
                bool includeBarcode = chkIncludeBarcode.IsChecked ?? true;
                string barcodeType = cboSingleBarcodeType.SelectedIndex == 0 ? "Code128" : "QR";

                if (includeBarcode)
                {
                    // Generate preview
                    var preview = GenerateBarcodeImage(number, barcodeType);
                    imgPreview.Source = ConvertToAvaloniaBitmap(preview);
                    
                    PrintLabel(number, barcodeType);
                }
                else
                {
                    PrintTextOnly(number);
                }
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }

        private void BtnSaveSettings_Click(object? sender, RoutedEventArgs e)
        {
            _settings.PaperWidth = (int)(numPaperWidth.Value ?? 80);
            _settings.PaperHeight = (int)(numPaperHeight.Value ?? 50);
            _settings.ThermalLevel = (int)(numThermalLevel.Value ?? 50);
            _settings.IsThermalPrinter = chkThermalPrinter.IsChecked ?? true;
            
            SaveSettings();
            txtSettingsStatus.Text = "تم حفظ الإعدادات بنجاح!";
        }

        private Bitmap GenerateBarcodeImage(string data, string type)
        {
            if (type == "QR")
            {
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new QRCode(qrCodeData);
                return qrCode.GetGraphic(20);
            }
            else // Code128
            {
                var barcode = new Barcode();
                return barcode.Encode(BarcodeLib.TYPE.CODE128, data, 
                    System.Drawing.Color.Black, System.Drawing.Color.White, 
                    400, 100);
            }
        }

        private Avalonia.Media.Imaging.Bitmap ConvertToAvaloniaBitmap(Bitmap bitmap)
        {
            using var memory = new MemoryStream();
            bitmap.Save(memory, ImageFormat.Png);
            memory.Position = 0;
            return new Avalonia.Media.Imaging.Bitmap(memory);
        }

        private void PrintLabel(string number, string barcodeType)
        {
            var barcodeImage = GenerateBarcodeImage(number, barcodeType);
            
            // Create label with number and barcode
            using var labelImage = new Bitmap(_settings.PaperWidth * 4, _settings.PaperHeight * 4);
            using var graphics = Graphics.FromImage(labelImage);
            
            graphics.Clear(System.Drawing.Color.White);
            
            // Draw number
            var font = new Font("Arial", 24, FontStyle.Bold);
            var textSize = graphics.MeasureString(number, font);
            graphics.DrawString(number, font, Brushes.Black, 
                (labelImage.Width - textSize.Width) / 2, 10);
            
            // Draw barcode
            int barcodeY = (int)textSize.Height + 20;
            int barcodeWidth = labelImage.Width - 40;
            int barcodeHeight = labelImage.Height - barcodeY - 20;
            
            graphics.DrawImage(barcodeImage, 20, barcodeY, barcodeWidth, barcodeHeight);
            
            // Save to file (in real implementation, send to printer)
            string outputDir = "prints";
            Directory.CreateDirectory(outputDir);
            labelImage.Save($"{outputDir}/label_{number}.png", ImageFormat.Png);
            
            Console.WriteLine($"طباعة الملصق رقم: {number}");
        }

        private void PrintTextOnly(string number)
        {
            using var labelImage = new Bitmap(_settings.PaperWidth * 4, _settings.PaperHeight * 4);
            using var graphics = Graphics.FromImage(labelImage);
            
            graphics.Clear(System.Drawing.Color.White);
            
            var font = new Font("Arial", 36, FontStyle.Bold);
            var textSize = graphics.MeasureString(number, font);
            graphics.DrawString(number, font, Brushes.Black, 
                (labelImage.Width - textSize.Width) / 2, 
                (labelImage.Height - textSize.Height) / 2);
            
            string outputDir = "prints";
            Directory.CreateDirectory(outputDir);
            labelImage.Save($"{outputDir}/text_{number}.png", ImageFormat.Png);
            
            Console.WriteLine($"طباعة النص: {number}");
        }

        private void LoadSettings()
        {
            numPaperWidth.Value = _settings.PaperWidth;
            numPaperHeight.Value = _settings.PaperHeight;
            numThermalLevel.Value = _settings.ThermalLevel;
            chkThermalPrinter.IsChecked = _settings.IsThermalPrinter;
        }

        private void SaveSettings()
        {
            // In real implementation, save to config file
            Console.WriteLine("تم حفظ الإعدادات");
        }
    }

    public class PrinterSettings
    {
        public int PaperWidth { get; set; } = 80;
        public int PaperHeight { get; set; } = 50;
        public int ThermalLevel { get; set; } = 50;
        public bool IsThermalPrinter { get; set; } = true;
    }
}
