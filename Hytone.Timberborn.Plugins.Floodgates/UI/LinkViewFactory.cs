using Timberborn.Localization;
using TimberbornAPI.Common;
using TimberbornAPI.UIBuilderSystem;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.Length.Unit;

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
            var foo = _builder.CreateComponentBuilder()
                              .CreateVisualElement()
                              .AddComponent(
                                _builder.CreateComponentBuilder()
                                        .CreateButton()
                                        .SetName("Target")
                                        .AddClass("entity-fragment__button")
                                        .AddClass("entity-fragment__button--green")
                                        .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                        .SetFontSize(new Length(14, Pixel))
                                        .SetFontStyle(FontStyle.Normal)
                                        .SetHeight(new Length(30, Pixel))
                                        .SetWidth(new Length(290, Pixel))
                                        .SetPadding(new Padding(0, 0, 0, 0))
                                        .SetMargin(new Margin(new Length(2, Pixel), 0, new Length(2, Pixel), 0))
                                        .AddComponent(
                                            _builder.CreateComponentBuilder()
                                                    .CreateVisualElement()
                                                    .SetFlexWrap(Wrap.Wrap)
                                                    .SetFlexDirection(FlexDirection.Row)
                                                    .SetJustifyContent(Justify.SpaceBetween)
                                                    .AddComponent(
                                                         _builder.CreateComponentBuilder()
                                                                 .CreateVisualElement()
                                                                 .SetFlexWrap(Wrap.Wrap)
                                                                 .SetFlexDirection(FlexDirection.Row)
                                                                 .SetJustifyContent(Justify.FlexStart)
                                                                 .AddComponent(
                                                                      _builder.CreateComponentBuilder()
                                                                              .CreateVisualElement()
                                                                              .SetName("ImageContainer")
                                                                              .SetWidth(new Length(28, Pixel))
                                                                              .SetHeight(new Length(28, Pixel))
                                                                              .SetMargin(new Margin(new Length(1, Pixel), 0, 0, new Length(6, Pixel)))
                                                                              .Build())
                                                                 .AddPreset(factory => factory.Labels()
                                                                                              .GameTextBig(text: _loc.T(streamgaugeLocKey),
                                                                                                           builder: builder => builder.SetWidth(new Length(100, Pixel))))
                                                                 .Build())
                                                    .AddComponent(
                                                        _builder.CreateComponentBuilder()
                                                              .CreateButton()
                                                              .AddClass("unity-text-element")
                                                              .AddClass("unity-button")
                                                              .AddClass("entity-panel__button")
                                                              .AddClass("entity-panel__button--red")
                                                              .AddClass("distribution-route__icon-wrapper")
                                                              .SetName("DetachLinkButton")
                                                              .SetMargin(new Margin(new Length(1, Pixel), new Length(2, Pixel), 0, 0))
                                                              .AddComponent(_builder.CreateComponentBuilder()
                                                                                    .CreateVisualElement()
                                                                                    .AddClass("entity-panel__button")
                                                                                    .AddClass("delete-building__icon")
                                                                                    .Build())
                                                              .Build())
                                                    .Build())
                                              .Build())
                                  .AddComponent(
                                        _builder.CreateComponentBuilder()
                                                .CreateVisualElement()
                                                .AddPreset(
                                                    factory => factory.Labels()
                                                                      .GameTextBig(name: $"Threshold1Label{index}",
                                                                                   locKey: "Floodgates.Triggers.Threshold1",
                                                                                   builder:
                                                                                    builder => builder.SetStyle(
                                                                                        style => style.alignSelf = Align.Center)))
                                                .AddPreset(
                                                    factory => factory.Sliders()
                                                                      .SliderCircle(0f,
                                                                                    3f,
                                                                                    name: $"Threshold1Slider{index}",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, Pixel), 0))))
                                                .AddPreset(
                                                    factory => factory.Labels()
                                                                      .GameTextBig(name: $"Threshold1FloodgateHeightLabel{index}",
                                                                                   locKey: "Floodgates.Triggers.HeightWhenBelowThreshold1",
                                                                                   builder:
                                                                                    builder => builder.SetStyle(
                                                                                        style => style.alignSelf = Align.Center)))
                                                .AddPreset(
                                                    factory => factory.Sliders()
                                                                      .SliderCircle(0f,
                                                                                    3f,
                                                                                    name: $"Threshold1FloodgateHeightSlider{index}",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, Pixel), 0))))
                                                .AddPreset(
                                                    factory => factory.Labels()
                                                                      .GameTextBig(name: $"Threshold2Label{index}",
                                                                                   locKey: "Floodgates.Triggers.Threshold2",
                                                                                   builder:
                                                                                    builder => builder.SetStyle(
                                                                                        style => style.alignSelf = Align.Center)))
                                                .AddPreset(
                                                    factory => factory.Sliders()
                                                                      .SliderCircle(0f,
                                                                                    3f,
                                                                                    name: $"Threshold2Slider{index}",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, Pixel), 0))))
                                                .AddPreset(
                                                    factory => factory.Labels()
                                                                      .GameTextBig(name: $"Threshold2FloodgateHeightLabel{index}",
                                                                                   locKey: "Floodgates.Triggers.HeightWhenAboveThreshold2",
                                                                                   builder:
                                                                                    builder => builder.SetStyle(
                                                                                        style => style.alignSelf = Align.Center)))
                                                .AddPreset(
                                                    factory => factory.Sliders()
                                                                      .SliderCircle(0f,
                                                                                    3f,
                                                                                    name: $"Threshold2FloodgateHeightSlider{index}",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, Pixel), 0))))
                                                .Build())
                                  .Build();

            return foo;
        }

        /// <summary>
        /// Createa a link view that is shown on the StreamGauge's fragment
        /// </summary>
        /// <returns></returns>
        public VisualElement CreateViewForStreamGauge(string objectLocKey)
        {
            var foo = _builder.CreateComponentBuilder()
                                     .CreateButton()
                                     .SetName("Target")
                                     .AddClass("entity-fragment__button")
                                     .AddClass("entity-fragment__button--green")
                                     .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                     .SetFontSize(new Length(14, Pixel))
                                     .SetFontStyle(FontStyle.Normal)
                                     .SetHeight(new Length(30, Pixel))
                                     .SetWidth(new Length(290, Pixel))
                                     .SetPadding(new Padding(0, 0, 0, 0))
                                     .SetMargin(new Margin(new Length(2, Pixel), 0, new Length(2, Pixel), 0))
                                     .AddComponent(
                                         _builder.CreateComponentBuilder()
                                                 .CreateVisualElement()
                                                 .SetFlexWrap(Wrap.Wrap)
                                                 .SetFlexDirection(FlexDirection.Row)
                                                 .SetJustifyContent(Justify.SpaceBetween)
                                                 .AddComponent(
                                                      _builder.CreateComponentBuilder()
                                                              .CreateVisualElement()
                                                              .SetFlexWrap(Wrap.Wrap)
                                                              .SetFlexDirection(FlexDirection.Row)
                                                              .SetJustifyContent(Justify.FlexStart)
                                                              .AddComponent(
                                                                   _builder.CreateComponentBuilder()
                                                                           .CreateVisualElement()
                                                                           .SetName("ImageContainer")
                                                                           .SetWidth(new Length(28, Pixel))
                                                                           .SetHeight(new Length(28, Pixel))
                                                                           .SetMargin(new Margin(new Length(1, Pixel), 0, 0, new Length(6, Pixel)))
                                                                           .Build())
                                                              .AddPreset(factory => factory.Labels()
                                                                                           .GameTextBig(text: _loc.T(objectLocKey),
                                                                                                        builder: builder => builder.SetWidth(new Length(200, Pixel))
                                                                                                                                   .SetStyle(style =>
                                                                                                                                   {
                                                                                                                                       //style.alignContent = Align.FlexStart;
                                                                                                                                       //style.alignSelf = Align.FlexStart;
                                                                                                                                       style.unityTextAlign = TextAnchor.MiddleLeft;
                                                                                                                                       //style.alignItems = Align.FlexStart;
                                                                                                                                   })))
                                                              .Build())
                                                 .AddComponent(
                                                     _builder.CreateComponentBuilder()
                                                           .CreateButton()
                                                           .AddClass("unity-text-element")
                                                           .AddClass("unity-button")
                                                           .AddClass("entity-panel__button")
                                                           .AddClass("entity-panel__button--red")
                                                           .AddClass("distribution-route__icon-wrapper")
                                                           .SetName("DetachLinkButton")
                                                           .SetMargin(new Margin(new Length(1, Pixel), new Length(2, Pixel), 0, 0))
                                                           .AddComponent(_builder.CreateComponentBuilder()
                                                                                 .CreateVisualElement()
                                                                                 .AddClass("entity-panel__button")
                                                                                 .AddClass("delete-building__icon")
                                                                                 .Build())
                                                           .Build())
                                                 .Build()) 
                                     .Build();

            return foo;
        }


        /// <summary>
        /// Create a link view that is shown on the Floodgate's fragment
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VisualElement CreateViewForWaterpump(int index, string streamgaugeLocKey)
        {
            var root = _builder.CreateComponentBuilder()
                              .CreateVisualElement()
                              .AddComponent(
                                _builder.CreateComponentBuilder()
                                        .CreateButton()
                                        .SetName("Target")
                                        .AddClass("entity-fragment__button")
                                        .AddClass("entity-fragment__button--green")
                                        .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                        .SetFontSize(new Length(14, Pixel))
                                        .SetFontStyle(FontStyle.Normal)
                                        .SetHeight(new Length(30, Pixel))
                                        .SetWidth(new Length(290, Pixel))
                                        .SetPadding(new Padding(0, 0, 0, 0))
                                        .SetMargin(new Margin(new Length(2, Pixel), 0, new Length(2, Pixel), 0))
                                        .AddComponent(
                                            _builder.CreateComponentBuilder()
                                                    .CreateVisualElement()
                                                    .SetFlexWrap(Wrap.Wrap)
                                                    .SetFlexDirection(FlexDirection.Row)
                                                    .SetJustifyContent(Justify.SpaceBetween)
                                                    .AddComponent(
                                                         _builder.CreateComponentBuilder()
                                                                 .CreateVisualElement()
                                                                 .SetFlexWrap(Wrap.Wrap)
                                                                 .SetFlexDirection(FlexDirection.Row)
                                                                 .SetJustifyContent(Justify.FlexStart)
                                                                 .AddComponent(
                                                                      _builder.CreateComponentBuilder()
                                                                              .CreateVisualElement()
                                                                              .SetName("ImageContainer")
                                                                              .SetWidth(new Length(28, Pixel))
                                                                              .SetHeight(new Length(28, Pixel))
                                                                              .SetMargin(new Margin(new Length(1, Pixel), 0, 0, new Length(6, Pixel)))
                                                                              .Build())
                                                                 .AddPreset(factory => factory.Labels()
                                                                                              .GameTextBig(text: _loc.T(streamgaugeLocKey),
                                                                                                           builder: builder => builder.SetWidth(new Length(100, Pixel))))
                                                                 .Build())
                                                    .AddComponent(
                                                        _builder.CreateComponentBuilder()
                                                              .CreateButton()
                                                              .AddClass("unity-text-element")
                                                              .AddClass("unity-button")
                                                              .AddClass("entity-panel__button")
                                                              .AddClass("entity-panel__button--red")
                                                              .AddClass("distribution-route__icon-wrapper")
                                                              .SetName("DetachLinkButton")
                                                              .SetMargin(new Margin(new Length(1, Pixel), new Length(2, Pixel), 0, 0))
                                                              .AddComponent(_builder.CreateComponentBuilder()
                                                                                    .CreateVisualElement()
                                                                                    .AddClass("entity-panel__button")
                                                                                    .AddClass("delete-building__icon")
                                                                                    .Build())
                                                              .Build())
                                                    .Build())
                                              .Build())
                                  .AddComponent(
                                        _builder.CreateComponentBuilder()
                                                .CreateVisualElement()
                                                .AddPreset(
                                                    factory => factory.Toggles()
                                                                      .CheckmarkInverted(name: $"Threshold1Toggle{index}",
                                                                                         locKey: "Floodgates.WaterpumpTrigger.Threshold1",
                                                                                         color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                                         builder:
                                                                                          builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
                                                .AddPreset(
                                                    factory => factory.Sliders()
                                                                      .SliderCircle(0f,
                                                                                    3f,
                                                                                    name: $"Threshold1Slider{index}",
                                                                                    builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                                           .SetPadding(new Padding(new Length(21, Pixel), 0))))
                                                .AddPreset(
                                                    factory => factory.Toggles()
                                                                      .CheckmarkInverted(name: $"Threshold2Toggle{index}",
                                                                                         locKey: "Floodgates.WaterpumpTrigger.Threshold2",
                                                                                         color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                                         builder:
                                                                                          builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
                                                .AddPreset(
                                                    factory => factory.Sliders()
                                                                      .SliderCircle(0f,
                                                                                    3f,
                                                                                    name: $"Threshold2Slider{index}",
                                                                                    builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                                           .SetPadding(new Padding(new Length(21, Pixel), 0))))
                                                .AddPreset(
                                                    factory => factory.Toggles()
                                                                      .CheckmarkInverted(name: $"Threshold3Toggle{index}",
                                                                                         locKey: "Floodgates.WaterpumpTrigger.Threshold3",
                                                                                         color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                                         builder:
                                                                                          builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
                                                .AddPreset(
                                                    factory => factory.Sliders()
                                                                      .SliderCircle(0f,
                                                                                    3f,
                                                                                    name: $"Threshold3Slider{index}",
                                                                                    builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                                           .SetPadding(new Padding(new Length(21, Pixel), 0))))
                                                .AddPreset(
                                                    factory => factory.Toggles()
                                                                      .CheckmarkInverted(name: $"Threshold4Toggle{index}",
                                                                                         locKey: "Floodgates.WaterpumpTrigger.Threshold4",
                                                                                         color: new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)),
                                                                                         builder:
                                                                                          builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
                                                .AddPreset(
                                                    factory => factory.Sliders()
                                                                      .SliderCircle(0f,
                                                                                    3f,
                                                                                    name: $"Threshold4Slider{index}",
                                                                                    builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                                           .SetPadding(new Padding(new Length(21, Pixel), 0))))
                                                .Build())
                                  .Build();

            return root;
        }

    }
}
