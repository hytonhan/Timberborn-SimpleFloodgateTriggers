using System;
using System.Collections.Generic;
using System.Text;
using TimberbornAPI.Common;
using TimberbornAPI.UIBuilderSystem;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.Length.Unit;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    public class StreamGaugeFloodgateLinkViewFactory
    {
        private readonly UIBuilder _builder;
        public StreamGaugeFloodgateLinkViewFactory(
            UIBuilder builder)
        {
            _builder = builder;
        }


        public VisualElement CreateViewForFloodgate()
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
                                                                                        .GameTextBig(text: "StreamGauge",
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
                                  .AddComponent(
                                        _builder.CreateComponentBuilder()
                                                .CreateVisualElement()
                                                .AddPreset(
                                                    factory => factory.Labels()
                                                                      .GameTextBig(name: "Threshold1Label",
                                                                                   locKey: "Floodgates.Triggers.Threshold1",
                                                                                   builder: 
                                                                                    builder => builder.SetStyle(
                                                                                        style => style.alignSelf = Align.Center)))
                                                .AddPreset(
                                                    factory => factory.Sliders()
                                                                      .SliderCircle(0f,
                                                                                    3f,
                                                                                    name: "Threshold1Slider",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, Pixel), 0))))
                                                .AddPreset(
                                                    factory => factory.Labels()
                                                                      .GameTextBig(name: "Threshold1FloodgateHeightLabel",
                                                                                   locKey: "Floodgates.Triggers.HeightWhenBelowThreshold1",
                                                                                   builder:
                                                                                    builder => builder.SetStyle(
                                                                                        style => style.alignSelf = Align.Center)))
                                                .AddPreset(
                                                    factory => factory.Sliders()
                                                                      .SliderCircle(0f,
                                                                                    3f,
                                                                                    name: "Threshold1FloodgateHeightSlider",
                                                                   builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                          .SetPadding(new Padding(new Length(21, Pixel), 0))))
                                                .Build())
                                  .Build();

            return foo;
        }

        public VisualElement CreateViewForStreamGauge()
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
                                                                                           .GameTextBig(text: "Floodgate",
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
                                     .Build();

            return foo;
        }
    }
}
