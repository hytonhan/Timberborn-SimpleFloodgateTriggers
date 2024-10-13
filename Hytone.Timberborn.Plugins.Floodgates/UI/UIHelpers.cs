using System.Diagnostics;
using System.IO;
using System.Reflection;
using Timberborn.WaterBuildings;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    /// <summary>
    /// This is a stupid class that helped this mod to work between stable and experimental for a while in summerof 2022
    /// </summary>
    public static class UIHelpers
    {

        public static int GetMaxHeight(Floodgate floodgate)
        {
            return floodgate.MaxHeight;
        }

        public static float GetMaxHeight(StreamGauge streamGauge)
        {
            var maxWaterLevelField = typeof(StreamGauge).GetField("_maxWaterLevel",  BindingFlags.NonPublic | BindingFlags.Instance);
            return (float)maxWaterLevelField.GetValue(streamGauge);
        }
    }
}
