
using System;
using System.IO;
using QRCoder;
using BarcodeLib;
using System.Drawing;
using System.Drawing.Imaging;

namespace MasrPrinter
{
    class Program
    {
        static PrinterSettings settings = new PrinterSettings();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            while (true)
            {
                Console.Clear();
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("       برنامج طباعة الباركود");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine();
                Console.WriteLine("1. طباعة متسلسلة");
                Console.WriteLine("2. طباعة مخصصة");
                Console.WriteLine("3. الإعدادات");
                Console.WriteLine("4. خروج");
                Console.WriteLine();
                Console.Write("اختر (1-4): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        BatchPrint();
                        break;
                    case "2":
                        SinglePrint();
                        break;
                    case "3":
                        Settings();
                        break;
                    case "4":
                        return;
                }
            }
        }

        static void BatchPrint()
        {
            Console.Clear();
            Console.WriteLine("═══ طباعة متسلسلة ═══");
            Console.WriteLine();

            Console.Write("من رقم: ");
            int start = int.Parse(Console.ReadLine() ?? "1");

            Console.Write("إلى رقم: ");
            int end = int.Parse(Console.ReadLine() ?? "50");

            Console.Write("نوع الباركود (1=Code128, 2=QR): ");
            string type = Console.ReadLine() == "2" ? "QR" : "Code128";

            Directory.CreateDirectory("prints");

            Console.WriteLine();
            Console.WriteLine("جاري الطباعة...");

            for (int i = start; i <= end; i++)
            {
                PrintLabel(i.ToString(), type);
                Console.Write($"\rتم طباعة {i - start + 1}/{end - start + 1}");
            }

            Console.WriteLine();
            Console.WriteLine($"\n✓ تم طباعة {end - start + 1} ملصق في مجلد prints/");
            Console.WriteLine("\nاضغط أي زر للعودة...");
            Console.ReadKey();
        }

        static void SinglePrint()
        {
            Console.Clear();
            Console.WriteLine("═══ طباعة مخصصة ═══");
            Console.WriteLine();

            Console.Write("الرقم: ");
            string number = Console.ReadLine() ?? "1";

            Console.Write("طباعة مع باركود؟ (y/n): ");
            bool withBarcode = Console.ReadLine()?.ToLower() == "y";

            Directory.CreateDirectory("prints");

            if (withBarcode)
            {
                Console.Write("نوع الباركود (1=Code128, 2=QR): ");
                string type = Console.ReadLine() == "2" ? "QR" : "Code128";
                PrintLabel(number, type);
                Console.WriteLine($"\n✓ تم طباعة الملصق: prints/label_{number}.png");
            }
            else
            {
                PrintTextOnly(number);
                Console.WriteLine($"\n✓ تم طباعة النص: prints/text_{number}.png");
            }

            Console.WriteLine("\nاضغط أي زر للعودة...");
            Console.ReadKey();
        }

        static void Settings()
        {
            Console.Clear();
            Console.WriteLine("═══ الإعدادات ═══");
            Console.WriteLine();

            Console.Write($"عرض الورقة بالمم [{settings.PaperWidth}]: ");
            string width = Console.ReadLine();
            if (!string.IsNullOrEmpty(width)) settings.PaperWidth = int.Parse(width);

            Console.Write($"ارتفاع الورقة بالمم [{settings.PaperHeight}]: ");
            string height = Console.ReadLine();
            if (!string.IsNullOrEmpty(height)) settings.PaperHeight = int.Parse(height);

            Console.Write($"الرقم الحراري (0-100) [{settings.ThermalLevel}]: ");
            string thermal = Console.ReadLine();
            if (!string.IsNullOrEmpty(thermal)) settings.ThermalLevel = int.Parse(thermal);

            Console.WriteLine("\n✓ تم حفظ الإعدادات");
            Console.WriteLine("\nاضغط أي زر للعودة...");
            Console.ReadKey();
        }

        static void PrintLabel(string number, string barcodeType)
        {
            var barcodeImage = GenerateBarcodeImage(number, barcodeType);

            int width = settings.PaperWidth * 4;
            int height = settings.PaperHeight * 4;

            using var labelImage = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(labelImage);

            graphics.Clear(Color.White);

            var font = new Font("Arial", 24, FontStyle.Bold);
            var textSize = graphics.MeasureString(number, font);
            graphics.DrawString(number, font, Brushes.Black,
                (width - textSize.Width) / 2, 10);

            int barcodeY = (int)textSize.Height + 20;
            int barcodeWidth = width - 40;
            int barcodeHeight = height - barcodeY - 20;

            graphics.DrawImage(barcodeImage, 20, barcodeY, barcodeWidth, barcodeHeight);

            labelImage.Save($"prints/label_{number}.png", ImageFormat.Png);
        }

        static void PrintTextOnly(string number)
        {
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

            labelImage.Save($"prints/text_{number}.png", ImageFormat.Png);
        }

        static Bitmap GenerateBarcodeImage(string data, string type)
        {
            if (type == "QR")
            {
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new QRCode(qrCodeData);
                return qrCode.GetGraphic(20);
            }
            else
            {
                var barcode = new Barcode();
                return barcode.Encode(BarcodeLib.TYPE.CODE128, data,
                    Color.Black, Color.White, 400, 100);
            }
        }
    }

    class PrinterSettings
    {
        public int PaperWidth { get; set; } = 80;
        public int PaperHeight { get; set; } = 50;
        public int ThermalLevel { get; set; } = 50;
    }
}
