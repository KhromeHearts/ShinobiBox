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


namespace ShinobiBox
{
    [HarmonyPatch(typeof(BabyMaker), "makeBaby")]
    public static class HagoromoBirthPatch
    {
        [HarmonyPostfix]
        public static void Postfix(Actor __result, Actor pParent1, Actor pParent2)
        {
            if (__result == null || __result.asset == null || __result.asset.id != "human") return;
            if (__result.hasTrait("indra_chakra") || __result.hasTrait("asura_chakra")) return;

            bool parentHasHagoromo =
                (pParent1 != null && pParent1.hasTrait("hagoromo_chakra")) ||
                (pParent2 != null && pParent2.hasTrait("hagoromo_chakra"));

            if (!parentHasHagoromo) return;

            if (UnityEngine.Random.value < 0.5f)
            {
                __result.addTrait("indra_chakra");
            }
            else
            {
                __result.addTrait("asura_chakra");
            }
        }
    }

    [HarmonyPatch(typeof(BabyMaker), "makeBaby")]
    public static class GuaranteedInheritancePatch
    {
        [HarmonyPostfix]
        public static void Postfix(Actor __result, Actor pParent1, Actor pParent2)
        {
            if (__result == null) return;

            ApplyGuaranteedTraitsFromParent(__result, pParent1);
            ApplyGuaranteedTraitsFromParent(__result, pParent2);
        }

        private static void ApplyGuaranteedTraitsFromParent(Actor child, Actor parent)
        {
            if (child == null || parent == null) return;

            foreach (ActorTrait trait in parent.getTraits())
            {
                if (trait == null) continue;
                if (trait.rate_inherit < 100) continue;

                child.addTrait(trait.id);
            }
        }
    }


    #region JinchurikiStressPatch
    [HarmonyPatch(typeof(Actor), "getHit")]
    public static class JinchurikiStressPatch
    {
        [HarmonyPostfix]
        public static void Postfix(Actor __instance, float pDamage)
        {
            if (__instance == null || !__instance.isAlive()) return;

            // Creation Rebirth (Slug Sage Mode)
            if (__instance.hasTrait("slug_sage_mode") && __instance.hasStatus("status_perfect_slug_sage") && !__instance.hasStatus("status_creation_rebirth_cooldown"))
            {
                
                float slugHealthPct = (float)__instance.data.health / __instance.getMaxHealth();
                if (slugHealthPct < 0.15f)
                {
                    __instance.restoreHealth(__instance.getMaxHealth());
                    __instance.addStatusEffect("status_creation_rebirth_cooldown", 100f);

                    // -3 year lifespan tax
                    int lifespan = 0;
                    __instance.data.get("lifespan", out lifespan);
                    if (lifespan <= 0)
                    {
                        lifespan = Mathf.Max(__instance.data.getAge() + 1, 1);
                    }
                    __instance.data.set("lifespan", Mathf.Max(1, lifespan - 3));
                }
            }

            if (!__instance.hasTrait("nine_tails_jinchuriki")) return;

            if ((__instance.hasStatus("status_jinchuriki_initial_release") || __instance.hasStatus("status_jinchuriki_v1_cloak") ||
                __instance.hasStatus("status_jinchuriki_v2_cloak") || __instance.hasStatus("status_jinchuriki_incomplete_beast")) &&
                UnityEngine.Random.value < 0.15f && !__instance.hasStatus("angry"))
            {
                __instance.addStatusEffect("angry", 10f);
            }
        }
    }
    #endregion

    #region Mangekyo To EMS
    // Mangekyo To EMS
    [HarmonyPatch(typeof(Actor), "getHit")]
    public static class EMSKillPatch
    {
        private const float EyeStealChance = 0.009f;

        [HarmonyPrefix]
        public static void Prefix(Actor __instance, float pDamage, bool pFlash, AttackType pAttackType, BaseSimObject pAttacker)
        {
            if (__instance == null || !__instance.isAlive() || pAttacker == null || pAttacker.a == null) return;

            if (__instance.data.health - pDamage <= 0)
            {

                if (UnityEngine.Random.value < EyeStealChance)
                {
                    string stolenSharinganTier = GetStealableSharinganTier(__instance);
                    if (!string.IsNullOrEmpty(stolenSharinganTier))
                    {
                        RemoveAllSharinganTiers(pAttacker.a);
                        pAttacker.a.addTrait(stolenSharinganTier);

                        pAttacker.a.removeTrait("trait_blind");
                        pAttacker.a.data.set("stolen_sharingan_eye", 1);
                        pAttacker.a.addStatusEffect("weak_eye", 450f);
                    }
                }

                if (UnityEngine.Random.value < EyeStealChance && __instance.hasTrait("byakugan"))
                {
                    if (!pAttacker.a.hasTrait("byakugan"))
                    {
                        pAttacker.a.addTrait("byakugan");
                    }
                    pAttacker.a.data.set("stolen_byakugan_eye", 1);
                    pAttacker.a.addStatusEffect("weak_eye", 450f);
                }

                // EMS Awaken.
                if (__instance.hasTrait("mangekyo_sharingan") || __instance.hasTrait("trait_blind"))
                {
                    if (pAttacker.a.hasTrait("mangekyo_sharingan") || pAttacker.a.hasTrait("trait_blind"))
                    {
                        pAttacker.a.removeTrait("mangekyo_sharingan");
                        pAttacker.a.removeTrait("trait_blind");

                        pAttacker.a.addTrait("eternal_mangekyo");
                        pAttacker.a.restoreHealth(pAttacker.a.getMaxHealth());

                        ShinobiWorldLogs.AddWorldLog("log_awaken_ems", "worldlog_awaken_ems", "ui/icons/ems", pAttacker.a);
                    }

                }
            }
        }

        private static string GetStealableSharinganTier(Actor victim)
        {
            if (victim == null) return null;

            if (victim.hasTrait("madara_eternal_mangekyo")) return "madara_eternal_mangekyo";
            if (victim.hasTrait("eternal_mangekyo")) return "eternal_mangekyo";
            if (victim.hasTrait("mangekyo_sharingan")) return "mangekyo_sharingan";
            if (victim.hasStatus("status_sharingan_3t") || victim.hasStatus("status_sharingan_2t") || victim.hasStatus("status_sharingan_1t")) return "sharingan";
            if (victim.hasTrait("sharingan")) return "sharingan";

            return null;
        }

        private static void RemoveAllSharinganTiers(Actor actor)
        {
            if (actor == null) return;

            actor.finishStatusEffect("status_sharingan_1t");
            actor.finishStatusEffect("status_sharingan_2t");
            actor.finishStatusEffect("status_sharingan_3t");

            actor.removeTrait("mangekyo_sharingan");
            actor.removeTrait("madara_eternal_mangekyo");
            actor.removeTrait("eternal_mangekyo");
        }

    }
    #endregion

}