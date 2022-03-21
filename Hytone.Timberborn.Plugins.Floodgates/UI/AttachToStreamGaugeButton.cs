using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.DistributionSystemUI;
using Timberborn.Localization;
using Timberborn.PickObjectToolSystem;
using Timberborn.SelectionSystem;
using Timberborn.ToolSystem;
using Timberborn.WaterBuildings;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    public class AttachToStreamGaugeButton
    {
        private static readonly string PickStreamGaugeTipLocKey = "Floodgate.Triggers.PickStreamGaugeTip";
        private static readonly string PickStreamGaugeTitleLocKey = "Floodgate.Triggers.PickStreamGaugeTitle";
        private static readonly string PickStreamGaugeWarningLocKey = "Floodgate.Triggers.PickStreamGaugeWarning";
        private static readonly string AttachToStreamGaugeLocKey = "Floodgate.Triggers.AttachToStreamGauge";

        //private readonly GoodSelectionBox _goodSelectionBox;
        private readonly ILoc _loc;
        private readonly PickObjectTool _pickObjectTool;
        private readonly SelectionManager _selectionManager;
        private readonly ToolManager _toolManager;
        private Button _button;

        public AttachToStreamGaugeButton(ILoc loc, 
                                         PickObjectTool pickObjectTool, 
                                         SelectionManager selectionManager, 
                                         ToolManager toolManager)
        {
            _loc = loc;
            _pickObjectTool = pickObjectTool;
            _selectionManager = selectionManager;
            _toolManager = toolManager;
        }
        public void Initialize(VisualElement root, 
                               Func<FloodgateTriggerMonoBehaviour> floodgateProvider, 
                               Action createdRouteCallback)
        {
            _button = root.Q<Button>(nameof(AttachToStreamGaugeButton));
            _button.clicked += delegate
            {
                StartAttachStreamGauge(floodgateProvider(), createdRouteCallback);
            };
        }

        public void StopStreamGaugeAttachment()
        {
            if (_toolManager.ActiveTool == _pickObjectTool)
            {
                _toolManager.SwitchToDefaultTool();
            }
        }

        public void UpdateRemainingGaugeSlots(int currentGauges, int maxGauges)
        {
            _button.text = $"{_loc.T(AttachToStreamGaugeLocKey)} ({currentGauges}/{maxGauges})";
            _button.SetEnabled(currentGauges < maxGauges);
        }

        private void StartAttachStreamGauge(FloodgateTriggerMonoBehaviour floodgate, Action createdRouteCallback)
        {
            _pickObjectTool.StartPicking<StreamGaugeMonoBehaviour>(
                _loc.T(PickStreamGaugeTitleLocKey), 
                _loc.T(PickStreamGaugeTipLocKey), 
                (GameObject gameObject) => ValidateStreamGauge(floodgate, gameObject), 
                delegate (GameObject streamGauge)
            {
                FinishDropOffPointSelection(floodgate, streamGauge, createdRouteCallback);
            });
            _selectionManager.Select(floodgate.gameObject);
        }

        private string ValidateStreamGauge(FloodgateTriggerMonoBehaviour floodgate, GameObject gameObject)
        {
            FloodgateTriggerMonoBehaviour component = gameObject.GetComponent<FloodgateTriggerMonoBehaviour>();
            //if (!floodgate.CanCompleteRoute(component))
            //{
            //    return _loc.T(PickDropOffPointWarningLocKey);
            //}
            return "";
        }
        private void FinishDropOffPointSelection(FloodgateTriggerMonoBehaviour floodgate, GameObject streamGauge, Action attachedStreamGaugeCallback)
        {
            StreamGauge streamGaugeComponent = streamGauge.GetComponent<StreamGauge>();
            attachedStreamGaugeCallback();
            //_goodSelectionBox.Show(delegate (GoodSpecification good)
            //{
            //    FinishGoodSelection(distributionPost, dropOffPointComponent, good, createdRouteCallback);
            //});
        }
    }
}
