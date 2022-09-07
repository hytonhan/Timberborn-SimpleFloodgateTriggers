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
        private readonly AttachFloodgateToStreamGaugeButton _attachToStreamGaugeButton;
        //private WaterPumpMonobehaviour _waterpumpMonoBehaviour;


        private Toggle _droughtStartedPauseToggle;
        private Toggle _droughtStartedResumeToggle;
        private Toggle _droughtEndedPauseToggle;
        private Toggle _droughtEndedResumeToggle;

        private WaterPumpMonobehaviour _waterPumpMonobehaviour;


        public WaterpumpDroughtSettingsFragment(
            AttachFloodgateToStreamGaugeButton attachToStreamGaugeButton,
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
            var pauseOnDrought = _builder.CreateComponentBuilder()
                                         .CreateVisualElement()
                                         .AddPreset(factory => factory.Toggles()
                                                                 .CheckmarkInverted(locKey: "Floodgate.Triggers.PauseOnDroughtStart",
                                                                                    name: "PauseOnDroughtStart",
                                                                                    fontStyle: FontStyle.Normal,
                                                                                    color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                                    builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                   .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))));
            
            var unpauseOnDrought = _builder.CreateComponentBuilder()
                                           .CreateVisualElement()
                                           .AddPreset(factory => factory.Toggles()
                                                                   .CheckmarkInverted(locKey: "Floodgate.Triggers.UnpauseOnDroughtStart",
                                                                                       name: "UnpauseOnDroughtStart",
                                                                                       fontStyle: FontStyle.Normal,
                                                                                       color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                                       builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                               .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(30, Pixel), 0))));
            var unpauseOnTemperate = _builder.CreateComponentBuilder()
                                             .CreateVisualElement()
                                             .AddPreset(factory => factory.Toggles()
                                                                    .CheckmarkInverted(locKey: "Floodgate.Triggers.UnpauseOnDroughtEnded",
                                                                                       name: "UnpauseOnDroughtEnded",
                                                                                       fontStyle: FontStyle.Normal,
                                                                                       color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                                       builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                                  .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))));

            var pauseOnTemperate = _builder.CreateComponentBuilder()
                                           .CreateVisualElement()
                                           .AddPreset(factory => factory.Toggles()
                                                                  .CheckmarkInverted(locKey: "Floodgate.Triggers.PauseOnDroughtEnded",
                                                                                     name: "PauseOnDroughtEnded",
                                                                                     fontStyle: FontStyle.Normal,
                                                                                     color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                                     builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                                .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))));

            var root = _builder.CreateComponentBuilder()
                               .CreateVisualElement();

            root.AddComponent(pauseOnDrought.Build());
            root.AddComponent(unpauseOnDrought.Build());
            root.AddComponent(pauseOnTemperate.Build());
            root.AddComponent(unpauseOnTemperate.Build());

            _root = root.BuildAndInitialize();

            _droughtStartedPauseToggle = _root.Q<Toggle>("PauseOnDroughtStart");
            _droughtStartedPauseToggle.RegisterValueChangedCallback(TogglePauseDroughtStarted);

            _droughtStartedResumeToggle = _root.Q<Toggle>("UnpauseOnDroughtStart");
            _droughtStartedResumeToggle.RegisterValueChangedCallback(ToggleUnpauseDroughtStarted);

            _droughtEndedPauseToggle = _root.Q<Toggle>("PauseOnDroughtEnded");
            _droughtEndedPauseToggle.RegisterValueChangedCallback(TogglePauseDroughtEnded);

            _droughtEndedResumeToggle = _root.Q<Toggle>("UnpauseOnDroughtEnded");
            _droughtEndedResumeToggle.RegisterValueChangedCallback(ToggleUnpauseDroughtEnded);

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

        private void ToggleUnpauseDroughtEnded(ChangeEvent<bool> changeEvent)
        {
            _waterPumpMonobehaviour.UnpauseOnDroughtEnded = changeEvent.newValue;
            if(changeEvent.newValue == true && _droughtEndedPauseToggle.value == true)
            {
                _droughtEndedPauseToggle.value = false;
                _waterPumpMonobehaviour.PauseOnDroughtEnded = false;
            }

        }
        private void TogglePauseDroughtEnded(ChangeEvent<bool> changeEvent)
        {
            _waterPumpMonobehaviour.PauseOnDroughtEnded = changeEvent.newValue;
            if (changeEvent.newValue == true && _droughtEndedResumeToggle.value == true)
            {
                _droughtEndedResumeToggle.value = false;
                _waterPumpMonobehaviour.UnpauseOnDroughtEnded = false;
            }
        }

        private void TogglePauseDroughtStarted(ChangeEvent<bool> changeEvent)
        {
            _waterPumpMonobehaviour.PauseOnDroughtStart = changeEvent.newValue;
            if (changeEvent.newValue == true && _droughtStartedResumeToggle.value == true)
            {
                _droughtStartedResumeToggle.value = false;
                _waterPumpMonobehaviour.UnpauseOnDroughtStart = false;
            }
        }
        private void ToggleUnpauseDroughtStarted(ChangeEvent<bool> changeEvent)
        {
            _waterPumpMonobehaviour.UnpauseOnDroughtStart = changeEvent.newValue;
            if (changeEvent.newValue == true && _droughtStartedPauseToggle.value == true)
            {
                _droughtStartedPauseToggle.value = false;
                _waterPumpMonobehaviour.PauseOnDroughtStart = false;
            }
        }
    }
}
