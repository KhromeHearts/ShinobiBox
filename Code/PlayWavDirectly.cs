using System;
using System.IO;
using FMOD;
using FMODUnity;
using UnityEngine;

// THANK YOU AHOYOS
namespace Isekai
{
    public class PlayWavDirectly
    {
        private static PlayWavDirectly _instance;
        public static PlayWavDirectly Instance => _instance ??= new PlayWavDirectly();
        private FMOD.System fmodSystem;
        private ChannelGroup masterChannelGroup;
        public FMOD.VECTOR fmodPosition;
        public FMOD.VECTOR zeroVel;

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
            if (!File.Exists(filePath))
            {
                UnityEngine.Debug.LogError($"File not found at path: {filePath}");
                return;
            }
            try
            {
                FMOD.Sound sound;
                RESULT result = fmodSystem.createSound(filePath, MODE.DEFAULT, out sound);
                if (result != RESULT.OK)
                {
                    UnityEngine.Debug.LogError($"FMOD failed to create sound: {result}");
                    return;
                }
                Channel channel;
                result = fmodSystem.playSound(sound, masterChannelGroup, false, out channel);
                if (result != RESULT.OK)
                {
                    UnityEngine.Debug.LogError($"FMOD failed to play sound: {result}");
                    sound.release();
                    return;
                }
                UnityEngine.Debug.Log($"Playing sound: {filePath}");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Error while playing sound: {ex.Message}");
            }
        }

        public void PlaySoundAtPosition(string filePath, Vector3 position)
        {
            if (!File.Exists(filePath))
            {
                UnityEngine.Debug.LogError($"File not found at path: {filePath}");
                return;
            }
            try
            {
                FMOD.Sound sound;
                RESULT result = fmodSystem.createSound(filePath, MODE.DEFAULT, out sound);
                if (result != RESULT.OK)
                {
                    UnityEngine.Debug.LogError($"FMOD failed to create sound: {result}");
                    return;
                }
                Channel channel;
                result = fmodSystem.playSound(sound, masterChannelGroup, false, out channel);
                if (result != RESULT.OK)
                {
                    UnityEngine.Debug.LogError($"FMOD failed to play sound: {result}");
                    sound.release();
                    return;
                }
                Vector3 sourcePosition = new Vector3
                {
                    x = position.x,
                    y = position.y,
                    z = position.z
                };
                Vector3 listenerPosition = Camera.main.transform.position;
                zeroVel = new FMOD.VECTOR { x = 0f, y = 0f, z = 0f };
                channel.setVolume(0.1f);
                float normalizedDistance = Mathf.Clamp01(Vector3.Distance(sourcePosition, listenerPosition) / 300.0f);
                channel.setVolume(0.2f * (1f - normalizedDistance));
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Error while playing sound: {ex.Message}");
            }
        }
    }
}
