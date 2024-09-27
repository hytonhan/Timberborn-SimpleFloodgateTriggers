using Bindito.Core;

namespace Hytone.Timberborn.Plugins.Floodgates.Schedule
{
    [Context("Game")]
    public class ScheduleSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ScheduleTriggerService>().AsSingleton();
            containerDefinition.Bind<IScheduleTriggerFactory>().To<ScheduleTriggerFactory>().AsSingleton();
        }
    }
}
