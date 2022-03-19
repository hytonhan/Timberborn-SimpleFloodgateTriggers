using Timberborn.Persistence;
using Timberborn.WaterBuildings;
using UnityEngine;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    /// <summary>
    /// This class handles the data related to Floodgate Triggers. It also holds the actions
    /// which are executed when certain events happen.
    /// </summary>
    public class FloodgateTriggerMonoBehaviour : MonoBehaviour, IPersistentEntity
    {
        //Keys used in data saving/loading
        private static readonly ComponentKey FloodgateTriggerKey = new ComponentKey(nameof(FloodgateTriggerMonoBehaviour));
        private static readonly PropertyKey<bool> DroughtEndedEnabledKey = new PropertyKey<bool>(nameof(DroughtEndedEnabled));
        private static readonly PropertyKey<float> DroughtEndedHeightKey = new PropertyKey<float>(nameof(DroughtEndedHeight));
        private static readonly PropertyKey<bool> DroughtStartedEnabledKey = new PropertyKey<bool>(nameof(DroughtStartedEnabled));
        private static readonly PropertyKey<float> DroughtStartedHeightKey = new PropertyKey<float>(nameof(DroughtStartedHeight));

        public bool DroughtEndedEnabled { get; set; }
        public float DroughtEndedHeight { get; set; }
        public bool DroughtStartedEnabled { get; set; }
        public float DroughtStartedHeight { get; set; }

        /// <summary>
        /// Save the data so we can load it later
        /// </summary>
        /// <param name="entitySaver"></param>
        public void Save(IEntitySaver entitySaver)
        {
            IObjectSaver component = entitySaver.GetComponent(FloodgateTriggerKey);
            component.Set(DroughtEndedEnabledKey, DroughtEndedEnabled);
            component.Set(DroughtEndedHeightKey, DroughtEndedHeight);
            component.Set(DroughtStartedEnabledKey, DroughtStartedEnabled);
            component.Set(DroughtStartedHeightKey, DroughtStartedHeight);
        }

        /// <summary>
        /// Load saved data if it exists
        /// </summary>
        /// <param name="entityLoader"></param>
        public void Load(IEntityLoader entityLoader)
        {
            if (!entityLoader.HasComponent(FloodgateTriggerKey))
            {
                return;
            }
            IObjectLoader component = entityLoader.GetComponent(FloodgateTriggerKey);
            if(component.Has(DroughtEndedEnabledKey))
            {
                DroughtEndedEnabled = component.Get(DroughtEndedEnabledKey);
            }
            if (component.Has(DroughtEndedHeightKey))
            {
                DroughtEndedHeight = component.Get(DroughtEndedHeightKey);
            }
            if (component.Has(DroughtStartedEnabledKey))
            {
                DroughtStartedEnabled = component.Get(DroughtStartedEnabledKey);
            }
            if (component.Has(DroughtStartedHeightKey))
            {
                DroughtStartedHeight = component.Get(DroughtStartedHeightKey);
            }
        }

        /// <summary>
        /// The stuff to do to FLoodgates when a Drought starts
        /// </summary>
        public void OnDroughtStarted()
        {
            var floodgate = GetComponent<Floodgate>();
            if (DroughtStartedEnabled == true &&
               floodgate.Height != DroughtStartedHeight)
            {
                floodgate.SetHeight(DroughtStartedHeight);
            }
        }

        /// <summary>
        /// The stuff to do to Floodgates when a Drought ends
        /// </summary>
        public void OnDroughtEnded()
        {
            var floodgate = GetComponent<Floodgate>();
            if (DroughtEndedEnabled == true &&
               floodgate.Height != DroughtEndedHeight)
            {
                floodgate.SetHeight(DroughtEndedHeight);
            }
        }
    }
}
