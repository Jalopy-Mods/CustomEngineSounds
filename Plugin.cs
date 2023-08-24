using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomEngineSounds
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Debug.LogWarning(Application.streamingAssetsPath);
            Harmony.CreateAndPatchAll(typeof(Plugin));
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(EngineComponentC), "Start")]
        public static void EngineCreatedPostfix(EngineComponentC __instance)
        {
            __instance.engineAudio = LoadClip("engine.wav");
            Debug.LogWarning("Moteur patché!");
        }

        private static AudioClip LoadClip(string clipName)
        {
            var clip = new WWW($"file://{Application.streamingAssetsPath}/{clipName}");
            while (!clip.isDone) { }  // Wait for the clip to load
            return clip.GetAudioClip();
        }

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;

        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Debug.Log("Scene Loaded");
            // if (scene.name == "Scn2_CheckpointBravo")
            // {
            //     foreach (var audioClip in FindObjectsOfType<AudioClip>())
            //     {
            //         Debug.Log(audioClip.name);
            //     }
            // }
        }
    }
}
