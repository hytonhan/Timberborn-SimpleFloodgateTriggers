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
            containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
        }

        /// <summary>
        /// This magic class somehow adds our UI fragment into the game
        /// </summary>
        private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
        {
            private readonly FloodGateUIFragment _floodGateFragment;

            public EntityPanelModuleProvider(FloodGateUIFragment floodGateFragment)
            {
                _floodGateFragment = floodGateFragment ?? throw new ArgumentNullException(nameof(floodGateFragment)); ;
            }

            public EntityPanelModule Get()
            {
                EntityPanelModule.Builder builder = new EntityPanelModule.Builder();
                builder.AddBottomFragment(_floodGateFragment);
                return builder.Build();
            }
        }
    }
}
