using System;

namespace survey_polling.api.Utils
{
    /// <summary>
    /// Generates a random pin for a newly created lobby.
    /// By default the generator will create a pin that is 6 digits long.
    /// </summary>
    public static class LobbyPinGenerator
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        // The pins minimum and maximum values
        private const int minValue = 0;
        private const int maxValue = 999999;

        /// <summary>
        /// Generates the pin and adds leading zeros when necessary.
        /// </summary>
        /// <returns>A 6 digit lobby pin</returns>
        public static string GetPin()
        {
            lock (syncLock)
            {
                return random.Next(minValue, maxValue + 1).ToString("D6");
            }
        }
    }
}
