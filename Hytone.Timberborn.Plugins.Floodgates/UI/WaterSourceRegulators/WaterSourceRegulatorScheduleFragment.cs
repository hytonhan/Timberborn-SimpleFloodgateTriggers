using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterSourceRegulators;
using System;
using System.Globalization;
using TimberApi.UIBuilderSystem;
using TimberApi.UIBuilderSystem.ElementBuilders;
using TimberApi.UIBuilderSystem.StylingElements;
using TimberApi.UIPresets.Builders;
using TimberApi.UIPresets.Labels;
using TimberApi.UIPresets.Sliders;
using TimberApi.UIPresets.Toggles;
using Timberborn.Localization;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hytone.Timberborn.Plugins.Floodgates.UI.WaterSourceRegulators
{
    public class WaterSourceRegulatorScheduleFragment
    {
        private readonly UIBuilder _builder;
        private VisualElement _root;
        private readonly ILoc _loc;

        private WaterSourceRegulatorMonobehaviour _waterSourceRegulatorMonobehaviour;

        private Toggle _scheduleEnabledToggle;
        private Toggle _disableScheduleOnDrought;
        private Toggle _disableScheduleOnTemperate;
        private Toggle _disableScheduleOnBadtide;

        private Label _CloseStartLabel;
        private Slider _CloseStartSlider;
        private Label _OpenStartLabel;
        private Slider _OpenStartSlider;

        public WaterSourceRegulatorScheduleFragment(
            UIBuilder builder,
            ILoc loc)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _loc = loc;
        }

        public void ClearFragment()
        {
            _waterSourceRegulatorMonobehaviour = null;
        }

        public VisualElement InitializeFragment()
        {
            var rootBuilder = _builder.Create<VisualElementBuilder>()
                .AddComponent<GameToggle>(nameof(WaterSourceRegulatorMonobehaviour.ScheduleEnabled) + "Toggle", toggle => 
                    toggle.SetLocKey("Floodgate.Schedule.CloseOnSchedule")
                          .ModifyRoot(root => root.SetMargin(new Margin(new Length(3), 0, new Length(1), 0))
                          )
                )
                .AddComponent<GameToggle>(nameof(WaterSourceRegulatorMonobehaviour.DisableScheduleOnDrought) + "Toggle", toggle => 
                    toggle.SetLocKey("Floodgate.Schedule.DisableOnDrought")
                          .ModifyRoot(root => root.SetMargin(new Margin(new Length(3), 0, new Length(1), 0))
                          )
                )
                .AddComponent<GameToggle>(nameof(WaterSourceRegulatorMonobehaviour.DisableScheduleOnTemperate) + "Toggle", toggle => 
                    toggle.SetLocKey("Floodgate.Schedule.DisableOnTemperate")
                          .ModifyRoot(root => root.SetMargin(new Margin(new Length(3), 0, new Length(1), 0))
                          )
                )
                .AddComponent<GameToggle>(nameof(WaterSourceRegulatorMonobehaviour.DisableScheduleOnBadtide) + "Toggle", toggle => 
                    toggle.SetLocKey("Floodgate.Schedule.DisableOnBadtide")
                          .ModifyRoot(root => root.SetMargin(new Margin(new Length(3), 0, new Length(1), 0))
                          )
                )
                .AddComponent<GameLabel>(nameof(WaterSourceRegulatorMonobehaviour.CloseOnScheduleTime) + "Label", label => 
                    label.Big()
                         .SetLocKey("Floodgates.WaterSourceRegulator.CloseStartTime")
                         .ModifyRoot(root => root.SetStyle(style => style.alignSelf = Align.Center))
                )
                .AddComponent<GameSlider>(nameof(WaterSourceRegulatorMonobehaviour.CloseOnScheduleTime) + "Slider", slider => 
                    slider.Small()
                          .SetLowValue(0)
                          .SetLocKey("Floodgates.Triggers.Empty")
                          .ModifyRoot(root => root.SetHighValue(23.5f)
                                                  .SetPadding(new Padding(new Length(21), 0))
                                                  .SetStyle(style => style.flexGrow = 1f)
                          )
                )
                .AddComponent<GameLabel>(nameof(WaterSourceRegulatorMonobehaviour.OpenOnScheduleTime) + "Label", label => 
                    label.Big()
                         .SetLocKey("Floodgates.WaterSourceRegulator.OpenStartTime")
                         .ModifyRoot(root => root.SetStyle(style => style.alignSelf = Align.Center))
                )
                .AddComponent<GameSlider>(nameof(WaterSourceRegulatorMonobehaviour.OpenOnScheduleTime) + "Slider", slider => 
                    slider.Small()
                          .SetLowValue(0)
                          .SetLocKey("Floodgates.Triggers.Empty")
                          .ModifyRoot(root => root.SetHighValue(23.5f)
                                                  .SetPadding(new Padding(new Length(21), 0))
                                                  .SetStyle(style => style.flexGrow = 1f)
                          )
                )
                ;

            _root = rootBuilder.BuildAndInitialize();

            _CloseStartLabel = _root.Q<Label>(nameof(WaterSourceRegulatorMonobehaviour.CloseOnScheduleTime) + "Label");
            _CloseStartSlider = _root.Q<Slider>(nameof(WaterSourceRegulatorMonobehaviour.CloseOnScheduleTime) + "Slider");

            _OpenStartLabel = _root.Q<Label>(nameof(WaterSourceRegulatorMonobehaviour.OpenOnScheduleTime) + "Label");
            _OpenStartSlider = _root.Q<Slider>(nameof(WaterSourceRegulatorMonobehaviour.OpenOnScheduleTime) + "Slider");

            _CloseStartSlider.RegisterValueChangedCallback(x => ChangeHeight(x, ref _CloseStartSlider));
            _OpenStartSlider.RegisterValueChangedCallback(x => ChangeHeight(x, ref _OpenStartSlider));

            _scheduleEnabledToggle = _root.Q<Toggle>(nameof(WaterSourceRegulatorMonobehaviour.ScheduleEnabled) + "Toggle");
            _disableScheduleOnDrought = _root.Q<Toggle>(nameof(WaterSourceRegulatorMonobehaviour.DisableScheduleOnDrought) + "Toggle");
            _disableScheduleOnTemperate = _root.Q<Toggle>(nameof(WaterSourceRegulatorMonobehaviour.DisableScheduleOnTemperate) + "Toggle");
            _disableScheduleOnBadtide = _root.Q<Toggle>(nameof(WaterSourceRegulatorMonobehaviour.DisableScheduleOnBadtide) + "Toggle");

            _scheduleEnabledToggle.RegisterValueChangedCallback(ToggleScheduleEnabled);
            _disableScheduleOnDrought.RegisterValueChangedCallback(ToggleDisableScheduleOnDrought);
            _disableScheduleOnTemperate.RegisterValueChangedCallback(ToggleDisableScheduleOnTemperate);
            _disableScheduleOnBadtide.RegisterValueChangedCallback(ToggleDisableScheduleOnBadtide);


            return _root;
        }
        public void ShowFragment(WaterSourceRegulatorMonobehaviour WaterSourceRegulatorMonobehaviour)
        {
            if ((bool)WaterSourceRegulatorMonobehaviour)
            {
                _CloseStartSlider.SetValueWithoutNotify(WaterSourceRegulatorMonobehaviour.CloseOnScheduleTime);
                _OpenStartSlider.SetValueWithoutNotify(WaterSourceRegulatorMonobehaviour.OpenOnScheduleTime);
            }
            _waterSourceRegulatorMonobehaviour = WaterSourceRegulatorMonobehaviour;
        }

        public void UpdateFragment()
        {
            if ((bool)_waterSourceRegulatorMonobehaviour)
            {
                _CloseStartLabel.text = $"{_loc.T("Floodgates.WaterSourceRegulator.CloseStartTime")}: " + _CloseStartSlider.value.ToString(CultureInfo.InvariantCulture);
                _OpenStartLabel.text = $"{_loc.T("Floodgates.WaterSourceRegulator.OpenStartTime")}: " + _OpenStartSlider.value.ToString(CultureInfo.InvariantCulture);

                _scheduleEnabledToggle.SetValueWithoutNotify(_waterSourceRegulatorMonobehaviour.ScheduleEnabled);
                _disableScheduleOnDrought.SetValueWithoutNotify(_waterSourceRegulatorMonobehaviour.DisableScheduleOnDrought);
                _disableScheduleOnTemperate.SetValueWithoutNotify(_waterSourceRegulatorMonobehaviour.DisableScheduleOnTemperate);
                _disableScheduleOnBadtide.SetValueWithoutNotify(_waterSourceRegulatorMonobehaviour.DisableScheduleOnBadtide);
            }
        }

        private void ChangeHeight(ChangeEvent<float> changeEvent,
                                  ref Slider slider)
        {
            float num = UpdateSliderValue(changeEvent.newValue, ref slider);
            switch (slider.name)
            {
                case nameof(WaterSourceRegulatorMonobehaviour.CloseOnScheduleTime) + "Slider":
                    if ((bool)_waterSourceRegulatorMonobehaviour &&
                       _waterSourceRegulatorMonobehaviour.CloseOnScheduleTime != num)
                    {
                        _waterSourceRegulatorMonobehaviour.CloseOnScheduleTime = num;
                        _waterSourceRegulatorMonobehaviour.ChangeScheduleValues();
                    }
                    return;
                case nameof(WaterSourceRegulatorMonobehaviour.OpenOnScheduleTime) + "Slider":
                    if ((bool)_waterSourceRegulatorMonobehaviour &&
                       _waterSourceRegulatorMonobehaviour.OpenOnScheduleTime != num)
                    {
                        _waterSourceRegulatorMonobehaviour.OpenOnScheduleTime = num;
                        _waterSourceRegulatorMonobehaviour.ChangeScheduleValues();
                    }
                    return;
            }
        }

        private float UpdateSliderValue(float value, ref Slider field)
        {
            float num = Mathf.Round(value * 2f) / 2f;
            field.SetValueWithoutNotify(num);
            return num;
        }

        private void ToggleScheduleEnabled(ChangeEvent<bool> changeEvent)
        {
            _waterSourceRegulatorMonobehaviour.ScheduleEnabled = changeEvent.newValue;
            _waterSourceRegulatorMonobehaviour.OnChangedScheduleToggles();
        }
        private void ToggleDisableScheduleOnDrought(ChangeEvent<bool> changeEvent)
        {
            _waterSourceRegulatorMonobehaviour.DisableScheduleOnDrought = changeEvent.newValue;
            _waterSourceRegulatorMonobehaviour.OnChangedScheduleToggles();
        }
        private void ToggleDisableScheduleOnTemperate(ChangeEvent<bool> changeEvent)
        {
            _waterSourceRegulatorMonobehaviour.DisableScheduleOnTemperate = changeEvent.newValue;
            _waterSourceRegulatorMonobehaviour.OnChangedScheduleToggles();
        }
        private void ToggleDisableScheduleOnBadtide(ChangeEvent<bool> changeEvent)
        {
            _waterSourceRegulatorMonobehaviour.DisableScheduleOnBadtide = changeEvent.newValue;
            _waterSourceRegulatorMonobehaviour.OnChangedScheduleToggles();
        }
    }
}
