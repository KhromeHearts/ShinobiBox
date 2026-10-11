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

    [HarmonyPatch(typeof(Actor), "getHit")]
    public static class rasenganFinish
    {
        static void Postfix(Actor __instance, BaseSimObject pAttacker)
        {
            if (pAttacker != null && pAttacker.a != null && pAttacker.a.hasStatus("status_rasengan"))
            {
                pAttacker.a.finishStatusEffect("status_rasengan");
            }
        }
    }

    [HarmonyPatch(typeof(BabyMaker), "makeBaby")]
    public static class HagoromoBirthPatch
    {
        [HarmonyPostfix]
        public static void Postfix(Actor __result, Actor pParent1, Actor pParent2)
        {
            if (__result == null || __result.asset == null || __result.asset.id != "human") return;
            if (__result.hasTrait("07_indra_chakra") || __result.hasTrait("08_asura_chakra")) return;

            bool parentHasHagoromo =
                (pParent1 != null && pParent1.hasTrait("05_hagoromo_chakra")) ||
                (pParent2 != null && pParent2.hasTrait("05_hagoromo_chakra"));

            if (!parentHasHagoromo) return;

            if (UnityEngine.Random.value < 0.5f)
            {
                __result.addTrait("07_indra_chakra");
            }
            else
            {
                __result.addTrait("08_asura_chakra");
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

            if (__result.hasTrait("otsutsuki_clan") && !__result.hasTrait("06_hamura_chakra") && !__result.hasTrait("05_hagoromo_chakra"))
            {
                if (Randy.randomChance(0.01f))
                {
                    __result.addTrait("05_hagoromo_chakra");
                }
                else if (Randy.randomChance(0.01f))
                {
                    __result.addTrait("06_hamura_chakra");
                }
            }

            if (__result.hasTrait("otsutsuki_clan"))
            {
                float roll = UnityEngine.Random.value;

                if (roll < 0.05f)
                {
                    __result.addTrait("rinnegan");
                }
                else if (roll < 0.20f)
                {
                    __result.addTrait("golden_byakugan");
                }
                else if (roll < 0.30f && __result.hasTrait("06_hamura_chakra"))
                {
                    __result.addTrait("tenseigan");
                }
                else
                {
                    __result.addTrait("byakugan");
                }
            }

            // NOTE: the clan birth rules below used to be nested INSIDE the Otsutsuki check above.
            // Clans are mutually exclusive, so an Otsutsuki baby could never also be Chinoike/Hyuga/Lee/etc.
            // and none of these rules could ever run. They now run for the matching clan.
            //
            // The old Senju block (5% wood_release / 0.1% hashi_cells) was removed from here on purpose:
            // BirthTraits.ApplyTraitsOnBirth already gives Senju babies a 1% chance of each,
            // and moving the old block out would have quietly made Senju stronger.

            if (!__result.hasTrait("ketsuryugan") && __result.hasTrait("chinoike_clan"))
            {
                if (Randy.randomChance(0.50f))
                {
                    __result.addTrait("ketsuryugan");
                }
            }

            if (!__result.hasTrait("byakugan") && __result.hasTrait("hyuga_clan"))
            {
                __result.addTrait("byakugan");
            }

            if (!__result.hasTrait("eight_inner_gates") && __result.hasTrait("lee_clan"))
            {
                if (Randy.randomChance(0.20f))
                {
                    __result.addTrait("eight_inner_gates");
                }
            }

            if (!__result.hasTrait("shikotsumyaku") && __result.hasTrait("kaguya_clan"))
            {
                if (Randy.randomChance(0.05f))
                {
                    __result.addTrait("shikotsumyaku");
                }
            }

            if (!__result.hasTrait("sharingan") && __result.hasTrait("uchiha_clan"))
            {
                if (Randy.randomChance(0.01f))
                {
                    __result.addTrait("sharingan");
                }
            }
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

    // creds to clonedschrott for this code
    #region Unit Maker
    [HarmonyPatch(typeof(Actor), "generatePersonality")]
    public static class GeneratePersonalityPatch
    {
        [HarmonyPostfix]
        public static void generatePersonality_Postfix(Actor __instance)
        {
            if (__instance.hasTrait("windN") && !__instance.hasTrait("rasenganJ") && Randy.randomChance(0.15f))
                __instance.addTrait("rasenganJ");

            if (__instance.hasTrait("lightningN"))
            {
                if (!__instance.hasTrait("chidoriJ") && Randy.randomChance(0.15f))
                {
                    __instance.addTrait("chidoriJ");
                }
            }

            if (__instance.data["birth_traits_assigned"] < 0.5f)
            {
                BirthTraits.AssignChakraTrait(__instance);
                BirthTraits.ApplyTraitsOnBirth(__instance);

                try { __instance.data.set("birth_traits_assigned", 1); } catch { }
            }
        }
    }
    #endregion

    #region Birth Traits
    public static class BirthTraits
    {
        private static readonly string[] ChakraReserveTraits =
        {
            "01_low_chakra_reserve",
            "02_high_chakra_reserve",
            "03_greater_chakra_reserve",
            "04_vast_chakra_reserve"
        };

        public static void AssignChakraTrait(Actor actor)
        {
            if (actor == null || !actor.isSapient()) return;

            foreach (string traitId in ChakraReserveTraits)
            {
                if (actor.hasTrait(traitId)) return;
            }

            float roll = UnityEngine.Random.value;
            string selectedTrait = null;
            if (roll < 0.10f) selectedTrait = "04_vast_chakra_reserve";
            else if (roll < 0.30f) selectedTrait = "03_greater_chakra_reserve";
            else if (roll < 0.65f) selectedTrait = "02_high_chakra_reserve";
            else if (roll < 0.90f) selectedTrait = "01_low_chakra_reserve";

            if (!string.IsNullOrEmpty(selectedTrait)) actor.addTrait(selectedTrait);
        }

        public static void ApplyTraitsOnBirth(Actor actor)
        {
            if (!actor.isSapient()) return;

            string[] commonElements = { "fireN", "earthN", "waterN", "windN", "lightningN" };
            int elementCount = 0;
            foreach (var element in commonElements)
            {
                if (actor.hasTrait(element)) continue;
                float chance = elementCount >= 3 ? 0.05f : 0.30f;
                if (Randy.randomChance(chance))
                {
                    actor.addTrait(element);
                    elementCount++;
                }
            }

            if (!actor.hasTrait("stormR"))
            {
                float stormChance = elementCount >= 3 ? 0.02f : 0.10f;
                if (Randy.randomChance(stormChance))
                    actor.addTrait("stormR");
            }

            if (Randy.randomChance(0.15f))
            {
                actor.addTrait("01rank_academy_student");
            }

            if (actor.hasTrait("senju_clan"))
            {
                if (!actor.hasTrait("hashi_cells") && Randy.randomChance(0.01f))
                {
                    actor.addTrait("hashi_cells");
                    if (ShinobiConfig.EnableWorldTips)
                    {
                        ShinobiWorldLogs.AddWorldLog("log_hashi_cells", "worldlog_hashi_cells", "ui/icons/hashirama", actor, "spawn");
                    }
                }

                if (!actor.hasTrait("wood_release") && Randy.randomChance(0.01f))
                {
                    actor.addTrait("wood_release");
                    if (ShinobiConfig.EnableWorldTips)
                    {
                        ShinobiWorldLogs.AddWorldLog("log_wood_release", "worldlog_wood_release", "ui/icons/wood_release", actor, "spawn");
                    }
                }
            }


            if (actor.hasTrait("otsutsuki_clan"))
            {
                if (!actor.hasTrait("05_hagoromo_chakra") && !actor.hasTrait("06_hamura_chakra"))
                {
                    actor.addTrait(UnityEngine.Random.value < 0.5f ? "05_hagoromo_chakra" : "06_hamura_chakra");
                }

                if (!actor.hasTrait("byakugan") && !actor.hasTrait("rinnegan") && Randy.randomChance(0.35f))
                {
                    actor.addTrait(actor.hasTrait("06_hamura_chakra") ? "byakugan" : "rinnegan");
                }
            }
        }
    }
    #endregion

}
