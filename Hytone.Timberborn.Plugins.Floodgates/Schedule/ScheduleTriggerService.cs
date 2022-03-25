using System;
using System.Collections.Generic;
using Timberborn.TickSystem;
using Timberborn.TimeSystem;

namespace Hytone.Timberborn.Plugins.Floodgates.Schedule
{
    /// <summary>
    /// This class holds all ScheduleTriggers and handles
    /// the actual triggering of them
    /// </summary>
    public class ScheduleTriggerService : ITickableSingleton
    {
        /// <summary>
        /// Helper class that allows for some optimizations?
        /// Dunno copied from Timberborn TimeTriggerService
        /// </summary>
        private readonly struct SortableKey : IComparable<SortableKey>
        {
            private readonly long _id;
            public float Timestamp1 { get; }
            public float Timestamp2 { get; }

            public SortableKey(float timestamp1,
                               float timestamp2,
                               long id)
            {
                Timestamp1 = timestamp1;
                Timestamp2 = timestamp2;
                _id = id;
            }

            public int CompareTo(SortableKey other)
            {

                int num = Math.Max(Timestamp1, Timestamp2).CompareTo(Math.Max(other.Timestamp1, other.Timestamp2));
                if (num == 0)
                {
                    return _id.CompareTo(other._id);
                }
                return num;
            }
        }

        private readonly IDayNightCycle _dayNightCycle;
        private readonly SortedDictionary<SortableKey, ScheduleTrigger> _sortedTimeTriggers = new SortedDictionary<SortableKey, ScheduleTrigger>();
        private readonly Dictionary<ScheduleTrigger, SortableKey> _timeTriggerKeys = new Dictionary<ScheduleTrigger, SortableKey>();
        private readonly List<ScheduleTrigger> _triggersToTrigger = new List<ScheduleTrigger>();
        private long _nextId;

        public ScheduleTriggerService(IDayNightCycle dayNightCycle)
        {
            _dayNightCycle = dayNightCycle;
        }

        public void Tick()
        {
            FindReadyToTrigger();
            Trigger();
        }

        /// <summary>
        /// Adds a ScheduleTrigger to the collection
        /// </summary>
        /// <param name="scheduleTrigger"></param>
        /// <param name="firstTriggerTimestamp"></param>
        /// <param name="secondTriggerTimestamp"></param>
        public void Add(ScheduleTrigger scheduleTrigger,
                        float firstTriggerTimestamp,
                        float secondTriggerTimestamp)
        {
            Remove(scheduleTrigger);
            SortableKey sortableKey = new SortableKey(firstTriggerTimestamp, secondTriggerTimestamp, _nextId++);
            _sortedTimeTriggers[sortableKey] = scheduleTrigger;
            _timeTriggerKeys[scheduleTrigger] = sortableKey;
        }

        /// <summary>
        /// Removes a ScheduleTrigger from collection
        /// </summary>
        /// <param name="scheduleTrigger"></param>
        public void Remove(ScheduleTrigger scheduleTrigger)
        {
            if (_timeTriggerKeys.TryGetValue(scheduleTrigger, out var value))
            {
                _sortedTimeTriggers.Remove(value);
                _timeTriggerKeys.Remove(scheduleTrigger);
            }
        }

        /// <summary>
        /// Finds all ScheduleTriggers that should be triggered
        /// at current time and adds them to _triggersToTrigger
        /// </summary>
        private void FindReadyToTrigger()
        {
            float hoursPassedToday = _dayNightCycle.HoursPassedToday;
            foreach (KeyValuePair<SortableKey, ScheduleTrigger> sortedTimeTrigger in _sortedTimeTriggers)
            {
                sortedTimeTrigger.Deconstruct(out var key, out var value);
                SortableKey sortableKey = key;
                ScheduleTrigger item = value;
                if (ReadyToTrigger(sortableKey, item))
                {
                    _triggersToTrigger.Add(item);
                }
            }
        }

        /// <summary>
        /// Checks if a given <paramref name="scheduleTrigger"/> 
        /// should be triggered
        /// </summary>
        /// <param name="sortableKey"></param>
        /// <param name="scheduleTrigger"></param>
        /// <returns></returns>
        private bool ReadyToTrigger(SortableKey sortableKey,
                                    ScheduleTrigger scheduleTrigger)
        {
            var currTime = _dayNightCycle.HoursPassedToday;
            if (scheduleTrigger.FirstScheduleInProgress && sortableKey.Timestamp1 < currTime
                && ((sortableKey.Timestamp1 < sortableKey.Timestamp2 && sortableKey.Timestamp2 > currTime)
                    || (sortableKey.Timestamp1 >= sortableKey.Timestamp2)))
            {
                return true;
            }
            else if (scheduleTrigger.SecondScheduleInProgress && sortableKey.Timestamp2 < currTime
                && ((sortableKey.Timestamp2 < sortableKey.Timestamp1 && sortableKey.Timestamp1 > currTime)
                    || (sortableKey.Timestamp2 > sortableKey.Timestamp1)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Loops through and triggers all ScheduleTriggers
        /// that are in _triggersToTrigger
        /// </summary>
        private void Trigger()
        {
            foreach (ScheduleTrigger item in _triggersToTrigger)
            {
                item.Trigger();
            }
            _triggersToTrigger.Clear();
        }
    }
}
