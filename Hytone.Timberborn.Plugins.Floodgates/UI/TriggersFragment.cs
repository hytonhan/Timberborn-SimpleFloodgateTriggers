using Timberborn.EntityPanelSystem;
using Timberborn.CoreUI;
using UnityEngine;
using UnityEngine.UIElements;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using static UnityEngine.UIElements.Length.Unit;
using Timberborn.WaterBuildings;
using TimberApi.UiBuilderSystem;
using Timberborn.BaseComponentSystem;
using System;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    /// <summary>
    /// The container fragment that holds all the trigger related
    /// UI Element related to a Floodgate
    /// </summary>
    public class TriggersFragment : IEntityPanelFragment
    {

        private readonly UIBuilder _builder;
        private static readonly string SelectedTabButtonCLass = "distribution-post-fragment__tab-button--selected";

        private VisualElement _root;

        private Floodgate _floodgate;
        private FloodgateTriggerMonoBehaviour _floodgateTriggerMonoBehaviour;

        private Button _basicButton;
        private Button _advancedButton;
        private Button _timerButton;
        private Button _newButton;

        private VisualElement _basicTab;
        private VisualElement _advancedTab;
        private VisualElement _timerTab;
        private VisualElement _floodgatesLinks;

        private AttachFloodgateToStreamGaugeFragment _attachToStreamGaugeFragment;
        private FloodgateHazardFragment _droughtSettingsFragment;
        private FloodgateScheduleFragment _scheduleFragment;

        private readonly Texture2D _buttonGameImage;
        private readonly Texture2D _buttonGameActiveImage;

        //private bool _lastActiveTabWasBasic = true;
        private ActiveTab _lastActiveTab = ActiveTab.Basic;

        public TriggersFragment(UIBuilder builder,
                                AttachFloodgateToStreamGaugeFragment attachToStreamGaugeFragment,
                                FloodgateHazardFragment floodgateUIFragment,
                                FloodgateScheduleFragment scheduleFragment)
        {
            _builder = builder;
            _attachToStreamGaugeFragment = attachToStreamGaugeFragment;
            _droughtSettingsFragment = floodgateUIFragment;
            _scheduleFragment = scheduleFragment;

            _buttonGameImage = Resources.Load<Texture2D>("ui/images/buttons/button-game");
            _buttonGameActiveImage = Resources.Load<Texture2D>("ui/images/buttons/button-game-active");
        }

        public VisualElement InitializeFragment()
        {

            var rootBuilder = _builder.CreateFragmentBuilder()
                                      .ModifyWrapper(builder => builder.SetFlexDirection(FlexDirection.Row)
                                                                       .SetFlexWrap(Wrap.Wrap)
                                                                       .SetJustifyContent(Justify.Center))
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateButton()
                                                            .SetName("BasicButton")
                                                            .SetLocKey("Floodgate.Triggers.Basic")
                                                            .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                                            .SetFontSize(new Length(14, Pixel))
                                                            .SetFontStyle(FontStyle.Normal)
                                                            .SetHeight(new Length(29, Pixel))
                                                            .SetWidth(new Length(96, Pixel))
                                                            .SetBackgroundImage(_buttonGameActiveImage)
                                                            .Build())
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateButton()
                                                            .SetName("TimerButton")
                                                            .SetLocKey("Floodgate.Triggers.Timer")
                                                            .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                                            .SetFontSize(new Length(14, Pixel))
                                                            .SetFontStyle(FontStyle.Normal)
                                                            .SetHeight(new Length(29, Pixel))
                                                            .SetWidth(new Length(96, Pixel))
                                                            .SetBackgroundImage(_buttonGameImage)
                                                            .Build())
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateButton()
                                                            .SetName("AdvancedButton")
                                                            .SetLocKey("Floodgate.Triggers.Advanced")
                                                            .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                                            .SetFontSize(new Length(14, Pixel))
                                                            .SetFontStyle(FontStyle.Normal)
                                                            .SetHeight(new Length(29, Pixel))
                                                            .SetWidth(new Length(96, Pixel))
                                                            .SetBackgroundImage(_buttonGameImage)
                                                            .Build())
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateVisualElement()
                                                            .SetName("BasicTab")
                                                            .SetPadding(new Padding(new Length(8, Pixel)))
                                                            .Build())
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateVisualElement()
                                                            .SetName("TimerTab")
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
                                                            .BuildAndInitialize());

            _root = rootBuilder.BuildAndInitialize();
            this._root.ToggleDisplayStyle(false);

            _floodgatesLinks = _root.Q<VisualElement>("Placeholder");
            _floodgatesLinks.Add(_attachToStreamGaugeFragment.InitiliazeFragment(_root));

            _basicTab = _root.Q<VisualElement>("BasicTab");
            _basicTab.Add(_droughtSettingsFragment.InitializeFragment());

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
            _floodgate = entity.GetComponentFast<Floodgate>();
            _floodgateTriggerMonoBehaviour = entity.GetComponentFast<FloodgateTriggerMonoBehaviour>();
            if ((bool)_floodgateTriggerMonoBehaviour)
            {
                _droughtSettingsFragment.ShowFragment(_floodgate, _floodgateTriggerMonoBehaviour);
                _scheduleFragment.ShowFragment(_floodgate, _floodgateTriggerMonoBehaviour);
                _attachToStreamGaugeFragment.ShowFragment(_floodgateTriggerMonoBehaviour);
                SwitchTriggerTab(_lastActiveTab);
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
            if ((bool)_floodgateTriggerMonoBehaviour)
            {
                _droughtSettingsFragment.UpdateFragment();
                _scheduleFragment.UpdateFragment();
                _attachToStreamGaugeFragment.UpdateFragment();
                _root.ToggleDisplayStyle(visible: true);
            }
        }

        public void SwitchTriggerTab(ActiveTab tabToShow)
        {
            _basicButton.style.backgroundImage = tabToShow == ActiveTab.Basic
                ? _buttonGameActiveImage
                : _buttonGameImage;
            _timerButton.style.backgroundImage = tabToShow == ActiveTab.Timer
                ? _buttonGameActiveImage
                : _buttonGameImage;
            _advancedButton.style.backgroundImage = tabToShow == ActiveTab.Advanced
                ? _buttonGameActiveImage
                : _buttonGameImage;

            _basicTab.ToggleDisplayStyle(tabToShow == ActiveTab.Basic ? true : false);
            _timerTab.ToggleDisplayStyle(tabToShow == ActiveTab.Timer ? true : false);
            _advancedTab.ToggleDisplayStyle(tabToShow == ActiveTab.Advanced ? true : false);

            _newButton.ToggleDisplayStyle(tabToShow == ActiveTab.Advanced ? true : false);
        }
    }
}
