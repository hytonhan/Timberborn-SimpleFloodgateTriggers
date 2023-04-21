using Timberborn.EntityPanelSystem;
using Timberborn.CoreUI;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.Length.Unit;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using TimberApi.UiBuilderSystem;
using Timberborn.BaseComponentSystem;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    public enum ActiveTab
    {
        Basic,
        Timer,
        Advanced
    }

    public class WaterPumpFragment : IEntityPanelFragment
    {

        private readonly UIBuilder _builder;
        private static readonly string SelectedTabButtonCLass = "distribution-post-fragment__tab-button--selected";
        private VisualElement _root;

        //private WaterInput _waterInput;
        private WaterPumpMonobehaviour _waterpumpMono;

        private Button _basicButton;
        private Button _advancedButton;
        private Button _timerButton;
        private Button _newButton;

        private VisualElement _basicTab;
        private VisualElement _timerTab;
        private VisualElement _advancedTab;
        //private VisualElement _floodgatesLinks;
        private VisualElement _links;

        private AttachWaterpumpToStreamGaugeFragment _attachWaterpumpToStreamGaugeFragment;
        private WaterpumpDroughtSettingsFragment _droughtSettingsFragment;
        private WaterpumpScheduleFragment _scheduleFragment;

        private ActiveTab _lastActiveTab = ActiveTab.Basic;


        //private AttachWaterpumpToStreamGaugeFragment _attachWaterpumpToStreamGaugeFragment;

        public WaterPumpFragment(
            UIBuilder builder,
            AttachWaterpumpToStreamGaugeFragment attachWaterpumpToStreamGaugeFragment,
            WaterpumpDroughtSettingsFragment droughtSettingsFragment,
            WaterpumpScheduleFragment scheduleFragment)
        {
            _builder = builder;
            _attachWaterpumpToStreamGaugeFragment = attachWaterpumpToStreamGaugeFragment;
            _droughtSettingsFragment = droughtSettingsFragment;
            _scheduleFragment = scheduleFragment;
        }

        public VisualElement InitializeFragment()
        {
            var rootBuilder = _builder.CreateFragmentBuilder()
                                      .ModifyWrapper(builder => builder.SetFlexDirection(FlexDirection.Row)
                                                                       .SetFlexWrap(Wrap.Wrap)
                                                                       .SetJustifyContent(Justify.Center))
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateButton()
                                                            .AddClass("distribution-post-fragment__tab-button")
                                                            .AddClass("distribution-post-fragment__tab-button--selected")
                                                            .SetName("BasicButton")
                                                            .SetLocKey("Floodgate.Triggers.Basic")
                                                            .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                                            .SetFontSize(new Length(14, Pixel))
                                                            .SetFontStyle(FontStyle.Normal)
                                                            .SetHeight(new Length(29, Pixel))
                                                            .SetWidth(new Length(96, Pixel))
                                                            .Build())
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateButton()
                                                            .AddClass("distribution-post-fragment__tab-button")
                                                            .AddClass("distribution-post-fragment__tab-button--selected")
                                                            .SetName("TimerButton")
                                                            .SetLocKey("Floodgate.Triggers.Timer")
                                                            .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                                            .SetFontSize(new Length(14, Pixel))
                                                            .SetFontStyle(FontStyle.Normal)
                                                            .SetHeight(new Length(29, Pixel))
                                                            .SetWidth(new Length(96, Pixel))
                                                            .Build())
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateButton()
                                                            .AddClass("distribution-post-fragment__tab-button")
                                                            .SetName("AdvancedButton")
                                                            .SetLocKey("Floodgate.Triggers.Advanced")
                                                            .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                                            .SetFontSize(new Length(14, Pixel))
                                                            .SetFontStyle(FontStyle.Normal)
                                                            .SetHeight(new Length(29, Pixel))
                                                            .SetWidth(new Length(96, Pixel))
                                                            .Build())
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateVisualElement()
                                                            .SetName("BasicTab")
                                                            //.SetHeight(new Length(420, Pixel))
                                                            .SetPadding(new Padding(new Length(8, Pixel)))
                                                            .Build())
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateVisualElement()
                                                            .SetName("TimerTab")
                                                            //.SetHeight(new Length(420, Pixel))
                                                            .SetPadding(new Padding(new Length(8, Pixel)))
                                                            .Build())
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateVisualElement()
                                                            .SetName("AdvancedTab")
                                                            .SetPadding(new Padding(new Length(8, Pixel), 0, 0, 0))
                                                            .AddComponent(builder => builder.AddComponent(_builder.CreateComponentBuilder()
                                                                                                                  .CreateVisualElement()
                                                                                                                  .SetName("Placeholder")
                                                                                                                  .BuildAndInitialize())
                                                                                            .AddComponent(_builder.CreateComponentBuilder()
                                                                                                                  .CreateButton()
                                                                                                                  .AddClass("entity-fragment__button")
                                                                                                                  .AddClass("entity-fragment__button--green")
                                                                                                                  .SetName("NewStreamGaugeButton")
                                                                                                                  .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                                                                                                  .SetFontSize(new Length(13, Pixel))
                                                                                                                  .SetFontStyle(FontStyle.Normal)
                                                                                                                  .SetHeight(new Length(29, Pixel))
                                                                                                                  .SetWidth(new Length(290, Pixel))
                                                                                                                  .Build()))
                                                            .Build());
            
            _root = rootBuilder.BuildAndInitialize();


            _links = _root.Q<VisualElement>("Placeholder");
            _links.Add(_attachWaterpumpToStreamGaugeFragment.InitiliazeFragment(_root));

            this._root.ToggleDisplayStyle(false);

            _basicTab = _root.Q<VisualElement>("BasicTab");
            _basicTab.Add(_droughtSettingsFragment.InitiliazeFragment(_root));

            _timerTab = _root.Q<VisualElement>("TimerTab");
            _timerTab.Add(_scheduleFragment.InitializeFragment());

            _advancedTab = _root.Q<VisualElement>("AdvancedTab");

            _basicButton = _root.Q<Button>("BasicButton");
            _basicButton.clicked += () =>
            {
                _lastActiveTab = ActiveTab.Basic;
                SwitchTriggerTab(_lastActiveTab);
            };
            _timerButton = _root.Q<Button>("TimerButton");
            _timerButton.clicked += () =>
            {
                _lastActiveTab = ActiveTab.Timer;
                SwitchTriggerTab(_lastActiveTab);
            };
            _advancedButton = _root.Q<Button>("AdvancedButton");
            _advancedButton.clicked += () =>
            {
                _lastActiveTab = ActiveTab.Advanced;
                SwitchTriggerTab(_lastActiveTab);
            };

            _newButton = _root.Q<Button>("NewStreamGaugeButton");

            return _root;
        }

        public void ShowFragment(BaseComponent entity)
        {
            _waterpumpMono = entity.GetComponentFast<WaterPumpMonobehaviour>();
            if ((bool)_waterpumpMono)
            {
                _droughtSettingsFragment.ShowFragment( _waterpumpMono);
                _attachWaterpumpToStreamGaugeFragment.ShowFragment(_waterpumpMono);
                _scheduleFragment.ShowFragment(_waterpumpMono);
                SwitchTriggerTab(_lastActiveTab);
            }
        }

        public void ClearFragment()
        {
            _waterpumpMono = null;
            _droughtSettingsFragment.ClearFragment();
            _attachWaterpumpToStreamGaugeFragment.ClearFragment();
            _scheduleFragment.ClearFragment();
            _root.ToggleDisplayStyle(visible: false);
        }

        public void UpdateFragment()
        {
            if ((bool)_waterpumpMono)
            {
                _droughtSettingsFragment.UpdateFragment();
                _attachWaterpumpToStreamGaugeFragment.UpdateFragment();
                _scheduleFragment.UpdateFragment();
                _root.ToggleDisplayStyle(visible: true);

            }
        }

        public void SwitchTriggerTab(ActiveTab tabToShow)
        {
            _basicButton.EnableInClassList(SelectedTabButtonCLass, tabToShow == ActiveTab.Basic ? true : false);
            _basicTab.ToggleDisplayStyle(tabToShow == ActiveTab.Basic ? true : false);

            _timerButton.EnableInClassList(SelectedTabButtonCLass, tabToShow == ActiveTab.Timer ? true : false);
            _timerTab.ToggleDisplayStyle(tabToShow == ActiveTab.Timer ? true : false);

            _advancedButton.EnableInClassList(SelectedTabButtonCLass, tabToShow == ActiveTab.Advanced ? true : false);
            _advancedTab.ToggleDisplayStyle(tabToShow == ActiveTab.Advanced ? true : false);

            _newButton.ToggleDisplayStyle(tabToShow == ActiveTab.Advanced ? true : false);
        }
    }
}
