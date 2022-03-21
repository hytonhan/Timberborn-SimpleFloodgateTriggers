using System;
using System.Collections.Generic;
using System.Text;

namespace Hytone.Timberborn.Plugins.Floodgates.Schedule
{
    public interface IScheduleTrigger
    {
        public bool Enabled { get; }
        public bool Finished { get; }
        public bool FirstScheduleInProgress { get; }
        public bool SecondScheduleInProgress { get; }
        public float TimeUntilNextTrigger { get; }

        void Disable();

        void Enable();
    }
}
