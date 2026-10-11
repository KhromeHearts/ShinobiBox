using System;
using System.IO;
using FMOD;
using FMODUnity;
using UnityEngine;

namespace Isekai
{
    public class PlayWavDirectly
    {
        private static PlayWavDirectly _instance;
        public static PlayWavDirectly Instance => _instance ??= new PlayWavDirectly();
        private FMOD.System fmodSystem;
        private ChannelGroup masterChannelGroup;

        private PlayWavDirectly()
        {
            InitializeFMODSystem();
        }

        private void InitializeFMODSystem()
        {
            var result = RuntimeManager.StudioSystem.getCoreSystem(out fmodSystem);
            if (result != FMOD.RESULT.OK)
            {
                UnityEngine.Debug.LogError($"Failed to initialize FMOD Core System. Result: {result}");
                return;
            }
            result = fmodSystem.getMasterChannelGroup(out masterChannelGroup);
            if (result != FMOD.RESULT.OK)
            {
                UnityEngine.Debug.LogError($"Failed to retrieve master channel group. Result: {result}");
            }
        }

        public void PlaySoundFromFile(string filePath)
        {
            if (!File.Exists(filePath)) return;

            try
            {
                FMOD.Sound sound;
                RESULT result = fmodSystem.createSound(filePath, MODE._2D, out sound);
                if (result != RESULT.OK) return;

                Channel channel;
                result = fmodSystem.playSound(sound, masterChannelGroup, false, out channel);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Error playing 2D sound: {ex.Message}");
            }
        }

        public void PlaySoundAtPosition(string filePath, Vector3 position, float minDistance = 5f, float maxDistance = 45f, float volume = 0.25f)
        {
            if (!File.Exists(filePath)) return;

            try
            {
                FMOD.Sound sound;
                RESULT result = fmodSystem.createSound(filePath, MODE._3D | MODE._3D_LINEARROLLOFF, out sound);
                if (result != RESULT.OK) return;

                sound.set3DMinMaxDistance(minDistance, maxDistance);

                Channel channel;
                result = fmodSystem.playSound(sound, masterChannelGroup, true, out channel);
                if (result != RESULT.OK)
                {
                    sound.release();
                    return;
                }

                FMOD.VECTOR fmodPos = new FMOD.VECTOR { x = position.x, y = position.y, z = position.z };
                FMOD.VECTOR fmodVel = new FMOD.VECTOR { x = 0f, y = 0f, z = 0f };
                channel.set3DAttributes(ref fmodPos, ref fmodVel);

                channel.setVolume(volume);

                channel.setPaused(false);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Error playing 3D sound: {ex.Message}");
            }
        }
    }
}