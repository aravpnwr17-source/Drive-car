# Joke Generator - External API Integration Guide

## Overview
The Joke Generator script fetches random jokes from the **JokeAPI** - a free, open-source joke API.

## API Details

### Base URL
```
https://v2.jokeapi.dev/joke/Any
```

### Supported Categories
- `Any` - Random joke from any category
- `General` - General/miscellaneous jokes
- `Programming` - Programming/tech jokes
- `Knock-Knock` - Classic knock-knock jokes
- `Dark` - Dark humor jokes

### API Endpoints

**Get a random joke:**
```
GET https://v2.jokeapi.dev/joke/Any
```

**Get joke from specific category:**
```
GET https://v2.jokeapi.dev/joke/Programming
GET https://v2.jokeapi.dev/joke/Dark
GET https://v2.jokeapi.dev/joke/Knock-Knock
```

**Example Response (Two-part joke):**
```json
{
  "error": false,
  "type": "twopart",
  "setup": "Why do programmers prefer dark mode?",
  "delivery": "Because light attracts bugs!",
  "id": 123
}
```

**Example Response (Single joke):**
```json
{
  "error": false,
  "type": "single",
  "joke": "Why did the scarecrow win an award? He was outstanding in his field!",
  "id": 456
}
```

## Setup Instructions

### 1. Add to Scene
1. Create an empty GameObject named "JokeGenerator"
2. Attach the `JokeGenerator.cs` script
3. Create UI Canvas with TextMeshPro text elements

### 2. Configure UI Elements
In the Inspector, assign:
- **Joke Display** - TextMeshProUGUI for displaying the joke
- **Joke Type Display** - TextMeshProUGUI for showing joke type
- **Status Text** - Optional, for status messages

### 3. Usage

**In Code:**
```csharp
JokeGenerator jokeGen = GetComponent<JokeGenerator>();

// Generate a new joke
jokeGen.GenerateJoke();

// Get current joke
string currentJoke = jokeGen.GetCurrentJoke();

// Get joke type
string jokeType = jokeGen.GetCurrentJokeType();

// Copy to clipboard
jokeGen.CopyJokeToClipboard();
```

**Player Controls:**
- **SPACE** - Generate new joke
- **C** - Copy joke to clipboard

## Customization

### Change Joke Category
Edit `jokeApiUrl` in Inspector:
```csharp
jokeApiUrl = "https://v2.jokeapi.dev/joke/Programming";
```

### Categories Available
```
Any
General
Programming
Knock-Knock
Dark
```

### Adjust Request Timeout
```csharp
requestTimeout = 15f; // 15 seconds
```

## Response Handling

### Joke Types
1. **single** - One-liner joke
2. **twopart** - Setup and delivery joke

### Error Handling
The script automatically handles:
- Network errors
- Timeout errors
- JSON parsing errors
- API errors

Errors are displayed on screen with a message to retry.

## API Features

### No Authentication Required
- Free to use
- No API key needed
- No rate limiting (public API)

### CORS Enabled
- Can be called from web and mobile apps
- Safe for cross-origin requests

### Performance
- Fast response times (typically < 500ms)
- Lightweight JSON payloads
- Reliable uptime

## Example Integration

```csharp
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    private JokeGenerator jokeGenerator;

    private void Start()
    {
        jokeGenerator = GetComponent<JokeGenerator>();
    }

    public void OnMainMenuLoad()
    {
        // Show a joke on main menu
        jokeGenerator.GenerateJoke();
    }

    public void OnShareJoke()
    {
        string joke = jokeGenerator.GetCurrentJoke();
        // Share joke to social media or friends
        ShareToSocial(joke);
    }
}
```

## Troubleshooting

### "No internet connection"
- Check device network connectivity
- Verify firewall settings allow outbound HTTPS
- Test with mobile data

### "API returned error"
- API service may be temporarily down
- Try again in a few seconds
- Check JokeAPI status page

### Jokes not displaying
- Verify TextMeshPro is installed
- Check UI Canvas is set up correctly
- Ensure script references are assigned in Inspector

## Rate Limits
- **Public API**: Unlimited (with reasonable usage)
- **Recommended**: Max 1 request per 500ms
- No authentication or throttling in place

## API Documentation
Full documentation: https://jokeapi.dev/

## License
- **JokeAPI**: Free and open-source
- **Joke Generator Script**: MIT License

## Future Enhancements
- [ ] Add joke filtering options
- [ ] Implement joke history
- [ ] Add favorite jokes system
- [ ] Create standalone joke app
- [ ] Add multiple API support

---

**Last Updated**: 2026-07-22
**API Version**: v2
**Status**: ✅ Working & Tested
