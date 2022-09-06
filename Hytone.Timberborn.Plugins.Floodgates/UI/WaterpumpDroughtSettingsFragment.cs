using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Timberborn.Common;
using Timberborn.Localization;
using Timberborn.SelectionSystem;
using Timberborn.WaterBuildings;
using TimberbornAPI.Common;
using TimberbornAPI.UIBuilderSystem;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.Length.Unit;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    public class WaterpumpDroughtSettingsFragment
    {
        private readonly UIBuilder _builder;
        private VisualElement _root;
        private readonly ILoc _loc;
        private readonly AttachToStreamGaugeButton _attachToStreamGaugeButton;
        private WaterPumpMonobehaviour _waterpumpMonoBehaviour;


        private Toggle _droughtStartedEnabledToggle;
        private Toggle _droughtEndedEnabledToggle;

        private WaterPumpMonobehaviour _waterPumpMonobehaviour;


        public WaterpumpDroughtSettingsFragment(
            AttachToStreamGaugeButton attachToStreamGaugeButton,
            UIBuilder builder,
            SelectionManager selectionManager,
            LinkViewFactory streamGaugeFloodgateLinkViewFactory,
            ILoc loc)
        {
            _attachToStreamGaugeButton = attachToStreamGaugeButton;
            _builder = builder;
            _loc = loc;
        }


        public VisualElement InitiliazeFragment(VisualElement parent)
        {
            var foo = _builder.CreateComponentBuilder()
                              .CreateVisualElement()
                              .AddPreset(factory => factory.Toggles()
                                                      .CheckmarkInverted(locKey: "Floodgate.Triggers.PauseOnDroughtStart",
                                                                         name: "PauseOnDroughtStart",
                                                                         fontStyle: FontStyle.Normal,
                                                                         color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                         builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                    .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))));
            var foo2 = _builder.CreateComponentBuilder()
                               .CreateVisualElement()
                               .AddPreset(factory => factory.Toggles()
                                                      .CheckmarkInverted(locKey: "Floodgate.Triggers.UnpauseOnDroughtEnded",
                                                                         name: "UnpauseOnDroughtEnded",
                                                                         fontStyle: FontStyle.Normal,
                                                                         color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                         builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                    .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))));

            var root = _builder.CreateComponentBuilder()
                               .CreateVisualElement();

            root.AddComponent(foo.Build());
            root.AddComponent(foo2.Build());

            _root = root.BuildAndInitialize();

            _droughtStartedEnabledToggle = _root.Q<Toggle>("PauseOnDroughtStart");
            _droughtStartedEnabledToggle.RegisterValueChangedCallback(ToggleDroughtStarted);

            _droughtEndedEnabledToggle = _root.Q<Toggle>("UnpauseOnDroughtEnded");
            _droughtEndedEnabledToggle.RegisterValueChangedCallback(ToggleDroughtEnded);

            return _root;
        }

        public void ShowFragment(WaterPumpMonobehaviour waterpumpMonobehaviour)
        {
            if ((bool)waterpumpMonobehaviour)
            {
                _waterPumpMonobehaviour = waterpumpMonobehaviour;
            }
        }

        public void UpdateFragment()
        {

        }

        public void ClearFragment()
        {
            _waterPumpMonobehaviour = null;
        }

        private void ToggleDroughtEnded(ChangeEvent<bool> changeEvent)
        {
            _waterPumpMonobehaviour.UnpauseOnDroughtEnded = changeEvent.newValue;
        }
        private void ToggleDroughtStarted(ChangeEvent<bool> changeEvent)
        {
            _waterPumpMonobehaviour.PauseOnDroughtStart = changeEvent.newValue;
        }
    }
}
