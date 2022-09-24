using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace Hytone.Timberborn.Plugins.Floodgates.Schedule
{
    [Configurator(SceneEntrypoint.InGame)]
    public class ScheduleSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ScheduleTriggerService>().AsSingleton();
            containerDefinition.Bind<IScheduleTriggerFactory>().To<ScheduleTriggerFactory>().AsSingleton();
        }
    }
}
