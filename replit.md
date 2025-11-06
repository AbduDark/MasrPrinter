# MasrPrinter

## Overview
MasrPrinter is a .NET 8.0 WPF (Windows Presentation Foundation) desktop application. 

**IMPORTANT LIMITATION**: WPF is a Windows-only framework that requires the Microsoft.WindowsDesktop.App runtime, which is not available on Linux systems. This application cannot run natively on Replit's Linux-based environment.

## Project Structure
- **MasrPrinter/** - Main project directory containing the WPF application
  - `MainWindow.xaml` - Main window XAML layout
  - `MainWindow.xaml.cs` - Main window code-behind
  - `App.xaml` - Application-level XAML resources
  - `App.xaml.cs` - Application startup logic
  - `MasrPrinter.csproj` - Project file for .NET 8.0
- **MasrPrinter.sln** - Visual Studio solution file

## Technology Stack
- .NET 8.0
- WPF (Windows Presentation Foundation) - Windows-only framework
- C#

## Current Status
The project has been successfully configured for Replit with a dual-mode setup:

- **On Linux/Replit**: Runs as a console application that displays information about the WPF limitation
- **On Windows**: Can run as the full WPF GUI application

The project file uses conditional compilation to target:
- `net8.0` (console) on Linux
- `net8.0-windows` (WPF GUI) on Windows

The build completes successfully and the console version runs on Replit, clearly explaining that the full WPF GUI requires a Windows environment.

## Possible Solutions
1. **Run on Windows**: This application needs to be run on a Windows machine with .NET 8.0 Desktop Runtime installed
2. **Convert to Cross-Platform**: Port the application to Avalonia UI (WPF-like framework that works on Linux, macOS, and Windows)
3. **Convert to Web App**: Rebuild as an ASP.NET Core web application
4. **Convert to Console**: If the functionality doesn't require GUI, convert to a console application

## Files Added for Replit
- `Program.cs` - Console entry point that runs on Linux and displays WPF limitation information

## Recent Changes
- 2025-11-06: Initial Replit setup and WPF compatibility solution
  - Installed .NET 8.0 SDK
  - Modified project file to support dual-mode compilation (console on Linux, WPF on Windows)
  - Created `Program.cs` as console entry point for Linux environments
  - Configured console workflow that successfully runs and displays information
  - Updated .gitignore with Replit-specific entries
  - Created comprehensive README.md explaining WPF limitations and alternatives
