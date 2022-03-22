using System;
using System.Collections.Generic;
using System.Text;

namespace Hytone.Timberborn.Plugins.Floodgates.Schedule
{
    public interface IScheduleTrigger
    {
        /// <summary>
        /// Indicates whether this Schedule is enabled or not
        /// </summary>
        public bool Enabled { get; }

        /// <summary>
        /// Indicates that the first schedule in currently active
        /// </summary>
        public bool FirstScheduleInProgress { get; }

        /// <summary>
        /// Indicates that the second schedule is currently active
        /// </summary>
        public bool SecondScheduleInProgress { get; }

        void Disable();

        void Enable();
    }
}
