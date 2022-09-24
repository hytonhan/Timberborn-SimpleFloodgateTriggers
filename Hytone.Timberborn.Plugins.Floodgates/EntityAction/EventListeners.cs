using Bindito.Core;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using Timberborn.SingletonSystem;
using Timberborn.WeatherSystem;
using UnityEngine;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    public class EventListeners: ILoadableSingleton
    {
        private EventBus _eventBus;
        
        [Inject]
        public void InjectDependencies(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Load()
        {
            _eventBus.Register(this);
        }

        [OnEvent]
        public void OnDroughtStarted(DroughtStartedEvent droughtStartedEvent)
        {
            var floodgates = GameObject.FindObjectsOfType(typeof(FloodgateTriggerMonoBehaviour));
            if (floodgates != null)
            {
                foreach (FloodgateTriggerMonoBehaviour floodgate in floodgates)
                {
                    floodgate.OnDroughtStarted();
                }
            }

            var waterpumps = GameObject.FindObjectsOfType(typeof(WaterPumpMonobehaviour));
            if(waterpumps != null)
            {
                foreach (WaterPumpMonobehaviour waterpump in waterpumps)
                {
                    waterpump.OnDroughtStarted();
                }
            }
        }

        [OnEvent]
        public void OnDroughtEnded(DroughtEndedEvent droughtEndedEvent)
        {
            var floodgates = GameObject.FindObjectsOfType(typeof(FloodgateTriggerMonoBehaviour));
            if (floodgates != null)
            {
                foreach(FloodgateTriggerMonoBehaviour floodgate in floodgates)
                {
                    floodgate.OnDroughtEnded();
                }
            }
            var waterpumps = GameObject.FindObjectsOfType(typeof(WaterPumpMonobehaviour));
            if (waterpumps != null)
            {
                foreach(WaterPumpMonobehaviour waterpump in waterpumps)
                {
                    waterpump.OnDroughtEnded();
                }
            }
        }
    }
}
