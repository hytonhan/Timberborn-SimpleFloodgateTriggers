using Bindito.Core;
using Hytone.Timberborn.Plugins.Floodgates.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Timberborn.Persistence;
using Timberborn.WeatherSystem;
using Timberborn.BuildingsBlocking;
using Timberborn.BaseComponentSystem;
using Timberborn.DeconstructionSystem;
using Timberborn.HazardousWeatherSystem;
using Timberborn.BlockSystem;
using System.Reflection;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using Timberborn.WaterSourceSystem;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterSourceRegulators
{
    public class WaterSourceRegulatorMonobehaviour : BaseComponent, IPersistentEntity, IFinishedStateListener
    {
        //Keys used in data saving/loading
        private static readonly ComponentKey WaterPumpKey = new ComponentKey(nameof(WaterSourceRegulatorMonobehaviour));
        private static readonly PropertyKey<bool> CloseOnDroughtStartKey = new PropertyKey<bool>(nameof(CloseOnDroughtStart));
        private static readonly PropertyKey<bool> OpenOnDroughtStartKey = new PropertyKey<bool>(nameof(OpenOnDroughtStart));
        private static readonly PropertyKey<bool> CloseOnTemperateStartKey = new PropertyKey<bool>(nameof(CloseOnTemperateStarted));
        private static readonly PropertyKey<bool> OpenOnTemperateStartedKey = new PropertyKey<bool>(nameof(OpenOnTemperateStarted));
        private static readonly PropertyKey<bool> CloseOnBadtideStartedKey = new PropertyKey<bool>(nameof(CloseOnBadtideStarted));
        private static readonly PropertyKey<bool> OpenOnBadtideStartedKey = new PropertyKey<bool>(nameof(OpenOnBadtideStarted));

        private static readonly PropertyKey<bool> ScheduleEnabledKey = new PropertyKey<bool>(nameof(ScheduleEnabled));
        private static readonly PropertyKey<bool> DisableScheduleOnDroughtKey = new PropertyKey<bool>(nameof(DisableScheduleOnDrought));
        private static readonly PropertyKey<bool> DisableScheduleOnTemperateKey = new PropertyKey<bool>(nameof(DisableScheduleOnTemperate));
        private static readonly PropertyKey<bool> DisableScheduleOnBadtideKey = new PropertyKey<bool>(nameof(DisableScheduleOnBadtide));

        private static readonly PropertyKey<float> CloseOnScheduleTimeKey = new PropertyKey<float>(nameof(CloseOnScheduleTime));
        private static readonly PropertyKey<float> OpenOnScheduleTimeKey = new PropertyKey<float>(nameof(OpenOnScheduleTime));

        private static readonly ListKey<WaterSourceRegulatorStreamGaugeLink> WaterpumpLinksKey = new ListKey<WaterSourceRegulatorStreamGaugeLink>(nameof(WaterSourceRegulatorLinks));

        private IScheduleTriggerFactory _scheduleTriggerFactory;
        private IScheduleTrigger _scheduleTrigger;
        private WeatherService _weatherServíce;
        private WaterSourceRegulatorLinkSerializer _linkSerializer;

        private readonly List<WaterSourceRegulatorStreamGaugeLink> _waterSourceRegulatorLinks = new List<WaterSourceRegulatorStreamGaugeLink>();
        public ReadOnlyCollection<WaterSourceRegulatorStreamGaugeLink> WaterSourceRegulatorLinks { get; private set; }

        public bool CloseOnDroughtStart { get; set; }
        public bool OpenOnDroughtStart { get; set; }
        public bool CloseOnTemperateStarted { get; set; }
        public bool OpenOnTemperateStarted { get; set; }
        public bool CloseOnBadtideStarted { get; set; }
        public bool OpenOnBadtideStarted { get; set; }


        public bool ScheduleEnabled { get; set; }
        public bool DisableScheduleOnDrought { get; set; }
        public bool DisableScheduleOnTemperate { get; set; }
        public bool DisableScheduleOnBadtide { get; set; }

        public float CloseOnScheduleTime { get; set; }
        public float OpenOnScheduleTime { get; set; }

        public int MaxStreamGaugeLinks = 1;


        [Inject]
        public void InjectDependencies(
            IScheduleTriggerFactory scheduleTriggerFactory,
            WeatherService weatherService,
            WaterSourceRegulatorLinkSerializer linkSerializer)
        {
            _scheduleTriggerFactory = scheduleTriggerFactory;
            _weatherServíce = weatherService;
            _linkSerializer = linkSerializer;
        }

        public void Awake()
        {
            WaterSourceRegulatorLinks = _waterSourceRegulatorLinks.AsReadOnly();
        }

        public void Save(IEntitySaver entitySaver)
        {
            IObjectSaver component = entitySaver.GetComponent(WaterPumpKey);

            component.Set(CloseOnDroughtStartKey, CloseOnDroughtStart);
            component.Set(OpenOnDroughtStartKey, OpenOnDroughtStart);
            component.Set(CloseOnTemperateStartKey, CloseOnTemperateStarted);
            component.Set(OpenOnTemperateStartedKey, OpenOnTemperateStarted);
            component.Set(CloseOnBadtideStartedKey, CloseOnBadtideStarted);
            component.Set(OpenOnBadtideStartedKey, OpenOnBadtideStarted);

            component.Set(ScheduleEnabledKey, ScheduleEnabled);
            component.Set(DisableScheduleOnDroughtKey, DisableScheduleOnDrought);
            component.Set(DisableScheduleOnTemperateKey, DisableScheduleOnTemperate);
            component.Set(DisableScheduleOnBadtideKey, DisableScheduleOnBadtide);
            component.Set(CloseOnScheduleTimeKey, CloseOnScheduleTime);
            component.Set(OpenOnScheduleTimeKey, OpenOnScheduleTime);

            component.Set(WaterpumpLinksKey, WaterSourceRegulatorLinks, _linkSerializer);
        }

        public void Load(IEntityLoader entityLoader)
        {
            if (!entityLoader.HasComponent(WaterPumpKey))
            {
                return;
            }
            IObjectLoader component = entityLoader.GetComponent(WaterPumpKey);
            if (component.Has(CloseOnDroughtStartKey))
            {
                CloseOnDroughtStart = component.Get(CloseOnDroughtStartKey);
            }
            if (component.Has(OpenOnDroughtStartKey))
            {
                OpenOnDroughtStart = component.Get(OpenOnDroughtStartKey);
            }
            if (component.Has(CloseOnTemperateStartKey))
            {
                CloseOnTemperateStarted = component.Get(CloseOnTemperateStartKey);
            }
            if (component.Has(OpenOnTemperateStartedKey))
            {
                OpenOnTemperateStarted = component.Get(OpenOnTemperateStartedKey);
            }
            if (component.Has(ScheduleEnabledKey))
            {
                ScheduleEnabled = component.Get(ScheduleEnabledKey);
            }
            if (component.Has(DisableScheduleOnDroughtKey))
            {
                DisableScheduleOnDrought = component.Get(DisableScheduleOnDroughtKey);
            }
            if (component.Has(DisableScheduleOnTemperateKey))
            {
                DisableScheduleOnTemperate = component.Get(DisableScheduleOnTemperateKey);
            }
            if (component.Has(CloseOnScheduleTimeKey))
            {
                CloseOnScheduleTime = component.Get(CloseOnScheduleTimeKey);
            }
            if (component.Has(OpenOnScheduleTimeKey))
            {
                OpenOnScheduleTime = component.Get(OpenOnScheduleTimeKey);
            }
            if (component.Has(WaterpumpLinksKey))
            {
                _waterSourceRegulatorLinks.AddRange(component.Get(WaterpumpLinksKey, _linkSerializer));

                foreach (var link in WaterSourceRegulatorLinks)
                {
                    PostAttachLink(link);
                }
            }
            if (component.Has(CloseOnBadtideStartedKey))
            {
                CloseOnBadtideStarted = component.Get(CloseOnBadtideStartedKey);
            }
            if (component.Has(OpenOnBadtideStartedKey))
            {
                OpenOnBadtideStarted = component.Get(OpenOnBadtideStartedKey);
            }
            if (component.Has(DisableScheduleOnBadtideKey))
            {
                DisableScheduleOnBadtide = component.Get(DisableScheduleOnBadtideKey);
            }
        }

        public void OnEnterFinishedState()
        {
            _scheduleTrigger = _scheduleTriggerFactory.Create(CloseBuilding,
                                                              OpenBuilding,
                                                              CloseOnScheduleTime,
                                                              OpenOnScheduleTime);
            if (ScheduleEnabled)
            {
                _scheduleTrigger.Enable();
            }
        }

        public void OnExitFinishedState()
        {
            _scheduleTrigger?.Disable();
            DetachAllLinks();
        }

        public void OnDroughtStarted()
        {
            var constructible = GetComponentFast<BlockObject>();
            if (constructible.IsUnfinished)
            {
                return;
            }
            var regulator = GetComponentFast<WaterSourceRegulator>();

            if (CloseOnDroughtStart == true && regulator.IsOpen)
            {
                regulator.Close();
            }
            else if (OpenOnDroughtStart == true && regulator.IsOpen == false)
            {
                regulator.Open();
            }
            if (ScheduleEnabled && !DisableScheduleOnDrought)
            {
                _scheduleTrigger.Enable();
            }
            else if (DisableScheduleOnDrought)
            {
                _scheduleTrigger.Disable();
            }
        }

        public void OnBadtideStarted()
        {
            var constructible = GetComponentFast<BlockObject>();
            if (constructible.IsUnfinished)
            {
                return;
            }
            var regulator = GetComponentFast<WaterSourceRegulator>();

            if (CloseOnBadtideStarted == true && regulator.IsOpen)
            {
                regulator.Close();
            }
            else if (OpenOnBadtideStarted == true && regulator.IsOpen == false)
            {
                regulator.Open();
            }
            if (ScheduleEnabled && !DisableScheduleOnBadtide)
            {
                _scheduleTrigger.Enable();
            }
            else if (DisableScheduleOnBadtide)
            {
                _scheduleTrigger.Disable();
            }
        }

        public void OnTemperateStarted()
        {
            var constructible = GetComponentFast<BlockObject>();
            if (constructible.IsUnfinished)
            {
                return;
            }
            var regulator = GetComponentFast<WaterSourceRegulator>();

            if (CloseOnTemperateStarted == true && regulator.IsOpen)
            {
                regulator.Close();
            }
            else if(OpenOnTemperateStarted == true && regulator.IsOpen == false)
            {
                regulator.Open();
            }
            if (ScheduleEnabled && !DisableScheduleOnTemperate)
            {
                _scheduleTrigger.Enable();
            }
            else if (DisableScheduleOnTemperate)
            {
                _scheduleTrigger.Disable();
            }
        }

        public void AttachLink(StreamGaugeMonoBehaviour streamGauge)
        {

            var link = new WaterSourceRegulatorStreamGaugeLink(this, streamGauge);
            _waterSourceRegulatorLinks.Add(link);
            PostAttachLink(link);
        }

        public void PostAttachLink(WaterSourceRegulatorStreamGaugeLink link)
        {
            link.StreamGauge.AttachWaterSourceRegulator(link);
        }

        public void DetachAllLinks()
        {
            foreach (var link in _waterSourceRegulatorLinks)
            {
                PostDetachLink(link);
            }
            _waterSourceRegulatorLinks.Clear();
        }
        public void DetachLink(WaterSourceRegulatorStreamGaugeLink link)
        {
            if (!_waterSourceRegulatorLinks.Remove(link))
            {
                throw new InvalidOperationException($"Coudln't remove {link} from {this}, it wasn't added.");
            }
            PostDetachLink(link);
        }

        private void PostDetachLink(WaterSourceRegulatorStreamGaugeLink link)
        {
            link.StreamGauge.DetachWaterSourceRegulator(link);
        }

        public void ChangeScheduleValues()
        {
            bool wasEnabled = _scheduleTrigger?.Enabled ?? false;
            if (_scheduleTrigger != null)
            {
                _scheduleTrigger.Disable();
            }
            _scheduleTrigger = _scheduleTriggerFactory.Create(OpenBuilding,
                                                              CloseBuilding,
                                                              CloseOnScheduleTime,
                                                              OpenOnScheduleTime);
            if (wasEnabled)
            {
                _scheduleTrigger.Enable();
            }
        }

        public void OnChangedScheduleToggles()
        {
            if (_scheduleTrigger == null)
            {
                return;
            }
            if (!ScheduleEnabled)
            {
                _scheduleTrigger.Disable();
                return;
            }
            if (_weatherServíce.IsHazardousWeather)
            {
                var hazardService = (HazardousWeatherService)typeof(WeatherService).GetField("_hazardousWeatherService", BindingFlags.NonPublic | BindingFlags.Instance)
                                                                               .GetValue(_weatherServíce);
                var hazardType = hazardService.CurrentCycleHazardousWeather.GetType();
                if (hazardType == typeof(DroughtWeather) &&
                    DisableScheduleOnDrought)
                {
                    _scheduleTrigger.Disable();
                    return;
                }
                else if(hazardType == typeof(BadtideWeather))
                {
                    _scheduleTrigger.Disable();
                    return;
                }
                _scheduleTrigger.Enable();
                return;
            }
            else
            {
                if (DisableScheduleOnTemperate)
                {
                    _scheduleTrigger.Disable();
                    return;
                }
                _scheduleTrigger.Enable();
                return;
            }
        }

        public void OpenBuilding()
        {
            var regulator = GetComponentFast<WaterSourceRegulator>();
            var constructible = GetComponentFast<BlockObject>();

            if (ScheduleEnabled == true &&
                regulator.IsOpen == false &&
                constructible.IsFinished)
            {
                regulator.Open();
            }
        }

        public void CloseBuilding()
        {
            var regulator = GetComponentFast<WaterSourceRegulator>();
            var constructible = GetComponentFast<BlockObject>();
            if (ScheduleEnabled == true &&
                regulator.IsOpen &&
                constructible.IsFinished)
            {
                regulator.Close();
            }
        }
    }
}
