using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using Hytone.Timberborn.Plugins.Floodgates.UI;
using TimberbornAPI;
using TimberbornAPI.Common;

namespace Hytone.Timberborn.Plugins.Floodgates
{
    [BepInPlugin("hytone.plugins.floodgatetriggers", "FloodgateTriggersPlugin", "0.1.2")]
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

            AddLabels();

            TimberAPI.DependencyRegistry.AddConfigurator(new FloodGatesUIFragmentConfigurator(), SceneEntryPoint.InGame);
            TimberAPI.DependencyRegistry.AddConfigurator(new FloodgateEntityActionConfigurator(), SceneEntryPoint.InGame);

            Logger.LogInfo("FloodgateTriggersPlugin is loaded.");
        }

        /// <summary>
        /// Add Labels used by FloodGateUIFragment
        /// </summary>
        private void AddLabels()
        {
            TimberAPI.Localization.AddLabel("Floodgate.Triggers.EnableOnDroughtStarted", "Set height when drought starts");
            TimberAPI.Localization.AddLabel("Floodgate.Triggers.EnableOnDroughtEnded", "Set height when drought ends");
        }
    }
}
