using System;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using NeoModLoader.api;

namespace ShinobiBox
{
    public class ShinobiConfig
    {
        public static bool EnableClanNames = true;
        public static bool EnableAutoRanking = true;
        public static bool EnableAutoFavorite = true;
        public static bool EnableWorldTips = true;

        public static void SetWorldTips(bool value) 
        {
            EnableWorldTips = value;
            UnityEngine.Debug.Log($"[ShinobiBox] EnableWorldTips set to: {value}");
        }

        public static void SetClanNames(bool value)
        {
            EnableClanNames = value;
            UnityEngine.Debug.Log($"[ShinobiBox] EnableClanNames set to: {value}");
        }

        public static void SetAutoRanking(bool value)
        {
            EnableAutoRanking = value;
            UnityEngine.Debug.Log($"[ShinobiBox] EnableAutoRanking set to: {value}");
        }

        public static void SetAutoFavorite(bool value)
        {
            EnableAutoFavorite = value;
            UnityEngine.Debug.Log($"[ShinobiBox] EnableAutoFavorite set to: {value}");
        }
    }
}
