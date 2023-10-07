namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps
{
    public class WaterPumpStreamGaugeLink
    {
        public WaterPumpMonobehaviour WaterPump { get; }

        public StreamGaugeMonoBehaviour StreamGauge { get; }

        public float Threshold1 { get; set; }
        public float Threshold2 { get; set; }
        public float Threshold3 { get; set; }
        public float Threshold4 { get; set; }

        public bool Enabled1 { get; set; }
        public bool Enabled2 { get; set; }
        public bool Enabled3 { get; set; }
        public bool Enabled4 { get; set; }

        public float ContaminationPauseBelowThreshold { get; set; }
        public float ContaminationPauseAboveThreshold { get; set; }
        public float ContaminationUnpauseBelowThreshold { get; set; }
        public float ContaminationUnpauseAboveThreshold { get; set; }

        public bool ContaminationPauseBelowEnabled { get; set; }
        public bool ContaminationPauseAboveEnabled { get; set; }
        public bool ContaminationUnpauseBelowEnabled { get; set; }
        public bool ContaminationUnpauseAboveEnabled { get; set; }

        public bool DisableDuringDrought;
        public bool DisableDuringTemperate;
        public bool DisableDuringBadtide;

        public WaterPumpStreamGaugeLink(
            WaterPumpMonobehaviour waterPump,
            StreamGaugeMonoBehaviour streamGauge)
        {
            WaterPump = waterPump;
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
            ContaminationPauseBelowEnabled = false;
            ContaminationPauseAboveEnabled = false;
            ContaminationUnpauseBelowEnabled = false;
            ContaminationUnpauseAboveEnabled = false;
            ContaminationPauseBelowThreshold = 0f;
            ContaminationPauseAboveThreshold = 0f;
            ContaminationUnpauseBelowThreshold = 0f;
            ContaminationUnpauseAboveThreshold = 0f;
    }
    }
}
