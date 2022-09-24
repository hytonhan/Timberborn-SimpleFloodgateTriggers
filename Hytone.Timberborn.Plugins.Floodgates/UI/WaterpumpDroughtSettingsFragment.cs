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
        private readonly ILoc _loc;
        //private WaterPumpMonobehaviour _waterpumpMonoBehaviour;


        private Toggle _droughtStartedPauseToggle;
        private Toggle _droughtStartedResumeToggle;
        private Toggle _droughtEndedPauseToggle;
        private Toggle _droughtEndedResumeToggle;

        private WaterPumpMonobehaviour _waterPumpMonobehaviour;


        public WaterpumpDroughtSettingsFragment(
            UIBuilder builder,
            ILoc loc)
        {
            _builder = builder;
            _loc = loc;
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
                                                                    .CheckmarkInverted(locKey: "Floodgate.Triggers.UnpauseOnDroughtEnded",
                                                                                       name: "UnpauseOnDroughtEndedToggle",
                                                                                       fontStyle: FontStyle.Normal,
                                                                                       color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                                       builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                                  .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))));

            var pauseOnTemperate = _builder.CreateComponentBuilder()
                                           .CreateVisualElement()
                                           .AddPreset(factory => factory.Toggles()
                                                                  .CheckmarkInverted(locKey: "Floodgate.Triggers.PauseOnDroughtEnded",
                                                                                     name: "PauseOnDroughtEndedToggle",
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

            _droughtStartedPauseToggle = _root.Q<Toggle>("PauseOnDroughtStartToggle");
            _droughtStartedPauseToggle.RegisterValueChangedCallback(TogglePauseDroughtStarted);

            _droughtStartedResumeToggle = _root.Q<Toggle>("UnpauseOnDroughtStartToggle");
            _droughtStartedResumeToggle.RegisterValueChangedCallback(ToggleUnpauseDroughtStarted);

            _droughtEndedPauseToggle = _root.Q<Toggle>("PauseOnDroughtEndedToggle");
            _droughtEndedPauseToggle.RegisterValueChangedCallback(TogglePauseDroughtEnded);

            _droughtEndedResumeToggle = _root.Q<Toggle>("UnpauseOnDroughtEndedToggle");
            _droughtEndedResumeToggle.RegisterValueChangedCallback(ToggleUnpauseDroughtEnded);

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
                _droughtEndedPauseToggle.SetValueWithoutNotify(_waterPumpMonobehaviour.PauseOnDroughtEnded);
                _droughtEndedResumeToggle.SetValueWithoutNotify(_waterPumpMonobehaviour.UnpauseOnDroughtEnded);
            }
        }

        public void ClearFragment()
        {
            _waterPumpMonobehaviour = null;
        }

        private void ToggleUnpauseDroughtEnded(ChangeEvent<bool> changeEvent)
        {
            _waterPumpMonobehaviour.UnpauseOnDroughtEnded = changeEvent.newValue;
            if (changeEvent.newValue == true && _droughtEndedPauseToggle.value == true)
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
