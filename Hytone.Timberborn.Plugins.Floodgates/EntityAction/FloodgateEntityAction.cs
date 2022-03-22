using Bindito.Unity;
using Timberborn.WaterBuildings;
using TimberbornAPI.EntityActionSystem;
using UnityEngine;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    public class FloodgateEntityAction : IEntityAction
    {
        private readonly IInstantiator _instantiator;

        public FloodgateEntityAction(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        /// <summary>
        /// Add our custom trigger class to all Floodgates
        /// </summary>
        /// <param name="entity"></param>
        public void ApplyToEntity(GameObject entity)
        {
            if (entity.GetComponent<Floodgate>() == null)
            {
                return;
            }

            _instantiator.AddComponent<FloodgateTriggerMonoBehaviour>(entity);
        }
    }
}
