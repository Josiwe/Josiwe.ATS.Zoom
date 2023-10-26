using BepInEx;
using Eremite;
using Eremite.Controller;
using Eremite.Model;
using Eremite.Services;
using HarmonyLib;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using Eremite.View.Cameras;

namespace Josiwe.ATS.Zoom
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;
        private Harmony harmony;
        static ZoomConfig _zoomConfig;

        private void Awake()
        {
            Instance = this;
            harmony = Harmony.CreateAndPatchAll(typeof(Plugin));  
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        // Increase zoom limit
        [HarmonyPatch(typeof(CameraController), nameof(CameraController.SetUp))]
        [HarmonyPostfix]
        public static void Setup_PostPatch(CameraController __instance)
        {
            // original code:
            // private Vector2 zoomLimit = new Vector2(-20f, -8f);

            ZoomConfig config = GetZoomConfig();

            int zoomMultiplier = 1;
            if(config != null)
            {
                zoomMultiplier = config.ZoomLimitMultiplier;
            }

            float x = -20f * zoomMultiplier;
            float y = -8f * zoomMultiplier;

            __instance.ZoomLimit = new UnityEngine.Vector2(x, y);
        }

        [HarmonyPatch(typeof(MainController), nameof(MainController.OnServicesReady))]
        [HarmonyPostfix]
        private static void HookMainControllerSetup()
        { 
            // This method will run after game load (Roughly on entering the main menu)
            // At this point a lot of the game's data will be available.
            // Your main entry point to access this data will be `Serviceable.Settings` or `MainController.Instance.Settings`
            Instance.Logger.LogInfo($"Performing game initialization on behalf of {PluginInfo.PLUGIN_GUID}.");
            Instance.Logger.LogInfo($"The game has loaded {MainController.Instance.Settings.effects.Length} effects.");
        }

        [HarmonyPatch(typeof(GameController), nameof(GameController.StartGame))]
        [HarmonyPostfix]
        private static void HookEveryGameStart()
        {
            // Too difficult to predict when GameController will exist and I can hook observers to it
            // So just use Harmony and save us all some time. This method will run after every game start
            var isNewGame = MB.GameSaveService.IsNewGame();
            WriteLog($"Entered a game. Is this a new game: {isNewGame}.");
        }

        static ZoomConfig GetZoomConfig()
        {
            if(_zoomConfig == null)
            {
                string basePath = Directory.GetCurrentDirectory() + "\\BepInEx\\plugins\\Josiwe.ATS.Zoom.Config.json";

                // Tries to load the zoom config from json
                if (File.Exists(basePath))
                {
                    try
                    {
                        string json = File.ReadAllText(basePath);
                        _zoomConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<ZoomConfig>(json);
                    }
                    catch { }
                }
                else
                {
                    WriteLog("Zoom config file not found");
                }
            }

            return _zoomConfig; ;
        }

        static void WriteLog(string message)
        {
            Instance.Logger.LogInfo("Josiwe.ATS.Zoom:: " + message);
        }
    }
}
