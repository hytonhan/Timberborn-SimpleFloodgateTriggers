using Bindito.Core;
using Hytone.Timberborn.Plugins.Floodgates.Schedule;
using Timberborn.ConstructibleSystem;
using Timberborn.Persistence;
using Timberborn.TimeSystem;
using Timberborn.WaterBuildings;
using Timberborn.WeatherSystem;
using UnityEngine;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    /// <summary>
    /// This class handles the data related to Floodgate Triggers. It also holds the actions
    /// which are executed when certain events happen.
    /// </summary>
    public class FloodgateTriggerMonoBehaviour : MonoBehaviour, IPersistentEntity, IFinishedStateListener
    {
        //Keys used in data saving/loading
        private static readonly ComponentKey FloodgateTriggerKey = new ComponentKey(nameof(FloodgateTriggerMonoBehaviour));
        private static readonly PropertyKey<bool> DroughtEndedEnabledKey = new PropertyKey<bool>(nameof(DroughtEndedEnabled));
        private static readonly PropertyKey<float> DroughtEndedHeightKey = new PropertyKey<float>(nameof(DroughtEndedHeight));
        private static readonly PropertyKey<bool> DroughtStartedEnabledKey = new PropertyKey<bool>(nameof(DroughtStartedEnabled));
        private static readonly PropertyKey<float> DroughtStartedHeightKey = new PropertyKey<float>(nameof(DroughtStartedHeight));
        private static readonly PropertyKey<float> FirstScheduleTimeKey = new PropertyKey<float>(nameof(FirstScheduleTime));
        private static readonly PropertyKey<float> FirstScheduleHeightKey = new PropertyKey<float>(nameof(FirstScheduleHeight));
        private static readonly PropertyKey<float> SecondScheduleTimeKey = new PropertyKey<float>(nameof(SecondScheduleTime));
        private static readonly PropertyKey<float> SecondScheduleHeightKey = new PropertyKey<float>(nameof(SecondScheduleHeight));
        private static readonly PropertyKey<bool> ScheduleEnabledKey = new PropertyKey<bool>(nameof(ScheduleEnabled));
        private static readonly PropertyKey<bool> DisableScheduleOnDroughtKey = new PropertyKey<bool>(nameof(DisableScheduleOnDrought));

        private IScheduleTriggerFactory _scheduleTriggerFactory;
        private IScheduleTrigger _scheduleTrigger;
        private DroughtService _droughtServíce;

        public bool DroughtEndedEnabled { get; set; }
        public float DroughtEndedHeight { get; set; }
        public bool DroughtStartedEnabled { get; set; }
        public float DroughtStartedHeight { get; set; }

        public bool ScheduleEnabled { get; set; }
        public bool DisableScheduleOnDrought { get; set; }
        public float FirstScheduleTime { get; set; }
        public float FirstScheduleHeight { get; set; }
        public float SecondScheduleTime { get; set; }
        public float SecondScheduleHeight { get; set; }

        [Inject]
        public void InjectDependencies(
            IScheduleTriggerFactory scheduleTriggerFactory,
            DroughtService droughtService)
        {
            _scheduleTriggerFactory = scheduleTriggerFactory;
            _droughtServíce = droughtService;
        }

        public void OnEnterFinishedState()
        {
            _scheduleTrigger = _scheduleTriggerFactory.Create(SetFirstScheduleHeight, SetSecondScheduleHeight, FirstScheduleTime, SecondScheduleTime);
            if (ScheduleEnabled)
            {
                _scheduleTrigger.Enable();
            }
        }

        public void OnExitFinishedState()
        {
            _scheduleTrigger?.Disable();
        }

        /// <summary>
        /// Save the data so we can load it later
        /// </summary>
        /// <param name="entitySaver"></param>
        public void Save(IEntitySaver entitySaver)
        {
            IObjectSaver component = entitySaver.GetComponent(FloodgateTriggerKey);
            component.Set(DroughtEndedEnabledKey, DroughtEndedEnabled);
            component.Set(DroughtEndedHeightKey, DroughtEndedHeight);
            component.Set(DroughtStartedEnabledKey, DroughtStartedEnabled);
            component.Set(DroughtStartedHeightKey, DroughtStartedHeight);
            component.Set(FirstScheduleTimeKey, FirstScheduleTime);
            component.Set(FirstScheduleHeightKey, FirstScheduleHeight);
            component.Set(SecondScheduleTimeKey, SecondScheduleTime);
            component.Set(SecondScheduleHeightKey, SecondScheduleHeight);
            component.Set(ScheduleEnabledKey, ScheduleEnabled);
            component.Set(DisableScheduleOnDroughtKey, DisableScheduleOnDrought);
        }

        /// <summary>
        /// Load saved data if it exists
        /// </summary>
        /// <param name="entityLoader"></param>
        public void Load(IEntityLoader entityLoader)
        {
            if (!entityLoader.HasComponent(FloodgateTriggerKey))
            {
                return;
            }
            IObjectLoader component = entityLoader.GetComponent(FloodgateTriggerKey);
            if (component.Has(DroughtEndedEnabledKey))
            {
                DroughtEndedEnabled = component.Get(DroughtEndedEnabledKey);
            }
            if (component.Has(DroughtEndedHeightKey))
            {
                DroughtEndedHeight = component.Get(DroughtEndedHeightKey);
            }
            if (component.Has(DroughtStartedEnabledKey))
            {
                DroughtStartedEnabled = component.Get(DroughtStartedEnabledKey);
            }
            if (component.Has(DroughtStartedHeightKey))
            {
                DroughtStartedHeight = component.Get(DroughtStartedHeightKey);
            }
            if (component.Has(FirstScheduleTimeKey))
            {
                FirstScheduleTime = component.Get(FirstScheduleTimeKey);
            }
            if (component.Has(FirstScheduleHeightKey))
            {
                FirstScheduleHeight = component.Get(FirstScheduleHeightKey);
            }
            if (component.Has(SecondScheduleTimeKey))
            {
                SecondScheduleTime = component.Get(SecondScheduleTimeKey);
            }
            if (component.Has(SecondScheduleHeightKey))
            {
                SecondScheduleHeight = component.Get(SecondScheduleHeightKey);
            }
            if (component.Has(ScheduleEnabledKey))
            {
                ScheduleEnabled = component.Get(ScheduleEnabledKey);
            }
            if (component.Has(DisableScheduleOnDroughtKey))
            {
                DisableScheduleOnDrought = component.Get(DisableScheduleOnDroughtKey);
            }
        }

        /// <summary>
        /// The stuff to do to FLoodgates when a Drought starts
        /// </summary>
        public void OnDroughtStarted()
        {
            var floodgate = GetComponent<Floodgate>();
            if (DroughtStartedEnabled == true &&
               floodgate.Height != DroughtStartedHeight)
            {
                floodgate.SetHeight(DroughtStartedHeight);
            }
            if (DisableScheduleOnDrought)
            {
                _scheduleTrigger.Disable();
            }
        }

        /// <summary>
        /// The stuff to do to Floodgates when a Drought ends
        /// </summary>
        public void OnDroughtEnded()
        {
            var floodgate = GetComponent<Floodgate>();
            if (DroughtEndedEnabled == true &&
               floodgate.Height != DroughtEndedHeight)
            {
                floodgate.SetHeight(DroughtEndedHeight);
            }
            if (ScheduleEnabled)
            {
                _scheduleTrigger.Enable();
            }
        }

        /// <summary>
        /// When toggles related to Schedules are changed,
        /// then check if triggers need to be enabled/disabled
        /// </summary>
        public void OnChangedScheduleToggles()
        {
            if(ScheduleEnabled &&
               ((_droughtServíce.IsDrought && !DisableScheduleOnDrought)
               || !_droughtServíce.IsDrought))
            {
                _scheduleTrigger.Enable();
                return;
            }
            _scheduleTrigger.Disable();
        }

        /// <summary>
        /// When schedule times or heights are changed, remove old trigger 
        /// and create new. Enabled new trigger is old was enabled
        /// </summary>
        public void ChangeScheduleValues()
        {
            bool wasEnabled = _scheduleTrigger.Enabled;
            _scheduleTrigger.Disable();
            _scheduleTrigger = _scheduleTriggerFactory.Create(SetFirstScheduleHeight,
                                                              SetSecondScheduleHeight,
                                                              FirstScheduleTime,
                                                              SecondScheduleTime);
            if (wasEnabled)
            {
                _scheduleTrigger.Enable();
            }
        }

        /// <summary>
        /// Set the height of floodgate to the configured height
        /// </summary>
        public void SetFirstScheduleHeight()
        {
            var floodgate = GetComponent<Floodgate>();
            if (ScheduleEnabled == true &&
                floodgate.Height != FirstScheduleHeight)
            {
                floodgate.SetHeight(FirstScheduleHeight);
            }
        }

        /// <summary>
        /// Set the height of floodgate to the configured height
        /// </summary>
        public void SetSecondScheduleHeight()
        {
            var floodgate = GetComponent<Floodgate>();
            if (ScheduleEnabled == true &&
                floodgate.Height != SecondScheduleHeight)
            {
                floodgate.SetHeight(SecondScheduleHeight);
            }
        }
    }
}
