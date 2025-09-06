@echo off
echo Starting ShootShapesUp...

if exist "bin\Windows\x86\Release\ShootShapesUp.exe" (
    cd bin\Windows\x86\Release
    ShootShapesUp.exe
) else if exist "bin\Windows\x86\Debug\ShootShapesUp.exe" (
    cd bin\Windows\x86\Debug
    ShootShapesUp.exe
) else (
    echo Executable not found! Please build the project first.
    echo Run build.bat to build the project.
    pause
)
