using HarmonyLib;
using System;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;
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


    //[HarmonyPatch]
    //public static class TestPatch
    //{
    //    [HarmonyPatch(typeof(FloodgateFragment), nameof(FloodgateFragment.InitializeFragment))]
    //    public static void Postfix(FloodgateFragment __instance)
    //    {
    //        Console.WriteLine($"slider: {__instance._slider}");

    //        foreach(var foo in __instance._slider.classList)
    //        {
    //            Console.WriteLine($"\tclass: {foo}");
    //        }

    //        Console.WriteLine($"\tbg: {__instance._slider.style.backgroundImage}");
    //    }
    //}
}
