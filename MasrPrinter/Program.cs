using System;

namespace MasrPrinter
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("MasrPrinter - WPF Application");
            Console.WriteLine("==============================================");
            Console.WriteLine();
            Console.WriteLine("NOTICE: This is a WPF (Windows Presentation Foundation) application.");
            Console.WriteLine("WPF requires Windows and the Microsoft.WindowsDesktop.App runtime.");
            Console.WriteLine();
            Console.WriteLine("Current Environment: " + Environment.OSVersion);
            Console.WriteLine("Runtime: .NET " + Environment.Version);
            Console.WriteLine();
            Console.WriteLine("This application cannot run on Linux/Replit because:");
            Console.WriteLine("  - WPF is Windows-only technology");
            Console.WriteLine("  - Microsoft.WindowsDesktop.App runtime is not available on Linux");
            Console.WriteLine();
            Console.WriteLine("To run this application:");
            Console.WriteLine("  1. Use a Windows machine with .NET 8.0 Desktop Runtime");
            Console.WriteLine("  2. Or convert to a cross-platform framework like Avalonia UI");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            
            if (Environment.UserInteractive)
            {
                Console.ReadKey();
            }
        }
    }
}
