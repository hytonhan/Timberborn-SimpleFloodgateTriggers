using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using System;
using System.Globalization;
using System.Linq;
using TimberApi.UIBuilderSystem;
using TimberApi.UIBuilderSystem.ElementBuilders;
using TimberApi.UIBuilderSystem.StylingElements;
using TimberApi.UIPresets.Builders;
using TimberApi.UIPresets.Labels;
using TimberApi.UIPresets.Sliders;
using TimberApi.UIPresets.Toggles;
using Timberborn.Localization;
using Timberborn.WaterBuildings;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    /// <summary>
    /// Custom UI Fragment that is added to Floodgate UI
    /// </summary>
    public class FloodgateHazardFragment
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

        private Slider _badtideStartedSlider;
        private Toggle _badtideStartedEnabledToggle;
        private Label _badtideStartedLabel;

        private Slider _badtideEndedSlider;
        private Toggle _badtideEndedEnabledToggle;
        private Label _badtideEndedLabel;


        public FloodgateHazardFragment(
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
            var test = (Texture2D)Resources.LoadAll("UI/Images/Core/scroll-bar-nine-slice", typeof(Texture2D))
                                          .FirstOrDefault();

            _root = _builder.Create<VisualElementBuilder>()
                .AddComponent<GameToggle>(toggle => toggle.SetName("DroughtStartedEnabled").SetLocKey("Floodgate.Triggers.EnableOnDroughtStarted")
                                                                                           .ModifyRoot(root => 
                                                                                                root.SetMargin(new Margin(new Length(11), 0, new Length(3), 0))
                                                                                                    // .SetStyle(style => style.alignSelf = Align.Center)
                                                                                            )
                )
                .AddComponent<GameTextLabel>(label => label.SetName("DroughtStartedValue").ModifyRoot(root => 
                                                                                                root.SetMargin(new Margin(new Length(8), 0))
                                                                                                    .SetStyle(style => style.alignSelf = Align.Center)))
                .AddComponent<GameSlider>(slider => slider.SetName("DroughtStartedSlider").Small().SetLocKey("Floodgates.Triggers.Empty"))
                .AddComponent<GameToggle>(toggle => toggle.SetName("DroughtEndedEnabled").SetLocKey("Floodgate.Triggers.EnableOnDroughtEnded")
                                                                                           .ModifyRoot(root => 
                                                                                                root.SetMargin(new Margin(new Length(11), 0, new Length(3), 0))
                                                                                                    // .SetStyle(style => style.alignSelf = Align.Center)
                                                                                            )
                )
                .AddComponent<GameTextLabel>(label => label.SetName("DroughtEndedValue").ModifyRoot(root => 
                                                                                                root.SetMargin(new Margin(new Length(8), 0))
                                                                                                    .SetStyle(style => style.alignSelf = Align.Center)))
                .AddComponent<GameSlider>(slider => slider.SetName("DroughtEndedSlider").Small().SetLocKey("Floodgates.Triggers.Empty"))
                .AddComponent<GameToggle>(toggle => toggle.SetName("BadtideStartedEnabled").SetLocKey("Floodgate.Triggers.EnableOnBadtideStarted")
                                                                                           .ModifyRoot(root => 
                                                                                                root.SetMargin(new Margin(new Length(11), 0, new Length(3), 0))
                                                                                                    // .SetStyle(style => style.alignSelf = Align.Center)
                                                                                            )
                )
                .AddComponent<GameTextLabel>(label => label.SetName("BadtideStartedValue").ModifyRoot(root => 
                                                                                                root.SetMargin(new Margin(new Length(8), 0))
                                                                                                    .SetStyle(style => style.alignSelf = Align.Center)))
                .AddComponent<GameSlider>(slider => slider.SetName("BadtideStartedSlider").Small().SetLocKey("Floodgates.Triggers.Empty"))
                .AddComponent<GameToggle>(toggle => toggle.SetName("BadtideEndedEnabled").SetLocKey("Floodgate.Triggers.EnableOnBadtideEnded")
                                                                                           .ModifyRoot(root => 
                                                                                                root.SetMargin(new Margin(new Length(11), 0, new Length(3), 0))
                                                                                                    // .SetStyle(style => style.alignSelf = Align.Center)
                                                                                            )
                )
                .AddComponent<GameTextLabel>(label => label.SetName("BadtideEndedValue").ModifyRoot(root => 
                                                                                                root.SetMargin(new Margin(new Length(8), 0))
                                                                                                    .SetStyle(style => style.alignSelf = Align.Center)))
                .AddComponent<GameSlider>(slider => slider.SetName("BadtideEndedSlider").Small().SetLocKey("Floodgates.Triggers.Empty"))
                .BuildAndInitialize();


            // _root =
            //     _builder.CreateComponentBuilder()
            //             .CreateVisualElement()
            //             .SetMargin(new Margin(0, 0, new Length(10), 0))
            //             .AddPreset(factory => factory.Toggles()
            //                                          .CheckmarkInverted(locKey: "Floodgate.Triggers.EnableOnDroughtStarted",
            //                                                             name: "DroughtStartedEnabled",
            //                                                             fontStyle: FontStyle.Normal,
            //                                                             color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                             builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
            //                                                                                        .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))))
            //             .AddPreset(factory => factory.Labels()
            //                                          .GameTextBig(name: "DroughtStartedValue",
            //                                                       text: "Height: ",
            //                                                       builder: labelBuilder => labelBuilder.SetMargin(new Margin(new Length(8, Pixel), 0))
            //                                                                                            .SetStyle(style => style.alignSelf = Align.Center)))
            //             .AddPreset(factory => factory.Sliders()
            //                                          .SliderCircle(0f,
            //                                                        1f,
            //                                                        name: "DroughtStartedSlider",
            //                                                        builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                               .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                               //.SetMargin(new Margin(0, 0, new Length(20, Pixel), 0))
            //                                                                                               //.AddClass("floodgate-fragment__slider")
            //                                                                                               .AddClass("slider")))
            //             .AddPreset(factory => factory.Toggles()
            //                                           .CheckmarkInverted(locKey: "Floodgate.Triggers.EnableOnDroughtEnded",
            //                                                              name: "DroughtEndedEnabled",
            //                                                              fontStyle: FontStyle.Normal,
            //                                                              color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                              builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
            //                                                                                         .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))))
            //             .AddPreset(factory => factory.Labels()
            //                                          .GameTextBig(name: "DroughtEndedValue",
            //                                                       text: "Height: ",
            //                                                       builder: labelBuilder => labelBuilder.SetStyle(style => style.alignSelf = Align.Center)))
            //             .AddPreset(factory => factory.Sliders()
            //                                          .SliderCircle(0f,
            //                                                        1f,
            //                                                        name: "DroughtEndedSlider",
            //                                                        builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                               .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                               .AddClass("slider")))
            //             .AddPreset(factory => factory.Toggles()
            //                                           .CheckmarkInverted(locKey: "Floodgate.Triggers.EnableOnBadtideStarted",
            //                                                              name: "BadtideStartedEnabled",
            //                                                              fontStyle: FontStyle.Normal,
            //                                                              color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                              builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
            //                                                                                         .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))))
            //             .AddPreset(factory => factory.Labels()
            //                                          .GameTextBig(name: "BadtideStartedValue",
            //                                                       text: "Height: ",
            //                                                       builder: labelBuilder => labelBuilder.SetStyle(style => style.alignSelf = Align.Center)))
            //             .AddPreset(factory => factory.Sliders()
            //                                          .SliderCircle(0f,
            //                                                        1f,
            //                                                        name: "BadtideStartedSlider",
            //                                                        builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                               .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                               .AddClass("slider")))
            //             .AddPreset(factory => factory.Toggles()
            //                                           .CheckmarkInverted(locKey: "Floodgate.Triggers.EnableOnBadtideEnded",
            //                                                              name: "BadtideEndedEnabled",
            //                                                              fontStyle: FontStyle.Normal,
            //                                                              color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                              builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)
            //                                                                                         .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(11, Pixel), 0))))
            //             .AddPreset(factory => factory.Labels()
            //                                          .GameTextBig(name: "BadtideEndedValue",
            //                                                       text: "Height: ",
            //                                                       builder: labelBuilder => labelBuilder.SetStyle(style => style.alignSelf = Align.Center)))
            //             .AddPreset(factory => factory.Sliders()
            //                                          .SliderCircle(0f,
            //                                                        1f,
            //                                                        name: "BadtideEndedSlider",
            //                                                        builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                               .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                               .AddClass("slider")))

            //             .BuildAndInitialize();

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

            _badtideStartedSlider = _root.Q<Slider>("BadtideStartedSlider");
            _badtideStartedEnabledToggle = _root.Q<Toggle>("BadtideStartedEnabled");
            _badtideStartedLabel = _root.Q<Label>("BadtideStartedValue");
            _badtideStartedSlider.RegisterValueChangedCallback(ChangeBadtideStartedHeight);
            _badtideStartedEnabledToggle.RegisterValueChangedCallback(ToggleBadtideStarted);

            _badtideEndedSlider = _root.Q<Slider>("BadtideEndedSlider");
            _badtideEndedEnabledToggle = _root.Q<Toggle>("BadtideEndedEnabled");
            _badtideEndedLabel = _root.Q<Label>("BadtideEndedValue");
            _badtideEndedSlider.RegisterValueChangedCallback(ChangeBadtideEndedHeight);
            _badtideEndedEnabledToggle.RegisterValueChangedCallback(ToggleBadtideEnded);

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

                    _badtideEndedSlider.highValue = height;
                    _badtideEndedSlider.SetValueWithoutNotify(floodgateTriggerMonoBehaviour.BadtideEndedHeight);

                    _badtideStartedSlider.highValue = height;
                    _badtideStartedSlider.SetValueWithoutNotify(floodgateTriggerMonoBehaviour.BadtideStartedHeight);
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

                _badtideEndedLabel.text = $"{_loc.T("Floodgate.Triggers.Height")}: " + _badtideEndedSlider.value.ToString(CultureInfo.InvariantCulture);
                _badtideEndedEnabledToggle.SetValueWithoutNotify(_floodgateTriggerComponent.BadtideEndedEnabled);

                _badtideStartedLabel.text = $"{_loc.T("Floodgate.Triggers.Height")}: " + _badtideStartedSlider.value.ToString(CultureInfo.InvariantCulture);
                _badtideStartedEnabledToggle.SetValueWithoutNotify(_floodgateTriggerComponent.BadtideStartedEnabled);
            }
        }

        private void ToggleDroughtEnded(ChangeEvent<bool> changeEvent)
        {
            _floodgateTriggerComponent.DroughtEndedEnabled = changeEvent.newValue;
        }

        private void ToggleBadtideEnded(ChangeEvent<bool> changeEvent)
        {
            _floodgateTriggerComponent.BadtideEndedEnabled = changeEvent.newValue;
        }

        /// <summary>
        /// Store value when Drought Started toggle is toggled
        /// </summary>
        /// <param name="changeEvent"></param>
        private void ToggleDroughtStarted(ChangeEvent<bool> changeEvent)
        {
            _floodgateTriggerComponent.DroughtStartedEnabled = changeEvent.newValue;
        }

        private void ToggleBadtideStarted(ChangeEvent<bool> changeEvent)
        {
            _floodgateTriggerComponent.BadtideStartedEnabled = changeEvent.newValue;
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

        private void ChangeBadtideEndedHeight(ChangeEvent<float> changeEvent)
        {
            float num = Mathf.Round(changeEvent.newValue * 20f) / 20f;
            _badtideEndedSlider.SetValueWithoutNotify(num);
            if ((bool)_floodgateTriggerComponent && _floodgateTriggerComponent.BadtideEndedHeight != num)
            {
                _floodgateTriggerComponent.BadtideEndedHeight = num;
            }
        }

        /// <summary>
        /// Update Drought Ended slider value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float UpdateDroughtEndedSliderValue(float value)
        {
            float num = Mathf.Round(value * 20f) / 20f;
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
            float num = Mathf.Round(value * 20f) / 20f;
            _droughtStartedSlider.SetValueWithoutNotify(num);
            return num;
        }

        private void ChangeBadtideStartedHeight(ChangeEvent<float> changeEvent)
        {
            float num = Mathf.Round(changeEvent.newValue * 20f) / 20f;
            _badtideStartedSlider.SetValueWithoutNotify(num);

            if ((bool)_floodgateTriggerComponent && _floodgateTriggerComponent.BadtideStartedHeight != num)
            {
                _floodgateTriggerComponent.BadtideStartedHeight = num;
            }
        }

    }
}
