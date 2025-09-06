@echo off
echo Building ShootShapesUp...

REM Clean previous build
if exist "bin" rmdir /s /q "bin"
if exist "obj" rmdir /s /q "obj"

REM Build the project
msbuild ShootShapesUp.csproj /p:Configuration=Release /p:Platform=x86

if %ERRORLEVEL% EQU 0 (
    echo Build successful!
    echo Executable location: bin\Windows\x86\Release\ShootShapesUp.exe
) else (
    echo Build failed!
    exit /b 1
)
