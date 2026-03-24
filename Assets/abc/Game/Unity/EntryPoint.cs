using UnityEngine;

namespace abc.Game.Unity
{
    /// <summary>
    /// Subject for a ScriptableSingleton<T> + com.unity.settings-manager
    /// combo with app config, target scenes validation, etc.
    /// </summary>
    public class EntryPoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            QualitySettings.vSyncCount  = 0;
            Application.targetFrameRate = 60;
        }
    }
}