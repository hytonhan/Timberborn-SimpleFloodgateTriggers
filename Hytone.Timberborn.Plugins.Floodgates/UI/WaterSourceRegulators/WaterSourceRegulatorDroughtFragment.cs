using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterSourceRegulators;
using TimberApi.UIBuilderSystem;
using TimberApi.UIBuilderSystem.ElementBuilders;
using TimberApi.UIBuilderSystem.StylingElements;
using TimberApi.UIPresets.Toggles;
using Timberborn.Localization;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hytone.Timberborn.Plugins.Floodgates.UI.WaterSourceRegulators
{
    public class WaterSourceRegulatorDroughtFragment
    {
        private readonly UIBuilder _builder;
        private VisualElement _root;


        private Toggle _droughtStartedCloseToggle;
        private Toggle _droughtStartedResumeToggle;

        private Toggle _temperateStartedCloseToggle;
        private Toggle _temperateStartedResumeToggle;

        private Toggle _badtideStartedCloseToggle;
        private Toggle _badtideStartedResumeToggle;

        private WaterSourceRegulatorMonobehaviour _waterSourceRegulatorMonobehaviour;


        public WaterSourceRegulatorDroughtFragment(UIBuilder builder)
        {
            _builder = builder;
        }


        public VisualElement InitiliazeFragment(VisualElement parent)
        {
            var CloseOnDrought = _builder.Create<VisualElementBuilder>()
                .AddComponent<GameToggle>("CloseOnDroughtStartToggle", toggle => 
                    toggle.SetLocKey("Floodgate.WaterSourceRegulator.CloseOnDroughtStart")
                          .ModifyRoot(root => root.SetMargin(new Margin(new Length(3), 0, 0, 0)))
                );
  
            var OpenOnDrought = _builder.Create<VisualElementBuilder>()
                .AddComponent<GameToggle>("OpenOnDroughtStartToggle", toggle => 
                    toggle.SetLocKey("Floodgate.WaterSourceRegulator.OpenOnDroughtStart")
                          .ModifyRoot(root => root.SetMargin(new Margin(new Length(3), 0, new Length(11), 0)))
                );   
        
            var OpenOnTemperate = _builder.Create<VisualElementBuilder>()
                .AddComponent<GameToggle>("OpenOnTemperateStartToggle", toggle => 
                    toggle.SetLocKey("Floodgate.WaterSourceRegulator.OpenOnTemperateStart")
                          .ModifyRoot(root => root.SetMargin(new Margin(new Length(3), 0, new Length(11), 0)))
                );                                                                          

            var CloseOnTemperate = _builder.Create<VisualElementBuilder>()
                .AddComponent<GameToggle>("CloseOnTemperateStartToggle", toggle => 
                    toggle.SetLocKey("Floodgate.WaterSourceRegulator.CloseOnTemperateStart")
                          .ModifyRoot(root => root.SetMargin(new Margin(new Length(3), 0, 0, 0)))
                );                                                                            
        
            var OpenOnBadtide = _builder.Create<VisualElementBuilder>()
                .AddComponent<GameToggle>("OpenOnBadtideStartToggle", toggle => 
                    toggle.SetLocKey("Floodgate.WaterSourceRegulator.OpenOnBadtideStart")
                          .ModifyRoot(root => root.SetMargin(new Margin(new Length(3), 0, new Length(11), 0)))
                );                                                                      

            var CloseOnBadtide = _builder.Create<VisualElementBuilder>()
                .AddComponent<GameToggle>("CloseOnBadtideStartToggle", toggle => 
                    toggle.SetLocKey("Floodgate.WaterSourceRegulator.CloseOnBadtideStart")
                          .ModifyRoot(root => root.SetMargin(new Margin(new Length(3), 0, 0, 0)))
                );

            var root = _builder.Create<VisualElementBuilder>();
            root.AddComponent(CloseOnDrought.Build());
            root.AddComponent(OpenOnDrought.Build());
            root.AddComponent(CloseOnTemperate.Build());
            root.AddComponent(OpenOnTemperate.Build());
            root.AddComponent(CloseOnBadtide.Build());
            root.AddComponent(OpenOnBadtide.Build());

            _root = root.BuildAndInitialize();

            _droughtStartedCloseToggle = _root.Q<Toggle>("CloseOnDroughtStartToggle");
            _droughtStartedCloseToggle.RegisterValueChangedCallback(ToggleCloseDroughtStarted);

            _droughtStartedResumeToggle = _root.Q<Toggle>("OpenOnDroughtStartToggle");
            _droughtStartedResumeToggle.RegisterValueChangedCallback(ToggleOpenDroughtStarted);

            _temperateStartedCloseToggle = _root.Q<Toggle>("CloseOnTemperateStartToggle");
            _temperateStartedCloseToggle.RegisterValueChangedCallback(ToggleCloseTemperateStarted);

            _temperateStartedResumeToggle = _root.Q<Toggle>("OpenOnTemperateStartToggle");
            _temperateStartedResumeToggle.RegisterValueChangedCallback(ToggleOpenTemperateStarted);

            _badtideStartedCloseToggle = _root.Q<Toggle>("CloseOnBadtideStartToggle");
            _badtideStartedCloseToggle.RegisterValueChangedCallback(ToggleCloseBadtideStarted);

            _badtideStartedResumeToggle = _root.Q<Toggle>("OpenOnBadtideStartToggle");
            _badtideStartedResumeToggle.RegisterValueChangedCallback(ToggleOpenBadtideStarted);

            return _root;
        }

        public void ShowFragment(WaterSourceRegulatorMonobehaviour waterSourceRegulatorMonobehaviour)
        {
            if ((bool)waterSourceRegulatorMonobehaviour)
            {
            }
            _waterSourceRegulatorMonobehaviour = waterSourceRegulatorMonobehaviour;
        }

        public void UpdateFragment()
        {
            if ((bool)_waterSourceRegulatorMonobehaviour)
            {
                _droughtStartedCloseToggle.SetValueWithoutNotify(_waterSourceRegulatorMonobehaviour.CloseOnDroughtStart);
                _droughtStartedResumeToggle.SetValueWithoutNotify(_waterSourceRegulatorMonobehaviour.OpenOnDroughtStart);
                _temperateStartedCloseToggle.SetValueWithoutNotify(_waterSourceRegulatorMonobehaviour.CloseOnTemperateStarted);
                _temperateStartedResumeToggle.SetValueWithoutNotify(_waterSourceRegulatorMonobehaviour.OpenOnTemperateStarted);
                _badtideStartedCloseToggle.SetValueWithoutNotify(_waterSourceRegulatorMonobehaviour.CloseOnBadtideStarted);
                _badtideStartedResumeToggle.SetValueWithoutNotify(_waterSourceRegulatorMonobehaviour.OpenOnBadtideStarted);
            }
        }

        public void ClearFragment()
        {
            _waterSourceRegulatorMonobehaviour = null;
        }

        private void ToggleOpenTemperateStarted(ChangeEvent<bool> changeEvent)
        {
            _waterSourceRegulatorMonobehaviour.OpenOnTemperateStarted = changeEvent.newValue;
            if (changeEvent.newValue == true && _temperateStartedCloseToggle.value == true)
            {
                _temperateStartedCloseToggle.value = false;
                _waterSourceRegulatorMonobehaviour.CloseOnTemperateStarted = false;
            }

        }
        private void ToggleCloseTemperateStarted(ChangeEvent<bool> changeEvent)
        {
            _waterSourceRegulatorMonobehaviour.CloseOnTemperateStarted = changeEvent.newValue;
            if (changeEvent.newValue == true && _temperateStartedResumeToggle.value == true)
            {
                _temperateStartedResumeToggle.value = false;
                _waterSourceRegulatorMonobehaviour.OpenOnTemperateStarted = false;
            }
        }

        private void ToggleCloseDroughtStarted(ChangeEvent<bool> changeEvent)
        {
            _waterSourceRegulatorMonobehaviour.CloseOnDroughtStart = changeEvent.newValue;
            if (changeEvent.newValue == true && _droughtStartedResumeToggle.value == true)
            {
                _droughtStartedResumeToggle.value = false;
                _waterSourceRegulatorMonobehaviour.OpenOnDroughtStart = false;
            }
        }
        private void ToggleOpenDroughtStarted(ChangeEvent<bool> changeEvent)
        {
            _waterSourceRegulatorMonobehaviour.OpenOnDroughtStart = changeEvent.newValue;
            if (changeEvent.newValue == true && _droughtStartedCloseToggle.value == true)
            {
                _droughtStartedCloseToggle.value = false;
                _waterSourceRegulatorMonobehaviour.CloseOnDroughtStart = false;
            }
        }

        private void ToggleCloseBadtideStarted(ChangeEvent<bool> changeEvent)
        {
            _waterSourceRegulatorMonobehaviour.CloseOnBadtideStarted = changeEvent.newValue;
            if (changeEvent.newValue == true && _badtideStartedCloseToggle.value == true)
            {
                _badtideStartedResumeToggle.value = false;
                _waterSourceRegulatorMonobehaviour.OpenOnBadtideStarted = false;
            }
        }
        private void ToggleOpenBadtideStarted(ChangeEvent<bool> changeEvent)
        {
            _waterSourceRegulatorMonobehaviour.OpenOnBadtideStarted = changeEvent.newValue;
            if (changeEvent.newValue == true && _badtideStartedCloseToggle.value == true)
            {
                _badtideStartedCloseToggle.value = false;
                _waterSourceRegulatorMonobehaviour.CloseOnBadtideStarted = false;
            }
        }
    }
}
