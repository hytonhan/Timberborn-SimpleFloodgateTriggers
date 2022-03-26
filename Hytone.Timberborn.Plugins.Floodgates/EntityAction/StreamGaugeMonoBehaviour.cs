using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Timberborn.ConstructibleSystem;
using Timberborn.EntitySystem;
using UnityEngine;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
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
            base.enabled = false;
        }

        //private void Start()
        //{
        //    // HACK: No way this is how this is supposed to be used, but hey, it works!
        //    _entityComponentRegistry.Register(this);
        //}

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
