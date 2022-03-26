using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.EntityPanelSystem;
using Timberborn.CoreUI;
using Timberborn.DistributionSystem;
using Timberborn.DistributionSystemUI;
using Timberborn.EntityPanelSystem;
using Timberborn.InventorySystemUI;
using UnityEngine;
using UnityEngine.UIElements;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using TimberbornAPI.UIBuilderSystem;
using static UnityEngine.UIElements.Length.Unit;
using Timberborn.WaterBuildings;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    internal class TriggersFragment : IEntityPanelFragment
    {

        private readonly UIBuilder _builder;
        private static readonly string SelectedTabButtonCLass = "distribution-post-fragment__tab-button--selected";

        private VisualElement _root;

        private Floodgate _floodgate;
        private FloodgateTriggerMonoBehaviour _floodgateTriggerMonoBehaviour;

        private Button _basicButton;
        private Button _advancedButton;
        private Button _newButton;

        private VisualElement _basicTab;
        private VisualElement _advancedTab;

        private AttachToStreamGaugeFragment _attachToStreamGaugeFragment;
        private FloodGateUIFragment _droughtSettingsFragment;
        private FloodgateScheduleFragment _scheduleFragment;

        public TriggersFragment(UIBuilder builder,
                                AttachToStreamGaugeFragment attachToStreamGaugeFragment,
                                FloodGateUIFragment floodgateUIFragment,
                                FloodgateScheduleFragment scheduleFragment)
        {
            _builder = builder;
            //_basicButton.style.color
            _attachToStreamGaugeFragment = attachToStreamGaugeFragment;
            _droughtSettingsFragment = floodgateUIFragment;
            _scheduleFragment = scheduleFragment;
        }

        public VisualElement InitializeFragment()
        {
            //VisualElement child = _builder.CreateFragmentBuilder()
            //                    .AddPreset(builder => builder.Labels()
            //                                                    .DefaultBig(text: "Foo"))
            //                    .BuildAndInitialize();
            VisualElement child = _builder.CreateComponentBuilder()
                                          .CreateLabel()
                                          .AddPreset(builder => builder.Labels()
                                                                       .DefaultBig(text: "Bar"))
                                          .BuildAndInitialize();

            var rootBuilder = _builder.CreateFragmentBuilder()
                                      .ModifyWrapper(builder => builder.SetFlexDirection(FlexDirection.Row)
                                                                       .SetFlexWrap(Wrap.Wrap)
                                                                       .SetJustifyContent(Justify.Center))
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateButton()
                                                            //.AddClass(TimberApiStyle.Buttons.Normal.ButtonGame)
                                                            //.AddClass(TimberApiStyle.Buttons.Hover.ButtonGameHover)
                                                            //.AddClass(TimberApiStyle.Sounds.Click)
                                                            //.AddClass(TimberApiStyle.Scales.Scale5)
                                                            .AddClass("distribution-post-fragment__tab-button")
                                                            .AddClass("distribution-post-fragment__tab-button--selected")
                                                            .SetName("BasicButton")
                                                            .SetLocKey("Floodgate.Triggers.Basic")
                                                            .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                                            .SetFontSize(new Length(14, Pixel))
                                                            .SetFontStyle(FontStyle.Normal)
                                                            .SetHeight(new Length(29, Pixel))
                                                            .SetWidth(new Length(145, Pixel))
                                                            .Build())
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateButton()
                                                            //.AddClass(TimberApiStyle.Buttons.Normal.ButtonGame)
                                                            //.AddClass(TimberApiStyle.Buttons.Hover.ButtonGameHover)
                                                            //.AddClass(TimberApiStyle.Sounds.Click)
                                                            //.AddClass(TimberApiStyle.Scales.Scale5)
                                                            .AddClass("distribution-post-fragment__tab-button")
                                                            .SetName("AdvancedButton")
                                                            .SetLocKey("Floodgate.Triggers.Advanced")
                                                            .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                                            .SetFontSize(new Length(14, Pixel))
                                                            .SetFontStyle(FontStyle.Normal)
                                                            .SetHeight(new Length(29, Pixel))
                                                            .SetWidth(new Length(145, Pixel))
                                                            .Build())
                                      .AddPreset(factory => factory.ScrollViews()
                                                                   .MainScrollView(name: "BasicTab",
                                                                                   height: new Length(300, Pixel)))
                                      //.AddComponent(builder => builder.AddPreset(factory => factory.ScrollViews()
                                      //                                                              .MainScrollView(name: "AdvancedTab",
                                      //                                                                               height: new Length(250, Pixel)))
                                      //                                .AddComponent(_builder.CreateComponentBuilder()
                                      //                                                      .CreateButton()
                                      //                                                      .AddClass("entity-fragment__button")
                                      //                                                      .AddClass("entity-fragment__button--green")
                                      //                                                      .SetName("NewStreamGaugeButton")
                                      //                                                      .SetLocKey("Floodgate.Triggers.NewStreamGauge")
                                      //                                                      .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                      //                                                      .SetFontSize(new Length(14, Pixel))
                                      //                                                      .SetFontStyle(FontStyle.Normal)
                                      //                                                      .SetHeight(new Length(29, Pixel))
                                      //                                                      .SetWidth(new Length(290, Pixel))
                                      //                                                      .Build()));
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateVisualElement()
                                                            .SetName("AdvancedTab")
                                                            .SetHeight(new Length(300, Pixel))
                                                            .AddComponent(builder => builder.AddPreset(factory => factory.ScrollViews()
                                                                                            .MainScrollView(name: "AdvancedScroll",
                                                                                                            height: new Length(250, Pixel)))
                                                                                            .AddComponent(_builder.CreateComponentBuilder()
                                                                                                                  .CreateButton()
                                                                                                                  .AddClass("entity-fragment__button")
                                                                                                                  .AddClass("entity-fragment__button--green")
                                                                                                                  .SetName("NewStreamGaugeButton")
                                                                                                                  .SetLocKey("Floodgate.Triggers.NewStreamGauge")
                                                                                                                  .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                                                                                                  .SetFontSize(new Length(14, Pixel))
                                                                                                                  .SetFontStyle(FontStyle.Normal)
                                                                                                                  .SetHeight(new Length(29, Pixel))
                                                                                                                  .SetWidth(new Length(290, Pixel))
                                                                                                                  .Build()))
                                                            .BuildAndInitialize());


            //.AddPreset(factory => factory.Buttons)                             
            //.AddPreset(factory => factory.Buttons().ButtonGame(locKey: "Floodgate.Triggers.Basic",
            //                                                   name: "BasicButton",
            //                                                   width: new Length(147, Pixel),
            //                                                   height: new Length(29,  Pixel),
            //                                                   fontStyle: FontStyle.Normal
            //                                                   ))
            //.AddPreset(factory => factory.Buttons().ButtonGame(locKey: "Floodgate.Triggers.Advanced",
            //                                                   name: "AdvancedButton",
            //                                                   width: new Length(147, Pixel),
            //                                                   height: new Length(29,  Pixel),
            //                                                   fontStyle: FontStyle.Italic
            //                                                   ));



            _root = rootBuilder.BuildAndInitialize();
            this._root.ToggleDisplayStyle(false);

            _basicTab = _root.Q<VisualElement>("BasicTab");
            _basicTab.Add(_droughtSettingsFragment.InitializeFragment());
            _basicTab.Add(_scheduleFragment.InitializeFragment());

            _advancedTab = _root.Q<VisualElement>("AdvancedTab");
            _advancedTab.Add(_attachToStreamGaugeFragment.InitiliazeFragment(_root));

            _basicButton = _root.Q<Button>("BasicButton");
            _basicButton.clicked += () => SwitchTriggerTab(true);
            _advancedButton = _root.Q<Button>("AdvancedButton");
            _advancedButton.clicked += () => SwitchTriggerTab(false);

            _newButton = _root.Q<Button>("NewStreamGaugeButton");

            return _root;
        }

        public void ShowFragment(GameObject entity)
        {
            _floodgate = entity.GetComponent<Floodgate>();
            _floodgateTriggerMonoBehaviour = entity.GetComponent<FloodgateTriggerMonoBehaviour>();
            if ((bool)_floodgateTriggerMonoBehaviour)
            {
                _droughtSettingsFragment.ShowFragment(_floodgate, _floodgateTriggerMonoBehaviour);
                _scheduleFragment.ShowFragment(_floodgate, _floodgateTriggerMonoBehaviour);
                _attachToStreamGaugeFragment.ShowFragment(_floodgateTriggerMonoBehaviour);
                SwitchTriggerTab(true);
            }
        }

        public void ClearFragment()
        {
            _floodgateTriggerMonoBehaviour = null;
            _droughtSettingsFragment.ClearFragment();
            _scheduleFragment.ClearFragment();
            _attachToStreamGaugeFragment.ClearFragment();
            _root.ToggleDisplayStyle(visible: false);
        }

        public void UpdateFragment()
        {
            if((bool)_floodgateTriggerMonoBehaviour)
            {
                _droughtSettingsFragment.UpdateFragment();
                _scheduleFragment.UpdateFragment();
                _attachToStreamGaugeFragment.UpdateFragment();
                _root.ToggleDisplayStyle(visible: true);
            }
        }

        public void SwitchTriggerTab(bool showBasic)
        {
            _basicButton.EnableInClassList(SelectedTabButtonCLass, showBasic);
            _basicTab.ToggleDisplayStyle(showBasic);
            _advancedButton.EnableInClassList(SelectedTabButtonCLass, !showBasic);
            _advancedTab.ToggleDisplayStyle(!showBasic);

            _newButton.ToggleDisplayStyle(!showBasic);
        }
    }
}
