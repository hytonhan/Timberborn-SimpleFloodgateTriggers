namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    /// <summary>
    /// Represents a link between a Floodgate and a Streamgauge
    /// </summary>
    public class StreamGaugeFloodgateLink
    {
        public FloodgateTriggerMonoBehaviour Floodgate { get; }

        public StreamGaugeMonoBehaviour StreamGauge { get; }

        public float Threshold1 { get; set; }
        public float Threshold2 { get; set; }
        public float Height1 { get; set; }
        public float Height2 { get; set; }

        public StreamGaugeFloodgateLink(
            FloodgateTriggerMonoBehaviour floodgate,
            StreamGaugeMonoBehaviour streamGauge)
        {
            Floodgate = floodgate;
            StreamGauge = streamGauge;
            Threshold1 = 0f;
            Threshold2 = 0f;
            Height1 = 0f;
            Height2 = 0f;
        }
    }
}
