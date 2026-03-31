using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace abc.Makeup
{
    public class Game
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Play()
        {
            GameObject.FindFirstObjectByType<Image>().StartCoroutine(AfterUIBuild());
        }
        private static IEnumerator AfterUIBuild()
        {
            yield return null;
            var room = GameObject.FindFirstObjectByType<MonoBuilder.Room>().Model;
        }
    }
}