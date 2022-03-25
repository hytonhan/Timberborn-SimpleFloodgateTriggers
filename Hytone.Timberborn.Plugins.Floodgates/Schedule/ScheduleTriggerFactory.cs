using System;
using Timberborn.TimeSystem;

namespace Hytone.Timberborn.Plugins.Floodgates.Schedule
{
    public class ScheduleTriggerFactory : IScheduleTriggerFactory
    {
        private readonly IDayNightCycle _dayNightCycle;
        private readonly ScheduleTriggerService _scheduleTriggerService;

        public ScheduleTriggerFactory(IDayNightCycle dayNightCycle, ScheduleTriggerService scheduleTriggerService)
        {
            _dayNightCycle = dayNightCycle;
            _scheduleTriggerService = scheduleTriggerService;
        }

        public IScheduleTrigger Create(Action action1, 
                                       Action action2, 
                                       float timestamp1, 
                                       float timestamp2)
        {
            return new ScheduleTrigger(_dayNightCycle, 
                                       _scheduleTriggerService, 
                                       action1, 
                                       action2, 
                                       timestamp1, 
                                       timestamp2);
        }
    }
}
