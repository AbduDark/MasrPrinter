# MasrPrinter

A .NET 8.0 WPF desktop application for Windows.

## Important Note for Replit Users

This application uses **WPF (Windows Presentation Foundation)**, which is a Windows-specific UI framework. Unfortunately, WPF applications cannot run on Replit because:

- Replit uses a Linux-based environment
- WPF requires the `Microsoft.WindowsDesktop.App` runtime, which is only available for Windows
- While the project can be built on Linux (with `EnableWindowsTargeting` enabled), it cannot be executed

## Running This Application

### On Windows
1. Install [.NET 8.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Clone this repository
3. Open the solution in Visual Studio or run:
   ```bash
   cd MasrPrinter
   dotnet run
   ```

### Alternative: Convert to Cross-Platform

If you want to run this on Replit or other Linux/macOS platforms, consider converting the application to use one of these cross-platform frameworks:

- **Avalonia UI** - Most similar to WPF, uses XAML
- **MAUI** - Microsoft's cross-platform framework
- **ASP.NET Core** - Web-based alternative
- **.NET Console** - If GUI is not essential

## Project Structure

```
MasrPrinter/
├── App.xaml              # Application resources and startup
├── App.xaml.cs           # Application code-behind
├── MainWindow.xaml       # Main window UI definition
├── MainWindow.xaml.cs    # Main window logic
└── MasrPrinter.csproj    # Project configuration
```

## Technology Stack

- .NET 8.0
- C# with WPF (Windows Presentation Foundation)
- XAML for UI markup
