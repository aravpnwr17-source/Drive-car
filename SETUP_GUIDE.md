# Development Setup Guide

## Prerequisites
- **Unity 2023 LTS** or **Unreal Engine 5.x**
- **Visual Studio 2022** (C# development)
- **Git** (version control)
- **Blender** (3D asset creation)
- **Substance Painter** (Texture creation)

## Project Setup Steps

### 1. Clone Repository
```bash
git clone https://github.com/aravpnwr17-source/Drive-car.git
cd Drive-car
```

### 2. Open in Unity
1. Open Unity Hub
2. Click "Open" and select the cloned folder
3. Wait for project to load

### 3. Import Required Packages
- TextMesh Pro (UI)
- Post Processing
- ProBuilder (for rapid prototyping)
- NavMesh Components

## Folder Structure

```
Assets/
├── Models/           # 3D car and environment models
├── Textures/         # PBR textures and materials
├── Materials/        # Unity material files
├── Scripts/
│   ├── Car/         # Car controller and physics
│   ├── AI/          # Traffic AI and NPCs
│   ├── UI/          # UI and HUD systems
│   ├── World/       # Environment and weather
│   ├── Gameplay/    # Game mechanics
│   └── Vehicle/     # Vehicle systems
├── Scenes/          # Game scenes
├── Audio/           # Sound effects and music
└── Prefabs/         # Reusable game objects
```

## Development Workflow

### 1. Create Feature Branch
```bash
git checkout -b feature/your-feature-name
```

### 2. Make Changes
- Edit scripts in your IDE (Visual Studio)
- Create/modify scenes in Unity
- Test frequently

### 3. Commit Changes
```bash
git add .
git commit -m "Descriptive commit message"
```

### 4. Push to Remote
```bash
git push origin feature/your-feature-name
```

### 5. Create Pull Request
- Go to GitHub
- Open PR and describe changes
- Request review

## Coding Standards

### C# Naming Conventions
- **Classes**: PascalCase (CarController)
- **Methods**: PascalCase (GetCurrentSpeed)
- **Variables**: camelCase (currentSpeed)
- **Constants**: UPPER_SNAKE_CASE (MAX_SPEED)
- **Private fields**: _camelCase (_speed) or [SerializeField] private

### Documentation
```csharp
/// <summary>
/// Brief description of what this does
/// </summary>
/// <param name="parameter">Description of parameter</param>
/// <returns>Description of return value</returns>
public void ExampleMethod(int parameter)
{
    // Implementation
}
```

## Testing

### Unit Testing
- Use Unity Test Framework
- Place tests in `Assets/Tests/`
- Run tests via Test Runner window

### Play Testing
- Use Play mode in Unity
- Test controls responsiveness
- Verify physics behavior
- Check for performance issues

## Performance Guidelines

### Target FPS
- PC: 60 FPS (minimum)
- Console: 60 FPS (target)
- Mobile: 30 FPS

### Memory
- Scene size: < 2GB
- Total RAM usage: < 8GB (target)

### Optimization Tips
- Use LOD (Level of Detail) for models
- Implement object pooling
- Batch UI updates
- Use appropriate physics quality settings

## Building & Deployment

### Build Settings
1. File > Build Settings
2. Add scenes to build
3. Configure player settings
4. Set graphics quality

### PC Build
```bash
# Development build
Build Settings > Development Build > Build
```

### Console Build
- Requires platform SDKs
- Platform-specific build settings
- Certification requirements vary

## Troubleshooting

### Common Issues

**Game runs slowly**
- Lower graphics quality
- Reduce draw distance
- Check script performance
- Profile with Unity Profiler

**Physics unstable**
- Increase Physics timestep
- Check rigidbody settings
- Verify collider setup

**Memory leaks**
- Use Memory Profiler
- Check for undestroyedGameObject
- Properly unload assets

## Resources

- [Unity Documentation](https://docs.unity.com)
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [Game Physics](https://www.gdc.com)

## Contact
For development help, reach out to the team leads.
