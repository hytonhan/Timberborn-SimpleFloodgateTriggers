using TimberApi.UIBuilderSystem;
using TimberApi.UIBuilderSystem.ElementBuilders;
using TimberApi.UIPresets.Buttons;
using TimberApi.UIPresets.Labels;
using TimberApi.UIPresets.Sliders;
using TimberApi.UIPresets.Toggles;
using Timberborn.Localization;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    /// <summary>
    /// Handles the creation of new floodgate-streamgauge link views.
    /// </summary>
    public class LinkViewFactory
    {
        private readonly UIBuilder _builder;
        private readonly ILoc _loc;

        public LinkViewFactory(
            UIBuilder builder,
            ILoc loc)
        {
            _builder = builder;
            _loc = loc;
        }

        /// <summary>
        /// Create a link view that is shown on the Floodgate's fragment
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VisualElement CreateViewForFloodgate(int index, string streamgaugeLocKey)
        {
            var foo = _builder.Create<VisualElementBuilder>()
                .AddComponent<ButtonBuilder>("Target", button => 
                    button.AddComponent<VisualElementBuilder>(element => 
                        element.AddComponent<VisualElementBuilder>(element2 => 
                                    element2.AddComponent<VisualElementBuilder>("ImageContainer", element4 => element4)
                                            .AddComponent<GameTextLabel>("StreamGaugeLabel", label => label.Big().SetText(_loc.T(streamgaugeLocKey)))
                                            .AddComponent<GameTextLabel>("StreamGaugeHeightLabel", label => label.Big())
                        )
                               .AddComponent<VisualElementBuilder>(element3 => 
                                    element3.AddComponent<ButtonBuilder>("DetachLinkButton", button => 
                                        button.AddComponent<VisualElementBuilder>(element => 
                                            element.AddClass("entity-panel__button")
                                                   .AddClass("delete-building__icon"))))
                    )
                    .AddComponent<VisualElementBuilder>(element => 
                        element.AddComponent<GameToggle>($"DisableDuringDroughtToggle{index}", toggle => toggle.SetLocKey("Floodgates.WaterpumpTrigger.DisableDuringDrought"))
                               .AddComponent<GameToggle>($"DisableDuringBadtideToggle{index}", toggle => toggle.SetLocKey("Floodgates.WaterpumpTrigger.DisableDuringBadtide"))
                               .AddComponent<GameToggle>($"DisableDuringTemperate{index}", toggle => toggle.SetLocKey("Floodgates.WaterpumpTrigger.DisableDuringTemperate"))
                               .AddComponent<GameTextLabel>($"Threshold1Label{index}", label => label.SetText(_loc.T("Floodgates.Triggers.Threshold1")))
                               .AddComponent<GameSlider>($"Threshold1Slider{index}", slider => slider)
                               .AddComponent<GameTextLabel>($"Threshold1FloodgateHeightLabel{index}", label => label.SetText(_loc.T("Floodgates.Triggers.HeightWhenBelowThreshold1")))
                               .AddComponent<GameSlider>($"Threshold1FloodgateHeightSlider{index}", slider => slider)
                               .AddComponent<GameTextLabel>($"Threshold2Label{index}", label => label.SetText(_loc.T("Floodgates.Triggers.Threshold2")))
                               .AddComponent<GameSlider>($"Threshold2Slider{index}", slider => slider)
                               .AddComponent<GameTextLabel>($"Threshold2FloodgateHeightLabel{index}", label => label.SetText(_loc.T("Floodgates.Triggers.HeightWhenAboveThreshold2")))
                               .AddComponent<GameSlider>($"Threshold2FloodgateHeightSlider{index}", slider => slider)
                               .AddComponent<GameTextLabel>($"ContaminationThresholdLowLabel{index}", label => label.SetText(_loc.T("Floodgates.Triggers.ContaminationThresholdLow")))
                               .AddComponent<GameSlider>($"ContaminationThresholdLowSlider{index}", slider => slider)
                               .AddComponent<GameToggle>($"ContaminationThresholdLowFloodgateHeightToggle{index}", toggle => toggle.SetLocKey("Floodgates.Triggers.HeightWhenBelowContaminationThresholdLow"))
                               .AddComponent<GameSlider>($"ContaminationThresholdLowFloodgateHeightSlider{index}", slider => slider)
                               .AddComponent<GameTextLabel>($"ContaminationThresholdHighLabel{index}", label => label.SetText(_loc.T("Floodgates.Triggers.ContaminationThresholdHigh")))
                               .AddComponent<GameSlider>($"ContaminationThresholdHighSlider{index}", slider => slider)
                               .AddComponent<GameToggle>($"ContaminationThresholdHighFloodgateHeightToggle{index}", toggle => toggle.SetLocKey("Floodgates.Triggers.HeightWhenAboveContaminationThresholdHigh"))
                               .AddComponent<GameSlider>($"ContaminationThresholdHighFloodgateHeightSlider{index}", slider => slider)
                               )
                );
            // var foo = _builder.CreateComponentBuilder()
            //                   .CreateVisualElement()
            //                   .AddComponent(
            //                     _builder.CreateComponentBuilder()
            //                             .CreateButton()
            //                             .SetName("Target")
            //                             .AddClass("entity-fragment__button")
            //                             .AddClass("entity-fragment__button--green")
            //                             .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
            //                             .SetFontSize(new Length(14, Pixel))
            //                             .SetFontStyle(FontStyle.Normal)
            //                             .SetHeight(new Length(30, Pixel))
            //                             .SetWidth(new Length(290, Pixel))
            //                             .SetPadding(new Padding(0, 0, 0, 0))
            //                             .SetMargin(new Margin(new Length(2, Pixel), 0, new Length(2, Pixel), 0))
            //                             .AddComponent(
            //                                 _builder.CreateComponentBuilder()
            //                                         .CreateVisualElement()
            //                                         .SetFlexWrap(Wrap.Wrap)
            //                                         .SetFlexDirection(FlexDirection.Row)
            //                                         .SetJustifyContent(Justify.SpaceBetween)
            //                                         .AddComponent(
            //                                              _builder.CreateComponentBuilder()
            //                                                      .CreateVisualElement()
            //                                                      .SetFlexWrap(Wrap.Wrap)
            //                                                      .SetFlexDirection(FlexDirection.Row)
            //                                                      .SetJustifyContent(Justify.FlexStart)
            //                                                      .AddComponent(
            //                                                           _builder.CreateComponentBuilder()
            //                                                                   .CreateVisualElement()
            //                                                                   .SetName("ImageContainer")
            //                                                                   .SetWidth(new Length(28, Pixel))
            //                                                                   .SetHeight(new Length(28, Pixel))
            //                                                                   .SetMargin(new Margin(new Length(1, Pixel), 0, 0, new Length(6, Pixel)))
            //                                                                   .Build())
            //                                                      .AddPreset(factory => factory.Labels()
            //                                                                                   .GameTextBig(text: _loc.T(streamgaugeLocKey),
            //                                                                                                name: "StreamGaugeLabel"))
            //                                                      .AddPreset(factory => factory.Labels()
            //                                                                                   .GameTextBig(name: "StreamGaugeHeightLabel",
            //                                                                                                builder: builder => builder.SetMargin(new Margin(0, 0, 0, new Length(2, Pixel)))))
            //                                                      .Build())
            //                                         .AddComponent(
            //                                             _builder.CreateComponentBuilder()
            //                                                   .CreateButton()
            //                                                   .AddClass("unity-text-element")
            //                                                   .AddClass("unity-button")
            //                                                   .AddClass("entity-panel__button")
            //                                                   .AddClass("entity-panel__button--red")
            //                                                   .AddClass("distribution-route__icon-wrapper")
            //                                                   .SetName("DetachLinkButton")
            //                                                   .SetMargin(new Margin(new Length(1, Pixel), new Length(2, Pixel), 0, 0))
            //                                                   .AddComponent(_builder.CreateComponentBuilder()
            //                                                                         .CreateVisualElement()
            //                                                                         .AddClass("entity-panel__button")
            //                                                                         .AddClass("delete-building__icon")
            //                                                                         .Build())
            //                                                   .Build())
            //                                         .Build())
            //                                   .Build())
            //                       .AddComponent(
            //                             _builder.CreateComponentBuilder()
            //                                     .CreateVisualElement()
            //                                     .AddPreset(
            //                                         factory => factory.Toggles()
            //                                                           .CheckmarkInverted(locKey: "Floodgates.WaterpumpTrigger.DisableDuringDrought",
            //                                                                              name: $"DisableDuringDroughtToggle{index}",
            //                                                                              color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                              builder:
            //                                                                               builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
            //                                     .AddPreset(
            //                                         factory => factory.Toggles()
            //                                                           .CheckmarkInverted(locKey: "Floodgates.WaterpumpTrigger.DisableDuringBadtide",
            //                                                                              name: $"DisableDuringBadtideToggle{index}",
            //                                                                              color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                              builder:
            //                                                                               builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
            //                                     .AddPreset(
            //                                         factory => factory.Toggles()
            //                                                           .CheckmarkInverted(locKey: "Floodgates.WaterpumpTrigger.DisableDuringTemperate",
            //                                                                              name: $"DisableDuringTemperate{index}",
            //                                                                              color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                              builder:
            //                                                                               builder => builder.SetStyle(style => style.alignSelf = Align.Center)
            //                                                                                                 .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(30, Pixel), 0))))
            //                                     .AddPreset(
            //                                         factory => factory.Labels()
            //                                                           .GameTextBig(name: $"Threshold1Label{index}",
            //                                                                        locKey: "Floodgates.Triggers.Threshold1",
            //                                                                        builder:
            //                                                                         builder => builder.SetStyle(
            //                                                                             style => style.alignSelf = Align.Center)))
            //                                     .AddPreset(
            //                                         factory => factory.Sliders()
            //                                                           .SliderCircle(0f,
            //                                                                         3f,
            //                                                                         name: $"Threshold1Slider{index}",
            //                                                        builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                               .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                               .AddClass("slider")))
            //                                     .AddPreset(
            //                                         factory => factory.Labels()
            //                                                           .GameTextBig(name: $"Threshold1FloodgateHeightLabel{index}",
            //                                                                        locKey: "Floodgates.Triggers.HeightWhenBelowThreshold1",
            //                                                                        builder:
            //                                                                         builder => builder.SetStyle(
            //                                                                             style => style.alignSelf = Align.Center)))
            //                                     .AddPreset(
            //                                         factory => factory.Sliders()
            //                                                           .SliderCircle(0f,
            //                                                                         3f,
            //                                                                         name: $"Threshold1FloodgateHeightSlider{index}",
            //                                                        builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                               .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                               .AddClass("slider")))
            //                                     .AddPreset(
            //                                         factory => factory.Labels()
            //                                                           .GameTextBig(name: $"Threshold2Label{index}",
            //                                                                        locKey: "Floodgates.Triggers.Threshold2",
            //                                                                        builder:
            //                                                                         builder => builder.SetStyle(
            //                                                                             style => style.alignSelf = Align.Center)))
            //                                     .AddPreset(
            //                                         factory => factory.Sliders()
            //                                                           .SliderCircle(0f,
            //                                                                         3f,
            //                                                                         name: $"Threshold2Slider{index}",
            //                                                        builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                               .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                               .AddClass("slider")))
            //                                     .AddPreset(
            //                                         factory => factory.Labels()
            //                                                           .GameTextBig(name: $"Threshold2FloodgateHeightLabel{index}",
            //                                                                        locKey: "Floodgates.Triggers.HeightWhenAboveThreshold2",
            //                                                                        builder:
            //                                                                         builder => builder.SetStyle(
            //                                                                             style => style.alignSelf = Align.Center)))
            //                                     .AddPreset(
            //                                         factory => factory.Sliders()
            //                                                           .SliderCircle(0f,
            //                                                                         3f,
            //                                                                         name: $"Threshold2FloodgateHeightSlider{index}",
            //                                                        builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                               .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                               .AddClass("slider")))
            //                                     .AddPreset(
            //                                         factory => factory.Labels()
            //                                                           .GameTextBig(name: $"ContaminationThresholdLowLabel{index}",
            //                                                                        locKey: "Floodgates.Triggers.ContaminationThresholdLow",
            //                                                                        builder:
            //                                                                         builder => builder.SetStyle(style => style.alignSelf = Align.Center)
            //                                                                                           .SetMargin(new Margin(new Length(5, Pixel), 0, 0, 0))))
            //                                     .AddPreset(
            //                                         factory => factory.Sliders()
            //                                                           .SliderCircle(0f,
            //                                                                         1f,
            //                                                                         name: $"ContaminationThresholdLowSlider{index}",
            //                                                                         builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                               .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                               .AddClass("slider")))
            //                                     .AddPreset(
            //                                         factory => factory.Toggles()
            //                                                           .CheckmarkInverted(name: $"ContaminationThresholdLowFloodgateHeightToggle{index}",
            //                                                                              color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                              locKey: "Floodgates.Triggers.HeightWhenBelowContaminationThresholdLow",
            //                                                                              builder:
            //                                                                               builder => builder.SetStyle(
            //                                                                                   style => style.alignSelf = Align.Center)))
            //                                     .AddPreset(
            //                                         factory => factory.Sliders()
            //                                                           .SliderCircle(0f,
            //                                                                         3f,
            //                                                                         name: $"ContaminationThresholdLowFloodgateHeightSlider{index}",
            //                                                                         builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                               .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                               .AddClass("slider")))
            //                                     .AddPreset(
            //                                         factory => factory.Labels()
            //                                                           .GameTextBig(name: $"ContaminationThresholdHighLabel{index}",
            //                                                                        locKey: "Floodgates.Triggers.ContaminationThresholdHigh",
            //                                                                        builder:
            //                                                                         builder => builder.SetStyle(
            //                                                                             style => style.alignSelf = Align.Center)))
            //                                     .AddPreset(
            //                                         factory => factory.Sliders()
            //                                                           .SliderCircle(0f,
            //                                                                         1f,
            //                                                                         name: $"ContaminationThresholdHighSlider{index}",
            //                                                        builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                               .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                               .AddClass("slider")))
            //                                     .AddPreset(
            //                                         factory => factory.Toggles()
            //                                                           .CheckmarkInverted(name: $"ContaminationThresholdHighFloodgateHeightToggle{index}",
            //                                                                              color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                              locKey: "Floodgates.Triggers.HeightWhenAboveContaminationThresholdHigh",
            //                                                                              builder:
            //                                                                               builder => builder.SetStyle(
            //                                                                                   style => style.alignSelf = Align.Center)))
            //                                     .AddPreset(
            //                                         factory => factory.Sliders()
            //                                                           .SliderCircle(0f,
            //                                                                         3f,
            //                                                                         name: $"ContaminationThresholdHighFloodgateHeightSlider{index}",
            //                                                        builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                               .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                               .AddClass("slider")))
            //                                     .Build())
            //                       .BuildAndInitialize();

            return foo.BuildAndInitialize();
        }

        /// <summary>
        /// Createa a link view that is shown on the StreamGauge's fragment
        /// </summary>
        /// <returns></returns>
        public VisualElement CreateViewForStreamGauge(string objectLocKey)
        {
            var foo = _builder.Create<GameButton>().SetName("Target").BuildAndInitialize();
            // var foo = _builder.CreateComponentBuilder()
            //                          .CreateButton()
            //                          .SetName("Target")
            //                          .AddClass("entity-fragment__button")
            //                          .AddClass("entity-fragment__button--green")
            //                          .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
            //                          .SetFontSize(new Length(14, Pixel))
            //                          .SetFontStyle(FontStyle.Normal)
            //                          .SetHeight(new Length(30, Pixel))
            //                          .SetWidth(new Length(290, Pixel))
            //                          .SetPadding(new Padding(0, 0, 0, 0))
            //                          .SetMargin(new Margin(new Length(2, Pixel), 0, new Length(2, Pixel), 0))
            //                          .AddComponent(
            //                              _builder.CreateComponentBuilder()
            //                                      .CreateVisualElement()
            //                                      .SetFlexWrap(Wrap.Wrap)
            //                                      .SetFlexDirection(FlexDirection.Row)
            //                                      .SetJustifyContent(Justify.SpaceBetween)
            //                                      .AddComponent(
            //                                           _builder.CreateComponentBuilder()
            //                                                   .CreateVisualElement()
            //                                                   .SetFlexWrap(Wrap.Wrap)
            //                                                   .SetFlexDirection(FlexDirection.Row)
            //                                                   .SetJustifyContent(Justify.FlexStart)
            //                                                   .AddComponent(
            //                                                        _builder.CreateComponentBuilder()
            //                                                                .CreateVisualElement()
            //                                                                .SetName("ImageContainer")
            //                                                                .SetWidth(new Length(28, Pixel))
            //                                                                .SetHeight(new Length(28, Pixel))
            //                                                                .SetMargin(new Margin(new Length(1, Pixel), 0, 0, new Length(6, Pixel)))
            //                                                                .Build())
            //                                                   .AddPreset(factory => factory.Labels()
            //                                                                                .GameTextBig(text: _loc.T(objectLocKey),
            //                                                                                             builder: builder => builder.SetWidth(new Length(200, Pixel))
            //                                                                                                                        .SetStyle(style =>
            //                                                                                                                        {
            //                                                                                                                            //style.alignContent = Align.FlexStart;
            //                                                                                                                            //style.alignSelf = Align.FlexStart;
            //                                                                                                                            style.unityTextAlign = TextAnchor.MiddleLeft;
            //                                                                                                                            //style.alignItems = Align.FlexStart;
            //                                                                                                                        })))
            //                                                   .Build())
            //                                      .AddComponent(
            //                                          _builder.CreateComponentBuilder()
            //                                                .CreateButton()
            //                                                .AddClass("unity-text-element")
            //                                                .AddClass("unity-button")
            //                                                .AddClass("entity-panel__button")
            //                                                .AddClass("entity-panel__button--red")
            //                                                .AddClass("distribution-route__icon-wrapper")
            //                                                .SetName("DetachLinkButton")
            //                                                .SetMargin(new Margin(new Length(1, Pixel), new Length(2, Pixel), 0, 0))
            //                                                .AddComponent(_builder.CreateComponentBuilder()
            //                                                                      .CreateVisualElement()
            //                                                                      .AddClass("entity-panel__button")
            //                                                                      .AddClass("delete-building__icon")
            //                                                                      .Build())
            //                                                .Build())
            //                                      .Build()) 
            //                          .Build();

            return foo;
        }


        /// <summary>
        /// Create a link view that is shown on the Floodgate's fragment
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VisualElement CreateViewForWaterpump(int index, string streamgaugeLocKey)
        {
            var root = _builder.Create<GameButton>().SetName("Target").BuildAndInitialize();
            // var root = _builder.CreateComponentBuilder()
            //                   .CreateVisualElement()
            //                   .AddComponent(
            //                     _builder.CreateComponentBuilder()
            //                             .CreateButton()
            //                             .SetName("Target")
            //                             .AddClass("entity-fragment__button")
            //                             .AddClass("entity-fragment__button--green")
            //                             .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
            //                             .SetFontSize(new Length(14, Pixel))
            //                             .SetFontStyle(FontStyle.Normal)
            //                             .SetHeight(new Length(30, Pixel))
            //                             .SetWidth(new Length(290, Pixel))
            //                             .SetPadding(new Padding(0, 0, 0, 0))
            //                             .SetMargin(new Margin(new Length(2, Pixel), 0, new Length(2, Pixel), 0))
            //                             .AddComponent(
            //                                 _builder.CreateComponentBuilder()
            //                                         .CreateVisualElement()
            //                                         .SetFlexWrap(Wrap.Wrap)
            //                                         .SetFlexDirection(FlexDirection.Row)
            //                                         .SetJustifyContent(Justify.SpaceBetween)
            //                                         .AddComponent(
            //                                              _builder.CreateComponentBuilder()
            //                                                      .CreateVisualElement()
            //                                                      .SetFlexWrap(Wrap.Wrap)
            //                                                      .SetFlexDirection(FlexDirection.Row)
            //                                                      .SetJustifyContent(Justify.FlexStart)
            //                                                      .AddComponent(
            //                                                           _builder.CreateComponentBuilder()
            //                                                                   .CreateVisualElement()
            //                                                                   .SetName("ImageContainer")
            //                                                                   .SetWidth(new Length(28, Pixel))
            //                                                                   .SetHeight(new Length(28, Pixel))
            //                                                                   .SetMargin(new Margin(new Length(1, Pixel), 0, 0, new Length(6, Pixel)))
            //                                                                   .Build())
            //                                                      .AddPreset(factory => factory.Labels()
            //                                                                                   .GameTextBig(text: _loc.T(streamgaugeLocKey),
            //                                                                                                name: "StreamGaugeLabel"))
            //                                                      .AddPreset(factory => factory.Labels()
            //                                                                                   .GameTextBig(name: "StreamGaugeHeightLabel",
            //                                                                                                builder: builder => builder.SetMargin(new Margin(0, 0, 0, new Length(2, Pixel)))))
            //                                                      .Build())
            //                                         .AddComponent(
            //                                             _builder.CreateComponentBuilder()
            //                                                   .CreateButton()
            //                                                   .AddClass("unity-text-element")
            //                                                   .AddClass("unity-button")
            //                                                   .AddClass("entity-panel__button")
            //                                                   .AddClass("entity-panel__button--red")
            //                                                   .AddClass("distribution-route__icon-wrapper")
            //                                                   .SetName("DetachLinkButton")
            //                                                   .SetMargin(new Margin(new Length(1, Pixel), new Length(2, Pixel), 0, 0))
            //                                                   .AddComponent(_builder.CreateComponentBuilder()
            //                                                                         .CreateVisualElement()
            //                                                                         .AddClass("entity-panel__button")
            //                                                                         .AddClass("delete-building__icon")
            //                                                                         .Build())
            //                                                   .Build())
            //                                         .Build())
            //                                   .Build())
            //                       .AddPreset(
            //                             factory => factory.Toggles()
            //                                                 .CheckmarkInverted(locKey: "Floodgates.WaterpumpTrigger.DisableDuringDrought",
            //                                                                     name: $"DisableDuringDroughtToggle{index}",
            //                                                                     color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                     builder:
            //                                                                     builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
            //                         .AddPreset(
            //                             factory => factory.Toggles()
            //                                                 .CheckmarkInverted(locKey: "Floodgates.WaterpumpTrigger.DisableDuringBadtide",
            //                                                                     name: $"DisableDuringBadtideToggle{index}",
            //                                                                     color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                     builder:
            //                                                                     builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
            //                         .AddPreset(
            //                             factory => factory.Toggles()
            //                                                 .CheckmarkInverted(locKey: "Floodgates.WaterpumpTrigger.DisableDuringTemperate",
            //                                                                     name: $"DisableDuringTemperate{index}",
            //                                                                     color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                     builder:
            //                                                                     builder => builder.SetStyle(style => style.alignSelf = Align.Center)
            //                                                                                     .SetMargin(new Margin(new Length(3, Pixel), 0, new Length(10, Pixel), 0))))
            //                       .AddComponent(
            //                             _builder.CreateComponentBuilder()
            //                                     .CreateVisualElement()
            //                                     .SetFlexWrap(Wrap.Wrap)
            //                                     .SetFlexDirection(FlexDirection.Row)
            //                                     .SetJustifyContent(Justify.Center)
            //                                     .AddComponent(
            //                                         _builder.CreateComponentBuilder()
            //                                                 .CreateVisualElement()
            //                                                 .SetWidth(145)
            //                                                 .SetJustifyContent(Justify.Center)
            //                                                 .AddPreset(factory =>
            //                                                     factory.Labels()
            //                                                             .GameTextHeading(locKey: "Floodgates.Triggers.Depth",
            //                                                                              builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
                                                            
            //                                                 .AddPreset(
            //                                                     factory => factory.Toggles()
            //                                                                       .CheckmarkInverted(name: $"Threshold1Toggle{index}",
            //                                                                                          locKey: "Floodgates.WaterpumpTrigger.Threshold1",
            //                                                                                          color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                                          builder:
            //                                                                                           builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
            //                                                 .AddPreset(
            //                                                     factory => factory.Sliders()
            //                                                                       .SliderCircle(0f,
            //                                                                                     3f,
            //                                                                                     name: $"Threshold1Slider{index}",
            //                                                                                     builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                                                            .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                                                            .AddClass("slider")))
            //                                                 .AddPreset(
            //                                                     factory => factory.Toggles()
            //                                                                       .CheckmarkInverted(name: $"Threshold2Toggle{index}",
            //                                                                                          locKey: "Floodgates.WaterpumpTrigger.Threshold2",
            //                                                                                          color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                                          builder:
            //                                                                                           builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
            //                                                 .AddPreset(
            //                                                     factory => factory.Sliders()
            //                                                                       .SliderCircle(0f,
            //                                                                                     3f,
            //                                                                                     name: $"Threshold2Slider{index}",
            //                                                                                     builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                                                            .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                                                            .AddClass("slider")))
            //                                                 .AddPreset(
            //                                                     factory => factory.Toggles()
            //                                                                       .CheckmarkInverted(name: $"Threshold3Toggle{index}",
            //                                                                                          locKey: "Floodgates.WaterpumpTrigger.Threshold3",
            //                                                                                          color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                                          builder:
            //                                                                                           builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
            //                                                 .AddPreset(
            //                                                     factory => factory.Sliders()
            //                                                                       .SliderCircle(0f,
            //                                                                                     3f,
            //                                                                                     name: $"Threshold3Slider{index}",
            //                                                                                     builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                                                            .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                                                            .AddClass("slider")))
            //                                                 .AddPreset(
            //                                                     factory => factory.Toggles()
            //                                                                       .CheckmarkInverted(name: $"Threshold4Toggle{index}",
            //                                                                                          locKey: "Floodgates.WaterpumpTrigger.Threshold4",
            //                                                                                          color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                                          builder:
            //                                                                                           builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
            //                                                 .AddPreset(
            //                                                     factory => factory.Sliders()
            //                                                                       .SliderCircle(0f,
            //                                                                                     3f,
            //                                                                                     name: $"Threshold4Slider{index}",
            //                                                                                     builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                                                            .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                                                            .AddClass("slider")))
            //                                                 .BuildAndInitialize())
            //                                     .AddComponent(
            //                                         _builder.CreateComponentBuilder()
            //                                                 .CreateVisualElement()
            //                                                 .SetWidth(145)
            //                                                 .SetJustifyContent(Justify.Center)
            //                                                 .AddPreset(factory =>
            //                                                     factory.Labels()
            //                                                             .GameTextHeading(locKey: "Floodgates.Triggers.Contamination",
            //                                                                              builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
            //                                                 .AddPreset(
            //                                                     factory => factory.Toggles()
            //                                                                       .CheckmarkInverted(name: $"ContaminationPauseBelowToggle{index}",
            //                                                                                          locKey: "Floodgates.WaterpumpTrigger.Threshold1",
            //                                                                                          color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                                          builder:
            //                                                                                           builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
            //                                                 .AddPreset(
            //                                                     factory => factory.Sliders()
            //                                                                       .SliderCircle(0f,
            //                                                                                     1f,
            //                                                                                     name: $"ContaminationPauseBelowSlider{index}",
            //                                                                                     builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                                                            .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                                                            .AddClass("slider")))
            //                                                 .AddPreset(
            //                                                     factory => factory.Toggles()
            //                                                                       .CheckmarkInverted(name: $"ContaminationPauseAboveToggle{index}",
            //                                                                                          locKey: "Floodgates.WaterpumpTrigger.Threshold2",
            //                                                                                          color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                                          builder:
            //                                                                                           builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
            //                                                 .AddPreset(
            //                                                     factory => factory.Sliders()
            //                                                                       .SliderCircle(0f,
            //                                                                                     1f,
            //                                                                                     name: $"ContaminationPauseAboveSlider{index}",
            //                                                                                     builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                                                            .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                                                            .AddClass("slider")))
            //                                                 .AddPreset(
            //                                                     factory => factory.Toggles()
            //                                                                       .CheckmarkInverted(name: $"ContaminationUnpauseBelowToggle{index}",
            //                                                                                          locKey: "Floodgates.WaterpumpTrigger.Threshold3",
            //                                                                                          color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                                          builder:
            //                                                                                           builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
            //                                                 .AddPreset(
            //                                                     factory => factory.Sliders()
            //                                                                       .SliderCircle(0f,
            //                                                                                     1f,
            //                                                                                     name: $"ContaminationUnpauseBelowSlider{index}",
            //                                                                                     builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                                                            .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                                                            .AddClass("slider")))
            //                                                 .AddPreset(
            //                                                     factory => factory.Toggles()
            //                                                                       .CheckmarkInverted(name: $"ContaminationUnpauseAboveToggle{index}",
            //                                                                                          locKey: "Floodgates.WaterpumpTrigger.Threshold4",
            //                                                                                          color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
            //                                                                                          builder:
            //                                                                                           builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
            //                                                 .AddPreset(
            //                                                     factory => factory.Sliders()
            //                                                                       .SliderCircle(0f,
            //                                                                                     1f,
            //                                                                                     name: $"ContaminationUnpauseAboveSlider{index}",
            //                                                                                     builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
            //                                                                                                                            .SetPadding(new Padding(new Length(21, Pixel), 0))
            //                                                                                                                            .AddClass("slider")))
            //                                                 .BuildAndInitialize())
            //                                     .Build())
            //                       .AddComponent(
            //                             _builder.CreateComponentBuilder()
            //                                     .CreateVisualElement()
                                                
            //                                     .Build())
            //                       .BuildAndInitialize();

            return root;
        }

    }
}
