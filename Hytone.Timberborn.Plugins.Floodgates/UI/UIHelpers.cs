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
        private static readonly string _maxHeightName1 = "MaxHeight";
        private static readonly string _maxHeightName2 = "_maxHeight";

        public static int GetMaxHeight(Floodgate floodgate)
        {
            return floodgate.MaxHeight;
            // var height = floodgate.GetType()
            //                       .GetField(_maxHeightName1,
            //                                 System.Reflection.BindingFlags.NonPublic |
            //                                 System.Reflection.BindingFlags.Public |
            //                                 System.Reflection.BindingFlags.Instance)
            //                       ?.GetValue(floodgate);

            // if (height == null)
            // {
            //     height = floodgate.GetType()
            //                       .GetField(_maxHeightName2,
            //                                 System.Reflection.BindingFlags.NonPublic |
            //                                 System.Reflection.BindingFlags.Public |
            //                                 System.Reflection.BindingFlags.Instance)
            //                       ?.GetValue(floodgate);
            // }
            // return (int)height;
        }

        public static float GetMaxHeight(StreamGauge streamGauge)
        {
            UnityEngine.Debug.Log($"streamgauge2: {streamGauge}");
            var maxWaterLevelField = typeof(StreamGauge).GetField("_maxWaterLevel",  BindingFlags.NonPublic | BindingFlags.Instance);
            UnityEngine.Debug.Log($"maxWaterLevelField: {maxWaterLevelField}");
            UnityEngine.Debug.Log($"maxWaterLevelFieldValue: {maxWaterLevelField.GetValue(streamGauge)}");
            return (float)maxWaterLevelField.GetValue(streamGauge);
        }
    }
}
