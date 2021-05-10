using System.Reflection;
using System.Collections.Generic;

using UnityEngine;
using Horror;
using Opsive.UltimateCharacterController.Character;

namespace DevourMono
{
    public class Main : MonoBehaviour
    {
        #region Vars
        // Main Vars
        public static GameObject LocalPlayer = null;
        public static GameObject Anna = null;
        public static GameObject RitualBowl = null;
        public static Camera Cam = null;
        public static Menu menu = null;
        public static string Map = "Farm";

        // Lists
        public static List<GameObject> Players = new List<GameObject>();
        public static List<SurvivalDemonBehaviour> Demons = new List<SurvivalDemonBehaviour>();
        public static List<GoatBehaviour> Goats = new List<GoatBehaviour>();
        public static List<SurvivalInteractable> Items = new List<SurvivalInteractable>();
        public static List<KeyBehaviour> Keys = new List<KeyBehaviour>();
        public static List<CollectableInteractable> Collectibles = new List<CollectableInteractable>();

        // Other
        float t, ti = 4f;
        #endregion

        public void Start()
        {
            Drawing.InitDrawing();
        }

        public void Update()
        {
            // Input
            if (Input.GetKeyDown(Settings.MenuKey))
            {
                Settings.Menu = !Settings.Menu;
            }
            if (Input.GetKeyDown(Settings.ExitKey))
            {
                Loader.Unload();
            }

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                Utility.UnlockAll();
            } // Get achievements & robes
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                foreach (DoorBehaviour db in FindObjectsOfType<DoorBehaviour>())
                    db.Unlock();
            } // Unlock Doors
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                Settings.Flashlight = !Settings.Flashlight;
                Light flashlight = LocalPlayer.GetComponent<NolanBehaviour>().flashlightSpot;
                if (Settings.Flashlight)
                {
                    flashlight.intensity = 4f;
                    flashlight.range = 60f;
                }
                else
                {
                    flashlight.intensity = 2f;
                    flashlight.range = 20f;
                }

            } // Strong Flashlight
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                Settings.FullBright = !Settings.FullBright;
                Transform head = LocalPlayer.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head);
                Light light = LocalPlayer.AddComponent<Light>();
                if (Settings.FullBright)
                {
                    light.color = Color.white;
                    light.type = LightType.Spot;
                    light.shadows = LightShadows.None;
                    light.range = 10000f;
                    light.spotAngle = 9999f;
                    light.intensity = 0.5f;
                }
                else
                {
                    Destroy(light);
                }
            } // Fullbright
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                Settings.Speed = !Settings.Speed;
                if (Settings.Speed)
                {
                    LocalPlayer.GetComponent<UltimateCharacterLocomotion>().TimeScale = 5f;
                }
                else
                {
                    LocalPlayer.GetComponent<UltimateCharacterLocomotion>().TimeScale = 1f;
                }
            } // Speed
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                foreach (SurvivalDemonBehaviour demon in Demons)
                {
                    demon.Despawn();
                }
            } // Destroy Demons
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                foreach (KeyBehaviour key in Keys)
                {
                    if (key.transform != null)
                    {
                        key.transform.position = LocalPlayer.GetComponent<NolanBehaviour>().transform.position + LocalPlayer.GetComponent<NolanBehaviour>().transform.forward * 2.5f;
                    }
                }
            } // Teleport Keys 2.5m Ahead
            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                foreach (GoatBehaviour goat in Goats)
                {
                    goat.GetComponent<UltimateCharacterLocomotion>().SetPosition(LocalPlayer.GetComponent<NolanBehaviour>().transform.position + LocalPlayer.GetComponent<NolanBehaviour>().transform.forward * 2.5f);
                }
            } // Teleport Goats 2.5m Ahead
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                foreach (SurvivalInteractable si in Items)
                {
                    if (si.name.ToLower().Contains("gasoline") || si.name.ToLower().Contains("fuse"))
                        si.transform.position = LocalPlayer.transform.position + LocalPlayer.transform.forward * 2.5f;
                    else if (si.name.ToLower().Contains("hay") || si.name.ToLower().Contains("food"))
                        si.transform.position = LocalPlayer.transform.position + LocalPlayer.transform.forward * 5f;
                }
            } // Teleport Fuses/Gasoline 2.5m and Food/Hay 3m
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                LocalPlayer.GetComponent<UltimateCharacterLocomotion>().SetPosition(LocalPlayer.GetComponent<NolanBehaviour>().transform.position + LocalPlayer.GetComponent<NolanBehaviour>().transform.forward * 2.5f);
            } // Teleport 2.5m Ahead

            // Timer
            if (Time.time > t) // Every 4 seconds
            {
                Cam = Camera.main; // Make sure we have the correct camera, no idea if this is needed
                menu = FindObjectOfType<Menu>();

                // Clear lists
                Players.Clear();
                Demons.Clear();
                Goats.Clear();
                Items.Clear();
                Keys.Clear();
                Collectibles.Clear();

                // Set Lists
                foreach (GameObject g in FindObjectsOfType<GameObject>())
                {
                    if (g.tag == "Player" && !Players.Contains(g)) // If they're tagged player & isn't in list
                    {
                        Players.Add(g);
                        if (Utility.GetDistance(Cam.gameObject, g) < 2)
                            LocalPlayer = g;
                    }
                    else if (g.GetComponent<SurvivalAnnaBehaviour>() != null) // Anna
                        Anna = g;
                    else if (g.name == "SM_RitualBowl") // Ritual Bowl
                        RitualBowl = g;
                }
                foreach (SurvivalDemonBehaviour demon in FindObjectsOfType<SurvivalDemonBehaviour>())
                {
                    if (!Demons.Contains(demon)) // If it isn't in the list
                        Demons.Add(demon);
                }
                foreach (GoatBehaviour goat in FindObjectsOfType<GoatBehaviour>())
                {
                    if (!Goats.Contains(goat)) // If it isn't in the list
                        Goats.Add(goat);
                }
                foreach (SurvivalInteractable item in FindObjectsOfType<SurvivalInteractable>())
                {
                    if (!Items.Contains(item)) // If it isn't in the list
                        Items.Add(item);
                }
                foreach (KeyBehaviour key in FindObjectsOfType<KeyBehaviour>())
                {
                    if (!Keys.Contains(key)) // If it isn't in the list
                        Keys.Add(key);
                }
                foreach (CollectableInteractable collectible in FindObjectsOfType<CollectableInteractable>())
                {
                    if (!Collectibles.Contains(collectible)) // If it isn't in the list
                        Collectibles.Add(collectible);
                }

                // Get Map
                if (FindObjectOfType<SurvivalLobbyController>().state.Map == 0)
                    Map = "Farm";
                else
                    Map = "Asylum";

                // Add time
                t = Time.time + ti;
            }
        }

        public void OnGUI()
        {
            // Create Labels
            GUI.Label(new Rect(Screen.width - 150, 0, 350, 100), $"Press INS to open/close");
            GUI.Label(new Rect(Screen.width - 150, 15, 350, 100), $"Press DEL to exit");
            GUI.Label(new Rect(Screen.width - 150, 30, 350, 100), $"Anna Spawned? {Anna != null && Anna.transform.position != Vector3.zero}");
            
            if (Settings.Menu)
            {
                if (Settings.Visuals)
                    GUI.Window(0, new Rect(0, 0, 350, 300), SwitchWindow, "Visuals [Hi :)]");
                else if (Settings.PlayerEditor)
                    GUI.Window(1, new Rect(0, 0, 350, 300), SwitchWindow, "Player Editor [Hi :)] - Idk if works");
            }

            // Draw ESP
            foreach (GameObject g in Players)
            {
                if (g != LocalPlayer && Settings.PlayerEsp)
                {
                    Drawing.RenderObj(g, Color.cyan, "Player", true, true, Utility.GetBone(g, HumanBodyBones.RightFoot), Utility.GetBone(g, HumanBodyBones.Head));
                    Drawing.DrawBones(g, Utility.GetBones(g), Color.cyan);
                }
            }
            foreach (SurvivalDemonBehaviour g in Demons)
            {
                if (Settings.DemonEsp)
                {
                    Drawing.RenderObj(g.gameObject, Color.red, "Demon", true, true, Utility.GetBone(g.gameObject, HumanBodyBones.RightFoot), Utility.GetBone(g.gameObject, HumanBodyBones.Head));
                    Drawing.DrawBones(g.gameObject, Utility.GetBones(g.gameObject), Color.red);
                }
            }
            foreach (GoatBehaviour g in Goats)
            {
                if (Settings.GoatEsp)
                {
                    Drawing.RenderObj(g.gameObject, Color.green, "", true);
                }
            }
            foreach (SurvivalInteractable g in Items)
            {
                if (Settings.ItemEsp)
                {
                    Drawing.RenderObj(g.gameObject, Color.yellow);
                }
            }
            foreach (KeyBehaviour g in Keys)
            {
                if (Settings.KeyEsp && Vector3.Distance(g.transform.position, Cam.transform.position) < 420f)
                {
                    if (g.name != "Key(Clone)")
                        Drawing.RenderObj(g.gameObject, Color.blue, g.name);
                    else
                        Drawing.RenderObj(g.gameObject, Color.blue);
                }
            }
            foreach (CollectableInteractable g in Collectibles)
            {
                if (Settings.CollectibleEsp)
                {
                    Drawing.RenderObj(g.gameObject, Color.magenta, g.name);
                }
            }
            if (Settings.RitualEsp && Map == "Farm")
            {
                Drawing.RenderObj(RitualBowl, Color.red, "Ritual Bowl");
            }
            if (Settings.AnnaEsp && Anna != null)
            {
                Drawing.RenderObj(Anna, Color.red, "Anna", true, true, Utility.GetBone(Anna, HumanBodyBones.RightFoot), Utility.GetBone(Anna, HumanBodyBones.Head));
                Drawing.DrawBones(Anna, Utility.GetBones(Anna), Color.red);
            }
        }

        #region GUI
        void SwitchWindow(int id)
        {
            #region Window Switch
            if (GUI.Button(new Rect(1, 20, 175, 25), "Visuals [Hi :)]"))
            {
                Settings.Visuals = AllFalse();
            }
            if (GUI.Button(new Rect(175, 20, 175, 25), "Player Editor [Hi :)]-BROKE"))
            {
                Settings.PlayerEditor = AllFalse();
            }
            #endregion
            if (id == 0)
            {
                Settings.AnnaEsp = GUI.Toggle(new Rect(5, 45, 150, 25), Settings.AnnaEsp, "Anna Esp");
                Settings.PlayerEsp = GUI.Toggle(new Rect(5, 70, 150, 25), Settings.PlayerEsp, "Player Esp");
                Settings.DemonEsp = GUI.Toggle(new Rect(5, 95, 150, 25), Settings.DemonEsp, "Demon Esp");
                Settings.GoatEsp = GUI.Toggle(new Rect(5, 120, 150, 25), Settings.GoatEsp, "Goat Esp");
                Settings.ItemEsp = GUI.Toggle(new Rect(5, 145, 150, 25), Settings.ItemEsp, "Item Esp");
                Settings.KeyEsp = GUI.Toggle(new Rect(5, 170, 150, 25), Settings.KeyEsp, "Key Esp");
                Settings.CollectibleEsp = GUI.Toggle(new Rect(5, 195, 150, 25), Settings.CollectibleEsp, "Collectible Esp");
                Settings.RitualEsp = GUI.Toggle(new Rect(5, 220, 150, 25), Settings.RitualEsp, "Ritual Bowl Esp");

                GUI.Label(new Rect(125, 45, 300, 100), $"Numpad0-Unlock Robes & Ach.");
                GUI.Label(new Rect(125, 60, 300, 100), $"Numpad1-Unlock Doors");
                GUI.Label(new Rect(125, 75, 300, 100), $"Numpad2-Good Flashlight");
                GUI.Label(new Rect(125, 90, 300, 100), $"Numpad3-Full Bright");
                GUI.Label(new Rect(125, 105, 300, 100), $"Numpad4-5x Speed");
                GUI.Label(new Rect(125, 120, 300, 100), $"Numpad5-Kill Demons");
                GUI.Label(new Rect(125, 135, 300, 100), $"Numpad6-Tp Keys 2.5m Ahead");
                GUI.Label(new Rect(125, 150, 300, 100), $"Numpad7-Tp Goats 2.5m Ahead");
                GUI.Label(new Rect(125, 165, 300, 100), $"Numpad8-Tp Fuses/Gas 2.5m & Hay/Food 5m");
                GUI.Label(new Rect(125, 180, 300, 100), $"Numpad9-Tp 2.5m Ahead");
            }
        }
        bool AllFalse()
        {
            Settings.Visuals = false;
            Settings.PlayerEditor = false;
            return true;
        }
        #endregion

    }
}