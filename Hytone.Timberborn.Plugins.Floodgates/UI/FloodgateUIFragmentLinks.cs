using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.WaterBuildings;
using UnityEngine.UIElements;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    public class FloodgateUIFragmentLinks
    {
        private readonly AttachToStreamGaugeButton _attachButton;
        private Floodgate _floodgate;
        private FloodgateTriggerMonoBehaviour _floodgateTriggerMonoBehaviour;
        //private readonly List<>

        public FloodgateUIFragmentLinks(
            AttachToStreamGaugeButton attachButton,
            Floodgate floodgate)
        {
            _attachButton = attachButton;
            _floodgate = floodgate;
        }

        public void InitializeFragment(VisualElement root)
        {
            _attachButton.Initialize(root, () => _floodgateTriggerMonoBehaviour, delegate
            {

            });
        }

        public void ShowFragment(Floodgate floodgate)
        {
            _floodgate = floodgate;
            _floodgateTriggerMonoBehaviour = _floodgate.GetComponent<FloodgateTriggerMonoBehaviour>();
        }

        public void ClearFragment()
        {
            _floodgate = null;
            _floodgateTriggerMonoBehaviour = null;
            _attachButton.StopStreamGaugeAttachment();
        }
        public void UpdateFragment()
        {

        }
    }
}
