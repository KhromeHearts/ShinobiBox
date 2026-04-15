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


    #region Will Of Fire Patch
    [HarmonyPatch(typeof(Actor), "getHit")]
    public static class WillOfFireDeathPatch
    {
        [HarmonyPostfix]
        public static void Postfix(Actor __instance, float pDamage)
        {
            if (__instance == null || !__instance.isAlive()) return;

            if (!__instance.hasTrait("will_of_fire")) return;

            float healthPct = (float)__instance.data.health / __instance.getMaxHealth();
            if (healthPct < 0.01f)
            {
                if (__instance.city != null && __instance.current_tile != null)
                {
                    var nearby = Finder.getUnitsFromChunk(__instance.current_tile, 1);
                    foreach (var u in nearby)
                    {
                        if (u.a != null && u.a.isAlive() && u.a != __instance && u.a.city == __instance.city)
                        {
                            u.a.addStatusEffect("power_up");
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region Curse Of Hatred Patch
    [HarmonyPatch(typeof(Actor), "getHit")]
    public static class CurseOfHatredPatch
    {
        [HarmonyPostfix]
        public static void Postfix(Actor __instance, float pDamage)
        {
            if (__instance == null || !__instance.isAlive()) return;
            if (!__instance.hasTrait("curse_of_hatred")) return;

            float healthPct = (float)__instance.data.health / __instance.getMaxHealth();
            if (healthPct < 0.01f && __instance.current_tile != null)
            {
                var nearby = Finder.getUnitsFromChunk(__instance.current_tile, 1);
                foreach (var u in nearby)
                {
                    if (u.a == null || !u.a.isAlive() || u.a == __instance) continue;

                    bool isEnemy = false;
                    if (__instance.city != null && u.a.city != __instance.city) isEnemy = true;
                    else if (__instance.city == null && __instance.kingdom != null && u.a.kingdom != __instance.kingdom) isEnemy = true;

                    if (isEnemy)
                    {
                        u.a.addStatusEffect("cursed", 10f);
                    }
                }
            }
        }
    }
    #endregion

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

    #region Baryon and Incomplete Beast Awaken
    [HarmonyPatch(typeof(Actor), "getHit")]
    public static class LastStandPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(Actor __instance, float pDamage)
        {
            if (__instance == null || !__instance.isAlive()) return true;

            bool wouldDie = (__instance.data.health - pDamage) <= 0f;
            if (!wouldDie) return true;

            bool hasKcmOrHigher =
                __instance.hasStatus("status_jinchuriki_kcm2") ||
                __instance.hasStatus("status_jinchuriki_avatar");

            if (!hasKcmOrHigher) return true;
            if (__instance.hasStatus("status_jinchuriki_baryon_mode")) return true;
            if (UnityEngine.Random.value > 0.30f) return true;

            ShinobiProgression.RemoveLowerForms(__instance);
            __instance.addStatusEffect("status_jinchuriki_baryon_mode", 60f);
            __instance.restoreHealth(__instance.getMaxHealth());

            if (ShinobiConfig.EnableWorldTips)
            {
                ShinobiWorldLogs.AddWorldLog("log_baryon_mode", "worldlog_baryon_mode", "ui/icons/baryon_mode", __instance);
            }

            return false;
        }
    }
    #endregion

    #region Mangekyo To EMS
    // Mangekyo To EMS
    [HarmonyPatch(typeof(Actor), "getHit")]
    public static class EMSKillPatch
    {
        private const float EyeStealChance = 0.001f;

        [HarmonyPrefix]
        public static void Prefix(Actor __instance, float pDamage, bool pFlash, AttackType pAttackType, BaseSimObject pAttacker)
        {
            if (__instance == null || !__instance.isAlive() || pAttacker == null || pAttacker.a == null) return;

            if (__instance.data.health - pDamage <= 0)
            {
                bool killedHashiramaActor =
                    __instance.asset != null &&
                    __instance.asset.id == ShinobiActors.HashiramaActorId;
                bool killedMadaraActor =
                    __instance.asset != null &&
                    __instance.asset.id == ShinobiActors.MadaraActorId;
                bool killedKuramaActor =
                    __instance.asset != null &&
                    __instance.asset.id == ShinobiActors.KuramaActorId;
                bool killedJuubiActor =
                    __instance.asset != null &&
                    __instance.asset.id == ShinobiActors.JuubiActorId;

                // Killing Hashirama always grants either Hashirama Cells or Warring States armor.
                if (killedHashiramaActor)
                {
                    bool gaveReward = false;

                    if (UnityEngine.Random.value < 0.5f)
                    {
                        if (!pAttacker.a.hasTrait("hashi_cells"))
                        {
                            pAttacker.a.addTrait("hashi_cells");
                            gaveReward = true;
                        }
                    }

                    if (!gaveReward)
                    {
                        gaveReward = ShinobiItems.EquipItem(pAttacker.a, "armor_warring_states");
                    }

                    if (!gaveReward)
                    {
                        ShinobiItems.EquipItem(pAttacker.a, "armor_warring_states");
                    }
                }

                // Killing Madara always grants one reward: armor, gunbai, or Madara's Eternal Mangekyo.
                if (killedMadaraActor)
                {
                    int rewardRoll = UnityEngine.Random.Range(0, 3);
                    bool gaveMadaraReward = false;

                    if (rewardRoll == 0)
                    {
                        gaveMadaraReward = ShinobiItems.EquipItem(pAttacker.a, "weapon_gunbai");
                    }
                    else if (rewardRoll == 1)
                    {
                        if (!pAttacker.a.hasTrait("madara_eternal_mangekyo"))
                        {
                            pAttacker.a.addTrait("madara_eternal_mangekyo");
                            gaveMadaraReward = true;
                        }
                    }
                    else
                    {
                        gaveMadaraReward = ShinobiItems.EquipItem(pAttacker.a, "armor_warring_states");
                    }

                    if (!gaveMadaraReward)
                    {
                        gaveMadaraReward = ShinobiItems.EquipItem(pAttacker.a, "weapon_gunbai");
                    }
                    if (!gaveMadaraReward)
                    {
                        gaveMadaraReward = ShinobiItems.EquipItem(pAttacker.a, "armor_warring_states");
                    }
                    if (!gaveMadaraReward && !pAttacker.a.hasTrait("madara_eternal_mangekyo"))
                    {
                        pAttacker.a.addTrait("madara_eternal_mangekyo");
                    }

                    pAttacker.a.removeTrait("mangekyo_sharingan");
                    pAttacker.a.removeTrait("eternal_mangekyo");
                    if (!pAttacker.a.hasTrait("madara_eternal_mangekyo"))
                    {
                        pAttacker.a.addTrait("madara_eternal_mangekyo");
                    }
                }

                if (killedKuramaActor)
                {
                    if (!pAttacker.a.hasTrait("nine_tails_jinchuriki"))
                    {
                        pAttacker.a.addTrait("nine_tails_jinchuriki");
                        pAttacker.a.restoreHealth(pAttacker.a.getMaxHealth());
                    }
                }

                if (killedJuubiActor)
                {
                    if (!pAttacker.a.hasTrait("ten_tails_jinchuriki"))
                    {
                        pAttacker.a.addTrait("ten_tails_jinchuriki");
                        pAttacker.a.restoreHealth(pAttacker.a.getMaxHealth());
                    }
                }

                if (UnityEngine.Random.value < EyeStealChance)
                {
                    string stolenSharinganTier = GetStealableSharinganTier(__instance);
                    if (!string.IsNullOrEmpty(stolenSharinganTier))
                    {
                        RemoveAllSharinganTiers(pAttacker.a);
                        pAttacker.a.addTrait(stolenSharinganTier);
                        pAttacker.a.removeTrait("trait_blind");
                        pAttacker.a.data.set("stolen_sharingan_eye", 1);
                        pAttacker.a.addStatusEffect("weak_eye", 999f);
                    }
                }

                if (UnityEngine.Random.value < EyeStealChance && __instance.hasTrait("byakugan"))
                {
                    if (!pAttacker.a.hasTrait("byakugan"))
                    {
                        pAttacker.a.addTrait("byakugan");
                    }
                    pAttacker.a.data.set("stolen_byakugan_eye", 1);
                    pAttacker.a.addStatusEffect("weak_eye", 999f);
                }

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
            if (victim.hasTrait("sharingan_3t")) return "sharingan_3t";
            if (victim.hasTrait("sharingan_2t")) return "sharingan_2t";
            if (victim.hasTrait("sharingan_1t")) return "sharingan_1t";

            return null;
        }

        private static void RemoveAllSharinganTiers(Actor actor)
        {
            if (actor == null) return;

            actor.removeTrait("sharingan_1t");
            actor.removeTrait("sharingan_2t");
            actor.removeTrait("sharingan_3t");
            actor.removeTrait("mangekyo_sharingan");
            actor.removeTrait("madara_eternal_mangekyo");
            actor.removeTrait("eternal_mangekyo");
        }

    }
    #endregion

    #region KamuiPhasePatch
    [HarmonyPatch(typeof(Actor), "getHit")]
    public static class KamuiPhasePatch
    {
        [HarmonyPrefix]
        public static bool Prefix(Actor __instance)
        {
            if (__instance == null || !__instance.isAlive()) return true;

            if (__instance.hasStatus("status_kamui_phase")) return false;

            if (__instance.hasTrait("eternal_mangekyo") || __instance.hasTrait("mangekyo_sharingan"))
            {
                if (UnityEngine.Random.value < 0.03f)
                {
                    __instance.addStatusEffect("status_kamui_phase");
                    __instance.addStatusEffect("kamui_cooldown", 25f);
                    JutsuLibrary.AddNatureProgressExp(__instance, 10f);
                    int currentUsage = 0;
                    __instance.a.data.get("ms_usage", out currentUsage);
                    __instance.a.data.set("ms_usage", currentUsage + 1);

                    return false;
                }
            }
            return true;
        }
    }
    #endregion

}