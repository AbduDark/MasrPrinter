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
- Paper width and height settings (30-150mm × 20-100mm)
- **Printer selection** from installed system printers
- Thermal level control (0-100)
- Barcode quality (150-600 DPI)
- **Custom default number** for printing
- **2x1 Mode**: Enable printing number above and barcode below on the same sheet
- **Barcode Positioning**: X/Y position control in millimeters
- **Font Sizes**: Separate sizes for Number and Hashtag (#)
- **Barcode Dimensions**: Custom height and width in millimeters
- **Bar Width Control**: Narrow bar width customization (Wide bar calculated automatically)
- Live size preview

## Output
All generated labels are saved to the `prints/` folder as PNG images.

## Recent Changes
- 2025-11-06: **Advanced Barcode Customization & 2x1 Layout Mode**
  - **NEW FEATURE**: 2x1 printing mode that places number on top and barcode below
  - Added precise barcode positioning control (X/Y coordinates in mm)
  - Customizable font sizes for numbers and hashtag symbols
  - Custom barcode dimensions (height/width in millimeters)
  - Narrow bar width control using BarcodeLib.BarWidth property
  - Wide bar width field preserved (calculated automatically by library per Code128 standard)
  - Enhanced Settings UI with all new controls in a dedicated "Barcode Specifications" section
  - Full validation for all new numeric inputs
  - Informative tooltips explaining library behavior for bar width ratios
  - Clean build with 0 errors, 0 warnings
  
- 2025-11-06: **Modern UI Redesign with Gradient Theme**
  - **COMPLETE UI OVERHAUL**: Redesigned all 4 windows with modern gradient-based color system
  - New purple/pink gradient theme (#6A1B9A, #8E24AA, #E91E63) instead of blue/orange
  - Enhanced visual hierarchy with card-based layouts and shadow effects
  - Updated all windows: MainWindow, CustomPrintWindow, BatchPrintWindow, SettingsWindow
  - Modern button styles with hover effects and gradient backgrounds
  - Improved iconography with emoji and Material Design-inspired elements
  - Professional Arabic typography with better spacing and readability
  
- 2025-11-06: **Enhanced BarcodeGenerator with High-Quality Printing**
  - **CRITICAL FIX**: Improved print quality with DPI-aware image generation
  - High-quality graphics rendering (SmoothingMode, InterpolationMode, PixelOffsetMode)
  - Proper DPI scaling from settings (150-600 DPI support)
  - Custom paper size support with mm to pixel conversion
  - Advanced error handling with Arabic error messages
  - Fixed nullable reference warnings (clean build: 0 errors, 0 warnings)
  - PNG encoder optimization for maximum quality (Quality=100)
  - Smart barcode layout with proper margins and spacing
  
- 2025-11-06: **Physical Printing and Enhanced Settings**
  - **MAJOR**: Added actual printing functionality to both Batch and Custom print windows
  - Printer selection from installed system printers
  - Paper size settings now use TextBox input instead of sliders
  - Custom paper dimensions applied to print jobs (mm to hundredths of inch conversion)
  - Custom default number field in settings
  - Print jobs are isolated per label (no race conditions)
  - Proper resource disposal after each print
  - Input validation for paper dimensions (width: 30-150mm, height: 20-100mm)
