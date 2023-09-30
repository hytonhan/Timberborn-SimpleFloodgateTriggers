using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using System;
using System.Globalization;
using TimberApi.UiBuilderSystem;
using Timberborn.Localization;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.Length.Unit;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    public class WaterpumpScheduleFragment
    {
        private readonly UIBuilder _builder;
        private VisualElement _root;
        private readonly ILoc _loc;

        private WaterPumpMonobehaviour _waterPumpMonobehaviour;

        private Toggle _scheduleEnabledToggle;
        private Toggle _disableScheduleOnDrought;
        private Toggle _disableScheduleOnTemperate;
        private Toggle _disableScheduleOnBadtide;

        private Label _pauseStartLabel;
        private Slider _pauseStartSlider;
        private Label _resumeStartLabel;
        private Slider _resumeStartSlider;

        public WaterpumpScheduleFragment(
            UIBuilder builder,
            ILoc loc)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _loc = loc;
        }

        public void ClearFragment()
        {
            _waterPumpMonobehaviour = null;
        }

        public VisualElement InitializeFragment()
        {
            var rootBuilder =
                _builder.CreateComponentBuilder()
                        .CreateVisualElement()
                        .AddPreset(factory => factory.Toggles()
                                                      .CheckmarkInverted(locKey: "Floodgate.Triggers.PauseOnSchedule",
                                                                         name: nameof(WaterPumpMonobehaviour.ScheduleEnabled) + "Toggle",
                                                                         fontStyle: FontStyle.Normal,
                                                                         color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                         builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                    .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(1, Pixel), 0))))
                        .AddPreset(factory => factory.Toggles()
                                                      .CheckmarkInverted(locKey: "Floodgate.Schedule.DisableOnDrought",
                                                                         name: nameof(WaterPumpMonobehaviour.DisableScheduleOnDrought) + "Toggle",
                                                                         fontStyle: FontStyle.Normal,
                                                                         color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                         builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                    .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(1, Pixel), 0))))
                        .AddPreset(factory => factory.Toggles()
                                                      .CheckmarkInverted(locKey: "Floodgate.Schedule.DisableOnTemperate",
                                                                         name: nameof(WaterPumpMonobehaviour.DisableScheduleOnTemperate) + "Toggle",
                                                                         fontStyle: FontStyle.Normal,
                                                                         color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                         builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                    .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(1, Pixel), 0))))
                        .AddPreset(factory => factory.Toggles()
                                                      .CheckmarkInverted(locKey: "Floodgate.Schedule.DisableOnBadtide",
                                                                         name: nameof(WaterPumpMonobehaviour.DisableScheduleOnBadtide) + "Toggle",
                                                                         fontStyle: FontStyle.Normal,
                                                                         color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                         builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                    .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))))
                        .AddPreset(factory => factory.Labels()
                                                     .GameTextBig(name: nameof(WaterPumpMonobehaviour.PauseOnScheduleTime) + "Label",
                                                                  text: $"{_loc.T("Floodgates.WaterpumpTrigger.PauseStartTime")}: ",
                                                                  builder: labelBuilder => labelBuilder.SetStyle(style => style.alignSelf = Align.Center)))
                        .AddPreset(factory => factory.Sliders()
                                                     .SliderCircle(0f,
                                                                   23.5f,
                                                                   name: nameof(WaterPumpMonobehaviour.PauseOnScheduleTime) + "Slider",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, Pixel), 0))
                                                                                                          .AddClass("slider")))
                        .AddPreset(factory => factory.Labels()
                                                     .GameTextBig(name: nameof(WaterPumpMonobehaviour.ResumeOnScheduleTime) + "Label",
                                                                  text: $"{_loc.T("Floodgates.WaterpumpTrigger.ResumeStartTime")}: ",
                                                                  builder: labelBuilder => labelBuilder.SetStyle(style => style.alignSelf = Align.Center)))
                        .AddPreset(factory => factory.Sliders()
                                                     .SliderCircle(0f,
                                                                   23.5f,
                                                                   name: nameof(WaterPumpMonobehaviour.ResumeOnScheduleTime) + "Slider",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, Pixel), 0))
                                                                                                          .AddClass("slider")));

            _root = rootBuilder.BuildAndInitialize();

            _pauseStartLabel = _root.Q<Label>(nameof(WaterPumpMonobehaviour.PauseOnScheduleTime) + "Label");
            _pauseStartSlider = _root.Q<Slider>(nameof(WaterPumpMonobehaviour.PauseOnScheduleTime) + "Slider");

            _resumeStartLabel = _root.Q<Label>(nameof(WaterPumpMonobehaviour.ResumeOnScheduleTime) + "Label");
            _resumeStartSlider = _root.Q<Slider>(nameof(WaterPumpMonobehaviour.ResumeOnScheduleTime) + "Slider");

            _pauseStartSlider.RegisterValueChangedCallback(x => ChangeHeight(x, ref _pauseStartSlider));
            _resumeStartSlider.RegisterValueChangedCallback(x => ChangeHeight(x, ref _resumeStartSlider));

            _scheduleEnabledToggle = _root.Q<Toggle>(nameof(WaterPumpMonobehaviour.ScheduleEnabled) + "Toggle");
            _disableScheduleOnDrought = _root.Q<Toggle>(nameof(WaterPumpMonobehaviour.DisableScheduleOnDrought) + "Toggle");
            _disableScheduleOnTemperate = _root.Q<Toggle>(nameof(WaterPumpMonobehaviour.DisableScheduleOnTemperate) + "Toggle");
            _disableScheduleOnBadtide = _root.Q<Toggle>(nameof(WaterPumpMonobehaviour.DisableScheduleOnBadtide) + "Toggle");

            _scheduleEnabledToggle.RegisterValueChangedCallback(ToggleScheduleEnabled);
            _disableScheduleOnDrought.RegisterValueChangedCallback(ToggleDisableScheduleOnDrought);
            _disableScheduleOnTemperate.RegisterValueChangedCallback(ToggleDisableScheduleOnTemperate);
            _disableScheduleOnBadtide.RegisterValueChangedCallback(ToggleDisableScheduleOnBadtide);


            return _root;
        }
        public void ShowFragment(WaterPumpMonobehaviour waterPumpMonobehaviour)
        {
            if ((bool)waterPumpMonobehaviour)
            {
                _pauseStartSlider.SetValueWithoutNotify(waterPumpMonobehaviour.PauseOnScheduleTime);
                _resumeStartSlider.SetValueWithoutNotify(waterPumpMonobehaviour.ResumeOnScheduleTime);
            }
            _waterPumpMonobehaviour = waterPumpMonobehaviour;
        }

        public void UpdateFragment()
        {
            if ((bool)_waterPumpMonobehaviour)
            {
                _pauseStartLabel.text = $"{_loc.T("Floodgates.WaterpumpTrigger.PauseStartTime")}: " + _pauseStartSlider.value.ToString(CultureInfo.InvariantCulture);
                _resumeStartLabel.text = $"{_loc.T("Floodgates.WaterpumpTrigger.ResumeStartTime")}: " + _resumeStartSlider.value.ToString(CultureInfo.InvariantCulture);

                _scheduleEnabledToggle.SetValueWithoutNotify(_waterPumpMonobehaviour.ScheduleEnabled);
                _disableScheduleOnDrought.SetValueWithoutNotify(_waterPumpMonobehaviour.DisableScheduleOnDrought);
                _disableScheduleOnTemperate.SetValueWithoutNotify(_waterPumpMonobehaviour.DisableScheduleOnTemperate);
            }
        }

        private void ChangeHeight(ChangeEvent<float> changeEvent,
                                  ref Slider slider)
        {
            float num = UpdateSliderValue(changeEvent.newValue, ref slider);
            switch (slider.name)
            {
                case nameof(WaterPumpMonobehaviour.PauseOnScheduleTime) + "Slider":
                    if ((bool)_waterPumpMonobehaviour &&
                       _waterPumpMonobehaviour.PauseOnScheduleTime != num)
                    {
                        _waterPumpMonobehaviour.PauseOnScheduleTime = num;
                        _waterPumpMonobehaviour.ChangeScheduleValues();
                    }
                    return;
                case nameof(WaterPumpMonobehaviour.ResumeOnScheduleTime) + "Slider":
                    if ((bool)_waterPumpMonobehaviour &&
                       _waterPumpMonobehaviour.ResumeOnScheduleTime != num)
                    {
                        _waterPumpMonobehaviour.ResumeOnScheduleTime = num;
                        _waterPumpMonobehaviour.ChangeScheduleValues();
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
            _waterPumpMonobehaviour.ScheduleEnabled = changeEvent.newValue;
            _waterPumpMonobehaviour.OnChangedScheduleToggles();
        }
        private void ToggleDisableScheduleOnDrought(ChangeEvent<bool> changeEvent)
        {
            _waterPumpMonobehaviour.DisableScheduleOnDrought = changeEvent.newValue;
            _waterPumpMonobehaviour.OnChangedScheduleToggles();
        }
        private void ToggleDisableScheduleOnTemperate(ChangeEvent<bool> changeEvent)
        {
            _waterPumpMonobehaviour.DisableScheduleOnTemperate = changeEvent.newValue;
            _waterPumpMonobehaviour.OnChangedScheduleToggles();
        }
        private void ToggleDisableScheduleOnBadtide(ChangeEvent<bool> changeEvent)
        {
            _waterPumpMonobehaviour.DisableScheduleOnBadtide = changeEvent.newValue;
            _waterPumpMonobehaviour.OnChangedScheduleToggles();
        }
    }
}
