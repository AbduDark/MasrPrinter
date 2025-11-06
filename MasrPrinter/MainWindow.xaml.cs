using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Media.Imaging;
using System.Linq;

namespace MasrPrinter
{
    public partial class MainWindow : Window
    {
        private string currentBarcodeType = "Code128";
        private bool isBatchMode = true;

        public MainWindow()
        {
            InitializeComponent();
            LoadPrinters();
            LoadSettings();
            StartNumberTextBox.Text = "1";
            EndNumberTextBox.Text = "10";
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
                    PrinterComboBox.Items.Add("لا توجد طابعات متاحة");
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
            BarcodeQualitySlider.Value = settings.BarcodeQuality;
            ThermalLevelSlider.Value = settings.ThermalLevel;
            TextSizeTextBox.Text = settings.TextSize.ToString();
            BarcodeSizeTextBox.Text = settings.BarcodeSize.ToString();
            
            Enable2x1CheckBox.IsChecked = settings.Enable2x1Mode;
            BarcodePositionXTextBox.Text = settings.BarcodePositionX.ToString();
            BarcodePositionYTextBox.Text = settings.BarcodePositionY.ToString();
            NumberFontSizeTextBox.Text = settings.NumberFontSize.ToString();
            HashtagFontSizeTextBox.Text = settings.HashtagFontSize.ToString();
            BarcodeHeightTextBox.Text = settings.BarcodeHeightMM.ToString();
            BarcodeWidthTextBox.Text = settings.BarcodeWidthMM.ToString();
            NarrowBarWidthTextBox.Text = settings.NarrowBarWidth.ToString();
            WideBarWidthTextBox.Text = settings.WideBarWidth.ToString();
            
            CustomTextBox.Text = settings.CustomNumber;
            
            if (!string.IsNullOrEmpty(settings.SelectedPrinter) && PrinterComboBox.Items.Contains(settings.SelectedPrinter))
            {
                PrinterComboBox.SelectedItem = settings.SelectedPrinter;
            }
            else if (PrinterComboBox.Items.Count > 0)
            {
                PrinterComboBox.SelectedIndex = 0;
            }
        }

        private void PrintMode_Changed(object sender, RoutedEventArgs e)
        {
            if (BatchModeRadio == null || CustomModeRadio == null) return;

            isBatchMode = BatchModeRadio.IsChecked == true;
            BatchPrintPanel.Visibility = isBatchMode ? Visibility.Visible : Visibility.Collapsed;
            CustomPrintPanel.Visibility = isBatchMode ? Visibility.Collapsed : Visibility.Visible;
            
            UpdatePreview();
        }

        private void PaperSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePreview();
        }

        private void CustomText_Changed(object sender, TextChangedEventArgs e)
        {
            UpdatePreview();
        }

        private void CustomBarcodeType_Changed(object sender, SelectionChangedEventArgs e)
        {
            UpdatePreview();
        }

        private void WithBarcode_Changed(object sender, RoutedEventArgs e)
        {
            if (CustomBarcodeTypeComboBox != null)
            {
                bool isChecked = WithBarcodeCheckBox.IsChecked == true;
                CustomBarcodeTypeComboBox.IsEnabled = isChecked;
            }
            UpdatePreview();
        }

        private void UpdatePreview_Click(object sender, RoutedEventArgs e)
        {
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            try
            {
                if (PreviewImage == null || PreviewPlaceholder == null) return;

                if (isBatchMode)
                {
                    if (!int.TryParse(StartNumberTextBox.Text, out int start))
                    {
                        ShowPlaceholder("أدخل رقم البداية");
                        return;
                    }

                    currentBarcodeType = BatchBarcodeTypeComboBox.SelectedIndex == 1 ? "QR" : "Code128";
                    var barcodeImage = BarcodeGenerator.GenerateBarcodeImage(start.ToString(), currentBarcodeType);
                    PreviewImage.Source = barcodeImage;
                    PreviewPlaceholder.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(CustomTextBox.Text))
                    {
                        ShowPlaceholder("أدخل النص المخصص");
                        return;
                    }

                    bool withBarcode = WithBarcodeCheckBox.IsChecked == true;
                    currentBarcodeType = CustomBarcodeTypeComboBox.SelectedIndex == 1 ? "QR" : "Code128";

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
            }
            catch (Exception ex)
            {
                ShowPlaceholder($"خطأ: {ex.Message}");
            }
        }

