using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using System;
using System.Globalization;
using TimberApi.UiBuilderSystem;
using Timberborn.Localization;
using Timberborn.WaterBuildings;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.Length.Unit;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    /// <summary>
    /// Custom UI Fragment that is added to Floodgate UI
    /// </summary>
    public class FloodgateDroughtFragment
    {
        private readonly UIBuilder _builder;
        private readonly ILoc _loc;
        private VisualElement _root;
        private Floodgate _floodgate;
        private FloodgateTriggerMonoBehaviour _floodgateTriggerComponent;

        private Slider _droughtEndedSlider;
        private Toggle _droughtEndedEnabledToggle;
        private Label _droughtEndedLabel;

        private Slider _droughtStartedSlider;
        private Toggle _droughtStartedEnabledToggle;
        private Label _droughtStartedLabel;

        public FloodgateDroughtFragment(
            UIBuilder builder,
            ILoc loc)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _loc = loc;
        }

        public void ClearFragment()
        {
            _floodgate = null;
            _floodgateTriggerComponent = null;
        }

        /// <summary>
        /// Build the actual fragment
        /// </summary>
        /// <returns></returns>
        public VisualElement InitializeFragment()
        {
            _root =
                _builder.CreateComponentBuilder()
                        .CreateVisualElement()
                        .AddPreset(factory => factory.Toggles()
                                                      .CheckmarkInverted(locKey: "Floodgate.Triggers.EnableOnDroughtEnded",
                                                                         name: "DroughtEndedEnabled",
                                                                         fontStyle: FontStyle.Normal,
                                                                         color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                         builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                    .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))))
                        .AddPreset(factory => factory.Labels()
                                                     .GameTextBig(name: "DroughtEndedValue",
                                                                  text: "Height: ",
                                                                  builder: labelBuilder => labelBuilder.SetStyle(style => style.alignSelf = Align.Center)))
                        .AddPreset(factory => factory.Sliders()
                                                     .SliderCircle(0f,
                                                                   1f,
                                                                   name: "DroughtEndedSlider",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, Pixel), 0))))
                        .AddPreset(factory => factory.Toggles()
                                                     .CheckmarkInverted(locKey: "Floodgate.Triggers.EnableOnDroughtStarted",
                                                                        name: "DroughtStartedEnabled",
                                                                        fontStyle: FontStyle.Normal,
                                                                        color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                        builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
                                                                                                   .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))))
                        .AddPreset(factory => factory.Labels()
                                                     .GameTextBig(name: "DroughtStartedValue",
                                                                  text: "Height: ",
                                                                  builder: labelBuilder => labelBuilder.SetMargin(new Margin(new Length(8, Pixel), 0))
                                                                                                       .SetStyle(style => style.alignSelf = Align.Center)))
                        .AddPreset(factory => factory.Sliders()
                                                     .SliderCircle(0f,
                                                                   1f,
                                                                   name: "DroughtStartedSlider",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, Pixel), 0))))
                        .BuildAndInitialize();

            _droughtEndedSlider = _root.Q<Slider>("DroughtEndedSlider");
            _droughtEndedEnabledToggle = _root.Q<Toggle>("DroughtEndedEnabled");
            _droughtEndedLabel = _root.Q<Label>("DroughtEndedValue");
            _droughtEndedSlider.RegisterValueChangedCallback(ChangeDroughtEndedHeight);
            _droughtEndedEnabledToggle.RegisterValueChangedCallback(ToggleDroughtEnded);

            _droughtStartedSlider = _root.Q<Slider>("DroughtStartedSlider");
            _droughtStartedEnabledToggle = _root.Q<Toggle>("DroughtStartedEnabled");
            _droughtStartedLabel = _root.Q<Label>("DroughtStartedValue");
            _droughtStartedSlider.RegisterValueChangedCallback(ChangeDroughtStartedHeight);
            _droughtStartedEnabledToggle.RegisterValueChangedCallback(ToggleDroughtStarted);

            return _root;
        }

        /// <summary>
        /// Initial stuff to do when fragment is shown
        /// </summary>
        /// <param name="entity"></param>
        public void ShowFragment(Floodgate floodgate,
                                 FloodgateTriggerMonoBehaviour floodgateTriggerMonoBehaviour)
        {
            if ((bool)floodgate)
            {
                if ((bool)floodgateTriggerMonoBehaviour)
                {
                    var height = UIHelpers.GetMaxHeight(floodgate);
                    _droughtEndedSlider.highValue = height;
                    _droughtEndedSlider.SetValueWithoutNotify(floodgateTriggerMonoBehaviour.DroughtEndedHeight);

                    _droughtStartedSlider.highValue = height;
                    _droughtStartedSlider.SetValueWithoutNotify(floodgateTriggerMonoBehaviour.DroughtStartedHeight);
                }
                _floodgateTriggerComponent = floodgateTriggerMonoBehaviour;
            }
            _floodgate = floodgate;
        }

        public void UpdateFragment()
        {
            if ((bool)_floodgate && (bool)_floodgateTriggerComponent)
            {
                _droughtEndedLabel.text = $"{_loc.T("Floodgate.Triggers.Height")}: " + _droughtEndedSlider.value.ToString(CultureInfo.InvariantCulture);
                _droughtEndedEnabledToggle.SetValueWithoutNotify(_floodgateTriggerComponent.DroughtEndedEnabled);

                _droughtStartedLabel.text = $"{_loc.T("Floodgate.Triggers.Height")}: " + _droughtStartedSlider.value.ToString(CultureInfo.InvariantCulture);
                _droughtStartedEnabledToggle.SetValueWithoutNotify(_floodgateTriggerComponent.DroughtStartedEnabled);
            }
        }

        private void ToggleDroughtEnded(ChangeEvent<bool> changeEvent)
        {
            _floodgateTriggerComponent.DroughtEndedEnabled = changeEvent.newValue;
        }

        /// <summary>
        /// Store value when Drought Started toggle is toggled
        /// </summary>
        /// <param name="changeEvent"></param>
        private void ToggleDroughtStarted(ChangeEvent<bool> changeEvent)
        {
            _floodgateTriggerComponent.DroughtStartedEnabled = changeEvent.newValue;
        }

        /// <summary>
        /// Store value when Drought Ended Slider is changed
        /// </summary>
        /// <param name="changeEvent"></param>
        private void ChangeDroughtEndedHeight(ChangeEvent<float> changeEvent)
        {
            float num = UpdateDroughtEndedSliderValue(changeEvent.newValue);
            if ((bool)_floodgateTriggerComponent && _floodgateTriggerComponent.DroughtEndedHeight != num)
            {
                _floodgateTriggerComponent.DroughtEndedHeight = num;
            }
        }

        /// <summary>
        /// Update Drought Ended slider value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float UpdateDroughtEndedSliderValue(float value)
        {
            float num = Mathf.Round(value * 2f) / 2f;
            _droughtEndedSlider.SetValueWithoutNotify(num);
            return num;
        }

        /// <summary>
        /// Store value when Drought Started Slider is changed
        /// </summary>
        /// <param name="changeEvent"></param>
        private void ChangeDroughtStartedHeight(ChangeEvent<float> changeEvent)
        {
            float num = UpdateDroughtStartedSliderValue(changeEvent.newValue);
            if ((bool)_floodgateTriggerComponent && _floodgateTriggerComponent.DroughtStartedHeight != num)
            {
                _floodgateTriggerComponent.DroughtStartedHeight = num;
            }
        }

        /// <summary>
        /// Update Drought Started slider value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float UpdateDroughtStartedSliderValue(float value)
        {
            float num = Mathf.Round(value * 2f) / 2f;
            _droughtStartedSlider.SetValueWithoutNotify(num);
            return num;
        }

    }
}
