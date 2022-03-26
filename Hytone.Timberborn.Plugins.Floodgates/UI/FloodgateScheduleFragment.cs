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
    public class FloodgateScheduleFragment
    {
        private readonly UIBuilder _builder;
        private VisualElement _root;
        private Floodgate _floodgate;
        private FloodgateTriggerMonoBehaviour _floodgateTriggerComponent;

        private Toggle _scheduleEnabledToggle;
        private Slider _firstScheduleTimeSlider;
        private Label _firstScheduleTimeLabel;
        private Slider _firstScheduleHeightSlider;
        private Label _firstScheduleHeightLabel;
        private Slider _secondScheduleTimeSlider;
        private Label _secondScheduleTimeLabel;
        private Slider _secondScheduleHeightSlider;
        private Label _secondScheduleHeightLabel;

        private Toggle _disableScheduleOnDrought;

        public FloodgateScheduleFragment(UIBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        //public void ClearFragment()
        //{
        //    _floodgate = null;
        //    _floodgateTriggerComponent = null;
        //    _root.ToggleDisplayStyle(false);
        //}
        public void ClearFragment()
        {
            _floodgate = null;
            _floodgateTriggerComponent = null;
        }
        public VisualElement InitializeFragment()
        {
            var rootBuilder =
                _builder.CreateFragmentBuilder()
                        //.ModifyWrapper(builder => builder.SetName)
                        .AddPreset(factory => factory.Toggles()
                                                      .CheckmarkInverted(locKey: "Floodgate.Schedule.Enable",
                                                                         name: nameof(FloodgateTriggerMonoBehaviour.ScheduleEnabled) + "Toggle",
                                                                         fontStyle: FontStyle.Normal,
                                                                         color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                         builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                    .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))))
                        .AddPreset(factory => factory.Toggles()
                                                      .CheckmarkInverted(locKey: "Floodgate.Schedule.DisableOnDrought",
                                                                         name: nameof(FloodgateTriggerMonoBehaviour.DisableScheduleOnDrought) + "Toggle",
                                                                         fontStyle: FontStyle.Normal,
                                                                         color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                         builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                    .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))))
                        .AddPreset(factory => factory.Labels()
                                                     .GameTextBig(name: nameof(FloodgateTriggerMonoBehaviour.FirstScheduleTime) + "Label",
                                                                  text: "Time: ",
                                                                  builder: labelBuilder => labelBuilder.SetStyle(style => style.alignSelf = Align.Center)))
                        .AddPreset(factory => factory.Sliders()
                                                     .SliderCircle(0f,
                                                                   23.5f,
                                                                   name: nameof(FloodgateTriggerMonoBehaviour.FirstScheduleTime) + "Slider",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, Pixel), 0))))
                        .AddPreset(factory => factory.Labels()
                                                     .GameTextBig(name: nameof(FloodgateTriggerMonoBehaviour.FirstScheduleHeight) + "Label",
                                                                  text: "Height: ",
                                                                  builder: labelBuilder => labelBuilder.SetStyle(style => style.alignSelf = Align.Center)))
                        .AddPreset(factory => factory.Sliders()
                                                     .SliderCircle(0f,
                                                                   1f,
                                                                   name: nameof(FloodgateTriggerMonoBehaviour.FirstScheduleHeight) + "Slider",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, Pixel), 0))))
                        .AddPreset(factory => factory.Labels()
                                                     .GameTextBig(name: nameof(FloodgateTriggerMonoBehaviour.SecondScheduleTime) + "Label",
                                                                  text: "Time: ",
                                                                  builder: labelBuilder => labelBuilder.SetStyle(style => style.alignSelf = Align.Center)))
                        .AddPreset(factory => factory.Sliders()
                                                     .SliderCircle(0f,
                                                                   23.5f,
                                                                   name: nameof(FloodgateTriggerMonoBehaviour.SecondScheduleTime) + "Slider",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, Pixel), 0))))
                        .AddPreset(factory => factory.Labels()
                                                     .GameTextBig(name: nameof(FloodgateTriggerMonoBehaviour.SecondScheduleHeight) + "Label",
                                                                  text: "Height: ",
                                                                  builder: labelBuilder => labelBuilder.SetStyle(style => style.alignSelf = Align.Center)))
                        .AddPreset(factory => factory.Sliders()
                                                     .SliderCircle(0f,
                                                                   1f,
                                                                   name: nameof(FloodgateTriggerMonoBehaviour.SecondScheduleHeight) + "Slider",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, Pixel), 0))))
                        ;

            _root = rootBuilder.BuildAndInitialize();

            //this._root.ToggleDisplayStyle(false);

            _firstScheduleTimeLabel = _root.Q<Label>(nameof(FloodgateTriggerMonoBehaviour.FirstScheduleTime) + "Label");
            _firstScheduleTimeSlider = _root.Q<Slider>(nameof(FloodgateTriggerMonoBehaviour.FirstScheduleTime) + "Slider");

            _firstScheduleHeightLabel = _root.Q<Label>(nameof(FloodgateTriggerMonoBehaviour.FirstScheduleHeight) + "Label");
            _firstScheduleHeightSlider = _root.Q<Slider>(nameof(FloodgateTriggerMonoBehaviour.FirstScheduleHeight) + "Slider");

            _secondScheduleTimeLabel = _root.Q<Label>(nameof(FloodgateTriggerMonoBehaviour.SecondScheduleTime) + "Label");
            _secondScheduleTimeSlider = _root.Q<Slider>(nameof(FloodgateTriggerMonoBehaviour.SecondScheduleTime) + "Slider");

            _secondScheduleHeightLabel = _root.Q<Label>(nameof(FloodgateTriggerMonoBehaviour.SecondScheduleHeight) + "Label");
            _secondScheduleHeightSlider = _root.Q<Slider>(nameof(FloodgateTriggerMonoBehaviour.SecondScheduleHeight) + "Slider");

            _scheduleEnabledToggle = _root.Q<Toggle>(nameof(FloodgateTriggerMonoBehaviour.ScheduleEnabled) + "Toggle");
            _disableScheduleOnDrought = _root.Q<Toggle>(nameof(FloodgateTriggerMonoBehaviour.DisableScheduleOnDrought) + "Toggle");

            _firstScheduleHeightSlider.RegisterValueChangedCallback(x => ChangeHeight(x, ref _firstScheduleHeightSlider));
            _firstScheduleTimeSlider.RegisterValueChangedCallback(x => ChangeHeight(x, ref _firstScheduleTimeSlider));

            _secondScheduleHeightSlider.RegisterValueChangedCallback(x => ChangeHeight(x, ref _secondScheduleHeightSlider));
            _secondScheduleTimeSlider.RegisterValueChangedCallback(x => ChangeHeight(x, ref _secondScheduleTimeSlider));

            _scheduleEnabledToggle.RegisterValueChangedCallback(ToggleScheduleEnabled);
            _disableScheduleOnDrought.RegisterValueChangedCallback(ToggleDisableScheduleOnDrought);

            //_droughtStartedSlider = _root.Q<Slider>("DroughtStartedSlider");
            //_droughtStartedEnabledToggle = _root.Q<Toggle>("DroughtStartedEnabled");
            //_droughtStartedLabel = _root.Q<Label>("DroughtStartedValue");
            //_droughtStartedSlider.RegisterValueChangedCallback(ChangeDroughtStartedHeight);
            //_droughtStartedEnabledToggle.RegisterValueChangedCallback(ToggleDroughtStarted);

            return _root;
        }
        /// <summary>
        /// Initial stuff to do when fragment is shown
        /// </summary>
        /// <param name="entity"></param>
        //public void ShowFragment(GameObject entity)
        //{
        //    var component = entity.GetComponent<Floodgate>();
        //    if ((bool)component)
        //    {
        //        var triggerComponent = entity.GetComponent<FloodgateTriggerMonoBehaviour>();
        //        if ((bool)triggerComponent)
        //        {
        //            _firstScheduleHeightSlider.highValue = component.MaxHeight;
        //            _firstScheduleTimeSlider.SetValueWithoutNotify(triggerComponent.FirstScheduleTime);
        //            _firstScheduleHeightSlider.SetValueWithoutNotify(triggerComponent.FirstScheduleHeight);

        //            _secondScheduleHeightSlider.highValue = component.MaxHeight;
        //            _secondScheduleTimeSlider.SetValueWithoutNotify(triggerComponent.SecondScheduleTime);
        //            _secondScheduleHeightSlider.SetValueWithoutNotify(triggerComponent.SecondScheduleHeight);
        //        }
        //        _floodgateTriggerComponent = triggerComponent;
        //    }
        //    _floodgate = component;
        //}
        public void ShowFragment(Floodgate floodgate,
                                 FloodgateTriggerMonoBehaviour floodgateTriggerMonoBehaviour)
        {
            //var component = entity.GetComponent<Floodgate>();
            if ((bool)floodgate)
            {
                //var triggerComponent = entity.GetComponent<FloodgateTriggerMonoBehaviour>();
                if ((bool)floodgateTriggerMonoBehaviour)
                {
                    _firstScheduleHeightSlider.highValue = floodgate.MaxHeight;
                    _firstScheduleTimeSlider.SetValueWithoutNotify(floodgateTriggerMonoBehaviour.FirstScheduleTime);
                    _firstScheduleHeightSlider.SetValueWithoutNotify(floodgateTriggerMonoBehaviour.FirstScheduleHeight);

                    _secondScheduleHeightSlider.highValue = floodgate.MaxHeight;
                    _secondScheduleTimeSlider.SetValueWithoutNotify(floodgateTriggerMonoBehaviour.SecondScheduleTime);
                    _secondScheduleHeightSlider.SetValueWithoutNotify(floodgateTriggerMonoBehaviour.SecondScheduleHeight);
                }
                _floodgateTriggerComponent = floodgateTriggerMonoBehaviour;
            }
            _floodgate = floodgate;
        }

        /// <summary>
        /// Update ui elements when fragment is updated
        /// </summary>
        //public void UpdateFragment()
        //{
        //    if ((bool)_floodgate && _floodgate.enabled && (bool)_floodgateTriggerComponent)
        //    {
        //        _firstScheduleTimeLabel.text = "Time: " + _firstScheduleTimeSlider.value.ToString(CultureInfo.InvariantCulture);
        //        _firstScheduleHeightLabel.text = "Height: " + _firstScheduleHeightSlider.value.ToString(CultureInfo.InvariantCulture);

        //        _secondScheduleTimeLabel.text = "Time: " + _secondScheduleTimeSlider.value.ToString(CultureInfo.InvariantCulture);
        //        _secondScheduleHeightLabel.text = "Height: " + _secondScheduleHeightSlider.value.ToString(CultureInfo.InvariantCulture);

        //        _scheduleEnabledToggle.SetValueWithoutNotify(_floodgateTriggerComponent.ScheduleEnabled);
        //        _disableScheduleOnDrought.SetValueWithoutNotify(_floodgateTriggerComponent.DisableScheduleOnDrought);

        //        _root.ToggleDisplayStyle(visible: true);
        //    }
        //    else
        //    {
        //        _root.ToggleDisplayStyle(visible: false);
        //    }
        //}
        public void UpdateFragment()
        {
            if ((bool)_floodgate && _floodgate.enabled && (bool)_floodgateTriggerComponent)
            {
                _firstScheduleTimeLabel.text = "Time: " + _firstScheduleTimeSlider.value.ToString(CultureInfo.InvariantCulture);
                _firstScheduleHeightLabel.text = "Height: " + _firstScheduleHeightSlider.value.ToString(CultureInfo.InvariantCulture);

                _secondScheduleTimeLabel.text = "Time: " + _secondScheduleTimeSlider.value.ToString(CultureInfo.InvariantCulture);
                _secondScheduleHeightLabel.text = "Height: " + _secondScheduleHeightSlider.value.ToString(CultureInfo.InvariantCulture);

                _scheduleEnabledToggle.SetValueWithoutNotify(_floodgateTriggerComponent.ScheduleEnabled);
                _disableScheduleOnDrought.SetValueWithoutNotify(_floodgateTriggerComponent.DisableScheduleOnDrought);
            }
        }

        private void ToggleScheduleEnabled(ChangeEvent<bool> changeEvent)
        {
            _floodgateTriggerComponent.ScheduleEnabled = changeEvent.newValue;
            _floodgateTriggerComponent.OnChangedScheduleToggles();
        }
        private void ToggleDisableScheduleOnDrought(ChangeEvent<bool> changeEvent)
        {
            _floodgateTriggerComponent.DisableScheduleOnDrought = changeEvent.newValue;
            _floodgateTriggerComponent.OnChangedScheduleToggles();
        }

        private void ChangeHeight(ChangeEvent<float> changeEvent,
                                  ref Slider slider)
        {
            float num = UpdateSliderValue(changeEvent.newValue, ref slider);
            switch (slider.name)
            {
                case nameof(FloodgateTriggerMonoBehaviour.FirstScheduleHeight) + "Slider":
                    if ((bool)_floodgateTriggerComponent &&
                       _floodgateTriggerComponent.FirstScheduleHeight != num)
                    {
                        _floodgateTriggerComponent.FirstScheduleHeight = num;
                        _floodgateTriggerComponent.ChangeScheduleValues();
                    }
                    return;
                case nameof(FloodgateTriggerMonoBehaviour.SecondScheduleHeight) + "Slider":
                    if ((bool)_floodgateTriggerComponent &&
                       _floodgateTriggerComponent.SecondScheduleHeight != num)
                    {
                        _floodgateTriggerComponent.SecondScheduleHeight = num;
                        _floodgateTriggerComponent.ChangeScheduleValues();
                    }
                    return;
                case nameof(FloodgateTriggerMonoBehaviour.FirstScheduleTime) + "Slider":
                    if ((bool)_floodgateTriggerComponent &&
                       _floodgateTriggerComponent.FirstScheduleTime != num)
                    {
                        _floodgateTriggerComponent.FirstScheduleTime = num;
                        _floodgateTriggerComponent.ChangeScheduleValues();
                    }
                    return;
                case nameof(FloodgateTriggerMonoBehaviour.SecondScheduleTime) + "Slider":
                    if ((bool)_floodgateTriggerComponent &&
                       _floodgateTriggerComponent.SecondScheduleTime != num)
                    {
                        _floodgateTriggerComponent.SecondScheduleTime = num;
                        _floodgateTriggerComponent.ChangeScheduleValues();
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
    }
}
