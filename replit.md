# MasrPrinter - برنامج طباعة الباركود

## Overview
MasrPrinter is a professional .NET 8.0 WPF (Windows Presentation Foundation) desktop application for printing barcodes and QR codes.

**IMPORTANT**: WPF is a Windows-only framework. This application builds successfully on Replit but must be run on Windows.

## Project Structure
- **MasrPrinter/** - Main project directory
  - `App.xaml` - Application resources with professional Arabic themes
  - `MainWindow.xaml` - Main window with RTL support and modern UI
  - `BatchPrintWindow.xaml` - Batch printing with live preview
  - `CustomPrintWindow.xaml` - Custom single label printing
  - `SettingsWindow.xaml` - Printer and paper settings
  - `BarcodeGenerator.cs` - Barcode and QR code generation logic
  - `PrinterSettings.cs` - Settings management
  - `MasrPrinter.csproj` - .NET 8.0 WPF project file
- **MasrPrinter.sln** - Visual Studio solution file

## Technology Stack
- .NET 8.0 (net8.0-windows)
- WPF (Windows Presentation Foundation)
- C# with modern UI patterns
- QRCoder library for QR code generation
- BarcodeLib for Code128 barcode generation
- System.Drawing.Common for image processing

## Features
- ✨ **Professional Arabic Interface**: Full RTL support with modern Material Design-inspired UI
- 📦 **Batch Printing**: Generate multiple sequential labels with live preview
- 🎨 **Custom Printing**: Create single labels with custom text/numbers
- ⚙️ **Advanced Settings**: 
  - Configure paper size, thermal level, and barcode quality
  - Select from installed system printers
  - Set custom default number for printing
- 🔍 **Live Preview**: See barcodes before printing
- 💾 **PNG Export**: Save all labels as high-quality PNG images
- 📊 **Dual Barcode Support**: Code128 (linear) and QR Code (2D)
- 🖨️ **Printer Integration**: Automatic detection and selection of system printers

## Running the Application

### On Windows
1. Ensure .NET 8.0 Desktop Runtime is installed
2. Clone or download the project
3. Open `MasrPrinter.sln` in Visual Studio, or
4. Run from command line:
   ```bash
   cd MasrPrinter
   dotnet run
   ```

### On Replit (Build Only)
The project builds successfully on Replit:
```bash
cd MasrPrinter
dotnet build
```
However, it cannot be executed due to WPF's Windows-only requirement.

## User Interface

### Main Window
- **Batch Print**: Opens dialog for printing multiple sequential labels
- **Custom Print**: Create a single label with custom content
- **Settings**: Configure printer and paper settings
- **Info**: Application version and usage tips

### Batch Print Window
- Set start and end numbers for sequential printing
- Choose between Code128 and QR Code
- Live preview of sample label
- Progress bar during batch generation

### Custom Print Window
- Enter custom text or numbers
- Optional barcode generation
- Choose barcode type
- Live preview with automatic updates

### Settings Window
- Paper width and height sliders (30-150mm × 20-100mm)
- **Printer selection** from installed system printers
- Thermal level control (0-100)
- Barcode quality (150-600 DPI)
- **Custom default number** for printing
- Live size preview

## Output
All generated labels are saved to the `prints/` folder as PNG images.

## Recent Changes
- 2025-11-06: **Physical Printing and Enhanced Settings**
  - **MAJOR**: Added actual printing functionality to both Batch and Custom print windows
  - Printer selection from installed system printers
  - Paper size settings now use TextBox input instead of sliders
  - Custom paper dimensions applied to print jobs (mm to hundredths of inch conversion)
  - Custom default number field in settings
  - Print jobs are isolated per label (no race conditions)
  - Proper resource disposal after each print
  - Input validation for paper dimensions (width: 30-150mm, height: 20-100mm)
  
- 2025-11-06: **Enhanced Settings and Printer Integration**
  - Added printer selection from installed system printers
  - Added custom default number field in settings
  - Integrated with Windows printer system (System.Drawing.Printing)
  - Custom number auto-populates in Custom Print window
  - Improved user experience with printer detection
  
- 2025-11-06: Complete WPF redesign
  - Removed console fallback, full WPF on net8.0-windows
  - Created professional Arabic UI with RTL support
  - Added 3 specialized windows (Batch, Custom, Settings)
  - Implemented live preview system
  - Added Material Design-inspired theming
  - Integrated QRCoder and BarcodeLib libraries
  - Built complete barcode generation system
  - Project builds cleanly with 0 errors, 0 warnings
