# Project Summary: Stuage the Last Hope (ShootShapesUp)

## Overview
Stuage the Last Hope is a 2D space shooter game originally developed as a university coursework project in 2018. Inspired by the movie "Sausage Party" and classic arcade games like Star Wars, it tells the epic tale of a sausage warrior defending his planet from invading buzzards. The game has been modernized and prepared for GitHub with improved code structure, additional features, and comprehensive documentation.

## Creative Inspiration
The game was inspired by:
- **"Sausage Party" movie** - The humorous concept of sausages as characters
- **Star Wars arcade games** - Classic space shooter mechanics
- **Evolutionary storytelling** - Creating a unique narrative in the shoot-em-up genre

## Key Features
- **Epic storyline** - Stuage the Last Hope defending the sausage planet
- **Multi-level gameplay** with 3 distinct war phases (20s, 30s, 40s)
- **Dual weapon system** - Different bullet types for various buzzard enemies
- **Enemy AI** - Seek and Wanderer buzzards with different attack patterns
- **Scoring system** - Points for eliminating buzzards with multiplier bonuses
- **Lives system** - 4 lives to start, bonus lives every 3000 points
- **Pause functionality** - For better user experience during intense battles
- **Sound effects** and background music - Immersive audio experience
- **Custom mouse cursor** - Precise aiming for buzzard elimination

## Technical Stack
- **Framework**: MonoGame 3.0
- **Language**: C#
- **Platform**: Windows (.NET Framework 4.7.2)
- **Graphics**: 2D sprites with XNA Content Pipeline
- **Audio**: WAV/MP3 sound effects and music

## Project Structure
```
ShootShapesUp/
├── Core Game Files/
│   ├── Game1.cs              # Main game class and state management
│   ├── PlayerShip.cs         # Player ship logic and controls
│   ├── Enemy.cs              # Enemy AI and behaviors
│   ├── Enemy2.cs             # Second enemy type
│   ├── Bullet.cs             # Primary bullet system
│   ├── Bullet2.cs            # Secondary bullet system
│   └── EntityManager.cs      # Entity management and collision detection
├── Supporting Files/
│   ├── PlayerStatus.cs       # Score, lives, and game state
│   ├── EnemySpawner.cs       # Enemy spawning logic
│   ├── Input.cs              # Input handling
│   ├── Art.cs                # Asset loading
│   └── MathUtil.cs           # Mathematical utilities
├── Content/
│   ├── Art/                  # Texture assets
│   ├── Sound/                # Audio assets
│   └── Fonts/                # Font assets
├── Documentation/
│   ├── README.md             # Main documentation
│   ├── INSTALL.md            # Installation guide
│   ├── CHANGELOG.md          # Version history
│   └── PROJECT_SUMMARY.md    # This file
└── Build Files/
    ├── build.bat             # Build script
    ├── run.bat               # Run script
    └── .gitignore            # Git ignore rules
```

## Improvements Made (2024)
1. **Code Cleanup**: Removed commented code and debug statements
2. **Feature Additions**: Added pause functionality and improved restart system
3. **Documentation**: Created comprehensive README and installation guide
4. **Modernization**: Updated to .NET Framework 4.7.2
5. **GitHub Preparation**: Added proper .gitignore, build scripts, and project structure
6. **User Experience**: Enhanced game over screen and pause menu

## Gameplay Mechanics
- **Movement**: WASD or arrow keys to control Stuage (the sausage warrior)
- **Combat**: Mouse aiming to shoot at invading buzzards
- **Objectives**: Survive each war phase for the specified time limit
- **Scoring**: Points for eliminating buzzards with multiplier system
- **Lives**: 4 lives to start, bonus lives every 3000 points
- **Pause**: P key to pause/resume during intense battles
- **Story**: Epic tale of defending the sausage planet from buzzard invaders

## Development Notes
- Originally created as CS3005 coursework at Brunel University London (2018)
- Modernized for GitHub showcase and portfolio purposes
- Maintains original gameplay while improving code quality and user experience
- Ready for further development and feature additions

## Future Enhancements
- Particle effects for explosions
- Power-ups and special weapons
- More enemy types and behaviors
- Controller support
- Save/load functionality
- Visual effects and animations
- Multiplayer support

## Getting Started
1. Install Visual Studio 2019+ and MonoGame 3.8+
2. Clone the repository
3. Open `ShootShapesUp.csproj` in Visual Studio
4. Build and run the project
5. Enjoy the game!

This project demonstrates skills in game development, C# programming, object-oriented design, and software engineering practices learned during university coursework.