        private void ShowPlaceholder(string message)
        {
            if (PreviewImage != null && PreviewPlaceholder != null)
            {
                PreviewImage.Source = null;
                PreviewPlaceholder.Text = message;
                PreviewPlaceholder.Visibility = Visibility.Visible;
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

            int fontSize = settings.NumberFontSize;
            using var font = new System.Drawing.Font("Tahoma", fontSize, System.Drawing.FontStyle.Bold, GraphicsUnit.Point);
            var textSize = g.MeasureString(text, font);
            float textX = (width - textSize.Width) / 2;
            float textY = (height - textSize.Height) / 2;

            g.DrawString(text, font, System.Drawing.Brushes.Black, textX, textY);

            using var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var settings = PrinterSettings.Instance;
                
                if (int.TryParse(PaperWidthTextBox.Text, out int width) && width > 0)
                {
                    settings.PaperWidth = width;
                }
                else
                {
                    MessageBox.Show("الرجاء إدخال عرض صحيح (أكبر من 0 مم)", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (int.TryParse(PaperHeightTextBox.Text, out int height) && height > 0)
                {
                    settings.PaperHeight = height;
                }
                else
                {
                    MessageBox.Show("الرجاء إدخال ارتفاع صحيح (أكبر من 0 مم)", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                settings.BarcodeQuality = (int)BarcodeQualitySlider.Value;
                settings.ThermalLevel = (int)ThermalLevelSlider.Value;
                
                if (int.TryParse(TextSizeTextBox.Text, out int textSize))
                    settings.TextSize = textSize;
                
                if (int.TryParse(BarcodeSizeTextBox.Text, out int barcodeSize))
                    settings.BarcodeSize = barcodeSize;
                
                settings.Enable2x1Mode = Enable2x1CheckBox.IsChecked == true;
                
                if (int.TryParse(BarcodePositionXTextBox.Text, out int posX))
                    settings.BarcodePositionX = posX;
                
                if (int.TryParse(BarcodePositionYTextBox.Text, out int posY))
                    settings.BarcodePositionY = posY;
                
                if (int.TryParse(NumberFontSizeTextBox.Text, out int numSize))
                    settings.NumberFontSize = numSize;
                
                if (int.TryParse(HashtagFontSizeTextBox.Text, out int hashSize))
                    settings.HashtagFontSize = hashSize;
                
                if (int.TryParse(BarcodeHeightTextBox.Text, out int bHeight))
                    settings.BarcodeHeightMM = bHeight;
                
                if (int.TryParse(BarcodeWidthTextBox.Text, out int bWidth))
                    settings.BarcodeWidthMM = bWidth;
                
                if (int.TryParse(NarrowBarWidthTextBox.Text, out int narrowWidth))
                    settings.NarrowBarWidth = narrowWidth;
                
                if (int.TryParse(WideBarWidthTextBox.Text, out int wideWidth))
                    settings.WideBarWidth = wideWidth;

                if (PrinterComboBox.SelectedItem != null)
                {
                    settings.SelectedPrinter = PrinterComboBox.SelectedItem.ToString() ?? "";
                }

                settings.CustomNumber = CustomTextBox.Text;

                MessageBox.Show("تم حفظ الإعدادات بنجاح!", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في حفظ الإعدادات: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SavePNG_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Directory.CreateDirectory("prints");

                if (isBatchMode)
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

                    currentBarcodeType = BatchBarcodeTypeComboBox.SelectedIndex == 1 ? "QR" : "Code128";

                    for (int i = start; i <= end; i++)
                    {
                        string fileName = Path.Combine("prints", $"label_{i}.png");
                        BarcodeGenerator.SaveLabel(i.ToString(), currentBarcodeType, fileName);
                    }

                    MessageBox.Show($"تم حفظ {end - start + 1} ملف PNG بنجاح في مجلد prints!", 
                        "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(CustomTextBox.Text))
                    {
                        MessageBox.Show("الرجاء إدخال نص", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    bool withBarcode = WithBarcodeCheckBox.IsChecked == true;
                    currentBarcodeType = CustomBarcodeTypeComboBox.SelectedIndex == 1 ? "QR" : "Code128";
                    
                    string fileName = Path.Combine("prints", $"custom_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    
                    if (withBarcode)
                    {
                        BarcodeGenerator.SaveLabel(CustomTextBox.Text, currentBarcodeType, fileName);
                    }
                    else
                    {
                        SaveTextOnlyLabel(CustomTextBox.Text, fileName);
                    }

                    MessageBox.Show($"تم حفظ الملف بنجاح في:\n{fileName}", 
                        "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في حفظ PNG: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveTextOnlyLabel(string text, string fileName)
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

            int fontSize = settings.NumberFontSize;
            using var font = new System.Drawing.Font("Tahoma", fontSize, System.Drawing.FontStyle.Bold, GraphicsUnit.Point);
            var textSize = g.MeasureString(text, font);
            float textX = (width - textSize.Width) / 2;
            float textY = (height - textSize.Height) / 2;

            g.DrawString(text, font, System.Drawing.Brushes.Black, textX, textY);

            bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
        }

        private async void Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Directory.CreateDirectory("prints");

                if (isBatchMode)
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

                    currentBarcodeType = BatchBarcodeTypeComboBox.SelectedIndex == 1 ? "QR" : "Code128";

                    PrintProgressBar.Maximum = end - start + 1;
                    PrintProgressBar.Value = 0;

                    for (int i = start; i <= end; i++)
                    {
                        string fileName = Path.Combine("prints", $"label_{i}.png");
                        BarcodeGenerator.SaveLabel(i.ToString(), currentBarcodeType, fileName);
                        
                        PrintLabel(fileName);
                        
                        PrintProgressBar.Value = i - start + 1;
                        ProgressText.Text = $"جاري الطباعة... {PrintProgressBar.Value} من {PrintProgressBar.Maximum}";
                        
                        await System.Threading.Tasks.Task.Delay(500);
                    }

                    MessageBox.Show($"تم طباعة {end - start + 1} ملصق بنجاح!", 
                        "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    ProgressText.Text = "اكتملت الطباعة بنجاح! ✓";
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(CustomTextBox.Text))
                    {
                        MessageBox.Show("الرجاء إدخال نص", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    bool withBarcode = WithBarcodeCheckBox.IsChecked == true;
                    currentBarcodeType = CustomBarcodeTypeComboBox.SelectedIndex == 1 ? "QR" : "Code128";
                    
                    string fileName = Path.Combine("prints", $"custom_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    
                    if (withBarcode)
                    {
                        BarcodeGenerator.SaveLabel(CustomTextBox.Text, currentBarcodeType, fileName);
                    }
                    else
                    {
                        SaveTextOnlyLabel(CustomTextBox.Text, fileName);
                    }

                    PrintLabel(fileName);

                    MessageBox.Show("تمت الطباعة بنجاح!", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                    ProgressText.Text = "اكتملت الطباعة! ✓";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في الطباعة: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                ProgressText.Text = "فشلت الطباعة";
            }
        }

        private void PrintLabel(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return;
                
                var printDoc = new PrintDocument();
                var settings = PrinterSettings.Instance;
                
                if (!string.IsNullOrEmpty(settings.SelectedPrinter))
                {
                    printDoc.PrinterSettings.PrinterName = settings.SelectedPrinter;
                }
                
                int widthInHundredthsOfInch = (int)(settings.PaperWidth / 25.4 * 100);
                int heightInHundredthsOfInch = (int)(settings.PaperHeight / 25.4 * 100);
                printDoc.DefaultPageSettings.PaperSize = new PaperSize("Custom", widthInHundredthsOfInch, heightInHundredthsOfInch);
                
                printDoc.PrintPage += (sender, e) =>
                {
                    if (e.Graphics != null)
                    {
                        using var img = System.Drawing.Image.FromFile(filePath);
                        
                        float paperWidthInPixels = widthInHundredthsOfInch / 100.0f * e.Graphics.DpiX;
                        float paperHeightInPixels = heightInHundredthsOfInch / 100.0f * e.Graphics.DpiY;
                        
                        e.Graphics.DrawImage(img, 0, 0, paperWidthInPixels, paperHeightInPixels);
                    }
                };
                
                printDoc.Print();
                printDoc.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في طباعة الملف {filePath}: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
