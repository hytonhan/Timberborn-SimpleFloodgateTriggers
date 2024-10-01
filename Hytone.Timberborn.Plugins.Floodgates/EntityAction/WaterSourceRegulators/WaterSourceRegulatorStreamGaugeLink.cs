using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterSourceRegulators;
using Timberborn.WaterSourceSystem;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps
{
    public class WaterSourceRegulatorStreamGaugeLink
    {
        public WaterSourceRegulatorMonobehaviour WaterSourceRegulator { get; }

        public StreamGaugeMonoBehaviour StreamGauge { get; }

        public float Threshold1 { get; set; }
        public float Threshold2 { get; set; }
        public float Threshold3 { get; set; }
        public float Threshold4 { get; set; }

        public bool Enabled1 { get; set; }
        public bool Enabled2 { get; set; }
        public bool Enabled3 { get; set; }
        public bool Enabled4 { get; set; }

        public float ContaminationCloseBelowThreshold { get; set; }
        public float ContaminationCloseAboveThreshold { get; set; }
        public float ContaminationOpenBelowThreshold { get; set; }
        public float ContaminationOpenAboveThreshold { get; set; }

        public bool ContaminationCloseBelowEnabled { get; set; }
        public bool ContaminationCloseAboveEnabled { get; set; }
        public bool ContaminationOpenBelowEnabled { get; set; }
        public bool ContaminationOpenAboveEnabled { get; set; }

        public bool DisableDuringDrought;
        public bool DisableDuringTemperate;
        public bool DisableDuringBadtide;

        public WaterSourceRegulatorStreamGaugeLink(
            WaterSourceRegulatorMonobehaviour waterSourceRegulator,
            StreamGaugeMonoBehaviour streamGauge)
        {
            WaterSourceRegulator = waterSourceRegulator;
            StreamGauge = streamGauge;
            Threshold1 = 0f;
            Threshold2 = 0f;
            Threshold3 = 0f;
            Threshold4 = 0f;
            Enabled1 = false;
            Enabled2 = false;
            Enabled3 = false;
            Enabled4 = false;
            DisableDuringDrought = false;
            DisableDuringTemperate = false;
            DisableDuringBadtide = false;
            ContaminationCloseBelowEnabled = false;
            ContaminationCloseAboveEnabled = false;
            ContaminationOpenBelowEnabled = false;
            ContaminationOpenAboveEnabled = false;
            ContaminationCloseBelowThreshold = 0f;
            ContaminationCloseAboveThreshold = 0f;
            ContaminationOpenBelowThreshold = 0f;
            ContaminationOpenAboveThreshold = 0f;
    }
    }
}
