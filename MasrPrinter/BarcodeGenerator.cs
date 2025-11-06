using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRCoder;
using BarcodeLib;

namespace MasrPrinter
{
    public class BarcodeGenerator
    {
        public static System.Windows.Media.Imaging.BitmapImage GenerateBarcodeImage(string data, string type, int width = 400, int height = 100)
        {
            Bitmap bitmap;
            
            if (type == "QR")
            {
                bitmap = GenerateQRCode(data);
            }
            else
            {
                bitmap = GenerateBarcode(data, width, height);
            }

            return ConvertBitmapToBitmapImage(bitmap);
        }

        private static Bitmap GenerateQRCode(string data)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrBytes = qrCode.GetGraphic(20);
            using var ms = new MemoryStream(qrBytes);
            return new Bitmap(ms);
        }

        private static Bitmap GenerateBarcode(string data, int width, int height)
        {
            var barcode = new Barcode();
            var barcodeImage = barcode.Encode(BarcodeLib.TYPE.CODE128, data, Color.Black, Color.White, width, height);
            return new Bitmap(barcodeImage);
        }

        public static void SaveLabel(string number, string barcodeType, string outputPath)
        {
            var settings = PrinterSettings.Instance;
            var barcodeImage = GenerateBarcodeImageAsBitmap(number, barcodeType);

            int width = settings.PaperWidth * 4;
            int height = settings.PaperHeight * 4;

            using var labelImage = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(labelImage);

            graphics.Clear(Color.White);

            var font = new Font("Arial", 24, FontStyle.Bold);
            var textSize = graphics.MeasureString(number, font);
            graphics.DrawString(number, font, Brushes.Black, (width - textSize.Width) / 2, 10);

            int barcodeY = (int)textSize.Height + 20;
            int barcodeWidth = width - 40;
            int barcodeHeight = height - barcodeY - 20;

            graphics.DrawImage(barcodeImage, 20, barcodeY, barcodeWidth, barcodeHeight);

            Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? "prints");
            labelImage.Save(outputPath, ImageFormat.Png);
        }

        public static void SaveTextOnly(string number, string outputPath)
        {
            var settings = PrinterSettings.Instance;
            int width = settings.PaperWidth * 4;
            int height = settings.PaperHeight * 4;

            using var labelImage = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(labelImage);

            graphics.Clear(Color.White);

            var font = new Font("Arial", 36, FontStyle.Bold);
            var textSize = graphics.MeasureString(number, font);
            graphics.DrawString(number, font, Brushes.Black,
                (width - textSize.Width) / 2,
                (height - textSize.Height) / 2);

            Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? "prints");
            labelImage.Save(outputPath, ImageFormat.Png);
        }

        private static Bitmap GenerateBarcodeImageAsBitmap(string data, string type)
        {
            if (type == "QR")
            {
                return GenerateQRCode(data);
            }
            else
            {
                return GenerateBarcode(data, 400, 100);
            }
        }

        private static System.Windows.Media.Imaging.BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
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
    }
}
