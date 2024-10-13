using Bindito.Core;
using Hytone.Timberborn.Plugins.Floodgates.UI.WaterSourceRegulators;
using Timberborn.EntityPanelSystem;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    [Context("Game")]
    public class FloodGatesUIFragmentConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<FloodgateHazardFragment>().AsSingleton();
            containerDefinition.Bind<FloodgateScheduleFragment>().AsSingleton();
            containerDefinition.Bind<AttachFloodgateToStreamGaugeButton>().AsSingleton();
            containerDefinition.Bind<AttachFloodgateToStreamGaugeFragment>().AsSingleton();
            containerDefinition.Bind<TriggersFragment>().AsSingleton();
            containerDefinition.Bind<LinkViewFactory>().AsSingleton();
            containerDefinition.Bind<StreamGaugeFloodgateLinksFragment>().AsSingleton();
            containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();

            containerDefinition.Bind<WaterPumpFragment>().AsSingleton();
            containerDefinition.Bind<AttachWaterpumpToStreamGaugeButton>().AsSingleton();
            containerDefinition.Bind<AttachWaterpumpToStreamGaugeFragment>().AsSingleton();
            containerDefinition.Bind<WaterpumpDroughtSettingsFragment>().AsSingleton();
            containerDefinition.Bind<WaterpumpScheduleFragment>().AsSingleton();

            containerDefinition.Bind<WaterSourceRegulatorFragment>().AsSingleton();
            containerDefinition.Bind<AttachWaterSourceRegulatorButton>().AsSingleton();
            containerDefinition.Bind<AttachWaterSourceRegulatorFragment>().AsSingleton();
            containerDefinition.Bind<WaterSourceRegulatorDroughtFragment>().AsSingleton();
            containerDefinition.Bind<WaterSourceRegulatorScheduleFragment>().AsSingleton();
        }

        private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
        {
           private readonly TriggersFragment _triggersFragment;
            private readonly StreamGaugeFloodgateLinksFragment _streamGaugeFloodgateLinksFragment;

            private readonly WaterPumpFragment _waterPumpFragment;

            private readonly WaterSourceRegulatorFragment _waterSourceRegulatorFragment;

            public EntityPanelModuleProvider(TriggersFragment triggersFragment,
                                             StreamGaugeFloodgateLinksFragment streamGaugeFloodgateLinksFragment,
                                             WaterPumpFragment waterPumpFragment,
                                             WaterSourceRegulatorFragment waterSourceRegulatorFragment)
            {
                _triggersFragment = triggersFragment;
                _streamGaugeFloodgateLinksFragment = streamGaugeFloodgateLinksFragment;
                _waterPumpFragment = waterPumpFragment;
                _waterSourceRegulatorFragment = waterSourceRegulatorFragment;
            }

            public EntityPanelModule Get()
            {
                EntityPanelModule.Builder builder = new EntityPanelModule.Builder();
                builder.AddMiddleFragment(_triggersFragment);
                builder.AddMiddleFragment(_streamGaugeFloodgateLinksFragment);
                builder.AddMiddleFragment(_waterPumpFragment);
                builder.AddMiddleFragment(_waterSourceRegulatorFragment);
                return builder.Build();
            }
        }
    }
}
