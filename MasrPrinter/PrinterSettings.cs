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
        public int TextSize { get; set; } = 20;
        public int BarcodeSize { get; set; } = 100;
        
        public bool Enable2x1Mode { get; set; } = false;
        public int BarcodePositionX { get; set; } = 31;
        public int BarcodePositionY { get; set; } = 7;
        public int NumberFontSize { get; set; } = 47;
        public int HashtagFontSize { get; set; } = 35;
        public int BarcodeHeightMM { get; set; } = 2;
        public int BarcodeWidthMM { get; set; } = 4;
        public int NarrowBarWidth { get; set; } = 2;
        public int WideBarWidth { get; set; } = 4;
    }
}
