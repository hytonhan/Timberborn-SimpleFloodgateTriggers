using Bindito.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Timberborn.ConstructibleSystem;
using Timberborn.EntitySystem;
using UnityEngine;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    /// <summary>
    /// Custom behaviour we want to add for StreamGauges
    /// </summary>
    public class StreamGaugeMonoBehaviour : MonoBehaviour, IRegisteredComponent, IFinishedStateListener
	{
        private List<StreamGaugeFloodgateLink> _floodgateLinks = new List<StreamGaugeFloodgateLink>();
        public ReadOnlyCollection<StreamGaugeFloodgateLink> FloodgateLinks { get; private set; }

        private EntityComponentRegistry _entityComponentRegistry;
		
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
        }

        public void OnExitFinishedState()
        {
            base.enabled = false;
            _entityComponentRegistry.Unregister(this);
            DetachAllFloodgates();
        }
    }
}
