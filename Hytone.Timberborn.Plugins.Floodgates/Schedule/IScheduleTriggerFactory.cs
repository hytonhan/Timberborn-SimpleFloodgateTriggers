using System;

namespace Hytone.Timberborn.Plugins.Floodgates.Schedule
{
    public interface IScheduleTriggerFactory
    {
        IScheduleTrigger Create(Action action1, Action action2, float timestamp1, float timestamp2);
    }
}
