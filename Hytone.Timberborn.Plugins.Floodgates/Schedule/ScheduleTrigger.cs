using System;
using Timberborn.TimeSystem;

namespace Hytone.Timberborn.Plugins.Floodgates.Schedule
{
    /// <summary>
    /// This class responsibility is to hold logic relating
    /// to ScheduleTriggers
    /// </summary>
    public class ScheduleTrigger : IScheduleTrigger
    {
        private readonly IDayNightCycle _dayNightCycle;
        private readonly ScheduleTriggerService _scheduleTriggerService;
        private readonly Action _action1;
        private readonly Action _action2;

        private float _firstSchedule;
        private float _secondSchedule;


        public bool Enabled { get; private set; }

        public bool FirstScheduleInProgress { get; private set; }
        public bool SecondScheduleInProgress { get; private set; }

        

        public ScheduleTrigger(IDayNightCycle dayNightCycle,
                               ScheduleTriggerService scheduleTriggerService,
                               Action action1,
                               Action action2,
                               float firstSchedule,
                               float secondSchedule)
        {
            _dayNightCycle = dayNightCycle;
            _scheduleTriggerService = scheduleTriggerService;
            _action1 = action1;
            _action2 = action2;
            _firstSchedule = firstSchedule;
            _secondSchedule = secondSchedule;
        }

        /// <summary>
        /// Enables this schedule trigger
        /// </summary>
        public void Enable()
        {
            if (!Enabled)
            {
                Enabled = true;
            }
            var currTime = _dayNightCycle.HoursPassedToday;
            if (_firstSchedule <= _secondSchedule)
            {
                if (_firstSchedule < currTime && _secondSchedule > currTime)
                {
                    FirstScheduleInProgress = true;
                    SecondScheduleInProgress = false;
                }
                else
                {
                    FirstScheduleInProgress = false;
                    SecondScheduleInProgress = true;
                }
            }
            else
            {
                if (_secondSchedule < currTime && _firstSchedule > currTime)
                {
                    FirstScheduleInProgress = false;
                    SecondScheduleInProgress = true;
                }
                else
                {
                    FirstScheduleInProgress = true;
                    SecondScheduleInProgress = false;
                }
            }
            _scheduleTriggerService.Add(this, _firstSchedule, _secondSchedule);
        }

        /// <summary>
        /// Disables this schedule trigger
        /// </summary>
        public void Disable()
        {
            if (Enabled)
            {
                Enabled = false;
                FirstScheduleInProgress = false;
                SecondScheduleInProgress = false;
            }
            _scheduleTriggerService.Remove(this);
        }

       
        /// <summary>
        /// Invokes appropriate method when the schedule
        /// is triggered
        /// </summary>
        public void Trigger()
        {
            if(Enabled)
            {
                if(FirstScheduleInProgress)
                {
                    _action1();
                }
                else
                {
                    _action2();
                }
                FirstScheduleInProgress = !FirstScheduleInProgress;
                SecondScheduleInProgress = !SecondScheduleInProgress;
            }
        }
    }
}
