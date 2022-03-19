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
    /// <summary>
    /// Custom UI Fragment that is added to Floodgate UI
    /// </summary>
    public class FloodGateUIFragment : IEntityPanelFragment
    {
        private readonly UIBuilder _builder;
        private VisualElement _root;
        private Floodgate _floodgate;
        private FloodgateTriggerMonoBehaviour _floodgateTriggerComponent;
        private readonly VisualElementLoader _visualElementLoader;

        private Slider _droughtEndedSlider;
        private Toggle _droughtEndedEnabledToggle;
        private Label _droughtEndedLabel;

        private Slider _droughtStartedSlider;
        private Toggle _droughtStartedEnabledToggle;
        private Label _droughtStartedLabel;

        public FloodGateUIFragment(UIBuilder builder,
                                   VisualElementLoader visualElementLoader,
                                   EventBus eventBus)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _visualElementLoader = visualElementLoader ?? throw new ArgumentNullException(nameof(visualElementLoader));
        }

        public void ClearFragment()
        {
            _floodgate = null;
            _floodgateTriggerComponent = null;
            _root.ToggleDisplayStyle(false);
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
                        .AddComponent(
                            _builder.CreateFragmentBuilder()
                                    .AddPreset(factory => factory.Toggles()
                                                                 .CheckmarkInverted(locKey: "Floodgate.Triggers.EnableOnDroughtEnded",
                                                                                   name: "DroughtEndedEnabled"))
                                    .AddPreset(factory => factory.Labels()
                                                                 .GameText(name: "DroughtEndedValue",
                                                                           text: "Height: ",
                                                                           builder: labelBuilder => labelBuilder.SetMargin(new Margin(new Length(8, Pixel), 0))))
                                    .AddPreset(factory => factory.Sliders()
                                                                 .SliderCircle(0f,
                                                                               1f,
                                                                               name: "DroughtEndedSlider",
                                                                               builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)))
                                    .AddPreset(factory => factory.Toggles()
                                                                 .CheckmarkInverted(locKey: "Floodgate.Triggers.EnableOnDroughtStarted",
                                                                                   name: "DroughtStartedEnabled"))
                                    .AddPreset(factory => factory.Labels()
                                                                 .GameText(name: "DroughtStartedValue",
                                                                           text: "Height: ",
                                                                           builder: labelBuilder => labelBuilder.SetMargin(new Margin(new Length(8, Pixel), 0))))
                                    .AddPreset(factory => factory.Sliders()
                                                                 .SliderCircle(0f,
                                                                               1f,
                                                                               name: "DroughtStartedSlider",
                                                                               builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)))
                                    .BuildAndInitialize())
                        .BuildAndInitialize();
            this._root.ToggleDisplayStyle(false);

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
        public void ShowFragment(GameObject entity)
        {
            var component = entity.GetComponent<Floodgate>();
            if ((bool)component)
            {
                var triggerComponent = entity.GetComponent<FloodgateTriggerMonoBehaviour>();
                if ((bool)triggerComponent)
                {
                    _droughtEndedSlider.highValue = component.MaxHeight;
                    _droughtEndedSlider.SetValueWithoutNotify(triggerComponent.DroughtEndedHeight);

                    _droughtStartedSlider.highValue = component.MaxHeight;
                    _droughtStartedSlider.SetValueWithoutNotify(triggerComponent.DroughtStartedHeight);
                }
                _floodgateTriggerComponent = triggerComponent;
            }
            _floodgate = component;
        }

        /// <summary>
        /// Update ui elements when fragment is updated
        /// </summary>
        public void UpdateFragment()
        {
            if ((bool)_floodgate && _floodgate.enabled && (bool)_floodgateTriggerComponent)
            {
                _droughtEndedLabel.text = "Height: " + _droughtEndedSlider.value.ToString(CultureInfo.InvariantCulture);
                _droughtEndedEnabledToggle.SetValueWithoutNotify(_floodgateTriggerComponent.DroughtEndedEnabled);

                _droughtStartedLabel.text = "Height: " + _droughtStartedSlider.value.ToString(CultureInfo.InvariantCulture);
                _droughtStartedEnabledToggle.SetValueWithoutNotify(_floodgateTriggerComponent.DroughtStartedEnabled);

                _root.ToggleDisplayStyle(visible: true);
            }
            else
            {
                _root.ToggleDisplayStyle(visible: false);
            }
        }

        //TODO: METHODS BELOW SHOULD BE REFACTORED

        /// <summary>
        /// Store value when Drought Ended toggle is toggled
        /// </summary>
        /// <param name="changeEvent"></param>
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
