using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hytone.Timberborn.Plugins.Floodgates.Schedule
{
    public class ScheduleSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ScheduleTriggerService>().AsSingleton();
            containerDefinition.Bind<IScheduleTriggerFactory>().To<ScheduleTriggerFactory>().AsSingleton();
        }
    }
}
