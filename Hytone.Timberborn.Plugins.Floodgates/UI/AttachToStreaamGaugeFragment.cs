using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using System;
using System.Globalization;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.SingletonSystem;
using Timberborn.WaterBuildings;
using TimberbornAPI.Common;
using TimberbornAPI.UIBuilderSystem;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.Length.Unit;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    public class AttachToStreamGaugeFragment
    {
        private readonly UIBuilder _builder;
        private readonly AttachToStreamGaugeButton _attachToStreamGaugeButton;
		private FloodgateTriggerMonoBehaviour _floodgateTriggerMonoBehaviour;

        public AttachToStreamGaugeFragment(
            AttachToStreamGaugeButton attachToStreamGaugeButton,
            //FloodgateTriggerMonoBehaviour floodgateTriggerMonoBehaviour,
            UIBuilder builder)
        {
            _attachToStreamGaugeButton = attachToStreamGaugeButton;
            //_floodgateTriggerMonoBehaviour = floodgateTriggerMonoBehaviour;
            _builder = builder;
        }

        //public VisualElement GetView()
        //{
        //    var root = _builder.CreateComponentBuilder()
        //                       .CreateVisualElement()
        //                       .AddPreset(builder => builder.Labels()
        //                                                     .DefaultBig(text: "Foo"))
        //                       .BuildAndInitialize();

        //    return root;
        //}

        public VisualElement InitiliazeFragment(VisualElement parent)
        {
            var root = _builder.CreateFragmentBuilder()
                               //.AddPreset(builder => builder.Labels()
                               //                             .DefaultBig(text: "Foo"))
                               .BuildAndInitialize();

            _attachToStreamGaugeButton.Initialize(parent, () => _floodgateTriggerMonoBehaviour, delegate
            {
                FloodGatesPlugin.Logger.LogInfo($"StreamGauge callback.");
                return;
            });

            return root;
        }

        public void ClearFragment()
        {
            _floodgateTriggerMonoBehaviour = null;
        }

        //public void ShowFragment(GameObject entity)
        //{
        //    _floodgateTriggerMonoBehaviour = entity.GetComponent<FloodgateTriggerMonoBehaviour>();
        //}
        public void ShowFragment(FloodgateTriggerMonoBehaviour floodgateTriggerMonoBehaviour)
        {
            _floodgateTriggerMonoBehaviour = floodgateTriggerMonoBehaviour;
        }

        public void UpdateFragment()
        {
            if ((bool)_floodgateTriggerMonoBehaviour)
            {
                //TODO
                //Update streamgauges
            }
        }
	}
}
