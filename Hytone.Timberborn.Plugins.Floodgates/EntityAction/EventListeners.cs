using Bindito.Core;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using System;
using System.Reflection;
using Timberborn.HazardousWeatherSystem;
using Timberborn.SingletonSystem;
using Timberborn.WaterBuildings;
using Timberborn.WeatherSystem;
using UnityEngine;

namespace Hytone.Timberborn.Plugins.Floodgates.EntityAction
{
    public class EventListeners : ILoadableSingleton
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
        public void OnHazardousWeatherStarted(HazardousWeatherStartedEvent hazardousWeatherStartedEvent)
        {
            var hazardWeather = hazardousWeatherStartedEvent.HazardousWeather.GetType();
            var floodgates = GameObject.FindObjectsOfType(typeof(FloodgateTriggerMonoBehaviour));
            if (floodgates != null)
            {
                foreach (FloodgateTriggerMonoBehaviour floodgate in floodgates)
                {
                    if (hazardWeather == typeof(DroughtWeather))
                    {
                        floodgate.OnDroughtStarted();
                    }
                    else if (hazardWeather == typeof(BadtideWeather))
                    {
                        floodgate.OnBadtideStarted();
                    }
                }
            }

            var waterpumps = GameObject.FindObjectsOfType(typeof(WaterPumpMonobehaviour));
            if (waterpumps != null)
            {
                foreach (WaterPumpMonobehaviour waterpump in waterpumps)
                {
                    if (hazardWeather == typeof(DroughtWeather))
                    {
                        waterpump.OnDroughtStarted();

                    }
                    else if (hazardWeather == typeof(BadtideWeather))
                    {
                        waterpump.OnBadtideStarted();
                    }
                }
            }
        }

        [OnEvent]
        public void OnHazardousWeatherEnded(HazardousWeatherEndedEvent hazardousWeatherEndedEndedEvent)
        {
            var hazardWeather = hazardousWeatherEndedEndedEvent.HazardousWeather.GetType();
            var floodgates = GameObject.FindObjectsOfType(typeof(FloodgateTriggerMonoBehaviour));
            if (floodgates != null)
            {
                foreach (FloodgateTriggerMonoBehaviour floodgate in floodgates)
                {
                    if (hazardWeather == typeof(DroughtWeather))
                    {
                        floodgate.OnDroughtEnded();
                    }
                    else if (hazardWeather == typeof(BadtideWeather))
                    {
                        floodgate.OnBadtideEnded();
                    }
                }
            }
            var waterpumps = GameObject.FindObjectsOfType(typeof(WaterPumpMonobehaviour));
            if (waterpumps != null)
            {
                foreach (WaterPumpMonobehaviour waterpump in waterpumps)
                {
                    //if (hazardWeather == typeof(DroughtWeather))
                    //{
                        waterpump.OnTemperateStarted();
                    //}
                    //else if (hazardWeather == typeof(BadtideWeather))
                    //{
                    //    Console.WriteLine($"UNCOMMENT WATERPUMP BADTIDE END");
                    //    //waterpump.OnBadtideEnded();
                    //}
                }
            }
        }
    }
}
