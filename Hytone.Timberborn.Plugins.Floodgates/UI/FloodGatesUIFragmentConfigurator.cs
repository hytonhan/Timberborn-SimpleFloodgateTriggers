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
            containerDefinition.Bind<FloodGateUIFragment>().AsSingleton();
            containerDefinition.Bind<FloodgateScheduleFragment>().AsSingleton();
            containerDefinition.Bind<AttachToStreamGaugeButton>().AsSingleton();
            containerDefinition.Bind<AttachToStreamGaugeFragment>().AsSingleton();
            containerDefinition.Bind<TriggersFragment>().AsSingleton();
            containerDefinition.Bind<StreamGaugeFloodgateLinkViewFactory>().AsSingleton();
            containerDefinition.Bind<StreamGaugeFloodgateLinksFragment>().AsSingleton();
            containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
        }

        /// <summary>
        /// This magic class somehow adds our UI fragment into the game
        /// </summary>
        private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
        {
            private readonly FloodGateUIFragment _floodGateFragment;
            private readonly FloodgateScheduleFragment _floodgateScheduleFragment;
            private readonly TriggersFragment _triggersFragment;
            private readonly StreamGaugeFloodgateLinksFragment _streamGaugeFloodgateLinksFragment;

            public EntityPanelModuleProvider(FloodGateUIFragment floodGateFragment,
                                             FloodgateScheduleFragment floodgateScheduleFragment,
                                             TriggersFragment triggersFragment,
                                             StreamGaugeFloodgateLinksFragment streamGaugeFloodgateLinksFragment)
            {
                _floodGateFragment = floodGateFragment ?? throw new ArgumentNullException(nameof(floodGateFragment)); ;
                _floodgateScheduleFragment = floodgateScheduleFragment;
                _triggersFragment = triggersFragment;
                //_attachToStreamGaugeButton = attachToStreamGaugeButton;
                _streamGaugeFloodgateLinksFragment = streamGaugeFloodgateLinksFragment;
            }

            public EntityPanelModule Get()
            {
                EntityPanelModule.Builder builder = new EntityPanelModule.Builder();
                builder.AddBottomFragment(_triggersFragment);
                builder.AddBottomFragment(_streamGaugeFloodgateLinksFragment);
                return builder.Build();
            }
        }
    }
}
