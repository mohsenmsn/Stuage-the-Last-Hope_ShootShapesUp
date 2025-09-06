@echo off
echo Setting up Git repository for Stuage the Last Hope...

REM Initialize Git repository
git init

REM Add all files
git add .

REM Create initial commit
git commit -m "Initial commit: Stuage the Last Hope - Modernized space shooter game

- Epic storyline: Sausage vs Buzzard war
- 3 levels with increasing difficulty (20s, 30s, 40s)
- Dual weapon system and enemy AI
- Pause functionality and improved UI
- Comprehensive documentation and references
- Built with MonoGame 3.0 and .NET Framework 4.7.2

Inspired by 'Sausage Party' movie and classic arcade games.
Originally created as CS3005 coursework at Brunel University London (2018)."

echo.
echo Git repository initialized successfully!
echo.
echo Next steps:
echo 1. Go to GitHub.com and create a new repository
echo 2. Copy the repository URL
echo 3. Run: git remote add origin YOUR_REPOSITORY_URL
echo 4. Run: git push -u origin main
echo.
pause
