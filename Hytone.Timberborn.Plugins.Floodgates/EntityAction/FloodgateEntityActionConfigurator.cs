using Bindito.Core;
using HarmonyLib;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using System;
using TimberApi.ConfiguratorSystem;
using TimberApi.DependencyContainerSystem;
using TimberApi.SceneSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.EntitySystem;
using Timberborn.IrrigationSystem;
using Timberborn.TemplateSystem;
using Timberborn.WaterBuildings;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{

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

    [HarmonyPatch(typeof(EntityService), "Instantiate", typeof(BaseComponent), typeof(Guid))]
    class MinWindStrengthPatch
    {
        public static void Postfix(BaseComponent __result)
        {
            if ((__result.GetComponentFast<WaterInput>() != null || __result.GetComponentFast<WaterOutput>() != null || __result.GetComponentFast<IrrigationTower>())
                && __result.name.ToLower().Contains("shower") == false)
            {
                var baseInstantiator = DependencyContainer.GetInstance<BaseInstantiator>();
                baseInstantiator.AddComponent<WaterPumpMonobehaviour>(__result.GameObjectFast);
            }
        }
    }
}
