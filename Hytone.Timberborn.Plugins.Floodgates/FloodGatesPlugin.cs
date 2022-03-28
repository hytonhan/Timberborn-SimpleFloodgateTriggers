using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using Hytone.Timberborn.Plugins.Floodgates.Schedule;
using Hytone.Timberborn.Plugins.Floodgates.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Timberborn.DistributionSystem;
using Timberborn.EntitySystem;
using Timberborn.PickObjectToolSystem;
using Timberborn.WaterBuildings;
using TimberbornAPI;
using TimberbornAPI.Common;
using UnityEngine;

namespace Hytone.Timberborn.Plugins.Floodgates
{
    [BepInPlugin("hytone.plugins.floodgatetriggers", "FloodgateTriggersPlugin", "0.2.0")]
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
            TimberAPI.DependencyRegistry.AddConfigurator(new ScheduleSystemConfigurator(), SceneEntryPoint.InGame);

            Logger.LogInfo("FloodgateTriggersPlugin is loaded.");
        }

        /// <summary>
        /// Add Labels used by FloodGateUIFragment
        /// </summary>
        private void AddLabels()
        {
            TimberAPI.Localization.AddLabel("Floodgate.Triggers.EnableOnDroughtStarted", "Set height when drought starts");
            TimberAPI.Localization.AddLabel("Floodgate.Triggers.EnableOnDroughtEnded", "Set height when drought ends");
            TimberAPI.Localization.AddLabel("Floodgate.Schedule.Enable", "Set height on a schedule");
            TimberAPI.Localization.AddLabel("Floodgate.Schedule.DisableOnDrought", "Disable schedule during drought");
            TimberAPI.Localization.AddLabel("Floodgate.Triggers.Basic", "Basic");
            TimberAPI.Localization.AddLabel("Floodgate.Triggers.Advanced", "Advanced");
            TimberAPI.Localization.AddLabel("Floodgate.Triggers.AttachToStreamGauge", "Attach Streamgauge");
            TimberAPI.Localization.AddLabel("Floodgate.Triggers.PickStreamGaugeTitle", "Pick a StreamGauge");
            TimberAPI.Localization.AddLabel("Floodgate.Triggers.PickStreamGaugeTip", "No really, pick any.");
            TimberAPI.Localization.AddLabel("Floodgates.Triggers.NoLinks", "No Streamgauges attached");
            TimberAPI.Localization.AddLabel("Floodgates.Triggers.NoFloodgateLinks", "No Floodgates attached");
            TimberAPI.Localization.AddLabel("Floodgates.Triggers.LinkedFloodgates", "Linked Floodgates");
            TimberAPI.Localization.AddLabel("Floodgates.Triggers.Threshold1", "Streamgauge threshold 1");
            TimberAPI.Localization.AddLabel("Floodgates.Triggers.Threshold2", "Streamgauge threshold 2");
            TimberAPI.Localization.AddLabel("Floodgates.Triggers.HeightWhenBelowThreshold1", "Floodgate height when below threshold 1");
        }
    }


    [HarmonyPatch(typeof(PickObjectTool), nameof(PickObjectTool.Enter))]
    public static class PickObjectToolPatch
    {
        static void Postfix(PickObjectTool __instance)
        {
            var foo = from component
                      in __instance._entityComponentRegistry.GetAll<StreamGaugeMonoBehaviour>()
                      where component.enabled
                      select component;

            IEnumerable<GameObject> values = from component
                                             in __instance._entityComponentRegistry.GetEnabled<StreamGaugeMonoBehaviour>()
                                             select component.gameObject;

            FloodGatesPlugin.Logger.LogInfo($"value count: {values.Count()}");
            FloodGatesPlugin.Logger.LogInfo($"value count2: {foo.Count()}");
            FloodGatesPlugin.Logger.LogInfo($"value count: ");


            IEnumerable<GameObject> values2 = from component
                                              in __instance._entityComponentRegistry.GetEnabled<DropOffPoint>()
                                              select component.gameObject;

            FloodGatesPlugin.Logger.LogInfo($"value count3: {values2.Count()}");
        }
    }

    //[HarmonyPatch(typeof(EntityComponentRegistry), MethodType.Constructor, new Type[] {typeof(RegisteredComponentService) })]
    //public static class EntityComponentRegistryPatch
    //{
    //    static void Postfix(EntityComponentRegistry __instance)
    //    {
    //        FloodGatesPlugin.Logger.LogInfo("EntityComponentRegistry!");
    //    }
    //}
}
