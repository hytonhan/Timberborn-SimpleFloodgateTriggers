using Bindito.Core;
using System;
using Timberborn.EntityPanelSystem;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    /// <summary>
    /// Configurator for FloodgateTriggers UI stuff
    /// </summary>
    public class FloodGatesUIFragmentConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<FloodgateDroughtFragment>().AsSingleton();
            containerDefinition.Bind<FloodgateScheduleFragment>().AsSingleton();
            containerDefinition.Bind<AttachToStreamGaugeButton>().AsSingleton();
            containerDefinition.Bind<AttachFloodgateToStreamGaugeFragment>().AsSingleton();
            containerDefinition.Bind<TriggersFragment>().AsSingleton();
            containerDefinition.Bind<LinkViewFactory>().AsSingleton();
            containerDefinition.Bind<StreamGaugeFloodgateLinksFragment>().AsSingleton();
            containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();

            containerDefinition.Bind<WaterPumpFragment>().AsSingleton();
            containerDefinition.Bind<AttachWaterpumpToStreamGaugeFragment>().AsSingleton();
            containerDefinition.Bind<WaterpumpDroughtSettingsFragment>().AsSingleton();
            containerDefinition.Bind<WaterpumpScheduleFragment>().AsSingleton();
        }

        /// <summary>
        /// This magic class somehow adds our UI fragment into the game
        /// </summary>
        private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
        {
           private readonly TriggersFragment _triggersFragment;
            private readonly StreamGaugeFloodgateLinksFragment _streamGaugeFloodgateLinksFragment;

            private readonly WaterPumpFragment _waterPumpFragment;

            public EntityPanelModuleProvider(TriggersFragment triggersFragment,
                                             StreamGaugeFloodgateLinksFragment streamGaugeFloodgateLinksFragment,
                                             WaterPumpFragment waterPumpFragment)
            {
                _triggersFragment = triggersFragment;
                _streamGaugeFloodgateLinksFragment = streamGaugeFloodgateLinksFragment;
                _waterPumpFragment = waterPumpFragment;
            }

            public EntityPanelModule Get()
            {
                EntityPanelModule.Builder builder = new EntityPanelModule.Builder();
                builder.AddBottomFragment(_triggersFragment);
                builder.AddBottomFragment(_streamGaugeFloodgateLinksFragment);
                builder.AddBottomFragment(_waterPumpFragment);
                return builder.Build();
            }
        }
    }
}
