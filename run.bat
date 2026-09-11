@echo off
echo Starting ShootShapesUp...

if exist "bin\Release\net6.0-windows\ShootShapesUp.exe" (
    cd bin\Release\net6.0-windows
    ShootShapesUp.exe
) else if exist "bin\Debug\net6.0-windows\ShootShapesUp.exe" (
    cd bin\Debug\net6.0-windows
    ShootShapesUp.exe
) else (
    echo Executable not found! Please build the project first.
    echo Run build.bat to build the project.
    pause
)
