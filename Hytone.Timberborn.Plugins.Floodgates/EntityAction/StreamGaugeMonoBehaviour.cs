using Bindito.Core;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Timberborn.BuildingsBlocking;
using Timberborn.ConstructibleSystem;
using Timberborn.EntitySystem;
using Timberborn.TickSystem;
using Timberborn.WaterBuildings;
using Timberborn.WeatherSystem;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    /// <summary>
    /// Custom behaviour we want to add for StreamGauges
    /// </summary>
    public class StreamGaugeMonoBehaviour : TickableComponent, IRegisteredComponent, IFinishedStateListener
	{
        private List<StreamGaugeFloodgateLink> _floodgateLinks = new List<StreamGaugeFloodgateLink>();
        public ReadOnlyCollection<StreamGaugeFloodgateLink> FloodgateLinks { get; private set; }

        private List<WaterPumpStreamGaugeLink> _waterpumpsLinks = new List<WaterPumpStreamGaugeLink>();
        public ReadOnlyCollection<WaterPumpStreamGaugeLink> WaterpumpLinks { get; private set; }

        private StreamGauge _streamGauge;


        private DroughtService _droughtServíce;

        [Inject]
        public void InjectDependencies(
            DroughtService droughtServíce)
        {
            _droughtServíce = droughtServíce;
        }


        private void Awake()
        {
            FloodgateLinks = _floodgateLinks.AsReadOnly();
            WaterpumpLinks = _waterpumpsLinks.AsReadOnly();
            base.enabled = false;
        }

        public void AttachFloodgate(StreamGaugeFloodgateLink link)
        {
            _floodgateLinks.Add(link);

        }

        public void AttachWaterpump(WaterPumpStreamGaugeLink link)
        {
            _waterpumpsLinks.Add(link);

        }

        public void DetachFloodgate(StreamGaugeFloodgateLink link)
        {
			_floodgateLinks.Remove(link);
        }

        public void DetachWaterpump(WaterPumpStreamGaugeLink link)
        {
            _waterpumpsLinks.Remove(link);
        }

        private void DetachAllFloodgates()
        {
			for (int i = _floodgateLinks.Count - 1; i >= 0; i--)
            {
				var link = _floodgateLinks[i];
				link.Floodgate.DetachLink(link);
            }
        }

        private void DetachAllWaterpumps()
        {
			for (int i = _waterpumpsLinks.Count - 1; i >= 0; i--)
            {
				var link = _waterpumpsLinks[i];
				link.WaterPump.DetachLink(link);
            }
        }

        public void OnEnterFinishedState()
        {
            base.enabled = true;
            _streamGauge = this.GetComponentFast<StreamGauge>();
        }

        public void OnExitFinishedState()
        {
            base.enabled = false;
            DetachAllFloodgates();
            DetachAllWaterpumps();
            _streamGauge = null;
        }

        /// <summary>
        /// Check every tick if streamgauge is linked to a floodagte
        /// and if the floodgate's height should be altered
        /// </summary>
        public override void Tick()
        {
            if(!enabled)
            {
                return;
            }
            var currHeight = _streamGauge.WaterLevel;
            foreach (var link in FloodgateLinks)
            {
                if (_droughtServíce.IsDrought && link.DisableDuringDrought)
                {
                    continue;
                }
                else if (_droughtServíce.IsDrought == false && link.DisableDuringTemperate)
                {
                    continue;
                }
                if (currHeight <= link.Threshold1)
                {
                    var floodgate = link.Floodgate.GetComponentFast<Floodgate>();
                    if (floodgate.Height != link.Height1)
                    {
                        floodgate.SetHeightAndSynchronize(link.Height1);
                    }
                    continue;
                }
                if (currHeight >= link.Threshold2)
                {
                    var floodgate = link.Floodgate.GetComponentFast<Floodgate>();
                    if (floodgate.Height != link.Height2)
                    {
                        floodgate.SetHeightAndSynchronize(link.Height2);
                    }
                }
            }
            foreach (var link in WaterpumpLinks)
            {
                if (_droughtServíce.IsDrought && link.DisableDuringDrought)
                {
                    continue;
                }
                else if(_droughtServíce.IsDrought == false && link.DisableDuringTemperate)
                {
                    continue;
                }

                var constructible = link.WaterPump.GetComponentFast<Constructible>();
                if (constructible.IsUnfinished)
                {
                    continue;
                }
                var pausable = link.WaterPump.GetComponentFast<PausableBuilding>();
                if (currHeight <= link.Threshold1 && link.Enabled1)
                {
                    if (pausable.Paused == false)
                    {
                        pausable.Pause();
                    }
                }
                else if (currHeight >= link.Threshold2 && link.Enabled2)
                {
                    if (pausable.Paused == false)
                    {
                        pausable.Pause();
                    }
                }
                else if (currHeight <= link.Threshold3 && link.Enabled3)
                {
                    if (pausable.Paused)
                    {
                        pausable.Resume();
                    }
                }
                else if (currHeight >= link.Threshold4 && link.Enabled4)
                {
                    if (pausable.Paused)
                    {
                        pausable.Resume();
                    }
                }
            }
        }
    }
}
