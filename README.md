# Makeup Room — Unity 2D Prototype

An experimental WIP project of makeup mechanics for Android in a scope of learning **DCI (Data-Context-Interaction)** pattern application in a game engine

The main intent is to make game features visible at the file-structure
level, keep code engine-agnostic, and maintain the following layout:
- `Model/` — pure C# data classes with nested Roles controlling mutations
- `Contexts/` — one file per user action, readable as a feature list
- `Unity/` — thin MonoBehaviours that render model state and fire contexts

Oh, and to try DI with some project-validation at the end.

/// Video place

## Tech-stack and notes
- Unity 6000.3.7f1
- target play resolution is 1290x2796 (iPhone 16 Plus), main Canvas references 1080x1920
- character is PSD-based object, background is a tiled sprite, everything else is uGUI for a convinient and possible layout adaptations + uGUI events
- hand drag uses raw mouse\touch input bypassing the EventSystem
- some texture reading (center pixel sampling) for a convinient brush color paint
- some textures packed in Unity Sprite Atlases to reduce draw calls
- [PrimeTween](https://github.com/KyryloKuzyk/PrimeTween) used for speed-based player hand animations + small separate UI tweens
- [ParticleEffectForUGUI](https://github.com/mob-sakai/ParticleEffectForUGUI) (mob-sakai) for a uGUI particle effects
- [Book - Page Curl Pro](https://assetstore.unity.com/packages/package/77222) (≥ 2.0) for a uGUI based animated book

## Download
See [Releases](https://github.com/feuzll/unity-makeup-private/releases) for the latest stable and smallest APK.