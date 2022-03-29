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

        public void Serialize(StreamGaugeFloodgateLink value, IObjectSaver objectSaver)
        {
            objectSaver.Set(FloodgateKey, value.Floodgate);
            objectSaver.Set(StreamGaugeKey, value.StreamGauge);
            objectSaver.Set(Threshold1Key, value.Threshold1);
            objectSaver.Set(Threshold2Key, value.Threshold2);
            objectSaver.Set(Height1Key, value.Height1);
            objectSaver.Set(Height2Key, value.Height2);
        }

        public Obsoletable<StreamGaugeFloodgateLink> Deserialize(IObjectLoader objectLoader)
        {

            var link = new StreamGaugeFloodgateLink(objectLoader.Get(FloodgateKey),
                                                    objectLoader.Get(StreamGaugeKey))
            {
                Threshold1 = objectLoader.Get(Threshold1Key),
                Threshold2 = objectLoader.Get(Threshold2Key),
                Height1 = objectLoader.Get(Height1Key),
                Height2 = objectLoader.Get(Height2Key)
            };
            return link;
        }
    }
}
