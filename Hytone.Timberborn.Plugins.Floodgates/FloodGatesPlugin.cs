using HarmonyLib;
using System;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;
using Timberborn.TimeSystem;
using Timberborn.WaterBuildingsUI;

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

    [HarmonyPatch(typeof(DayNightCycle), nameof(DayNightCycle.Awake))]
    public static class FooClass
    {
        [HarmonyPostfix]
        public static void Postfix(DayNightCycle __instance)
        {
            Console.WriteLine($"Day in secs: {__instance._configuredDayLengthInSeconds}");
        }
    }
}
