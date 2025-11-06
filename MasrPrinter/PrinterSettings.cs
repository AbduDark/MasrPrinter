namespace MasrPrinter
{
    public class PrinterSettings
    {
        private static PrinterSettings? _instance;
        
        public static PrinterSettings Instance
        {
            get
            {
                _instance ??= new PrinterSettings();
                return _instance;
            }
        }

        public int PaperWidth { get; set; } = 80;
        public int PaperHeight { get; set; } = 50;
        public int ThermalLevel { get; set; } = 50;
        public int BarcodeQuality { get; set; } = 300;
        public string SelectedPrinter { get; set; } = "";
        public string CustomNumber { get; set; } = "1";
    }
}
