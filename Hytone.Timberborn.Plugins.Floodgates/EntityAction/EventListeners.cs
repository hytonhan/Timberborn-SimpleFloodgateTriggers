using Timberborn.SingletonSystem;
using Timberborn.WeatherSystem;
using TimberbornAPI.EventSystem;
using UnityEngine;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    public class EventListeners : EventListener
    {
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
        }
    }
}
