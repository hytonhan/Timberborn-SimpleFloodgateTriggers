using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using System;
using Timberborn.Localization;
using Timberborn.SelectionSystem;
using Timberborn.ToolSystem;
using UnityEngine;
using UnityEngine.UIElements;
using TimberApi.EntityLinkerSystem;
using Timberborn.BaseComponentSystem;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterSourceRegulators;

namespace Hytone.Timberborn.Plugins.Floodgates.UI.WaterSourceRegulators
{
    /// <summary>
    /// Custom button behaviour which handles the linking
    /// of a Floodgate with a StreamGauge
    /// </summary>
    public class AttachWaterSourceRegulatorButton
    {
        private static readonly string PickStreamGaugeTipLocKey = "Floodgate.Triggers.PickStreamGaugeTip";
        private static readonly string PickStreamGaugeTitleLocKey = "Floodgate.Triggers.PickStreamGaugeTitle";
        private static readonly string AttachToStreamGaugeLocKey = "Floodgate.Triggers.AttachToStreamGauge";

        private readonly ILoc _loc;
        private readonly PickObjectTool _pickObjectTool;
        private readonly EntitySelectionService _EntitySelectionService;
        private readonly ToolManager _toolManager;
        private Button _button;

        public AttachWaterSourceRegulatorButton(
            ILoc loc, 
            PickObjectTool pickObjectTool,
            EntitySelectionService EntitySelectionService, 
            ToolManager toolManager)
        {
            _loc = loc;
            _pickObjectTool = pickObjectTool;
            _EntitySelectionService = EntitySelectionService;
            _toolManager = toolManager;
        }

        public void Initialize(VisualElement root, 
                               Func<WaterSourceRegulatorMonobehaviour> waterSourceRegulatorProvider, 
                               Action createdRouteCallback)
        {
            _button = root.Q<Button>("NewStreamGaugeButton");
            _button.clicked += delegate
            {
                StartAttachStreamGauge(waterSourceRegulatorProvider(), createdRouteCallback);
            };
        }

        /// <summary>
        /// If selection is cancelled, opt out of the 
        /// object picking tool
        /// </summary>
        public void StopStreamGaugeAttachment()
        {
            if (_toolManager.ActiveTool == _pickObjectTool)
            {
                _toolManager.SwitchToDefaultTool();
            }
        }

        /// <summary>
        /// Fire up the object picking tool when the button is clicked
        /// </summary>
        /// <param name="floodgate"></param>
        /// <param name="createdLinkCallback"></param>
        private void StartAttachStreamGauge(WaterSourceRegulatorMonobehaviour waterSourceRegulator, 
                                            Action createdLinkCallback)
        {
            _pickObjectTool.StartPicking<StreamGaugeMonoBehaviour>(
                _loc.T(PickStreamGaugeTitleLocKey), 
                _loc.T(PickStreamGaugeTipLocKey), 
                (GameObject gameObject) => ValidateStreamGauge(waterSourceRegulator, gameObject), 
                delegate (GameObject streamGauge)
            {
                FinishStreamGaugeSelection(waterSourceRegulator, streamGauge, createdLinkCallback);
            });
            _EntitySelectionService.Select(waterSourceRegulator);
        }

        /// <summary>
        /// This is basically useless as of now
        /// </summary>
        /// <param name="floodgate"></param>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        private string ValidateStreamGauge(WaterSourceRegulatorMonobehaviour waterSourceRegulator, 
                                           GameObject gameObject)
        {
            StreamGaugeMonoBehaviour streamGaugeComponent = gameObject.GetComponent<StreamGaugeMonoBehaviour>();
            return "";
        }

        /// <summary>
        /// Link the water pump and streamgauge when a streamgauge
        /// is selected
        /// </summary>
        /// <param name="floodgate"></param>
        /// <param name="streamGauge"></param>
        /// <param name="attachedStreamGaugeCallback"></param>
        private void FinishStreamGaugeSelection(
            WaterSourceRegulatorMonobehaviour waterSourceRegulator, 
            GameObject streamGauge, 
            Action attachedStreamGaugeCallback)
        {
            StreamGaugeMonoBehaviour streamGaugeComponent = streamGauge.GetComponent<StreamGaugeMonoBehaviour>();
            waterSourceRegulator.AttachLink(streamGaugeComponent);
            attachedStreamGaugeCallback();
        }

        /// <summary>
        /// Update the text on the button
        /// </summary>
        /// <param name="currentLinks"></param>
        /// <param name="maxLinks"></param>
        public void UpdateRemainingSlots(int currentLinks, int maxLinks)
        {
            _button.text = $"{_loc.T(AttachToStreamGaugeLocKey)} ({currentLinks}/{maxLinks})";
            _button.SetEnabled(currentLinks < maxLinks);
        }
    }
}
