using UnityEngine;

namespace ShinobiBox
{
    public static class ShinobiSounds
    {
        private static bool _initialized;

        public static void Init()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            Debug.Log("[ShinobiBox] ShinobiSounds initialized.");
        }

        public static void Play(string soundId, Vector3 worldPos)
        {
            if (!_initialized || string.IsNullOrEmpty(soundId))
            {
                return;
            }

            // Sound hooks can be implemented here as custom clips are registered.
        }
    }
}
