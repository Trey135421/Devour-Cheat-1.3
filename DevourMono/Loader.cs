using UnityEngine;

namespace DevourMono
{
    public class Loader
    {
        private static GameObject _Load;
        public static void Load()
        {
            _Load = new GameObject();
            _Load.AddComponent<Main>();
            Object.DontDestroyOnLoad(_Load);
        }
        public static void Unload()
        {
            Object.Destroy(_Load);
        }
    }
}