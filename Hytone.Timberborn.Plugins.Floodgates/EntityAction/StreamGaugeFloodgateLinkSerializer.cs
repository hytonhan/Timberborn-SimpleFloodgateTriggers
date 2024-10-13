using Timberborn.Persistence;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    /// <summary>
    /// Defines how a StreamGaugeFloodgateLink instance 
    /// should be serialized and deserialized
    /// </summary>
    public class StreamGaugeFloodgateLinkSerializer : IObjectSerializer<StreamGaugeFloodgateLink>
    {
        private static readonly PropertyKey<FloodgateTriggerMonoBehaviour> FloodgateKey = new PropertyKey<FloodgateTriggerMonoBehaviour>("Floodgate");
        private static readonly PropertyKey<StreamGaugeMonoBehaviour> StreamGaugeKey = new PropertyKey<StreamGaugeMonoBehaviour>("StreamGauge");
        private static readonly PropertyKey<float> Threshold1Key = new PropertyKey<float>("Threshold1");
        private static readonly PropertyKey<float> Threshold2Key = new PropertyKey<float>("Threshold2");
        private static readonly PropertyKey<float> Height1Key = new PropertyKey<float>("Height1");
        private static readonly PropertyKey<float> Height2Key = new PropertyKey<float>("Height2");
        private static readonly PropertyKey<float> ContaminationThresholdLowKey = new PropertyKey<float>("ContaminationThresholdLow");
        private static readonly PropertyKey<float> ContaminationThresholdHighKey = new PropertyKey<float>("ContaminationThresholdHigh");
        private static readonly PropertyKey<float> ContaminationHeight1Key = new PropertyKey<float>("ContaminationHeight1");
        private static readonly PropertyKey<float> ContaminationHeight2Key = new PropertyKey<float>("ContaminationHeight2");

        private static readonly PropertyKey<bool> DisableDuringDroughtKey = new PropertyKey<bool>("DisableDuringDrought");
        private static readonly PropertyKey<bool> DisableDuringTemperateKey = new PropertyKey<bool>("DisableDuringTemperate");
        private static readonly PropertyKey<bool> DisableDuringBadtideKey = new PropertyKey<bool>("DisableDuringBadtide");

        private static readonly PropertyKey<bool> EnableContaminationLowKey = new PropertyKey<bool>("EnableContaminationLow");
        private static readonly PropertyKey<bool> EnableContaminationHighKey = new PropertyKey<bool>("EnableContaminationHigh");

        public void Serialize(StreamGaugeFloodgateLink value, IObjectSaver objectSaver)
        {
            objectSaver.Set(FloodgateKey, value.Floodgate);
            objectSaver.Set(StreamGaugeKey, value.StreamGauge);
            objectSaver.Set(Threshold1Key, value.Threshold1);
            objectSaver.Set(Threshold2Key, value.Threshold2);
            objectSaver.Set(Height1Key, value.Height1);
            objectSaver.Set(Height2Key, value.Height2);
            objectSaver.Set(ContaminationThresholdLowKey, value.ContaminationThresholdLow);
            objectSaver.Set(ContaminationThresholdHighKey, value.ContaminationThresholdHigh);
            objectSaver.Set(ContaminationHeight1Key, value.ContaminationHeight1);
            objectSaver.Set(ContaminationHeight2Key, value.ContaminationHeight2);

            objectSaver.Set(DisableDuringDroughtKey, value.DisableDuringDrought);
            objectSaver.Set(DisableDuringTemperateKey, value.DisableDuringTemperate);
            objectSaver.Set(DisableDuringBadtideKey, value.DisableDuringBadtide);

            objectSaver.Set(EnableContaminationLowKey, value.EnableContaminationLow);
            objectSaver.Set(EnableContaminationHighKey, value.EnableContaminationHigh);
        }

        public Obsoletable<StreamGaugeFloodgateLink> Deserialize(IObjectLoader objectLoader)
        {

            var link = new StreamGaugeFloodgateLink(objectLoader.Get(FloodgateKey),
                                                    objectLoader.Get(StreamGaugeKey))
            {
                Threshold1 = objectLoader.Get(Threshold1Key),
                Threshold2 = objectLoader.Get(Threshold2Key),
                Height1 = objectLoader.Get(Height1Key),
                Height2 = objectLoader.Get(Height2Key),
                ContaminationThresholdLow = objectLoader.Get(ContaminationThresholdLowKey),
                ContaminationThresholdHigh = objectLoader.Get(ContaminationThresholdHighKey),
                ContaminationHeight1 = objectLoader.Get(ContaminationHeight1Key),
                ContaminationHeight2 = objectLoader.Get(ContaminationHeight2Key),
                DisableDuringDrought = objectLoader.Has(DisableDuringDroughtKey) && objectLoader.Get(DisableDuringDroughtKey),
                DisableDuringTemperate = objectLoader.Has(DisableDuringTemperateKey) && objectLoader.Get(DisableDuringTemperateKey),
                DisableDuringBadtide= objectLoader.Has(DisableDuringBadtideKey) && objectLoader.Get(DisableDuringBadtideKey),
                EnableContaminationLow = objectLoader.Has(EnableContaminationLowKey) && objectLoader.Get(EnableContaminationLowKey),
                EnableContaminationHigh = objectLoader.Has(EnableContaminationHighKey) && objectLoader.Get(EnableContaminationHighKey)
            };
            return link;
        }
    }
}
