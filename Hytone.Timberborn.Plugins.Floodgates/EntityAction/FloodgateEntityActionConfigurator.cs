using Bindito.Core;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using TimberbornAPI.EntityActionSystem;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    /// <summary>
    /// Configurator for FloodgateTriggers
    /// </summary>
    public class FloodgateEntityActionConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<StreamGaugeFloodgateLinkSerializer>().AsSingleton();
            containerDefinition.Bind<WaterpumpStreamGaugeLinkSerializer>().AsSingleton();
            containerDefinition.Bind<EventListeners>().AsSingleton();
            containerDefinition.MultiBind<IEntityAction>().To<EntityActions>().AsSingleton();
        }
    }
}
