using Timberborn.EntityPanelSystem;
using Timberborn.CoreUI;
using UnityEngine;
using UnityEngine.UIElements;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using TimberbornAPI.UIBuilderSystem;
using static UnityEngine.UIElements.Length.Unit;
using Timberborn.WaterBuildings;
using TimberbornAPI.Common;

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
        private Button _newButton;

        private VisualElement _basicTab;
        private VisualElement _advancedTab;
        private VisualElement _floodgatesLinks;

        private AttachToStreamGaugeFragment _attachToStreamGaugeFragment;
        private FloodGateUIFragment _droughtSettingsFragment;
        private FloodgateScheduleFragment _scheduleFragment;

        private bool _lastActiveTabWasBasic = true;

        public TriggersFragment(UIBuilder builder,
                                AttachToStreamGaugeFragment attachToStreamGaugeFragment,
                                FloodGateUIFragment floodgateUIFragment,
                                FloodgateScheduleFragment scheduleFragment)
        {
            _builder = builder;
            _attachToStreamGaugeFragment = attachToStreamGaugeFragment;
            _droughtSettingsFragment = floodgateUIFragment;
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
                                                            .SetWidth(new Length(145, Pixel))
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
                                                            .SetWidth(new Length(145, Pixel))
                                                            .Build())
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateVisualElement()
                                                            .SetName("BasicTab")
                                                            .SetHeight(new Length(420, Pixel))
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

            _basicTab = _root.Q<VisualElement>("BasicTab");
            _basicTab.Add(_droughtSettingsFragment.InitializeFragment());
            _basicTab.Add(_scheduleFragment.InitializeFragment());

            _advancedTab = _root.Q<VisualElement>("AdvancedTab");
            _floodgatesLinks = _root.Q<VisualElement>("Placeholder");
            _floodgatesLinks.Add(_attachToStreamGaugeFragment.InitiliazeFragment(_root));

            _basicButton = _root.Q<Button>("BasicButton");
            _basicButton.clicked += () => 
            {
                _lastActiveTabWasBasic = true;
                SwitchTriggerTab(_lastActiveTabWasBasic);
            };
            _advancedButton = _root.Q<Button>("AdvancedButton");
            _advancedButton.clicked += () =>
            {
                _lastActiveTabWasBasic = false;
                SwitchTriggerTab(_lastActiveTabWasBasic);
            };

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
                SwitchTriggerTab(_lastActiveTabWasBasic);
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
