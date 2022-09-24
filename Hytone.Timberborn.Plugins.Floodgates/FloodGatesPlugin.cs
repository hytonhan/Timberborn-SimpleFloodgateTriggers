using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;

namespace Hytone.Timberborn.Plugins.Floodgates
{
    [BepInPlugin("hytone.plugins.floodgatetriggers", "FloodgateTriggersPlugin", "3.0.0")]
    [HarmonyPatch]
    public class FloodGatesPlugin : BaseUnityPlugin, IModEntrypoint
    {
        internal static new ManualLogSource Logger;
        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            var harmony = new Harmony("hytone.plugins.floodgatetriggers");
            harmony.PatchAll();

            consoleWriter.LogInfo("FloodgateTriggersPlugin is loaded.");
        }
    }
}
