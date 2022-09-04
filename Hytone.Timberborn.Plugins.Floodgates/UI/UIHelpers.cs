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
            var height = floodgate.GetType()
                                  .GetField(_maxHeightName1,
                                            System.Reflection.BindingFlags.NonPublic |
                                            System.Reflection.BindingFlags.Public |
                                            System.Reflection.BindingFlags.Instance)
                                  ?.GetValue(floodgate);

            if (height == null)
            {
                height = floodgate.GetType()
                                  .GetField(_maxHeightName2,
                                            System.Reflection.BindingFlags.NonPublic |
                                            System.Reflection.BindingFlags.Public |
                                            System.Reflection.BindingFlags.Instance)
                                  ?.GetValue(floodgate);
            }
            return (int)height;
        }
    }
}
