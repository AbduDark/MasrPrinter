# MasrPrinter - برنامج طباعة الباركود

## Overview
MasrPrinter is a professional .NET 8.0 WPF (Windows Presentation Foundation) desktop application for printing barcodes and QR codes.

**IMPORTANT**: WPF is a Windows-only framework. This application builds successfully on Replit but must be run on Windows.

## Project Structure
- **MasrPrinter/** - Main project directory
  - `App.xaml` - Application resources with clean, modern styling
  - `MainWindow.xaml` - Single-page unified interface with RTL support
  - `MainWindow.xaml.cs` - All application logic (settings, batch print, custom print)
  - `BarcodeGenerator.cs` - Barcode and QR code generation logic
  - `PrinterSettings.cs` - Settings management (Singleton pattern)
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

**SIMPLIFIED SINGLE-PAGE DESIGN:**
All features are now consolidated into one lightweight, simple page for maximum efficiency.

### Main Window (All-in-One Interface)
The unified interface contains:

**⚙️ Settings Section (Collapsible):**
- Printer selection from installed system printers
- Paper dimensions (width: 30-150mm, height: 20-100mm)
- Barcode quality (DPI: 150-600)
- Thermal level control (0-100)
- Text size configuration
- Barcode size configuration
- 2x1 mode toggle (number above, barcode below)
- Barcode positioning (X/Y in millimeters)
- Barcode dimensions (height/width in millimeters)
- Font sizes (Number and Hashtag)
- Bar width control (Narrow and Wide)

**📋 Print Mode Selection:**
- Radio buttons to switch between Batch Print and Custom Print
- Batch Mode: Start/End numbers + barcode type
- Custom Mode: Custom text + optional barcode + type selection

**👁️ Live Preview:**
- Real-time preview of generated label
- Updates automatically as you type
- Shows exactly what will be printed

**Action Buttons:**
- 💾 Save Settings: Persist all configuration
- 📥 Save PNG: Export labels as PNG files
- 🖨️ Print: Send directly to printer

**Progress Bar:**
- Real-time printing progress for batch jobs
- Status messages during operations

## Output
All generated labels are saved to the `prints/` folder as PNG images.

## Recent Changes
- 2025-11-07: **Auto Paper Size Detection & Persistent Settings**
  - **NEW FEATURE**: Automatic detection of paper sizes from selected printer (XPRINTER XP-370B compatible)
  - Added PaperSizeComboBox to select from available paper sizes automatically
  - "مقاس مخصص" (Custom Size) option for manual input
  - Persistent settings storage using JSON file (printer_settings.json)
  - All settings automatically saved and restored on app restart
  - Fixed printing issue: pages now print correctly with full content (number + barcode)
  - Improved DPI calculation for proper print scaling
  - Clean build: 0 Errors, 0 Warnings
  
- 2025-11-06: **COMPLETE UI REDESIGN - Single Page Simplicity**
  - **REVOLUTIONARY CHANGE**: Consolidated all 4 windows into ONE simple, lightweight page
  - Removed separate windows: SettingsWindow, BatchPrintWindow, CustomPrintWindow
  - All features now accessible from single unified interface
  - Collapsible settings section for clean, uncluttered UI
  - Radio button mode switching (Batch/Custom) instead of separate windows
  - Integrated live preview directly in main window
  - Progress bar shows real-time status
  - Simplified workflow: No more window switching or navigation
  - Clean build: 0 Errors, 0 Warnings
  - **Result**: Faster workflow, simpler UX, all features in one place
  
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
