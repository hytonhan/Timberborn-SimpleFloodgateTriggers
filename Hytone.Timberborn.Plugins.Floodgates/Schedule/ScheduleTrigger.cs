using System;
using Timberborn.TimeSystem;
using UnityEngine;

namespace Hytone.Timberborn.Plugins.Floodgates.Schedule
{
    public class ScheduleTrigger : IScheduleTrigger
    {
        private readonly IDayNightCycle _dayNightCycle;
        private readonly ScheduleTriggerService _scheduleTriggerService;
        private readonly Action _action1;
        private readonly Action _action2;

        private float _firstSchedule;
        private float _secondSchedule;
        //private readonly float _fullDelayInDays;
        //private float _delayLeftInDays;
        private float _timeUntilNextTrigger;
        private float _resumedTimestamp;

        public bool Finished { get; private set; }

        //public bool InProgress { get; private set; }
        public bool Enabled { get; private set; }

        public bool FirstScheduleInProgress { get; private set; }
        public bool SecondScheduleInProgress { get; private set; }

        //public float DaysLeft
        //{
        //    get
        //    {
        //        if (!InProgress)
        //        {
        //            return _delayLeftInDays;
        //        }
        //        return _delayLeftInDays - DaysSinceStart;
        //    }
        //}
        public float TimeUntilNextTrigger
        {
            get
            {
                if (!Enabled)
                {
                    return float.MaxValue;
                }
                var currTime = _dayNightCycle.HoursPassedToday;
                if (FirstScheduleInProgress)
                {
                    if (_firstSchedule < _secondSchedule &&
                       _secondSchedule > currTime)
                    {
                        return (24f - _secondSchedule) + _firstSchedule;
                    }
                    else
                    {
                        return _firstSchedule - currTime;
                    }
                }
                else if(SecondScheduleInProgress)
                {
                    if (_secondSchedule < _firstSchedule &&
                        _firstSchedule > currTime)
                    {
                        return (24f - _firstSchedule) + _secondSchedule;
                    }
                    {
                        return _secondSchedule - currTime;
                    }
                }
                else
                {
                    return float.MaxValue;
                }
            }
        }

        //public float Progress
        //{
        //    get { return 1f - Mathf.Clamp01(DaysLeft / _fullDelayInDays); }
        //}

        //private float DaysSinceStart
        //{
        //    get { return _dayNightCycle.PartialDayNumber - _resumedTimestamp; }
        //}

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

        //public void Reset()
        //{
        //    Finished = false;
        //    Pause();
        //    _delayLeftInDays = _fullDelayInDays;
        //}

        //public void Resume()
        //{
        //    if (!InProgress && !Finished)
        //    {
        //        float partialDayNumber = _dayNightCycle.PartialDayNumber;
        //        _timeTriggerService.Add(this, partialDayNumber + _delayLeftInDays);
        //        _resumedTimestamp = partialDayNumber;
        //        InProgress = true;
        //    }
        //}
        public void Enable()
        {
            if (!Enabled)
            {
                Enabled = true;
            }
            var currTime = _dayNightCycle.HoursPassedToday;
            if (_firstSchedule <= _secondSchedule)
            {
                if (_firstSchedule < currTime)
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
                if (_secondSchedule < currTime)
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

        public void Disable()
        {
            if (Enabled)
            {
                Enabled = false;
                FirstScheduleInProgress = false;
                SecondScheduleInProgress = false;
                _scheduleTriggerService.Remove(this);
            }
        }

        //public void Pause()
        //{
        //    if (InProgress)
        //    {
        //        _timeTriggerService.Remove(this);
        //        _delayLeftInDays -= DaysSinceStart;
        //        InProgress = false;
        //    }
        //}

        //public void FastForwardProgress(float progress)
        //{
        //    bool inProgress = InProgress;
        //    Pause();
        //    _delayLeftInDays -= _fullDelayInDays * progress;
        //    if (_delayLeftInDays <= 0f)
        //    {
        //        Finish();
        //    }
        //    if (inProgress)
        //    {
        //        Resume();
        //    }
        //}

        //public void Finish()
        //{
        //    if (!Finished)
        //    {
        //        InProgress = false;
        //        Finished = true;
        //        _delayLeftInDays = 0f;
        //        _action();
        //    }
        //}

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
