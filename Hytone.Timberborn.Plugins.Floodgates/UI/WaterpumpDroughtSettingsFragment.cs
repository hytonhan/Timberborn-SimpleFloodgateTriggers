using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using TimberApi.UiBuilderSystem;
using Timberborn.Localization;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.Length.Unit;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    public class WaterpumpDroughtSettingsFragment
    {
        private readonly UIBuilder _builder;
        private VisualElement _root;


        private Toggle _droughtStartedPauseToggle;
        private Toggle _droughtStartedResumeToggle;

        private Toggle _temperateStartedPauseToggle;
        private Toggle _temperateStartedResumeToggle;

        private Toggle _badtideStartedPauseToggle;
        private Toggle _badtideStartedResumeToggle;

        private WaterPumpMonobehaviour _waterPumpMonobehaviour;


        public WaterpumpDroughtSettingsFragment(UIBuilder builder)
        {
            _builder = builder;
        }


        public VisualElement InitiliazeFragment(VisualElement parent)
        {
            var pauseOnDrought = _builder.CreateComponentBuilder()
                                         .CreateVisualElement()
                                         .AddPreset(factory => factory.Toggles()
                                                                 .CheckmarkInverted(locKey: "Floodgate.Triggers.PauseOnDroughtStart",
                                                                                    name: "PauseOnDroughtStartToggle",
                                                                                    fontStyle: FontStyle.Normal,
                                                                                    color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                                    builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                   .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))));

            var unpauseOnDrought = _builder.CreateComponentBuilder()
                                           .CreateVisualElement()
                                           .AddPreset(factory => factory.Toggles()
                                                                   .CheckmarkInverted(locKey: "Floodgate.Triggers.UnpauseOnDroughtStart",
                                                                                       name: "UnpauseOnDroughtStartToggle",
                                                                                       fontStyle: FontStyle.Normal,
                                                                                       color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                                       builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                               .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(30, Pixel), 0))));
            var unpauseOnTemperate = _builder.CreateComponentBuilder()
                                             .CreateVisualElement()
                                             .AddPreset(factory => factory.Toggles()
                                                                    .CheckmarkInverted(locKey: "Floodgate.Triggers.UnpauseOnTemperateStart",
                                                                                       name: "UnpauseOnTemperateStartToggle",
                                                                                       fontStyle: FontStyle.Normal,
                                                                                       color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                                       builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                                  .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))));

            var pauseOnTemperate = _builder.CreateComponentBuilder()
                                           .CreateVisualElement()
                                           .AddPreset(factory => factory.Toggles()
                                                                  .CheckmarkInverted(locKey: "Floodgate.Triggers.PauseOnTemperateStart",
                                                                                     name: "PauseOnTemperateStartToggle",
                                                                                     fontStyle: FontStyle.Normal,
                                                                                     color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                                     builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                                .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))));
            var unpauseOnBadtide = _builder.CreateComponentBuilder()
                                           .CreateVisualElement()
                                           .AddPreset(factory => factory.Toggles()
                                                                  .CheckmarkInverted(locKey: "Floodgate.Triggers.UnpauseOnBadtideStart",
                                                                                     name: "UnpauseOnBadtideStartToggle",
                                                                                     fontStyle: FontStyle.Normal,
                                                                                     color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                                     builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                                .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))));

            var pauseOnBadtide = _builder.CreateComponentBuilder()
                                         .CreateVisualElement()
                                         .AddPreset(factory => factory.Toggles()
                                                                .CheckmarkInverted(locKey: "Floodgate.Triggers.PauseOnBadtideStart",
                                                                                   name: "PauseOnBadtideStartToggle",
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
            root.AddComponent(pauseOnBadtide.Build());
            root.AddComponent(unpauseOnBadtide.Build());

            _root = root.BuildAndInitialize();

            _droughtStartedPauseToggle = _root.Q<Toggle>("PauseOnDroughtStartToggle");
            _droughtStartedPauseToggle.RegisterValueChangedCallback(TogglePauseDroughtStarted);

            _droughtStartedResumeToggle = _root.Q<Toggle>("UnpauseOnDroughtStartToggle");
            _droughtStartedResumeToggle.RegisterValueChangedCallback(ToggleUnpauseDroughtStarted);

            _temperateStartedPauseToggle = _root.Q<Toggle>("PauseOnTemperateStartToggle");
            _temperateStartedPauseToggle.RegisterValueChangedCallback(TogglePauseTemperateStarted);

            _temperateStartedResumeToggle = _root.Q<Toggle>("UnpauseOnTemperateStartToggle");
            _temperateStartedResumeToggle.RegisterValueChangedCallback(ToggleUnpauseTemperateStarted);

            _badtideStartedPauseToggle = _root.Q<Toggle>("PauseOnBadtideStartToggle");
            _badtideStartedPauseToggle.RegisterValueChangedCallback(TogglePauseBadtideStarted);

            _badtideStartedResumeToggle = _root.Q<Toggle>("UnpauseOnBadtideStartToggle");
            _badtideStartedResumeToggle.RegisterValueChangedCallback(ToggleUnpauseBadtideStarted);

            return _root;
        }

        public void ShowFragment(WaterPumpMonobehaviour waterpumpMonobehaviour)
        {
            if ((bool)waterpumpMonobehaviour)
            {
            }
            _waterPumpMonobehaviour = waterpumpMonobehaviour;
        }

        public void UpdateFragment()
        {
            if ((bool)_waterPumpMonobehaviour)
            {
                _droughtStartedPauseToggle.SetValueWithoutNotify(_waterPumpMonobehaviour.PauseOnDroughtStart);
                _droughtStartedResumeToggle.SetValueWithoutNotify(_waterPumpMonobehaviour.UnpauseOnDroughtStart);
                _temperateStartedPauseToggle.SetValueWithoutNotify(_waterPumpMonobehaviour.PauseOnTemperateStarted);
                _temperateStartedResumeToggle.SetValueWithoutNotify(_waterPumpMonobehaviour.UnpauseOnTemperateStarted);
                _badtideStartedPauseToggle.SetValueWithoutNotify(_waterPumpMonobehaviour.PauseOnBadtideStarted);
                _badtideStartedResumeToggle.SetValueWithoutNotify(_waterPumpMonobehaviour.UnpauseOnBadtideStarted);
            }
        }

        public void ClearFragment()
        {
            _waterPumpMonobehaviour = null;
        }

        private void ToggleUnpauseTemperateStarted(ChangeEvent<bool> changeEvent)
        {
            _waterPumpMonobehaviour.UnpauseOnTemperateStarted = changeEvent.newValue;
            if (changeEvent.newValue == true && _temperateStartedPauseToggle.value == true)
            {
                _temperateStartedPauseToggle.value = false;
                _waterPumpMonobehaviour.PauseOnTemperateStarted = false;
            }

        }
        private void TogglePauseTemperateStarted(ChangeEvent<bool> changeEvent)
        {
            _waterPumpMonobehaviour.PauseOnTemperateStarted = changeEvent.newValue;
            if (changeEvent.newValue == true && _temperateStartedResumeToggle.value == true)
            {
                _temperateStartedResumeToggle.value = false;
                _waterPumpMonobehaviour.UnpauseOnTemperateStarted = false;
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

        private void TogglePauseBadtideStarted(ChangeEvent<bool> changeEvent)
        {
            _waterPumpMonobehaviour.PauseOnBadtideStarted = changeEvent.newValue;
            if (changeEvent.newValue == true && _badtideStartedPauseToggle.value == true)
            {
                _badtideStartedResumeToggle.value = false;
                _waterPumpMonobehaviour.UnpauseOnBadtideStarted = false;
            }
        }
        private void ToggleUnpauseBadtideStarted(ChangeEvent<bool> changeEvent)
        {
            _waterPumpMonobehaviour.UnpauseOnBadtideStarted = changeEvent.newValue;
            if (changeEvent.newValue == true && _badtideStartedPauseToggle.value == true)
            {
                _badtideStartedPauseToggle.value = false;
                _waterPumpMonobehaviour.PauseOnBadtideStarted = false;
            }
        }
    }
}
