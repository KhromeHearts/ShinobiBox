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
    public static class ChakraSystem
    {
        private static readonly Dictionary<long, ChakraState> chakraData = new Dictionary<long, ChakraState>();

        public static void RegisterChakraBaseStat()
        {
            if (AssetManager.base_stats_library.get("chakra") == null)
            {
                AssetManager.base_stats_library.add(new BaseStatAsset
                {
                    id = "chakra",
                    icon = "ui/icons/staticon",
                    normalize = true,
                    normalize_min = 0f,
                    sort_rank = 895,
                    translation_key = "chakra"
                });
            }

            if (AssetManager.base_stats_library.get("multiplier_chakra") == null)
            {
                AssetManager.base_stats_library.add(new BaseStatAsset
                {
                    id = "multiplier_chakra",
                    show_as_percents = true,
                    multiplier = true,
                    main_stat_to_multiply = "chakra",
                    tooltip_multiply_for_visual_number = 100f,
                    translation_key = "chakra"
                });
            }
        }

        public static ChakraState Get(Actor actor)
        {
            if (actor == null) return null;
            long id = actor.data.id;

            if (!chakraData.TryGetValue(id, out var state))
            {
                state = InitChakra(actor);
                chakraData[id] = state;
            }
            return state;
        }

        public static float GetCurrent(Actor actor) => Get(actor)?.current ?? 0f;
        public static float GetMax(Actor actor) => Get(actor)?.max ?? 0f;

        public static bool IsChakraUser(Actor actor)
        {
            if (actor == null || actor.data == null || !actor.isAlive()) return false;

            float chakraStat = GetActorStatValue(actor, "chakra");
            float chakraMultiplier = GetActorStatValue(actor, "multiplier_chakra");
            if (chakraStat > 0f || chakraMultiplier > 0f) return true;

            if (chakraData.TryGetValue(actor.data.id, out var state))
            {
                return state.current > 0f || state.max > 0f;
            }

            return false;
        }

        public static float GetLastMoveTime(Actor actor)
        {
            return Get(actor)?.lastMoveTime ?? -7f;
        }

        public static void SetLastMoveTime(Actor actor, float time)
        {
            var chakra = Get(actor);
            if (chakra != null) chakra.lastMoveTime = time;
        }

        public static float GetRegenRate(Actor actor)
        {
            if (!IsChakraUser(actor)) return 0f;

            float rate = 1.5f;
            rate += actor.data.level * 0.35f;
            rate += Mathf.Sqrt(Mathf.Max(0f, CalculateBaseMaxChakra(actor))) * 0.08f;

            if (actor.hasStatus("status_perfect_frog_sage") || actor.hasStatus("status_perfect_slug_sage") || actor.hasStatus("status_perfect_snake_sage"))
            {
                rate *= 1.20f;
            }

            if (actor.hasStatus("chakra_exhaustion"))
            {
                rate *= 0.25f;
            }

            return Mathf.Max(0.1f, rate);
        }

        // Higher Ranks will Reduce the cost of moves
        public static float GetAbilityCostMultiplier(Actor actor)
        {
            if (actor == null) return 1f;

            if (actor.hasTrait("rank_god_of_shinobi")) return 0.70f;
            if (actor.hasTrait("rank_ghost_of_uchiha")) return 0.75f;
            if (actor.hasTrait("rank_sanin")) return 0.85f;
            if (actor.hasTrait("rank_kage")) return 0.90f;
            if (actor.hasTrait("rank_anbu")) return 0.95f;
            if (actor.hasTrait("rank_jonin")) return 1.00f;
            if (actor.hasTrait("rank_chunin")) return 1.10f;
            if (actor.hasTrait("rank_genin")) return 1.25f;
            if (actor.hasTrait("rank_academy_student")) return 1.40f;

            return 1.15f;
        }

        public static void SetCurrent(Actor actor, float value)
        {
            var chakra = Get(actor);
            if (chakra != null) chakra.current = Mathf.Clamp(value, 0f, chakra.max);
        }

        public static void AddChakra(Actor actor, float amount)
        {
            var chakra = Get(actor);
            if (chakra != null) chakra.current = Mathf.Clamp(chakra.current + amount, 0f, chakra.max);
        }

        public static void RemoveChakra(Actor actor, float amount)
        {
            var chakra = Get(actor);
            if (chakra != null) chakra.current = Mathf.Clamp(chakra.current - Mathf.Abs(amount), 0f, chakra.max);
        }

        public static bool TryConsume(Actor actor, float cost)
        {
            if (actor == null || !actor.isAlive()) return false;
            if (!IsChakraUser(actor)) return false;
            if (cost <= 0f) return true;

            var chakra = Get(actor);
            if (chakra == null) return false;

            // Indra's Chakra trait reduces all costs by 40%, making it cheaper to use abilities but decreases overall chakra pool
            if (actor.hasTrait("indra_chakra"))
            {
                cost *= 0.4f;
            }

            cost = Mathf.Max(0f, cost);

            if (chakra.current + 0.001f < cost) return false;

            chakra.current = Mathf.Max(0f, chakra.current - cost);
            return true;
        }

        public static void UpdateMax(Actor actor)
        {
            if (actor == null) return;
            var chakra = Get(actor);
            if (chakra != null)
            {
                float oldMax = chakra.max;
                chakra.max = CalculateBaseMaxChakra(actor);

                if (chakra.max > oldMax)
                {
                    chakra.current = chakra.max;
                }

                else if (chakra.current > chakra.max)
                {
                    chakra.current = chakra.max;
                }
            }
        }

        private static ChakraState InitChakra(Actor actor)
        {
            float max = CalculateBaseMaxChakra(actor);

            return new ChakraState
            {
                actor = actor,
                max = max,
                current = max,
                lastMoveTime = -7f
            };
        }

        public static float CalculateBaseMaxChakra(Actor actor)
        {
            if (actor == null || actor.data == null) return 0f;
            float traitChakra = GetActorStatValue(actor, "chakra");
            float traitChakraMultiplier = GetActorStatValue(actor, "multiplier_chakra");
            if (traitChakra <= 0f && traitChakraMultiplier <= 0f) return 0f;

            float total = 25f;
            total += traitChakra;
            total += actor.data.level * 30f;
            total += actor.data.kills * 10f;

            float multiplier = 1.0f + traitChakraMultiplier;

            if (actor.hasTrait("frog_sage_mode") && actor.hasStatus("status_perfect_frog_sage")) multiplier += 0.20f;
            if (actor.hasTrait("slug_sage_mode") && actor.hasStatus("status_perfect_slug_sage")) multiplier += 0.20f;

            float tsukuyomiBonusMultiplier = 0f;
            actor.data.get("it_bonus_multiplier", out tsukuyomiBonusMultiplier);
            if (tsukuyomiBonusMultiplier > 0f)
            {
                multiplier += tsukuyomiBonusMultiplier;
            }

            if (actor.hasStatus("chakra_exhaustion")) multiplier *= 0.1f;

            int stoleSharinganEye = 0;
            actor.data.get("stolen_sharingan_eye", out stoleSharinganEye);
            if (stoleSharinganEye > 0)
            {
                multiplier *= 0.90f;
            }

            int stoleByakuganEye = 0;
            actor.data.get("stolen_byakugan_eye", out stoleByakuganEye);
            if (stoleByakuganEye > 0)
            {
                multiplier *= 0.95f;
            }

            multiplier = Mathf.Max(0f, multiplier);
            total *= multiplier;

            return Mathf.Max(0f, total);
        }

        private static float GetActorStatValue(Actor actor, string statId)
        {
            try
            {
                return actor.stats[statId];
            }
            catch
            {
                return 0f;
            }
        }

        public static void ProcessDrain(Actor actor, float dt)
        {
            if (actor == null || !actor.isAlive()) return;

            int msUsage = 0;
            actor.data.get("ms_usage", out msUsage);
            if (msUsage >= 25 && actor.hasTrait("mangekyo_sharingan"))
            {
                actor.removeTrait("mangekyo_sharingan");
                actor.addTrait("trait_blind");
            }

            bool drainsChakra = false;
            float drainPerSecond = 0f;

            if (actor.hasStatus("status_jinchuriki_initial_release")) { drainPerSecond = 2f; drainsChakra = true; }
            else if (actor.hasStatus("status_jinchuriki_v1_cloak")) { drainPerSecond = 5f; drainsChakra = true; }
            else if (actor.hasStatus("status_jinchuriki_v2_cloak")) { drainPerSecond = 10f; drainsChakra = true; }
            else if (actor.hasStatus("status_jinchuriki_incomplete_beast")) { drainPerSecond = 20f; drainsChakra = true; }
            else if (actor.hasStatus("status_jinchuriki_kcm1")) { drainPerSecond = 25f; drainsChakra = true; }
            else if (actor.hasStatus("status_jinchuriki_kcm2")) { drainPerSecond = 32f; drainsChakra = true; }
            else if (actor.hasStatus("status_jinchuriki_avatar")) { drainPerSecond = 40f; drainsChakra = true; }
            else if (actor.hasStatus("status_jinchuriki_baryon_mode")) { drainPerSecond = 60f; drainsChakra = true; }

            if (actor.hasStatus("status_sharingan_3t")) { drainPerSecond += 1f; drainsChakra = true; }
            if (actor.hasTrait("mangekyo_sharingan")) { drainPerSecond += 3f; drainsChakra = true; }
            if (actor.hasTrait("rinnegan")) { drainPerSecond += 5f; drainsChakra = true; }

            if (actor.hasStatus("status_base_susanoo")) { drainPerSecond += 15f; drainsChakra = true; }
            if (actor.hasStatus("status_perfect_susanoo")) { drainPerSecond += 30f; drainsChakra = true; }

            if (!drainsChakra || drainPerSecond <= 0) return;

            float actualDrain = drainPerSecond * dt;
            bool hasEnoughChakra = ChakraSystem.TryConsume(actor, actualDrain);

            if (!hasEnoughChakra)
            {
                if (!actor.hasTrait("taijutsu_master") && !actor.hasStatus("chakra_exhaustion"))
                {
                    actor.addStatusEffect("chakra_exhaustion", 25f);
                }
            }
        }

        public static void CleanDeadActors()
        {
            var toRemove = new List<long>();
            foreach (var kvp in chakraData)
            {
                if (kvp.Value.actor == null || !kvp.Value.actor.isAlive())
                    toRemove.Add(kvp.Key);
            }
            foreach (long id in toRemove) chakraData.Remove(id);
        }
    }

    public class ChakraState
    {
        public Actor actor;
        public float current;
        public float max;
        public float lastMoveTime;
    }
}