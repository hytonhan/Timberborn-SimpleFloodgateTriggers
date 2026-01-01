using Bindito.Core;
using HarmonyLib;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterSourceRegulators;
using System;
using System.Collections.Generic;
using System.Linq;
using Timberborn.BaseComponentSystem;
using Timberborn.BehaviorSystem;
using Timberborn.EntitySystem;
// using Timberborn.IrrigationSystem;
using Timberborn.Persistence;
using Timberborn.SerializationSystem;
using Timberborn.TemplateInstantiation;
using Timberborn.TemplateSystem;
using Timberborn.WaterBuildings;
using Timberborn.WaterSourceSystem;
using Timberborn.WorldSerialization;
using UnityEngine;
// using UnityEngine.InputSystem;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{

    [Context("Game")]
    public class FloodgateEntityActionConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<FloodgateTriggerMonoBehaviour>().AsTransient();
            containerDefinition.Bind<WaterPumpMonobehaviour>().AsTransient();
            containerDefinition.Bind<WaterSourceRegulatorMonobehaviour>().AsTransient();
            containerDefinition.Bind<StreamGaugeMonoBehaviour>().AsTransient();
            containerDefinition.Bind<StreamGaugeFloodgateLinkSerializer>().AsSingleton();
            containerDefinition.Bind<WaterpumpStreamGaugeLinkSerializer>().AsSingleton();
            containerDefinition.Bind<WaterSourceRegulatorLinkSerializer>().AsSingleton();
            containerDefinition.Bind<EventListeners>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule()
        {
            TemplateModule.Builder builder = new TemplateModule.Builder();
            builder.AddDecorator<Floodgate, FloodgateTriggerMonoBehaviour>();
            builder.AddDecorator<StreamGauge, StreamGaugeMonoBehaviour>();
            builder.AddDecorator<WaterSourceRegulator, WaterSourceRegulatorMonobehaviour>();
            builder.AddDecorator<WaterInput, WaterPumpMonobehaviour>();
            builder.AddDecorator<WaterOutput, WaterPumpMonobehaviour>();
            return builder.Build();
        }
    }

    // [HarmonyPatch(typeof(EntityService), "Instantiate", typeof(BaseComponent), typeof(Guid))]
    // class MinWindStrengthPatch
    // {
    //     public static void Postfix(BaseComponent __result)
    //     {
    //         if ((__result.GetComponent<WaterInput>() != null || __result.GetComponent<WaterOutput>() != null)
    //             && __result.Name.ToLower().Contains("shower") == false)
    //         {
    //             var baseInstantiator = DependencyContainer.GetInstance<BaseInstantiator>();
    //             baseInstantiator.AddComponent<WaterPumpMonobehaviour>(__result.GameObjectFast);
    //         }
    //     }
    // }

}
