using Timberborn.WaterBuildings;
using TimberbornAPI.EntityActionSystem;
using UnityEngine;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    public class EntityActions : IEntityAction
    {
        /// <summary>
        /// Add our custom classes to all Floodgates and StreamGauges
        /// </summary>
        /// <param name="entity"></param>
        public void ApplyToEntity(GameObject entity)
        {
            if (entity.GetComponent<Floodgate>() != null)
            {
                entity.AddComponent<FloodgateTriggerMonoBehaviour>();
            }
            if (entity.GetComponent<StreamGauge>() != null)
            {
                entity.AddComponent<StreamGaugeMonoBehaviour>();
            }
        }
    }
}
