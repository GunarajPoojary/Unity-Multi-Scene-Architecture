# Unity Multi-Scene Architecture Example

A sample Unity project demonstrating a scalable multi-scene architecture using:
- Addressables for dynamic scene loading
- ScriptableObject-based Event Channels for decoupled communication  
- SceneReference management for clean scene address handling

---

## Features

- **Addressables Integration**
    - Scenes are managed as Addressable assets
    - Dynamically load/unload scenes by address at runtime

- **ScriptableObject Event Channels**
    - Decoupled event-driven architecture
    - EventChannels trigger scene loads without tight coupling

- **SceneReference ScriptableObjects**
    - Stores scene addresses in an easy-to-use, type-safe way
    - Avoids hardcoded scene names or indexes

- **Clean Bootstrap Pattern**
    - Bootstrap scene initializes services and listens to events
    - Allows simple swapping of feature scenes (e.g., Menu, Gameplay, Settings)