using Bindito.Unity;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using Timberborn.WaterBuildings;
using TimberbornAPI.EntityActionSystem;
using UnityEngine;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    public class EntityActions : IEntityAction
    {
        private readonly IInstantiator _instantiator;

        public EntityActions(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        /// <summary>
        /// Add our custom classes to all Floodgates and StreamGauges
        /// </summary>
        /// <param name="entity"></param>
        public void ApplyToEntity(GameObject entity)
        {
            if (entity.GetComponent<Floodgate>() != null)
            {
                _instantiator.AddComponent<FloodgateTriggerMonoBehaviour>(entity);
            }
            if (entity.GetComponent<StreamGauge>() != null)
            {
                _instantiator.AddComponent<StreamGaugeMonoBehaviour>(entity);
            }
            if ((entity.GetComponent<WaterInput>() != null || entity.GetComponent<WaterOutput>() != null)
                && entity.name.ToLower().Contains("shower") == false)
            {
                _instantiator.AddComponent<WaterPumpMonobehaviour>(entity);
            }
        }
    }
}
