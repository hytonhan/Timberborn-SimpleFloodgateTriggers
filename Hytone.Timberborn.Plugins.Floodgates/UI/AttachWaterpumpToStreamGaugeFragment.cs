using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using TimberApi.UiBuilderSystem;
using Timberborn.Common;
using Timberborn.EntitySystem;
using Timberborn.Localization;
using Timberborn.PrefabSystem;
using Timberborn.SelectionSystem;
using Timberborn.WaterBuildings;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.Length.Unit;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    public class AttachWaterpumpToStreamGaugeFragment
    {
        private readonly UIBuilder _builder;
        private readonly ILoc _loc;
        private readonly AttachWaterpumpToStreamGaugeButton _attachToStreamGaugeButton;
        private WaterPumpMonobehaviour _waterpumpMonoBehaviour;

        private VisualElement _linksScrollView;
        private Label _noLinks;
        private Sprite _streamGaugeSprite;

        private readonly SelectionManager _selectionManager;
        LinkViewFactory _linkViewFactory;

        //There has to be a better way for this...
        private List<Tuple<Toggle, Slider, Toggle, Slider, Toggle, Slider, Toggle, Tuple<Slider, Toggle, Toggle, Label>>> _settingsList = new ();

        public AttachWaterpumpToStreamGaugeFragment(
            AttachWaterpumpToStreamGaugeButton attachToStreamGaugeButton,
            UIBuilder builder,
            SelectionManager selectionManager,
            LinkViewFactory streamGaugeFloodgateLinkViewFactory,
            ILoc loc)
        {
            _attachToStreamGaugeButton = attachToStreamGaugeButton;
            _builder = builder;
            _selectionManager = selectionManager;
            _linkViewFactory = streamGaugeFloodgateLinkViewFactory;
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

            _attachToStreamGaugeButton.Initialize(parent, () => _waterpumpMonoBehaviour, delegate
            {
                RemoveAllStreamGaugeViews();
                ShowFragment(_waterpumpMonoBehaviour);
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
            _waterpumpMonoBehaviour = null;
            _settingsList.Clear();
            RemoveAllStreamGaugeViews();
        }
        public void ShowFragment(WaterPumpMonobehaviour waterpumpMonoBehaviour)
        {
            _waterpumpMonoBehaviour = waterpumpMonoBehaviour;
            AddAllStreamGaugeViews();
            var links = _waterpumpMonoBehaviour.WaterPumpLinks;
            for (int i = 0; i < links.Count(); i++)
            {
                var link = links[i];
                var waterpump = link.WaterPump.GetComponent<WaterInput>();
                var streamGauge = link.StreamGauge.GetComponent<StreamGauge>();
                var setting = _settingsList[i];
                setting.Item1.SetValueWithoutNotify(link.Enabled1);
                setting.Item2.highValue = UIHelpers.GetMaxHeight(streamGauge);
                setting.Item2.SetValueWithoutNotify(link.Threshold1);
                setting.Item3.SetValueWithoutNotify(link.Enabled2);
                setting.Item4.highValue = UIHelpers.GetMaxHeight(streamGauge);
                setting.Item4.SetValueWithoutNotify(link.Threshold2);
                setting.Item5.SetValueWithoutNotify(link.Enabled3);
                setting.Item6.highValue = UIHelpers.GetMaxHeight(streamGauge);
                setting.Item6.SetValueWithoutNotify(link.Threshold3);
                setting.Item7.SetValueWithoutNotify(link.Enabled4);
                setting.Rest.Item1.highValue = UIHelpers.GetMaxHeight(streamGauge);
                setting.Rest.Item1.SetValueWithoutNotify(link.Threshold4);

                setting.Rest.Item2.SetValueWithoutNotify(link.DisableDuringDrought);
                setting.Rest.Item3.SetValueWithoutNotify(link.DisableDuringTemperate);
            }
        }

        public void UpdateFragment()
        {
            if ((bool)_waterpumpMonoBehaviour)
            {
                var links = _waterpumpMonoBehaviour.WaterPumpLinks;
                for (int i = 0; i < links.Count(); i++)
                {
                    var setting = _settingsList[i];
                    setting.Item1.text = $"{_loc.T("Floodgates.WaterpumpTrigger.Threshold1")}: {setting.Item2.value.ToString(CultureInfo.InvariantCulture)}";
                    setting.Item3.text = $"{_loc.T("Floodgates.WaterpumpTrigger.Threshold2")}: {setting.Item4.value.ToString(CultureInfo.InvariantCulture)}";
                    setting.Item5.text = $"{_loc.T("Floodgates.WaterpumpTrigger.Threshold3")}: {setting.Item6.value.ToString(CultureInfo.InvariantCulture)}";
                    setting.Item7.text = $"{_loc.T("Floodgates.WaterpumpTrigger.Threshold4")}: {setting.Rest.Item1.value.ToString(CultureInfo.InvariantCulture)}";

                    var gauge = links[i].StreamGauge.GetComponent<StreamGauge>();
                    setting.Rest.Item4.text = $"({_loc.T("Building.StreamGauge.WaterLevel", gauge.WaterLevel.ToString("0.00"))})";
                }
            }
        }

        public void AddAllStreamGaugeViews()
        {
            ReadOnlyCollection<WaterPumpStreamGaugeLink> links = _waterpumpMonoBehaviour.WaterPumpLinks;
            for (int i = 0; i < links.Count; i++)
            {
                var j = i;
                var link = links[i];
                var streamGauge = link.StreamGauge.gameObject;
                var labeledPrefab = streamGauge.GetComponent<LabeledPrefab>();
                var view = _linkViewFactory.CreateViewForWaterpump(i, labeledPrefab.DisplayNameLocKey);

                var gaugeHeightLabel = view.Q<Label>("StreamGaugeHeightLabel");

                var imageContainer = view.Q<VisualElement>("ImageContainer");
                var img = new Image();
                img.sprite = _streamGaugeSprite;
                imageContainer.Add(img);

                var targetButton = view.Q<Button>("Target");
                targetButton.clicked += delegate
                {
                    _selectionManager.FocusOn(streamGauge);
                };

                view.Q<Button>("DetachLinkButton").clicked += delegate
                {
                    link.WaterPump.DetachLink(link);
                    ResetLinks();
                };

                var threshold1Toggle = view.Q<Toggle>($"Threshold1Toggle{i}");
                threshold1Toggle.RegisterValueChangedCallback((@event) => ChangeThresholdToggle(@event, j, 0));
                var threshold1Slider = view.Q<Slider>($"Threshold1Slider{i}");
                threshold1Slider.RegisterValueChangedCallback((@event) => ChangeThresholdSlider(@event, j, 0));
                var threshold2Toggle = view.Q<Toggle>($"Threshold2Toggle{i}");
                threshold2Toggle.RegisterValueChangedCallback((@event) => ChangeThresholdToggle(@event, j, 1));
                var threshold2Slider = view.Q<Slider>($"Threshold2Slider{i}");
                threshold2Slider.RegisterValueChangedCallback((@event) => ChangeThresholdSlider(@event, j, 1));
                var threshold3Toggle = view.Q<Toggle>($"Threshold3Toggle{i}");
                threshold3Toggle.RegisterValueChangedCallback((@event) => ChangeThresholdToggle(@event, j, 2));
                var threshold3Slider = view.Q<Slider>($"Threshold3Slider{i}");
                threshold3Slider.RegisterValueChangedCallback((@event) => ChangeThresholdSlider(@event, j, 2));
                var threshold4Toggle = view.Q<Toggle>($"Threshold4Toggle{i}");
                threshold4Toggle.RegisterValueChangedCallback((@event) => ChangeThresholdToggle(@event, j, 3));
                var threshold4Slider = view.Q<Slider>($"Threshold4Slider{i}");
                threshold4Slider.RegisterValueChangedCallback((@event) => ChangeThresholdSlider(@event, j, 3));

                var disableDuringDroughtToggle = view.Q<Toggle>($"DisableDuringDroughtToggle{i}");
                disableDuringDroughtToggle.RegisterValueChangedCallback((@event) => ChangeDisableOnDroughtToggle(@event, j));
                var disableDuringTemperate = view.Q<Toggle>($"DisableDuringTemperate{i}");
                disableDuringTemperate.RegisterValueChangedCallback((@event) => ChangeDisableOnTemperateToggle(@event, j));

                var foo = new Tuple<Toggle, Slider, Toggle, Slider, Toggle, Slider, Toggle, Tuple<Slider, Toggle, Toggle, Label>>(
                    threshold1Toggle,
                    threshold1Slider,
                    threshold2Toggle,
                    threshold2Slider,
                    threshold3Toggle,
                    threshold3Slider,
                    threshold4Toggle,
                    new Tuple<Slider, Toggle, Toggle, Label>(threshold4Slider,
                                                             disableDuringDroughtToggle,
                                                             disableDuringTemperate,
                                                             gaugeHeightLabel));

                _settingsList.Add(foo);
                _linksScrollView.Add(view);
            }

            _attachToStreamGaugeButton.UpdateRemainingSlots(links.Count, _waterpumpMonoBehaviour.MaxStreamGaugeLinks);
            if (links.IsEmpty())
            {
                _linksScrollView.Add(_noLinks);
            }
        }

        public void ChangeDisableOnDroughtToggle(ChangeEvent<bool> changeEvent,
                                                 int index)
        {
            Toggle toggle = _settingsList[index].Rest.Item2;
            _waterpumpMonoBehaviour.WaterPumpLinks[index].DisableDuringDrought = changeEvent.newValue;
        }
        public void ChangeDisableOnTemperateToggle(ChangeEvent<bool> changeEvent,
                                                 int index)
        {
            Toggle toggle = _settingsList[index].Rest.Item3;
            _waterpumpMonoBehaviour.WaterPumpLinks[index].DisableDuringTemperate = changeEvent.newValue;
        }

        public void ChangeThresholdSlider(ChangeEvent<float> changeEvent,
                                          int index,
                                          int sliderIndex)
        {
            Slider slider;
            if (sliderIndex == 0)
            {
                slider = _settingsList[index].Item2;
                _waterpumpMonoBehaviour.WaterPumpLinks[index].Threshold1 = changeEvent.newValue;
            }
            else if (sliderIndex == 1)
            {
                slider = _settingsList[index].Item4;
                _waterpumpMonoBehaviour.WaterPumpLinks[index].Threshold2 = changeEvent.newValue;
            }
            else if (sliderIndex == 2)
            {
                slider = _settingsList[index].Item6;
                _waterpumpMonoBehaviour.WaterPumpLinks[index].Threshold3 = changeEvent.newValue;
            }
            else
            {
                slider = _settingsList[index].Rest.Item1;
                _waterpumpMonoBehaviour.WaterPumpLinks[index].Threshold4 = changeEvent.newValue;
            }
            slider.SetValueWithoutNotify(changeEvent.newValue);
        }

        public void ChangeThresholdToggle(ChangeEvent<bool> changeEvent,
                                          int index,
                                          int toggleIndex)
        {
            Toggle toggle;
            if (toggleIndex == 0)
            {
                toggle = _settingsList[index].Item1;
                _waterpumpMonoBehaviour.WaterPumpLinks[index].Enabled1 = changeEvent.newValue;
            }
            else if (toggleIndex == 1)
            {
                toggle = _settingsList[index].Item3;
                _waterpumpMonoBehaviour.WaterPumpLinks[index].Enabled2 = changeEvent.newValue;
            }
            else if (toggleIndex == 2)
            {
                toggle = _settingsList[index].Item5;
                _waterpumpMonoBehaviour.WaterPumpLinks[index].Enabled3 = changeEvent.newValue;
            }
            else
            {
                toggle = _settingsList[index].Item7;
                _waterpumpMonoBehaviour.WaterPumpLinks[index].Enabled4 = changeEvent.newValue;
            }
        }

        public void ResetLinks()
        {
            RemoveAllStreamGaugeViews();
            AddAllStreamGaugeViews();
            UpdateFragment();
        }

        public void RemoveAllStreamGaugeViews()
        {
            _linksScrollView.Clear();
        }
    }
}
