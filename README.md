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
- **Mouse**: Aim (custom cursor)
- **Left mouse** (hold): Fire dual cannons — **Ctrl** or gamepad trigger also fire
- **Space** or **Enter**: Confirm menus and briefings
- **P** or **Esc**: Pause / resume during a level
- **R**: Restart from pause, Game Over, or Victory
- **Esc**: Exit from the main menu

### Objective
As Stuage the Last Hope, survive each level for the specified time while defending the sausage planet from the buzzard invaders. Each level is a briefing, then a timed fight: Level 1 (20s) teaches Seek and Wanderer buzzards, Level 2 (30s) turns up the swarm, Level 3 (40s) brings elite 3-point buzzards.

### Scoring
- **Seek Buzzards**: 2 points each (aggressive attackers)
- **Wanderer Buzzards**: 1 point each (patrolling enemies)
- **Elite Buzzards**: 3 points each (powerful adversaries)
- **Multiplier system**: Increases with consecutive buzzard eliminations
- **Extra lives**: Earned every 3000 points to help Stuage survive longer

## 🛠️ Technical Details

### Built With
- **MonoGame Framework** 3.8 (WindowsDX)
- **.NET 6** Windows
- **C#** programming language

### Project Structure
```
ShootShapesUp/
├── Game1.cs              # State machine, HUD, levels
├── PlayerShip.cs         # Movement, aim, fire, respawn
├── Enemy.cs              # Seek / Wanderer / Elite factories
├── Bullet.cs             # Dual-cannon shots
├── EntityManager.cs      # Update, draw, collisions
├── PlayerStatus.cs       # Lives, score, multiplier, timers
├── EnemySpawner.cs       # Per-level spawn profiles
├── Input.cs              # Keyboard, mouse, gamepad
├── Art.cs                # Asset loading
├── GameConfig.cs         # Gameplay constants
├── MathUtil.cs           # Polar helpers
├── Extensions.cs         # Vector / random helpers
└── Content/              # Textures, sounds, fonts
```

## 🚀 Getting Started

### Prerequisites
- Windows
- .NET 6 SDK
- Visual Studio 2022, or the `dotnet` CLI

### Installation
1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/stuage-the-last-hope.git
   cd stuage-the-last-hope
   ```

2. **Build and run**:
   - Open `ShootShapesUp.csproj` and press F5, or
   - Run `build.bat` then `run.bat` (`dotnet build -c Release`)

### Building from Source
```bash
dotnet build ShootShapesUp.csproj -c Release
```
The executable is at `bin\Release\net6.0-windows\ShootShapesUp.exe`.

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
- Single entity/enemy/bullet manager (no duplicated `*2` types)
- Simulation runs once per frame and only while playing
- Restart, pause, lives/respawn, and between-level briefings work as designed
- Builds with MonoGame 3.8 on .NET 6 (Windows)

### Future Enhancements
- [ ] Add particle effects for explosions
- [ ] Implement power-ups and special weapons
- [ ] Add more enemy types and behaviors
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
