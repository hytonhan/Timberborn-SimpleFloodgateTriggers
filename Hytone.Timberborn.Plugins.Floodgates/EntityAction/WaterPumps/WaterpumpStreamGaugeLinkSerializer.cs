using Timberborn.Persistence;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps
{
    public class WaterpumpStreamGaugeLinkSerializer : IObjectSerializer<WaterPumpStreamGaugeLink>
    {
        private static readonly PropertyKey<WaterPumpMonobehaviour> WaterPumpKey = new PropertyKey<WaterPumpMonobehaviour>("WaterPump");
        private static readonly PropertyKey<StreamGaugeMonoBehaviour> StreamGaugeKey = new PropertyKey<StreamGaugeMonoBehaviour>("StreamGauge");
        private static readonly PropertyKey<float> Threshold1Key = new PropertyKey<float>("Threshold1");
        private static readonly PropertyKey<float> Threshold2Key = new PropertyKey<float>("Threshold2");
        private static readonly PropertyKey<float> Threshold3Key = new PropertyKey<float>("Threshold3");
        private static readonly PropertyKey<float> Threshold4Key = new PropertyKey<float>("Threshold4");
        private static readonly PropertyKey<bool> Enabled1Key = new PropertyKey<bool>("Enabled1");
        private static readonly PropertyKey<bool> Enabled2Key = new PropertyKey<bool>("Enabled2");
        private static readonly PropertyKey<bool> Enabled3Key = new PropertyKey<bool>("Enabled3");
        private static readonly PropertyKey<bool> Enabled4Key = new PropertyKey<bool>("Enabled4");
        private static readonly PropertyKey<bool> DisableDuringDroughtKey = new PropertyKey<bool>("DisableDuringDrought");
        private static readonly PropertyKey<bool> DisableDuringTemperateKey = new PropertyKey<bool>("DisableDuringTemperate");
        private static readonly PropertyKey<bool> DisableDuringBadtideKey = new PropertyKey<bool>("DisableDuringBadtide");

        private static readonly PropertyKey<bool> ContaminationPauseBelowEnabledKey = new PropertyKey<bool>("ContaminationPauseBelowEnabled");
        private static readonly PropertyKey<bool> ContaminationPauseAboveEnabledKey = new PropertyKey<bool>("ContaminationPauseAboveEnabled");
        private static readonly PropertyKey<bool> ContaminationUnpauseBelowEnabledKey = new PropertyKey<bool>("ContaminationUnpauseBelowEnabled");
        private static readonly PropertyKey<bool> ContaminationUnpauseAboveEnabledKey = new PropertyKey<bool>("ContaminationUnpauseAboveEnabled");
        private static readonly PropertyKey<float> ContaminationPauseBelowThresholdKey = new PropertyKey<float>("ContaminationPauseBelowThreshold");
        private static readonly PropertyKey<float> ContaminationPauseAboveThresholdKey = new PropertyKey<float>("ContaminationPauseAboveThreshold");
        private static readonly PropertyKey<float> ContaminationUnpauseBelowThresholdKey = new PropertyKey<float>("ContaminationUnpauseBelowThreshold");
        private static readonly PropertyKey<float> ContaminationUnpauseAboveThresholdKey = new PropertyKey<float>("ContaminationUnpauseAboveThreshold");

        public void Serialize(WaterPumpStreamGaugeLink value, IObjectSaver objectSaver)
        {
            objectSaver.Set(WaterPumpKey, value.WaterPump);
            objectSaver.Set(StreamGaugeKey, value.StreamGauge);
            objectSaver.Set(Threshold1Key, value.Threshold1);
            objectSaver.Set(Threshold2Key, value.Threshold2);
            objectSaver.Set(Threshold3Key, value.Threshold3);
            objectSaver.Set(Threshold4Key, value.Threshold4);
            objectSaver.Set(Enabled1Key, value.Enabled1);
            objectSaver.Set(Enabled2Key, value.Enabled2);
            objectSaver.Set(Enabled3Key, value.Enabled3);
            objectSaver.Set(Enabled4Key, value.Enabled4);
            objectSaver.Set(DisableDuringDroughtKey, value.DisableDuringDrought);
            objectSaver.Set(DisableDuringTemperateKey, value.DisableDuringTemperate);
            objectSaver.Set(DisableDuringBadtideKey, value.DisableDuringBadtide);

            objectSaver.Set(ContaminationPauseBelowThresholdKey, value.ContaminationPauseBelowThreshold);
            objectSaver.Set(ContaminationPauseAboveThresholdKey, value.ContaminationPauseAboveThreshold);
            objectSaver.Set(ContaminationUnpauseBelowThresholdKey, value.ContaminationUnpauseBelowThreshold);
            objectSaver.Set(ContaminationUnpauseAboveThresholdKey, value.ContaminationUnpauseAboveThreshold);

            objectSaver.Set(ContaminationPauseBelowEnabledKey, value.ContaminationPauseBelowEnabled);
            objectSaver.Set(ContaminationPauseAboveEnabledKey, value.ContaminationPauseAboveEnabled);
            objectSaver.Set(ContaminationUnpauseBelowEnabledKey, value.ContaminationUnpauseBelowEnabled);
            objectSaver.Set(ContaminationUnpauseAboveEnabledKey, value.ContaminationUnpauseAboveEnabled);
        }

        public Obsoletable<WaterPumpStreamGaugeLink> Deserialize(IObjectLoader objectLoader)
        {
            var link = new WaterPumpStreamGaugeLink(objectLoader.Get(WaterPumpKey),
                                                    objectLoader.Get(StreamGaugeKey))
            {
                Threshold1 = objectLoader.Get(Threshold1Key),
                Threshold2 = objectLoader.Get(Threshold2Key),
                Threshold3 = objectLoader.Get(Threshold3Key),
                Threshold4 = objectLoader.Get(Threshold4Key),
                Enabled1 = objectLoader.Get(Enabled1Key),
                Enabled2 = objectLoader.Get(Enabled2Key),
                Enabled3 = objectLoader.Get(Enabled3Key),
                Enabled4 = objectLoader.Get(Enabled4Key),
                DisableDuringDrought = objectLoader.Has(DisableDuringDroughtKey) ? objectLoader.Get(DisableDuringDroughtKey) : false,
                DisableDuringTemperate = objectLoader.Has(DisableDuringTemperateKey) ? objectLoader.Get(DisableDuringTemperateKey) : false,
                DisableDuringBadtide = objectLoader.Has(DisableDuringBadtideKey) ? objectLoader.Get(DisableDuringBadtideKey) : false,
                ContaminationPauseBelowThreshold = objectLoader.Has(ContaminationPauseBelowThresholdKey) ? objectLoader.Get(ContaminationPauseBelowThresholdKey) : 0f,
                ContaminationPauseAboveThreshold = objectLoader.Has(ContaminationPauseAboveThresholdKey) ? objectLoader.Get(ContaminationPauseAboveThresholdKey) : 0f,
                ContaminationUnpauseBelowThreshold = objectLoader.Has(ContaminationUnpauseBelowThresholdKey) ? objectLoader.Get(ContaminationUnpauseBelowThresholdKey) : 0f,
                ContaminationUnpauseAboveThreshold = objectLoader.Has(ContaminationUnpauseAboveThresholdKey) ? objectLoader.Get(ContaminationUnpauseAboveThresholdKey) : 0f,
                ContaminationPauseBelowEnabled = objectLoader.Has(ContaminationPauseBelowEnabledKey) ? objectLoader.Get(ContaminationPauseBelowEnabledKey) : false,
                ContaminationPauseAboveEnabled = objectLoader.Has(ContaminationPauseAboveEnabledKey) ? objectLoader.Get(ContaminationPauseAboveEnabledKey) : false,
                ContaminationUnpauseBelowEnabled = objectLoader.Has(ContaminationUnpauseBelowEnabledKey) ? objectLoader.Get(ContaminationUnpauseBelowEnabledKey) : false,
                ContaminationUnpauseAboveEnabled = objectLoader.Has(ContaminationUnpauseAboveEnabledKey) ? objectLoader.Get(ContaminationUnpauseAboveEnabledKey) : false
            };
            return link;
        }
    }
}
