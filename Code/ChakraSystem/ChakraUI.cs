using System;
using System.Threading;
using NCMS;
using UnityEngine;
using ReflectionUtility;
using System.Text;
using System.Linq;
using ai;
using NeoModLoader.services;
using NeoModLoader.api;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using NCMS.Utils;
using UnityEngine.Events;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine.UI;
using Object = UnityEngine.Object;


namespace ShinobiBox
{

    [HarmonyPatch(typeof(StatsIconContainer), "Awake")]
    public static class ChakraIconPatch
    {
        [HarmonyPostfix]
        static void Postfix(StatsIconContainer __instance)
        {
            if (__instance == null || __instance._stats_icons == null || __instance._stats_icons.ContainsKey("i_Chakra"))
                return;

            if (__instance._stats_icons.Count == 0) return;

            var template = __instance._stats_icons.Values.First();
            GameObject cloneGO = Object.Instantiate(template.gameObject, template.transform.parent);
            cloneGO.name = "i_Chakra";

            StatsIcon icon = cloneGO.GetComponent<StatsIcon>();

            Sprite chakraSprite = SpriteTextureLoader.getSprite("ui/icons/staticon");
            icon.getIcon().sprite = chakraSprite;

            TipButton tip = cloneGO.GetComponent<TipButton>();
            tip.textOnClick = "Chakra";
            tip.textOnClickDescription = "Energy used for Shinobi techniques.";

            __instance._stats_icons.Add("i_Chakra", icon);
        }
    }

    [HarmonyPatch(typeof(StatsIconContainer), "Awake")]
    public static class BlindnessIconPatch
    {
        [HarmonyPostfix]
        static void Postfix(StatsIconContainer __instance)
        {
            if (__instance == null || __instance._stats_icons == null || __instance._stats_icons.ContainsKey("i_blindness"))
                return;

            if (__instance._stats_icons.Count == 0) return;

            var template = __instance._stats_icons.Values.First();
            GameObject cloneGO = Object.Instantiate(template.gameObject, template.transform.parent);
            cloneGO.name = "i_blindness";

            StatsIcon icon = cloneGO.GetComponent<StatsIcon>();

            Sprite blindnessSprite = SpriteTextureLoader.getSprite("ui/icons/Blind");
            icon.getIcon().sprite = blindnessSprite;

            TipButton tip = cloneGO.GetComponent<TipButton>();
            tip.textOnClick = "Blindness";
            tip.textOnClickDescription = "Mangekyo Sharingan usage level. At 25, permanent blindness occurs.";

            __instance._stats_icons.Add("i_blindness", icon);
        }
    }

    [HarmonyPatch(typeof(StatsIconContainer), "Awake")]
    public static class NatureExpIconPatch
    {
        [HarmonyPostfix]
        static void Postfix(StatsIconContainer __instance)
        {
            EnsureIcon(__instance);
        }

        public static void EnsureIcon(StatsIconContainer container)
        {
            if (container == null || container._stats_icons == null || container._stats_icons.ContainsKey("i_nature_exp"))
                return;

            if (container._stats_icons.Count == 0) return;

            var template = container._stats_icons.Values.First();
            GameObject cloneGO = Object.Instantiate(template.gameObject, template.transform.parent);
            cloneGO.name = "i_nature_exp";

            StatsIcon icon = cloneGO.GetComponent<StatsIcon>();
            Sprite natureExpSprite = SpriteTextureLoader.getSprite("ui/icons/natureexp");
            if (natureExpSprite != null)
            {
                icon.getIcon().sprite = natureExpSprite;
            }

            TipButton tip = cloneGO.GetComponent<TipButton>();
            tip.textOnClick = "Nature Exp";
            tip.textOnClickDescription = "Global chakra nature EXP. Every 100 EXP, there is a 10% chance to gain a random missing base nature.";

            container._stats_icons.Add("i_nature_exp", icon);
        }
    }

    [HarmonyPatch(typeof(UnitStatsElement), "showContent")]
    public static class ChakraValuePatch
    {
        [HarmonyPostfix]
        static void Postfix(UnitStatsElement __instance)
        {
            if (__instance == null || __instance.actor == null || !__instance.actor.isAlive()) return;

            NatureExpIconPatch.EnsureIcon(__instance.GetComponent<StatsIconContainer>());

            float current = ChakraSystem.GetCurrent(__instance.actor);
            float max = ChakraSystem.GetMax(__instance.actor);

            __instance.setIconValue("i_Chakra", (int)current, (int)max, "", false, "", '/');

            int blindness = 0;
            __instance.actor.data.get("ms_usage", out blindness);
            blindness = Mathf.Min(blindness, 25);
            __instance.setIconValue("i_blindness", blindness, 25, "", false, "", '/');

            int totalNatureExp = (int)JutsuLibrary.GetNatureProgressExp(__instance.actor);
            __instance.setIconValue("i_nature_exp", totalNatureExp, 1000, "", false, "", '/');
        }
    }

    [HarmonyPatch(typeof(UnitWindow), "Update")]
    public static class RealTimeChakraUpdate
    {
        [HarmonyPostfix]
        public static void Postfix(UnitWindow __instance)
        {
            if (!__instance.gameObject.activeSelf || __instance.actor == null || !__instance.actor.isAlive()) return;

            UnitStatsElement stats = __instance.GetComponentInChildren<UnitStatsElement>();
            if (stats != null)
            {
                NatureExpIconPatch.EnsureIcon(stats.GetComponent<StatsIconContainer>());

                float current = ChakraSystem.GetCurrent(__instance.actor);
                float max = ChakraSystem.GetMax(__instance.actor);

                stats.setIconValue("i_Chakra", (int)current, (int)max, "", false, "", '/');

                int blindness = 0;
                __instance.actor.data.get("ms_usage", out blindness);
                blindness = Mathf.Min(blindness, 25);
                stats.setIconValue("i_blindness", blindness, 20, "", false, "", '/');

                int totalNatureExp = (int)JutsuLibrary.GetNatureProgressExp(__instance.actor);
                stats.setIconValue("i_nature_exp", totalNatureExp, 1000, "", false, "", '/');
            }
        }
    }
}