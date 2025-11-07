using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using QRCoder;
using BarcodeLib;

namespace MasrPrinter
{
    public class BarcodeGenerator
    {
        public static System.Windows.Media.Imaging.BitmapImage GenerateBarcodeImage(string data, string type, int width = 400, int height = 100)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data))
                    throw new ArgumentException("البيانات لا يمكن أن تكون فارغة", nameof(data));

                Bitmap bitmap;

                if (type == "QR")
                    bitmap = GenerateQRCode(data);
                else
                    bitmap = GenerateBarcode(data, width, height);

                return ConvertBitmapToBitmapImage(bitmap);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في إنشاء الباركود: {ex.Message}", ex);
            }
        }

        private static Bitmap GenerateQRCode(string data, int pixelsPerModule = 20)
        {
            try
            {
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new PngByteQRCode(qrCodeData);
                byte[] qrBytes = qrCode.GetGraphic(pixelsPerModule);
                using var ms = new MemoryStream(qrBytes);
                return new Bitmap(ms);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في إنشاء QR Code: {ex.Message}", ex);
            }
        }

        private static Bitmap GenerateBarcode(string data, int width, int height)
        {
            try
            {
                var barcode = new Barcode
                {
                    IncludeLabel = false,
                    Alignment = BarcodeLib.AlignmentPositions.CENTER
                };

                var barcodeImage = barcode.Encode(BarcodeLib.TYPE.CODE128, data, Color.Black, Color.White, width, height);
                return new Bitmap(barcodeImage);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في إنشاء Code128: {ex.Message}", ex);
            }
        }

        public static void SaveLabel(string number, string barcodeType, string outputPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(number))
                    throw new ArgumentException("الرقم لا يمكن أن يكون فارغاً", nameof(number));

                var settings = PrinterSettings.Instance;
                int dpi = (int)settings.BarcodeQuality;

                int width = (int)(settings.PaperWidth * dpi / 25.4);
                int height = (int)(settings.PaperHeight * dpi / 25.4);
                width = Math.Max(width, 100);
                height = Math.Max(height, 100);

                using var labelImage = new Bitmap(width, height);
                labelImage.SetResolution(dpi, dpi);

                using var graphics = Graphics.FromImage(labelImage);
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                graphics.Clear(Color.White);

                int topMargin = (int)(height * 0.08f);
                int leftMargin = (int)(width * 0.05f);
                int rightMargin = (int)(width * 0.05f);
                
                int numberFontSize = (int)(Math.Min(width, height) / 3.5f);
                using var numberFont = new Font("Arial", numberFontSize, FontStyle.Bold, GraphicsUnit.Point);
                
                float numberX = leftMargin;
                float numberY = topMargin;
                graphics.DrawString(number, numberFont, Brushes.Black, numberX, numberY);
                
                int hashtagFontSize = (int)(numberFontSize * 0.9f);
                using var hashtagFont = new Font("Arial", hashtagFontSize, FontStyle.Bold, GraphicsUnit.Point);
                var hashtagSize = graphics.MeasureString("#", hashtagFont);
                float hashtagX = width - rightMargin - hashtagSize.Width;
                float hashtagY = topMargin;
                graphics.DrawString("#", hashtagFont, Brushes.Black, hashtagX, hashtagY);

                var numberSize = graphics.MeasureString(number, numberFont);
                int barcodeGap = (int)(height * 0.08f);
                int barcodeY = (int)(numberY + numberSize.Height + barcodeGap);
                
                int barcodeWidth, barcodeHeight;
                if (barcodeType == "QR")
                {
                    int availableSpace = Math.Min((int)(width * 0.85f), height - barcodeY - (int)(height * 0.05f));
                    barcodeWidth = barcodeHeight = availableSpace;
                }
                else
                {
                    barcodeWidth = (int)(width * 0.85f);
                    barcodeHeight = Math.Min((int)(height * 0.5f), height - barcodeY - (int)(height * 0.05f));
                }
                
                int barcodeX = (width - barcodeWidth) / 2;

                using var barcodeImage = GenerateBarcodeImageAsBitmap(number, barcodeType, barcodeWidth, barcodeHeight);
                graphics.DrawImage(barcodeImage, barcodeX, barcodeY, barcodeWidth, barcodeHeight);

                string? directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory))
                    Directory.CreateDirectory(directory);

                using var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                var pngEncoder = GetEncoder(ImageFormat.Png);

                if (pngEncoder != null)
                    labelImage.Save(outputPath, pngEncoder, encoderParams);
                else
                    labelImage.Save(outputPath, ImageFormat.Png);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في حفظ الملصق: {ex.Message}", ex);
            }
        }

        public static void SaveTextOnly(string text, string outputPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                    throw new ArgumentException("النص لا يمكن أن يكون فارغاً", nameof(text));

                var settings = PrinterSettings.Instance;
                int dpi = (int)settings.BarcodeQuality;

                int width = (int)(settings.PaperWidth * dpi / 25.4);
                int height = (int)(settings.PaperHeight * dpi / 25.4);
                width = Math.Max(width, 100);
                height = Math.Max(height, 100);

                using var labelImage = new Bitmap(width, height);
                labelImage.SetResolution(dpi, dpi);

                using var graphics = Graphics.FromImage(labelImage);
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                graphics.Clear(Color.White);

                int fontSize = (int)(Math.Min(width, height * 0.25f) / 3.5f);
                using var font = new Font("Tahoma", fontSize, FontStyle.Bold, GraphicsUnit.Point);
                var textSize = graphics.MeasureString(text, font);

                float textX = (width - textSize.Width) / 2;
                float textY = (height - textSize.Height) / 2;
                graphics.DrawString(text, font, Brushes.Black, textX, textY);

                string? directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory))
                    Directory.CreateDirectory(directory);

                using var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                var pngEncoder = GetEncoder(ImageFormat.Png);

                if (pngEncoder != null)
                    labelImage.Save(outputPath, pngEncoder, encoderParams);
                else
                    labelImage.Save(outputPath, ImageFormat.Png);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في حفظ النص: {ex.Message}", ex);
            }
        }

        private static Bitmap GenerateBarcodeImageAsBitmap(string data, string type, int width = 400, int height = 100)
        {
            try
            {
                if (type == "QR")
                {
                    int pixelsPerModule = Math.Max(10, Math.Min(width, height) / 30);
                    return GenerateQRCode(data, pixelsPerModule);
                }
                else
                {
                    return GenerateBarcode(data, width, height);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في إنشاء صورة الباركود: {ex.Message}", ex);
            }
        }

        private static System.Windows.Media.Imaging.BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            try
            {
                using var memory = new MemoryStream();
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في تحويل الصورة: {ex.Message}", ex);
            }
        }

        private static ImageCodecInfo? GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                    return codec;
            }
            return null;
        }
    }
}
