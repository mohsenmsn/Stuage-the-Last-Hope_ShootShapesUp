@echo off
echo Building ShootShapesUp...
dotnet build ShootShapesUp.csproj -c Release
if %ERRORLEVEL% EQU 0 (
    echo Build successful!
    echo Executable location: bin\Release\net6.0-windows\ShootShapesUp.exe
) else (
    echo Build failed!
    exit /b 1
)
