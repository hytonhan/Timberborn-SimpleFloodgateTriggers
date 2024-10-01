using Timberborn.EntityPanelSystem;
using Timberborn.CoreUI;
using UnityEngine;
using UnityEngine.UIElements;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using Timberborn.WaterBuildings;
using TimberApi.UIBuilderSystem;
using Timberborn.BaseComponentSystem;
using System;
using TimberApi.UIPresets.Builders;
using TimberApi.UIPresets.Buttons;
using Timberborn.ConstructionSites;
using TimberApi.UIBuilderSystem.ElementBuilders;
using UnityEngine.Categorization;
using TimberApi.UIBuilderSystem.StylingElements;
using Timberborn.WaterSourceSystem;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterSourceRegulators;

namespace Hytone.Timberborn.Plugins.Floodgates.UI.WaterSourceRegulators
{
    public class WaterSourceRegulatorFragment : IEntityPanelFragment
    {

        private readonly UIBuilder _builder;
        private static readonly string SelectedTabButtonCLass = "distribution-post-fragment__tab-button--selected";

        private VisualElement _root;

        private WaterSourceRegulator _watersourceRegulator;
        private WaterSourceRegulatorMonobehaviour _watersourceRegulatorMonoBehaviour;

        private Button _basicButton;
        private Button _advancedButton;
        private Button _timerButton;
        private Button _newButton;

        private VisualElement _basicTab;
        private VisualElement _advancedTab;
        private VisualElement _timerTab;
        private VisualElement _floodgatesLinks;

        private AttachWaterSourceRegulatorFragment _attachToStreamGaugeFragment;
        private WaterSourceRegulatorDroughtFragment _droughtSettingsFragment;
        private WaterSourceRegulatorScheduleFragment _scheduleFragment;

        private readonly Texture2D _buttonGameImage;
        private readonly Texture2D _buttonGameActiveImage;

        //private bool _lastActiveTabWasBasic = true;
        private ActiveTab _lastActiveTab = ActiveTab.Basic;

        public WaterSourceRegulatorFragment(
            UIBuilder builder,
            AttachWaterSourceRegulatorFragment attachToStreamGaugeFragment,
            WaterSourceRegulatorDroughtFragment waterSourceRegulatorDroughtFragment,
            WaterSourceRegulatorScheduleFragment scheduleFragment)
        {
            _builder = builder;
            _attachToStreamGaugeFragment = attachToStreamGaugeFragment;
            _droughtSettingsFragment = waterSourceRegulatorDroughtFragment;
            _scheduleFragment = scheduleFragment;

            _buttonGameImage = Resources.Load<Texture2D>("ui/images/buttons/button-game");
            _buttonGameActiveImage = Resources.Load<Texture2D>("ui/images/buttons/button-game-active");
        }

        public VisualElement InitializeFragment()               
        {
            var rootBuilder = _builder.Create<VisualElementBuilder>()
                .AddComponent<FragmentBuilder>("TriggersFragment", builder => 
                    builder.AddComponent<GameButton>(button => button.SetName("BasicButton")
                                                                     .SetLocKey("Floodgate.Triggers.Basic")
                                                                     .SetWidth(96)
                                                                     .ModifyRoot(root => 
                                                                        root.SetBackgroundImage(_buttonGameActiveImage)))
                           .AddComponent<GameButton>(button => button.SetName("TimerButton")
                                                                     .SetLocKey("Floodgate.Triggers.Timer")
                                                                     .SetWidth(96))
                           .AddComponent<GameButton>(button => button.SetName("AdvancedButton")
                                                                     .SetLocKey("Floodgate.Triggers.Advanced")
                                                                     .SetWidth(96))
                           .AddComponent<VisualElementBuilder>(element => element.SetName("BasicTab").SetWidth(288))
                           .AddComponent<VisualElementBuilder>(element => element.SetName("TimerTab").SetWidth(288))
                           .AddComponent<VisualElementBuilder>(element => 
                                element.SetName("AdvancedTab")
                                       .SetWidth(288)
                                       .AddComponent<VisualElementBuilder>(element => 
                                            element.SetName("Placeholder")
                                                   .AddComponent<GameButton>("NewStreamGaugeButton", button => 
                                                        button.AddClass("entity-fragment__button")
                                                              .AddClass("entity-fragment__button--green")
                                                              .SetLocKey("Floodgates.Triggers.Empty")
                                                              .ModifyRoot(root => 
                                                                    root.SetMargin(new Margin(0, new Length(2)))
                                                            )
                                                    )
                                        )
                            )
                    );
            

            _root = rootBuilder.BuildAndInitialize();

            var fragment = _root.Q<VisualElement>("TriggersFragment");
            fragment.style.flexDirection = FlexDirection.Row;
            fragment.style.flexWrap = Wrap.Wrap;
            fragment.style.justifyContent = Justify.Center;


            this._root.ToggleDisplayStyle(false);

            _floodgatesLinks = _root.Q<VisualElement>("Placeholder");
            _floodgatesLinks.Add(_attachToStreamGaugeFragment.InitiliazeFragment(_root));

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
            _watersourceRegulator = entity.GetComponentFast<WaterSourceRegulator>();
            _watersourceRegulatorMonoBehaviour = entity.GetComponentFast<WaterSourceRegulatorMonobehaviour>();
            if ((bool)_watersourceRegulatorMonoBehaviour)
            {
                _droughtSettingsFragment.ShowFragment(_watersourceRegulatorMonoBehaviour);
                _scheduleFragment.ShowFragment(_watersourceRegulatorMonoBehaviour);
                _attachToStreamGaugeFragment.ShowFragment(_watersourceRegulatorMonoBehaviour);
                SwitchTriggerTab(_lastActiveTab);
            }
        }

        public void ClearFragment()
        {
            _watersourceRegulatorMonoBehaviour = null;
            _droughtSettingsFragment.ClearFragment();
            _scheduleFragment.ClearFragment();
            _attachToStreamGaugeFragment.ClearFragment();
            _root.ToggleDisplayStyle(visible: false);
        }

        public void UpdateFragment()
        {
            if ((bool)_watersourceRegulatorMonoBehaviour)
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
