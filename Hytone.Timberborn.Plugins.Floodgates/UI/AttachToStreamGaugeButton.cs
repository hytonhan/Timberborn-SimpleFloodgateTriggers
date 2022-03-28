using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using System;
using Timberborn.Localization;
using Timberborn.PickObjectToolSystem;
using Timberborn.SelectionSystem;
using Timberborn.ToolSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    /// <summary>
    /// Custom button behaviour which handles the linking
    /// of a Floodgate with a StreamGauge
    /// </summary>
    public class AttachToStreamGaugeButton
    {
        private static readonly string PickStreamGaugeTipLocKey = "Floodgate.Triggers.PickStreamGaugeTip";
        private static readonly string PickStreamGaugeTitleLocKey = "Floodgate.Triggers.PickStreamGaugeTitle";
        private static readonly string AttachToStreamGaugeLocKey = "Floodgate.Triggers.AttachToStreamGauge";

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
            _button = root.Q<Button>("NewStreamGaugeButton");
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

        private void StartAttachStreamGauge(FloodgateTriggerMonoBehaviour floodgate, Action createdLinkCallback)
        {
            _pickObjectTool.StartPicking<StreamGaugeMonoBehaviour>(
                _loc.T(PickStreamGaugeTitleLocKey), 
                _loc.T(PickStreamGaugeTipLocKey), 
                (GameObject gameObject) => ValidateStreamGauge(floodgate, gameObject), 
                delegate (GameObject streamGauge)
            {
                FinishStreamGaugeSelection(floodgate, streamGauge, createdLinkCallback);
            });
            _selectionManager.Select(floodgate.gameObject);
        }

        private string ValidateStreamGauge(FloodgateTriggerMonoBehaviour floodgate, GameObject gameObject)
        {
            StreamGaugeMonoBehaviour streamGaugeComponent = gameObject.GetComponent<StreamGaugeMonoBehaviour>();
            return "";
        }
        private void FinishStreamGaugeSelection(
            FloodgateTriggerMonoBehaviour floodgate, 
            GameObject streamGauge, 
            Action attachedStreamGaugeCallback)
        {
            StreamGaugeMonoBehaviour streamGaugeComponent = streamGauge.GetComponent<StreamGaugeMonoBehaviour>();
            floodgate.AttachLink(floodgate, streamGaugeComponent);
            attachedStreamGaugeCallback();
        }

        public void UpdateRemainingSlots(int currentLinks, int maxLinks)
        {
            _button.text = $"{_loc.T(AttachToStreamGaugeLocKey)} ({currentLinks}/{maxLinks})";
            _button.SetEnabled(currentLinks < maxLinks);
        }
    }
}
