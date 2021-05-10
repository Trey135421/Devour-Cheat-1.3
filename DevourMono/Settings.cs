using UnityEngine;

namespace DevourMono
{
    public class Settings : Main
    {
        // Main
        public static bool Menu = true;
        public static bool Visuals = true;
        public static bool PlayerEditor = false;

        public static KeyCode MenuKey = KeyCode.Insert;
        public static KeyCode ExitKey = KeyCode.Delete;

        // Visuals
        public static bool PlayerEsp = true;
        public static bool DemonEsp = true;
        public static bool GoatEsp = true;
        public static bool ItemEsp = false;
        public static bool KeyEsp = true;
        public static bool CollectibleEsp = false;

        public static bool AnnaEsp = true;
        public static bool RitualEsp = true;

        // Cheats
        public static bool Flashlight = false;
        public static bool FullBright = false;
        public static bool Speed = false;
    }
}