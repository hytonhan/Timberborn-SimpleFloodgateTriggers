using Bindito.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Timberborn.ConstructibleSystem;
using Timberborn.EntitySystem;
using Timberborn.TickSystem;
using Timberborn.WaterBuildings;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    /// <summary>
    /// Custom behaviour we want to add for StreamGauges
    /// </summary>
    public class StreamGaugeMonoBehaviour : TickableComponent, IRegisteredComponent, IFinishedStateListener
	{
        private List<StreamGaugeFloodgateLink> _floodgateLinks = new List<StreamGaugeFloodgateLink>();
        public ReadOnlyCollection<StreamGaugeFloodgateLink> FloodgateLinks { get; private set; }

        private EntityComponentRegistry _entityComponentRegistry;

        private StreamGauge _streamGauge;
		
        [Inject]
        public void InjectDependencies(
            EntityComponentRegistry entityComponentRegistry)
        {
            _entityComponentRegistry = entityComponentRegistry;
        }


        private void Awake()
        {
            FloodgateLinks = _floodgateLinks.AsReadOnly();
            base.enabled = false;
        }

        public void AttachFloodgate(StreamGaugeFloodgateLink link)
        {
            _floodgateLinks.Add(link);

        }

        public void DetachFloodgate(StreamGaugeFloodgateLink link)
        {
			_floodgateLinks.Remove(link);
		}

		private void DetachAllFloodgates()
        {
			for (int i = _floodgateLinks.Count - 1; i >= 0; i--)
            {
				var link = _floodgateLinks[i];
				link.Floodgate.DetachLink(link);
            }
        }

        public void OnEnterFinishedState()
        {
            base.enabled = true;
            _entityComponentRegistry.Register(this);
            _streamGauge = this.GetComponent<StreamGauge>();
        }

        public void OnExitFinishedState()
        {
            base.enabled = false;
            _entityComponentRegistry.Unregister(this);
            DetachAllFloodgates();
            _streamGauge = null;
        }

        /// <summary>
        /// Check every tick if streamgauge is linked to a floodagte
        /// and if the floodgate's height should be altered
        /// </summary>
        public override void Tick()
        {
            var currHeight = _streamGauge.WaterLevel;
            foreach(var link in FloodgateLinks)
            {
                if (currHeight <= link.Threshold1)
                {
                    var floodgate = link.Floodgate.GetComponent<Floodgate>();
                    if (floodgate.Height != link.Height1)
                    {
                        floodgate.SetHeight(link.Height1);
                    }
                    continue;
                }
                if (currHeight >= link.Threshold2)
                {
                    var floodgate = link.Floodgate.GetComponent<Floodgate>();
                    if (floodgate.Height != link.Height2)
                    {
                        floodgate.SetHeight(link.Height2);
                    }
                }
            }
        }
    }
}
