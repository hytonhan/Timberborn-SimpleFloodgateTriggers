using Bindito.Core;
using Bindito.Unity;
using HarmonyLib;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using System;
using TimberApi.ConfiguratorSystem;
using TimberApi.DependencyContainerSystem;
using TimberApi.SceneSystem;
using Timberborn.EntitySystem;
using Timberborn.IrrigationSystem;
using Timberborn.TemplateSystem;
using Timberborn.WaterBuildings;
using UnityEngine;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    /// <summary>
    /// Configurator for FloodgateTriggers
    /// </summary>

    [Configurator(SceneEntrypoint.InGame)]
    public class FloodgateEntityActionConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<StreamGaugeFloodgateLinkSerializer>().AsSingleton();
            containerDefinition.Bind<WaterpumpStreamGaugeLinkSerializer>().AsSingleton();
            containerDefinition.Bind<EventListeners>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule()
        {
            TemplateModule.Builder builder = new TemplateModule.Builder();
            builder.AddDecorator<Floodgate, FloodgateTriggerMonoBehaviour>();
            builder.AddDecorator<StreamGauge, StreamGaugeMonoBehaviour>();
            return builder.Build();
        }
    }

    [HarmonyPatch(typeof(EntityService), "Instantiate", typeof(GameObject), typeof(Guid))]
    class MinWindStrengthPatch
    {
        public static void Postfix(GameObject __result)
        {
            if ((__result.GetComponent<WaterInput>() != null || __result.GetComponent<WaterOutput>() != null || __result.GetComponent<IrrigationTower>())
                && __result.name.ToLower().Contains("shower") == false)
            {
                var instantiator = DependencyContainer.GetInstance<IInstantiator>();
                instantiator.AddComponent<WaterPumpMonobehaviour>(__result);
            }
        }
    }
}
