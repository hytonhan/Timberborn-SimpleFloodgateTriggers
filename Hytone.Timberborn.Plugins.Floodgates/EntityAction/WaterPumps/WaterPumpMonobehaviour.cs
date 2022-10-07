using Bindito.Core;
using Hytone.Timberborn.Plugins.Floodgates.Schedule;
using Timberborn.ConstructibleSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Timberborn.Persistence;
using Timberborn.WaterBuildings;
using Timberborn.WeatherSystem;
using UnityEngine;
using Timberborn.Buildings;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps
{
    public class WaterPumpMonobehaviour : MonoBehaviour, IPersistentEntity, IFinishedStateListener
    {
        //Keys used in data saving/loading
        private static readonly ComponentKey WaterPumpKey = new ComponentKey(nameof(WaterPumpMonobehaviour));
        private static readonly PropertyKey<bool> PauseOnDroughtStartKey = new PropertyKey<bool>(nameof(PauseOnDroughtStart));
        private static readonly PropertyKey<bool> UnpauseOnDroughtStartKey = new PropertyKey<bool>(nameof(UnpauseOnDroughtStart));
        private static readonly PropertyKey<bool> PauseOnDroughtEndedKey = new PropertyKey<bool>(nameof(PauseOnDroughtEnded));
        private static readonly PropertyKey<bool> UnpauseOnDroughtEndedKey = new PropertyKey<bool>(nameof(UnpauseOnDroughtEnded));

        private static readonly PropertyKey<bool> ScheduleEnabledKey = new PropertyKey<bool>(nameof(ScheduleEnabled));
        private static readonly PropertyKey<bool> DisableScheduleOnDroughtKey = new PropertyKey<bool>(nameof(DisableScheduleOnDrought));
        private static readonly PropertyKey<bool> DisableScheduleOnTemperateKey = new PropertyKey<bool>(nameof(DisableScheduleOnTemperate));

        private static readonly PropertyKey<float> PauseOnScheduleTimeKey = new PropertyKey<float>(nameof(PauseOnScheduleTime));
        private static readonly PropertyKey<float> ResumeOnScheduleTimeKey = new PropertyKey<float>(nameof(ResumeOnScheduleTime));

        private static readonly ListKey<WaterPumpStreamGaugeLink> WaterpumpLinksKey = new ListKey<WaterPumpStreamGaugeLink>(nameof(WaterPumpLinks));

        private IScheduleTriggerFactory _scheduleTriggerFactory;
        private IScheduleTrigger _scheduleTrigger;
        private DroughtService _droughtServíce;
        private WaterpumpStreamGaugeLinkSerializer _linkSerializer;

        private readonly List<WaterPumpStreamGaugeLink> _waterpumpLinks = new List<WaterPumpStreamGaugeLink>();
        public ReadOnlyCollection<WaterPumpStreamGaugeLink> WaterPumpLinks { get; private set; }

        public bool PauseOnDroughtStart { get; set; }
        public bool UnpauseOnDroughtStart { get; set; }
        public bool PauseOnDroughtEnded { get; set; }
        public bool UnpauseOnDroughtEnded { get; set; }


        public bool ScheduleEnabled { get; set; }
        public bool DisableScheduleOnDrought { get; set; }
        public bool DisableScheduleOnTemperate { get; set; }

        public float PauseOnScheduleTime { get; set; }
        public float ResumeOnScheduleTime { get; set; }

        public int MaxStreamGaugeLinks = 1;


        [Inject]
        public void InjectDependencies(
            IScheduleTriggerFactory scheduleTriggerFactory,
            DroughtService droughtService,
            WaterpumpStreamGaugeLinkSerializer linkSerializer)
        {
            _scheduleTriggerFactory = scheduleTriggerFactory;
            _droughtServíce = droughtService;
            _linkSerializer = linkSerializer;
        }

        public void Awake()
        {
            WaterPumpLinks = _waterpumpLinks.AsReadOnly();
        }

        public void Save(IEntitySaver entitySaver)
        {
            IObjectSaver component = entitySaver.GetComponent(WaterPumpKey);

            component.Set(PauseOnDroughtStartKey, PauseOnDroughtStart);
            component.Set(UnpauseOnDroughtStartKey, UnpauseOnDroughtStart);
            component.Set(PauseOnDroughtEndedKey, PauseOnDroughtEnded);
            component.Set(UnpauseOnDroughtEndedKey, UnpauseOnDroughtEnded);

            component.Set(ScheduleEnabledKey, ScheduleEnabled);
            component.Set(DisableScheduleOnDroughtKey, DisableScheduleOnDrought);
            component.Set(DisableScheduleOnTemperateKey, DisableScheduleOnTemperate);
            component.Set(PauseOnScheduleTimeKey, PauseOnScheduleTime);
            component.Set(ResumeOnScheduleTimeKey, ResumeOnScheduleTime);

            component.Set(WaterpumpLinksKey, WaterPumpLinks, _linkSerializer);
        }

        public void Load(IEntityLoader entityLoader)
        {
            if (!entityLoader.HasComponent(WaterPumpKey))
            {
                return;
            }
            IObjectLoader component = entityLoader.GetComponent(WaterPumpKey);
            if (component.Has(PauseOnDroughtStartKey))
            {
                PauseOnDroughtStart = component.Get(PauseOnDroughtStartKey);
            }
            if (component.Has(UnpauseOnDroughtStartKey))
            {
                UnpauseOnDroughtStart = component.Get(UnpauseOnDroughtStartKey);
            }
            if (component.Has(PauseOnDroughtEndedKey))
            {
                PauseOnDroughtEnded = component.Get(PauseOnDroughtEndedKey);
            }
            if (component.Has(UnpauseOnDroughtEndedKey))
            {
                UnpauseOnDroughtEnded = component.Get(UnpauseOnDroughtEndedKey);
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
            if (component.Has(PauseOnScheduleTimeKey))
            {
                PauseOnScheduleTime = component.Get(PauseOnScheduleTimeKey);
            }
            if (component.Has(ResumeOnScheduleTimeKey))
            {
                ResumeOnScheduleTime = component.Get(ResumeOnScheduleTimeKey);
            }
            if (component.Has(WaterpumpLinksKey))
            {
                _waterpumpLinks.AddRange(component.Get(WaterpumpLinksKey, _linkSerializer));

                foreach (var link in WaterPumpLinks)
                {
                    PostAttachLink(link);
                }
            }
        }

        public void OnEnterFinishedState()
        {
            _scheduleTrigger = _scheduleTriggerFactory.Create(PauseBuilding,
                                                              ResumeBuilding,
                                                              PauseOnScheduleTime,
                                                              ResumeOnScheduleTime);
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
            var pausable = GetComponent<PausableBuilding>();

            if (PauseOnDroughtStart == true &&
               pausable.Paused == false)
            {
                pausable.Pause();
            }
            else if (UnpauseOnDroughtStart == true && pausable.Paused == true)
            {
                pausable.Resume();
            }
        }

        public void OnDroughtEnded()
        {
            var pausable = GetComponent<PausableBuilding>();

            if (UnpauseOnDroughtEnded == true &&
                pausable.Paused == true)
            {
                pausable.Resume();
            }
            else if(PauseOnDroughtEnded == true && pausable.Paused == false)
            {
                pausable.Pause();
            }
        }

        public void AttachLink(WaterPumpMonobehaviour waterpump,
                               StreamGaugeMonoBehaviour streamGauge)
        {

            var link = new WaterPumpStreamGaugeLink(waterpump, streamGauge);
            _waterpumpLinks.Add(link);
            PostAttachLink(link);
        }

        public void PostAttachLink(WaterPumpStreamGaugeLink link)
        {
            link.StreamGauge.AttachWaterpump(link);
        }

        public void DetachAllLinks()
        {
            foreach (var link in _waterpumpLinks)
            {
                PostDetachLink(link);
            }
            _waterpumpLinks.Clear();
        }
        public void DetachLink(WaterPumpStreamGaugeLink link)
        {
            if (!_waterpumpLinks.Remove(link))
            {
                throw new InvalidOperationException($"Coudln't remove {link} from {this}, it wasn't added.");
            }
            PostDetachLink(link);
        }

        private void PostDetachLink(WaterPumpStreamGaugeLink link)
        {
            link.StreamGauge.DetachWaterpump(link);
        }

        public void ChangeScheduleValues()
        {
            bool wasEnabled = _scheduleTrigger?.Enabled ?? false;
            if (_scheduleTrigger != null)
            {
                _scheduleTrigger.Disable();
            }
            _scheduleTrigger = _scheduleTriggerFactory.Create(PauseBuilding,
                                                              ResumeBuilding,
                                                              PauseOnScheduleTime,
                                                              ResumeOnScheduleTime);
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
            if (_droughtServíce.IsDrought)
            {
                if (DisableScheduleOnDrought)
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

        public void PauseBuilding()
        {
            var pausable = GetComponent<PausableBuilding>();
            if (ScheduleEnabled == true &&
                pausable.Paused == false)
            {
                pausable.Pause();
            }
        }

        public void ResumeBuilding()
        {
            var pausable = GetComponent<PausableBuilding>();
            if (ScheduleEnabled == true &&
                pausable.Paused == true)
            {
                pausable.Resume();
            }
        }
    }
}
