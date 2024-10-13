using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using Timberborn.Persistence;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterSourceRegulators
{
    public class WaterSourceRegulatorLinkSerializer : IObjectSerializer<WaterSourceRegulatorStreamGaugeLink>
    {
        private static readonly PropertyKey<WaterSourceRegulatorMonobehaviour> WaterSourceRegulatorKey = new PropertyKey<WaterSourceRegulatorMonobehaviour>($"{nameof(WaterSourceRegulatorStreamGaugeLink.WaterSourceRegulator)}");
        private static readonly PropertyKey<StreamGaugeMonoBehaviour> StreamGaugeKey = new PropertyKey<StreamGaugeMonoBehaviour>($"{nameof(WaterSourceRegulatorStreamGaugeLink.StreamGauge)}");
        private static readonly PropertyKey<float> Threshold1Key = new PropertyKey<float>($"{nameof(WaterSourceRegulatorStreamGaugeLink.Threshold1)}");
        private static readonly PropertyKey<float> Threshold2Key = new PropertyKey<float>($"{nameof(WaterSourceRegulatorStreamGaugeLink.Threshold2)}");
        private static readonly PropertyKey<float> Threshold3Key = new PropertyKey<float>($"{nameof(WaterSourceRegulatorStreamGaugeLink.Threshold3)}");
        private static readonly PropertyKey<float> Threshold4Key = new PropertyKey<float>($"{nameof(WaterSourceRegulatorStreamGaugeLink.Threshold4)}");
        private static readonly PropertyKey<bool> Enabled1Key = new PropertyKey<bool>($"{nameof(WaterSourceRegulatorStreamGaugeLink.Enabled1)}");
        private static readonly PropertyKey<bool> Enabled2Key = new PropertyKey<bool>($"{nameof(WaterSourceRegulatorStreamGaugeLink.Enabled2)}");
        private static readonly PropertyKey<bool> Enabled3Key = new PropertyKey<bool>($"{nameof(WaterSourceRegulatorStreamGaugeLink.Enabled3)}");
        private static readonly PropertyKey<bool> Enabled4Key = new PropertyKey<bool>($"{nameof(WaterSourceRegulatorStreamGaugeLink.Enabled4)}");
        private static readonly PropertyKey<bool> DisableDuringDroughtKey = new PropertyKey<bool>($"{nameof(WaterSourceRegulatorStreamGaugeLink.DisableDuringDrought)}");
        private static readonly PropertyKey<bool> DisableDuringTemperateKey = new PropertyKey<bool>($"{nameof(WaterSourceRegulatorStreamGaugeLink.DisableDuringTemperate)}");
        private static readonly PropertyKey<bool> DisableDuringBadtideKey = new PropertyKey<bool>($"{nameof(WaterSourceRegulatorStreamGaugeLink.DisableDuringBadtide)}");

        private static readonly PropertyKey<bool> ContaminationCloseBelowEnabledKey = new PropertyKey<bool>($"{nameof(WaterSourceRegulatorStreamGaugeLink.ContaminationCloseBelowEnabled)}");
        private static readonly PropertyKey<bool> ContaminationCloseAboveEnabledKey = new PropertyKey<bool>($"{nameof(WaterSourceRegulatorStreamGaugeLink.ContaminationCloseAboveEnabled)}");
        private static readonly PropertyKey<bool> ContaminationOpenBelowEnabledKey = new PropertyKey<bool>($"{nameof(WaterSourceRegulatorStreamGaugeLink.ContaminationOpenBelowEnabled)}");
        private static readonly PropertyKey<bool> ContaminationOpenAboveEnabledKey = new PropertyKey<bool>($"{nameof(WaterSourceRegulatorStreamGaugeLink.ContaminationOpenAboveEnabled)}");
        private static readonly PropertyKey<float> ContaminationCloseBelowThresholdKey = new PropertyKey<float>($"{nameof(WaterSourceRegulatorStreamGaugeLink.ContaminationCloseBelowThreshold)}");
        private static readonly PropertyKey<float> ContaminationCloseAboveThresholdKey = new PropertyKey<float>($"{nameof(WaterSourceRegulatorStreamGaugeLink.ContaminationCloseAboveThreshold)}");
        private static readonly PropertyKey<float> ContaminationOpenBelowThresholdKey = new PropertyKey<float>($"{nameof(WaterSourceRegulatorStreamGaugeLink.ContaminationOpenBelowThreshold)}");
        private static readonly PropertyKey<float> ContaminationOpenAboveThresholdKey = new PropertyKey<float>($"{nameof(WaterSourceRegulatorStreamGaugeLink.ContaminationOpenAboveThreshold)}");

        public void Serialize(WaterSourceRegulatorStreamGaugeLink value, IObjectSaver objectSaver)
        {
            objectSaver.Set(WaterSourceRegulatorKey, value.WaterSourceRegulator);
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

            objectSaver.Set(ContaminationCloseBelowThresholdKey, value.ContaminationCloseBelowThreshold);
            objectSaver.Set(ContaminationCloseAboveThresholdKey, value.ContaminationCloseAboveThreshold);
            objectSaver.Set(ContaminationOpenBelowThresholdKey, value.ContaminationOpenBelowThreshold);
            objectSaver.Set(ContaminationOpenAboveThresholdKey, value.ContaminationOpenAboveThreshold);

            objectSaver.Set(ContaminationCloseBelowEnabledKey, value.ContaminationCloseBelowEnabled);
            objectSaver.Set(ContaminationCloseAboveEnabledKey, value.ContaminationCloseAboveEnabled);
            objectSaver.Set(ContaminationOpenBelowEnabledKey, value.ContaminationOpenBelowEnabled);
            objectSaver.Set(ContaminationOpenAboveEnabledKey, value.ContaminationOpenAboveEnabled);
        }

        public Obsoletable<WaterSourceRegulatorStreamGaugeLink> Deserialize(IObjectLoader objectLoader)
        {
            var link = new WaterSourceRegulatorStreamGaugeLink(objectLoader.Get(WaterSourceRegulatorKey),
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
                ContaminationCloseBelowThreshold = objectLoader.Has(ContaminationCloseBelowThresholdKey) ? objectLoader.Get(ContaminationCloseBelowThresholdKey) : 0f,
                ContaminationCloseAboveThreshold = objectLoader.Has(ContaminationCloseAboveThresholdKey) ? objectLoader.Get(ContaminationCloseAboveThresholdKey) : 0f,
                ContaminationOpenBelowThreshold = objectLoader.Has(ContaminationOpenBelowThresholdKey) ? objectLoader.Get(ContaminationOpenBelowThresholdKey) : 0f,
                ContaminationOpenAboveThreshold = objectLoader.Has(ContaminationOpenAboveThresholdKey) ? objectLoader.Get(ContaminationOpenAboveThresholdKey) : 0f,
                ContaminationCloseBelowEnabled = objectLoader.Has(ContaminationCloseBelowEnabledKey) ? objectLoader.Get(ContaminationCloseBelowEnabledKey) : false,
                ContaminationCloseAboveEnabled = objectLoader.Has(ContaminationCloseAboveEnabledKey) ? objectLoader.Get(ContaminationCloseAboveEnabledKey) : false,
                ContaminationOpenBelowEnabled = objectLoader.Has(ContaminationOpenBelowEnabledKey) ? objectLoader.Get(ContaminationOpenBelowEnabledKey) : false,
                ContaminationOpenAboveEnabled = objectLoader.Has(ContaminationOpenAboveEnabledKey) ? objectLoader.Get(ContaminationOpenAboveEnabledKey) : false
            };
            return link;
        }
    }
}
