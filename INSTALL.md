# Installation Guide

## Prerequisites

Before running ShootShapesUp, ensure you have the following installed:

### Required Software
- **.NET 6 SDK**
- **Visual Studio 2022** (or `dotnet build` from the command line)
- Windows desktop (DirectX)

### Installing MonoGame
MonoGame is restored from NuGet when you build (`MonoGame.Framework.WindowsDX`). You do not need a separate MonoGame installer.

## Building the Project

### Method 1: Using Visual Studio
1. Open `ShootShapesUp.csproj` in Visual Studio
2. Right-click on the solution and select "Restore NuGet Packages"
3. Build the solution (Ctrl+Shift+B)
4. Run the project (F5)

### Method 2: Using Command Line
1. Open Command Prompt in the project directory
2. Run `build.bat` (or `dotnet build ShootShapesUp.csproj -c Release`)
3. Run `run.bat` to start the game

## Troubleshooting

### Common Issues

**"MonoGame not found" error:**
- Ensure MonoGame is properly installed
- Check that the MonoGame templates are installed in Visual Studio

**"Content not found" error:**
- Make sure all content files are in the `Content` folder
- Rebuild the content pipeline in Visual Studio

**Build errors:**
- Ensure you have the .NET 6 SDK installed (`dotnet --list-sdks`)
- Try cleaning and rebuilding the solution

### System Requirements

- **OS**: Windows 7 or later
- **RAM**: 512 MB minimum, 1 GB recommended
- **Graphics**: DirectX 9.0c compatible
- **Storage**: 100 MB free space

## Running the Game

Once built successfully, you can run the game by:
1. Double-clicking `ShootShapesUp.exe` in the output folder
2. Using the provided `run.bat` script
3. Running from Visual Studio (F5)

## Content Pipeline

The game uses MonoGame's content pipeline for assets. If you modify any content files:
1. Open the Content.mgcb file in MonoGame Pipeline Tool
2. Rebuild the content
3. Rebuild the main project

## Development Setup

For development work:
1. Install Visual Studio with C# development workload
2. Install MonoGame templates
3. Clone the repository
4. Open the solution in Visual Studio
5. Build and run

Happy gaming! 🚀
