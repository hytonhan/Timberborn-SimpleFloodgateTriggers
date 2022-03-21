using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.TickSystem;
using Timberborn.TimeSystem;

namespace Hytone.Timberborn.Plugins.Floodgates.Schedule
{
    public class ScheduleTriggerService : ITickableSingleton
	{
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

		public void Add(ScheduleTrigger scheduleTrigger, 
						float firstTriggerTimestamp,
						float secondTriggerTimestamp)
		{
			Remove(scheduleTrigger);
			SortableKey sortableKey = new SortableKey(firstTriggerTimestamp, secondTriggerTimestamp, _nextId++);
			_sortedTimeTriggers[sortableKey] = scheduleTrigger;
			_timeTriggerKeys[scheduleTrigger] = sortableKey;
		}

		public void Remove(ScheduleTrigger scheduleTrigger)
		{
			if (_timeTriggerKeys.TryGetValue(scheduleTrigger, out var value))
			{
				_sortedTimeTriggers.Remove(value);
				_timeTriggerKeys.Remove(scheduleTrigger);
			}
		}

		private void FindReadyToTrigger()
		{
			float partialDayNumber = _dayNightCycle.HoursPassedToday;
			foreach (KeyValuePair<SortableKey, ScheduleTrigger> sortedTimeTrigger in _sortedTimeTriggers)
			{
				sortedTimeTrigger.Deconstruct(out var key, out var value);
				SortableKey sortableKey = key;
				ScheduleTrigger item = value;
                if (Math.Max(sortableKey.Timestamp1, sortableKey.Timestamp2) > partialDayNumber)
                {
                    break;
                }
                _triggersToTrigger.Add(item);
			}
		}

		private void Trigger()
		{
			foreach (ScheduleTrigger item in _triggersToTrigger)
			{
				Trigger(item);
			}
			_triggersToTrigger.Clear();
		}

		private void Trigger(ScheduleTrigger scheduleTrigger)
		{
			Remove(scheduleTrigger);
			scheduleTrigger.Trigger();
		}
	}
}
