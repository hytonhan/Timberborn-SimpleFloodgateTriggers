using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using Hytone.Timberborn.Plugins.Floodgates.Schedule;
using Hytone.Timberborn.Plugins.Floodgates.UI;
using TimberbornAPI;
using TimberbornAPI.Common;

namespace Hytone.Timberborn.Plugins.Floodgates
{
    [BepInPlugin("hytone.plugins.floodgatetriggers", "FloodgateTriggersPlugin", "1.1.0")]
    [BepInDependency("com.timberapi.timberapi")]
    [HarmonyPatch]
    public class FloodGatesPlugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;
        public void Awake()
        {
            Logger = base.Logger;

            var harmony = new Harmony("hytone.plugins.floodgatetriggers");
            harmony.PatchAll();

            TimberAPI.DependencyRegistry.AddConfigurator(new FloodGatesUIFragmentConfigurator(), SceneEntryPoint.InGame);
            TimberAPI.DependencyRegistry.AddConfigurator(new FloodgateEntityActionConfigurator(), SceneEntryPoint.InGame);
            TimberAPI.DependencyRegistry.AddConfigurator(new ScheduleSystemConfigurator(), SceneEntryPoint.InGame);

            Logger.LogInfo("FloodgateTriggersPlugin is loaded.");
        }
    }
}
