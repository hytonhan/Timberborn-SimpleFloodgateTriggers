using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using TimberApi.UiBuilderSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.EntitySystem;
using Timberborn.Localization;
using Timberborn.PrefabSystem;
using Timberborn.SelectionSystem;
using Timberborn.UIFormatters;
using Timberborn.WaterBuildings;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.Length.Unit;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    public class AttachFloodgateToStreamGaugeFragment
    {
        private readonly UIBuilder _builder;
        private readonly ILoc _loc;
        private readonly AttachFloodgateToStreamGaugeButton _attachToStreamGaugeButton;
        private FloodgateTriggerMonoBehaviour _floodgateTriggerMonoBehaviour;

        private VisualElement _linksScrollView;
        private Label _noLinks;
        private Sprite _streamGaugeSprite;

        private readonly EntitySelectionService _EntitySelectionService;
        LinkViewFactory _streamGaugeFloodgateLinkViewFactory;

        //There has to be a better way for this...
        private List<Tuple<Label, Slider, Label, Slider, Label, Slider, Label, Tuple<Slider, Toggle, Toggle, Label, Toggle, Label, Slider, Tuple<Label, Slider, Toggle, Slider, Toggle, Slider>>>> _settingsList = new ();
        
        public AttachFloodgateToStreamGaugeFragment(
            AttachFloodgateToStreamGaugeButton attachToStreamGaugeButton,
            UIBuilder builder,
            EntitySelectionService EntitySelectionService,
            LinkViewFactory streamGaugeFloodgateLinkViewFactory,
            ILoc loc)
        {
            _attachToStreamGaugeButton = attachToStreamGaugeButton;
            _builder = builder;
            _EntitySelectionService = EntitySelectionService;
            _streamGaugeFloodgateLinkViewFactory = streamGaugeFloodgateLinkViewFactory;
            _loc = loc;
        }


        public VisualElement InitiliazeFragment(VisualElement parent)
        {
            _streamGaugeSprite = (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                                  .Where(x => x.name.StartsWith("StreamGauge"))
                                                  .SingleOrDefault();

            var root = _builder.CreateComponentBuilder()
                               .CreateVisualElement()
                               .SetName("LinksScrollView")
                               .SetWidth(new Length(290, Pixel))
                               .SetJustifyContent(Justify.Center)
                               .SetMargin(new Margin(0, 0, new Length(7, Pixel), 0))
                               .BuildAndInitialize();

            _attachToStreamGaugeButton.Initialize(parent, () => _floodgateTriggerMonoBehaviour, delegate
            {
                RemoveAllStreamGaugeViews();
                //AddAllStreamGaugeViews();
                ShowFragment(_floodgateTriggerMonoBehaviour);
            });

            _noLinks = _builder.CreateComponentBuilder()
                               .CreateLabel()
                               .AddPreset(factory => factory.Labels()
                                                            .GameTextBig(name: "NoLinksLabel",
                                                                         locKey: "Floodgates.Triggers.NoLinks",
                                                                         builder: builder => 
                                                                            builder.SetStyle(style => 
                                                                                style.alignSelf = Align.Center)))
                               .BuildAndInitialize();

            _linksScrollView = root.Q<VisualElement>("LinksScrollView");

            return root;
        }

        public void ClearFragment()
        {
            _floodgateTriggerMonoBehaviour = null;
            _settingsList.Clear();
            RemoveAllStreamGaugeViews();
        }

        public void ShowFragment(FloodgateTriggerMonoBehaviour floodgateTriggerMonoBehaviour)
        {
            _floodgateTriggerMonoBehaviour = floodgateTriggerMonoBehaviour;
            AddAllStreamGaugeViews();
            var links = _floodgateTriggerMonoBehaviour.FloodgateLinks;
            for (int i = 0; i < links.Count(); i++)
            {
                var link = links[i];
                var floodgate = link.Floodgate.GetComponentFast<Floodgate>();
                var streamGauge = link.StreamGauge.GetComponentFast<StreamGauge>();
                var setting = _settingsList[i];
                setting.Item2.highValue = UIHelpers.GetMaxHeight(streamGauge);
                setting.Item2.SetValueWithoutNotify(link.Threshold1);
                setting.Item4.highValue = UIHelpers.GetMaxHeight(streamGauge);
                setting.Item4.SetValueWithoutNotify(link.Threshold2);
                var height = UIHelpers.GetMaxHeight(floodgate);
                setting.Item6.highValue = height;
                setting.Item6.SetValueWithoutNotify(link.Height1);
                setting.Rest.Item1.highValue = height;
                setting.Rest.Item1.SetValueWithoutNotify(link.Height2);

                setting.Rest.Item2.SetValueWithoutNotify(link.DisableDuringDrought);
                setting.Rest.Item3.SetValueWithoutNotify(link.DisableDuringTemperate);
                setting.Rest.Item5.SetValueWithoutNotify(link.DisableDuringBadtide);

                //setting.Rest.Item7.highValue = UIHelpers.GetMaxHeight(streamGauge);
                setting.Rest.Item7.SetValueWithoutNotify(link.ContaminationThresholdLow);
                //setting.Rest.Rest.Item2.highValue = UIHelpers.GetMaxHeight(streamGauge);
                setting.Rest.Rest.Item2.SetValueWithoutNotify(link.ContaminationThresholdHigh);
                setting.Rest.Rest.Item4.highValue = height;
                setting.Rest.Rest.Item4.SetValueWithoutNotify(link.ContaminationHeight1);
                setting.Rest.Rest.Item6.highValue = height;
                setting.Rest.Rest.Item6.SetValueWithoutNotify(link.ContaminationHeight2);

                setting.Rest.Rest.Item3.SetValueWithoutNotify(link.EnableContaminationLow);
                setting.Rest.Rest.Item5.SetValueWithoutNotify(link.EnableContaminationHigh);
            }
        }
        
        public void UpdateFragment()
        {
            if ((bool)_floodgateTriggerMonoBehaviour)
            {
                var links = _floodgateTriggerMonoBehaviour.FloodgateLinks;
                for (int i = 0; i < links.Count(); i++)
                {
                    var setting = _settingsList[i];
                    setting.Item1.text = $"{_loc.T("Floodgates.Triggers.Threshold1")}: {setting.Item2.value.ToString(CultureInfo.InvariantCulture)}m";
                    setting.Item3.text = $"{_loc.T("Floodgates.Triggers.Threshold2")}: {setting.Item4.value.ToString(CultureInfo.InvariantCulture)}m";
                    setting.Item5.text = $"{_loc.T("Floodgates.Triggers.HeightWhenBelowThreshold1")}: {setting.Item6.value.ToString(CultureInfo.InvariantCulture)}";
                    setting.Item7.text = $"{_loc.T("Floodgates.Triggers.HeightWhenAboveThreshold2")}: {setting.Rest.Item1.value.ToString(CultureInfo.InvariantCulture)}";

                    setting.Rest.Item6.text = $"{_loc.T("Floodgates.Triggers.ContaminationThresholdLow")}: {NumberFormatter.FormatAsPercentRounded(setting.Rest.Item7.value)}";
                    setting.Rest.Rest.Item1.text = $"{_loc.T("Floodgates.Triggers.ContaminationThresholdHigh")}: {NumberFormatter.FormatAsPercentRounded(setting.Rest.Rest.Item2.value)}";
                    setting.Rest.Rest.Item3.text = $"{_loc.T("Floodgates.Triggers.HeightWhenBelowContaminationThresholdLow")}: {setting.Rest.Rest.Item4.value.ToString(CultureInfo.InvariantCulture)}";
                    setting.Rest.Rest.Item5.text = $"{_loc.T("Floodgates.Triggers.HeightWhenAboveContaminationThresholdHigh")}: {setting.Rest.Rest.Item6.value.ToString(CultureInfo.InvariantCulture)}";

                    var gauge = links[i].StreamGauge.GetComponentFast<StreamGauge>();
                    setting.Rest.Item4.text = $"({gauge.WaterLevel.ToString("0.00")}m, {NumberFormatter.FormatAsPercentRounded(gauge.ContaminationLevel)})";
                }
            }
        }

        /// <summary>
        /// Creates a little view for every existing floodgate-streamague link
        /// </summary>
        public void AddAllStreamGaugeViews()
        {
            ReadOnlyCollection<StreamGaugeFloodgateLink> links = _floodgateTriggerMonoBehaviour.FloodgateLinks;
            for (int i = 0; i < links.Count; i++)
            {
                var j = i;
                var link = links[i];
                var streamGauge = link.StreamGauge.GameObjectFast;
                var labeledPrefab = link.StreamGauge.GetComponentFast<LabeledPrefab>();

                var view = _streamGaugeFloodgateLinkViewFactory.CreateViewForFloodgate(i, labeledPrefab.DisplayNameLocKey);

                var gaugeHeightLabel = view.Q<Label>("StreamGaugeHeightLabel");

                var imageContainer = view.Q<VisualElement>("ImageContainer");
                var img = new Image();
                img.sprite = _streamGaugeSprite;
                imageContainer.Add(img);

                var targetButton = view.Q<Button>("Target");
                targetButton.clicked += delegate
                {
                    _EntitySelectionService.SelectAndFocusOn(link.StreamGauge);
                };

                view.Q<Button>("DetachLinkButton").clicked += delegate
                {
                    link.Floodgate.DetachLink(link);
                    ResetLinks();
                };

                var threshold1Label = view.Q<Label>($"Threshold1Label{i}");
                var threshold1Slider = view.Q<Slider>($"Threshold1Slider{i}");
                threshold1Slider.RegisterValueChangedCallback((@event) => ChangeThresholdSlider(@event, j, 0));
                var threshold2Label = view.Q<Label>($"Threshold2Label{i}");
                var threshold2Slider = view.Q<Slider>($"Threshold2Slider{i}");
                threshold2Slider.RegisterValueChangedCallback((@event) => ChangeThresholdSlider(@event, j, 1));

                var threshold1FloodgateLabel = view.Q<Label>($"Threshold1FloodgateHeightLabel{i}");
                var threshold1FloodgateSlider = view.Q<Slider>($"Threshold1FloodgateHeightSlider{i}");
                threshold1FloodgateSlider.RegisterValueChangedCallback((@event) => ChangeHeightSlider(@event, j, 0));
                var threshold2FloodgateLabel = view.Q<Label>($"Threshold2FloodgateHeightLabel{i}");
                var threshold2FloodgateSlider = view.Q<Slider>($"Threshold2FloodgateHeightSlider{i}");
                threshold2FloodgateSlider.RegisterValueChangedCallback((@event) => ChangeHeightSlider(@event, j, 1));

                var disableDuringDroughtToggle = view.Q<Toggle>($"DisableDuringDroughtToggle{i}");
                disableDuringDroughtToggle.RegisterValueChangedCallback((@event) => ChangeDisableOnDroughtToggle(@event, j));
                var disableDuringTemperate = view.Q<Toggle>($"DisableDuringTemperate{i}");
                disableDuringTemperate.RegisterValueChangedCallback((@event) => ChangeDisableOnTemperateToggle(@event, j));
                var disableDuringBadtideToggle = view.Q<Toggle>($"DisableDuringBadtideToggle{i}");
                disableDuringBadtideToggle.RegisterValueChangedCallback((@event) => ChangeDisableOnBadtideToggle(@event, j));


                var contaminationLowLabel = view.Q<Label>($"ContaminationThresholdLowLabel{i}");
                var contaminationLowSlider = view.Q<Slider>($"ContaminationThresholdLowSlider{i}");
                contaminationLowSlider.RegisterValueChangedCallback((@event) => ChangeThresholdSlider(@event, j, 2));
                var contaminationHighLabel = view.Q<Label>($"ContaminationThresholdHighLabel{i}");
                var contaminationHighSlider = view.Q<Slider>($"ContaminationThresholdHighSlider{i}");
                contaminationHighSlider.RegisterValueChangedCallback((@event) => ChangeThresholdSlider(@event, j, 3));

                var contaminationLowFloodgateToggle = view.Q<Toggle>($"ContaminationThresholdLowFloodgateHeightToggle{i}");
                contaminationLowFloodgateToggle.RegisterValueChangedCallback((@event) => ChangeContaminationLowToggle(@event, j));
                var contaminationLowFloodgateSlider = view.Q<Slider>($"ContaminationThresholdLowFloodgateHeightSlider{i}");
                contaminationLowFloodgateSlider.RegisterValueChangedCallback((@event) => ChangeHeightSlider(@event, j, 2));
                var contaminationHighFloodgateToggle = view.Q<Toggle>($"ContaminationThresholdHighFloodgateHeightToggle{i}");
                contaminationHighFloodgateToggle.RegisterValueChangedCallback((@event) => ChangeContaminationHighToggle(@event, j));
                var contaminationHighFloodgateSlider = view.Q<Slider>($"ContaminationThresholdHighFloodgateHeightSlider{i}");
                contaminationHighFloodgateSlider.RegisterValueChangedCallback((@event) => ChangeHeightSlider(@event, j, 3));

                var foo = new Tuple<Label, Slider, Label, Slider, Label, Slider, Label, Tuple<Slider, Toggle, Toggle, Label, Toggle, Label, Slider, Tuple<Label, Slider, Toggle, Slider, Toggle, Slider>>>(
                    threshold1Label, 
                    threshold1Slider, 
                    threshold2Label, 
                    threshold2Slider, 
                    threshold1FloodgateLabel, 
                    threshold1FloodgateSlider, 
                    threshold2FloodgateLabel, 
                    new Tuple<Slider, Toggle, Toggle, Label, Toggle, Label, Slider, Tuple<Label, Slider, Toggle, Slider, Toggle, Slider>>(
                        threshold2FloodgateSlider,
                        disableDuringDroughtToggle,
                        disableDuringTemperate,
                        gaugeHeightLabel,
                        disableDuringBadtideToggle,
                        contaminationLowLabel,
                        contaminationLowSlider,
                        new Tuple<Label, Slider, Toggle, Slider, Toggle, Slider>(
                            contaminationHighLabel,
                            contaminationHighSlider,
                            contaminationLowFloodgateToggle,
                            contaminationLowFloodgateSlider,
                            contaminationHighFloodgateToggle,
                            contaminationHighFloodgateSlider)));

                _settingsList.Add(foo);
                _linksScrollView.Add(view);
            }

            _attachToStreamGaugeButton.UpdateRemainingSlots(links.Count, _floodgateTriggerMonoBehaviour.MaxStreamGaugeLinks);
            if(links.IsEmpty())
            {
                _linksScrollView.Add(_noLinks);
            }
        }

        public void ChangeDisableOnDroughtToggle(ChangeEvent<bool> changeEvent,
                                                 int index)
        {
            Toggle toggle = _settingsList[index].Rest.Item2;
            _floodgateTriggerMonoBehaviour.FloodgateLinks[index].DisableDuringDrought = changeEvent.newValue;
        }
        public void ChangeContaminationLowToggle(ChangeEvent<bool> changeEvent,
                                                 int index)
        {
            Toggle toggle = _settingsList[index].Rest.Rest.Item3;
            _floodgateTriggerMonoBehaviour.FloodgateLinks[index].EnableContaminationLow = changeEvent.newValue;
        }
        public void ChangeContaminationHighToggle(ChangeEvent<bool> changeEvent,
                                                 int index)
        {
            Toggle toggle = _settingsList[index].Rest.Rest.Item5;
            _floodgateTriggerMonoBehaviour.FloodgateLinks[index].EnableContaminationHigh = changeEvent.newValue;
        }
        public void ChangeDisableOnBadtideToggle(ChangeEvent<bool> changeEvent,
                                                 int index)
        {
            Toggle toggle = _settingsList[index].Rest.Item5;
            _floodgateTriggerMonoBehaviour.FloodgateLinks[index].DisableDuringBadtide = changeEvent.newValue;
        }
        public void ChangeDisableOnTemperateToggle(ChangeEvent<bool> changeEvent,
                                                 int index)
        {
            Toggle toggle = _settingsList[index].Rest.Item3;
            _floodgateTriggerMonoBehaviour.FloodgateLinks[index].DisableDuringTemperate = changeEvent.newValue;
        }

        /// <summary>
        /// Generic logic for when a threshold slider is changed
        /// </summary>
        /// <param name="changeEvent"></param>
        /// <param name="index"></param>
        /// <param name="sliderIndex"></param>
        public void ChangeThresholdSlider(ChangeEvent<float> changeEvent, 
                                          int index, 
                                          int sliderIndex)
        {
            Slider slider;
            switch (sliderIndex)
            {
                case 0:
                    slider = _settingsList[index].Item2;
                    _floodgateTriggerMonoBehaviour.FloodgateLinks[index].Threshold1 = changeEvent.newValue;
                    break;
                case 1:
                    slider = _settingsList[index].Item4;
                    _floodgateTriggerMonoBehaviour.FloodgateLinks[index].Threshold2 = changeEvent.newValue;
                    break;
                case 2:
                    slider = _settingsList[index].Rest.Item7;
                    _floodgateTriggerMonoBehaviour.FloodgateLinks[index].ContaminationThresholdLow = changeEvent.newValue;
                    break;
                case 3:
                    slider = _settingsList[index].Rest.Rest.Item2;
                    _floodgateTriggerMonoBehaviour.FloodgateLinks[index].ContaminationThresholdHigh = changeEvent.newValue;
                    break;
                default:
                    return;
            }
            slider.SetValueWithoutNotify(changeEvent.newValue);
        }

        /// <summary>
        /// Generic logic for when a height slider is changed
        /// </summary>
        /// <param name="changeEvent"></param>
        /// <param name="index"></param>
        /// <param name="heightIndex"></param>
        public void ChangeHeightSlider(ChangeEvent<float> changeEvent, int index, int heightIndex)
        {
            Slider slider;

            switch (heightIndex)
            {
                case 0:
                    slider = _settingsList[index].Item6;
                    var num = UpdateHeightSliderValue(slider, changeEvent.newValue);
                    _floodgateTriggerMonoBehaviour.FloodgateLinks[index].Height1 = num;
                    break;
                case 1:
                    slider = _settingsList[index].Rest.Item1;
                    var num2 = UpdateHeightSliderValue(slider, changeEvent.newValue);
                    _floodgateTriggerMonoBehaviour.FloodgateLinks[index].Height2 = num2;
                    break;
                case 2:
                    slider = _settingsList[index].Rest.Rest.Item4;
                    var num3 = UpdateHeightSliderValue(slider, changeEvent.newValue);
                    _floodgateTriggerMonoBehaviour.FloodgateLinks[index].ContaminationHeight1 = num3;
                    break;
                case 3:
                    slider = _settingsList[index].Rest.Rest.Item6;
                    var num4 = UpdateHeightSliderValue(slider, changeEvent.newValue);
                    _floodgateTriggerMonoBehaviour.FloodgateLinks[index].ContaminationHeight2 = num4;
                    break;
            }
        }

        /// <summary>
        /// Removes the existing link views and builds
        /// them again
        /// </summary>
        public void ResetLinks()
        {
            RemoveAllStreamGaugeViews();
            AddAllStreamGaugeViews();
            UpdateFragment();
        }

        /// <summary>
        /// Removes all the existing link views
        /// </summary>
        public void RemoveAllStreamGaugeViews()
        {
            _linksScrollView.Clear();
        }
        
        /// <summary>
        /// Makes the height slider work in increments of 0.5
        /// </summary>
        /// <param name="slider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private float UpdateHeightSliderValue(Slider slider, float value)
        {
            float num = Mathf.Round(value * 2f) / 2f;
            slider.SetValueWithoutNotify(num);
            return num;
        }

    }
}
