using System;
using System.Collections.Generic;
using System.Text;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    public class StreamGaugeFloodgateLink
    {
        public FloodgateTriggerMonoBehaviour Floodgate { get; }

        public StreamGaugeMonoBehaviour StreamGauge { get; }

        public StreamGaugeFloodgateLink(
            FloodgateTriggerMonoBehaviour floodgate,
            StreamGaugeMonoBehaviour streamGauge)
        {
            Floodgate = floodgate;
            StreamGauge = streamGauge;
        }
    }
}
