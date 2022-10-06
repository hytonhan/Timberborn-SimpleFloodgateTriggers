using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;

namespace Hytone.Timberborn.Plugins.Floodgates
{
    [HarmonyPatch]
    public class FloodGatesPlugin : IModEntrypoint
    {
        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            var harmony = new Harmony("hytone.plugins.floodgatetriggers");
            harmony.PatchAll();

            consoleWriter.LogInfo("FloodgateTriggersPlugin is loaded.");
        }
    }
}
