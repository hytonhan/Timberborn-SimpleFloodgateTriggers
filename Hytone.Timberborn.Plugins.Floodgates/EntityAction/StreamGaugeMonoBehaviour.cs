using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Timberborn.EntitySystem;
using UnityEngine;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    public class StreamGaugeMonoBehaviour : MonoBehaviour, IRegisteredComponent
    {
        private List<StreamGaugeFloodgateLink> _floodgateLinks = new List<StreamGaugeFloodgateLink>();
        public ReadOnlyCollection<StreamGaugeFloodgateLink> FloodgateLinks { get; private set; }
		
		private void Awake()
		{
			//base.enabled = false;
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
	}
}
