using System;
using System.Collections.Generic;
using System.Text;

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
        }
    }
}
