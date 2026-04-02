# Makeup Room — Unity 2D Prototype

WIP more simple branch, target code in DressUp folder


https://github.com/user-attachments/assets/466c9741-6523-4cfa-94af-85defc2e7a36


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
- [Odin Inspector](https://odininspector.com/) (≥ 4.0) for some dictionary serialization, inspector buttons, etc. 

## Download
See [Releases](https://github.com/feuzll/unity-makeup-private/releases) for the latest stable and smallest APK.
