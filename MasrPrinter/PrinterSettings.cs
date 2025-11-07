using System;
using System.IO;
using System.Text.Json;

namespace MasrPrinter
{
    public class PrinterSettings
    {
        private static PrinterSettings? _instance;
        private static readonly string SettingsFilePath = "printer_settings.json";
        
        public static PrinterSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = LoadFromFile() ?? new PrinterSettings();
                }
                return _instance;
            }
        }

        public int PaperWidth { get; set; } = 80;
        public int PaperHeight { get; set; } = 50;
        public int BarcodeQuality { get; set; } = 203;
        public string SelectedPrinter { get; set; } = "";
        public string SelectedPaperSizeName { get; set; } = "";
        public string CustomNumber { get; set; } = "1";

        public void SaveToFile()
        {
            try
            {
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                };
                string jsonString = JsonSerializer.Serialize(this, options);
                File.WriteAllText(SettingsFilePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"خطأ في حفظ الإعدادات: {ex.Message}");
            }
        }

        public static PrinterSettings? LoadFromFile()
        {
            try
            {
                if (!File.Exists(SettingsFilePath))
                    return null;

                string jsonString = File.ReadAllText(SettingsFilePath);
                return JsonSerializer.Deserialize<PrinterSettings>(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"خطأ في تحميل الإعدادات: {ex.Message}");
                return null;
            }
        }
    }
}
