# Stuage the Last Hope (ShootShapesUp)

A 2D space shooter game built with MonoGame, originally created as a university coursework project in 2018 and modernized for GitHub. Inspired by the movie "Sausage Party" and classic arcade games like Star Wars, this game tells the epic tale of Stuage, the last hope of the sausage planet!

## 🌭 The Story

In a world where sausages and buzzards are great enemies, a great war has begun. You control **Stuage the Last Hope**, a young warrior who must resist and survive to win the battle and save the sausage planet from the invading buzzards. 

The game features three levels of increasing difficulty:
- **Level 1**: Survive for 20 seconds (Easy)
- **Level 2**: Survive for 30 seconds (Harder) 
- **Level 3**: Survive for 40 seconds (Hardest)

As time progresses, the buzzards grow stronger and attack faster, but you remain the same. With 4 lives to start and bonus lives every 3000 points, you must survive the attacks until time runs out to win the game and save the sausage planet!

## 🎮 Game Features

- **Multi-level gameplay** with 3 distinct levels
- **Dual weapon system** with different bullet types
- **Enemy AI** with seeking and wandering behaviors
- **Scoring system** with multipliers and high score tracking
- **Lives system** with respawn mechanics
- **Time-based survival** challenges
- **Sound effects** and background music
- **Custom mouse cursor** for precise aiming

## 🎯 How to Play

### Controls
- **WASD** or **Arrow Keys**: Move Stuage (the sausage warrior)
- **Mouse**: Aim and shoot at the invading buzzards
- **P**: Pause/Resume game
- **ESC**: Exit game or pause
- **R**: Restart after game over or when paused

### Objective
As Stuage the Last Hope, survive each level for the specified time while defending the sausage planet from the buzzard invaders. Each level represents a different phase of the great war, with increasing difficulty and enemy aggression.

### Scoring
- **Seek Buzzards**: 2 points each (aggressive attackers)
- **Wanderer Buzzards**: 1 point each (patrolling enemies)
- **Elite Buzzards**: 3 points each (powerful adversaries)
- **Multiplier system**: Increases with consecutive buzzard eliminations
- **Extra lives**: Earned every 3000 points to help Stuage survive longer

## 🛠️ Technical Details

### Built With
- **MonoGame Framework** 3.0
- **.NET Framework** 4.7.2
- **C#** programming language
- **XNA Content Pipeline**

### Project Structure
```
ShootShapesUp/
├── Game1.cs              # Main game class and state management
├── PlayerShip.cs         # Player ship logic and controls
├── Enemy.cs              # Enemy AI and behaviors
├── Enemy2.cs             # Second enemy type
├── Bullet.cs             # Primary bullet system
├── Bullet2.cs            # Secondary bullet system
├── EntityManager.cs      # Entity management and collision detection
├── EntityManager2.cs     # Secondary entity management
├── PlayerStatus.cs       # Score, lives, and game state
├── EnemySpawner.cs       # Enemy spawning logic
├── Input.cs              # Input handling
├── Art.cs                # Asset loading
├── MathUtil.cs           # Mathematical utilities
├── Extensions.cs         # Extension methods
└── Content/              # Game assets (textures, sounds, fonts)
```

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2019 or later
- MonoGame 3.8 or later
- .NET Framework 4.7.2 or later

### Installation
1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/stuage-the-last-hope.git
   cd stuage-the-last-hope
   ```

2. **Open in Visual Studio**:
   - Open `ShootShapesUp.csproj` in Visual Studio
   - Restore NuGet packages (right-click solution → Restore NuGet Packages)

3. **Build and run**:
   - Press F5 to build and run
   - Or use the provided scripts: `build.bat` and `run.bat`

### Building from Source
1. Ensure MonoGame is installed
2. Open the solution in Visual Studio
3. Build the project (Ctrl+Shift+B)
4. Run the executable from the output directory

## 🎨 Game Levels

### Level 1: The Initial Assault
- **Background**: Sausage planet under attack
- **Duration**: 20 seconds
- **Enemies**: Seek and Wanderer buzzards
- **Objective**: Survive the initial buzzard invasion

### Level 2: The Great War Escalates
- **Background**: Intensified battle scene
- **Duration**: 30 seconds
- **Enemies**: Seek and Wanderer buzzards (faster and more aggressive)
- **Objective**: Hold the line as the war escalates

### Level 3: The Final Stand
- **Background**: Epic final battle
- **Duration**: 40 seconds
- **Enemies**: All buzzard types including elite units
- **Objective**: Survive the final assault and save the sausage planet

## 🔧 Development Notes

### Code Improvements Made
- Removed commented-out code and debug statements
- Improved code organization and readability
- Added proper restart functionality
- Enhanced game over screen with instructions
- Updated to .NET Framework 4.7.2
- Added comprehensive documentation

### Future Enhancements
- [ ] Add particle effects for explosions
- [ ] Implement power-ups and special weapons
- [ ] Add more enemy types and behaviors
- [ ] Create a proper menu system
- [ ] Add controller support
- [ ] Implement save/load functionality
- [ ] Add more visual effects and animations

## 📝 License

This project was created as part of university coursework. Please respect academic integrity if using this code as reference.

## 🤝 Contributing

While this is primarily a showcase project, suggestions and improvements are welcome! Feel free to:
- Report bugs or issues
- Suggest new features
- Submit pull requests for improvements
- Share your own modifications

## 📚 References

This project was built using common MonoGame shoot 'em up tutorial patterns and inspired by various open-source space shooter implementations. See [REFERENCES.md](REFERENCES.md) for detailed source attributions and academic context.

## 📧 Contact

Created as part of CS3005 coursework at Brunel University London (2018).  
Inspired by the movie "Sausage Party" and classic arcade games like Star Wars.

---

*May Stuage's courage inspire you to save the sausage planet!* 🌭⚔️
