using System;
using UnityEngine;
using ReflectionUtility;

namespace ShinobiBox
{
    public static class ChakraSystem
    {
        private const string TailedBeastChakraKey = "tailed_beast_chakra";
        private const string SageChakraKey = "sage_chakra";

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

            if (AssetManager.base_stats_library.get("tailed_beast_chakra") == null)
            {
                AssetManager.base_stats_library.add(new BaseStatAsset
                {
                    id = "tailed_beast_chakra",
                    icon = "ui/icons/Seal",
                    normalize = true,
                    normalize_min = 0f,
                    sort_rank = 896,
                    translation_key = "tailed_beast_chakra"
                });
            }

            if (AssetManager.base_stats_library.get("sage_chakra") == null)
            {
                AssetManager.base_stats_library.add(new BaseStatAsset
                {
                    id = "sage_chakra",
                    icon = "ui/icons/staticon",
                    normalize = true,
                    normalize_min = 0f,
                    sort_rank = 897,
                    translation_key = "sage_chakra"
                });
            }

            if (AssetManager.base_stats_library.get("chakra_regen") == null)
            {
                AssetManager.base_stats_library.add(new BaseStatAsset
                {
                    id = "chakra_regen",
                    icon = "ui/icons/staticon",
                    normalize = false,
                    sort_rank = 898,
                    translation_key = "chakra_regen"
                });
            }

            if (AssetManager.base_stats_library.get("sage_chakra_regen") == null)
            {
                AssetManager.base_stats_library.add(new BaseStatAsset
                {
                    id = "sage_chakra_regen",
                    icon = "ui/icons/staticon",
                    normalize = false,
                    sort_rank = 899,
                    translation_key = "sage_chakra_regen"
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

        public static float GetMax(Actor actor)
        {
            return CalculateBaseMaxChakra(actor);
        }

        public static float GetCurrent(Actor actor)
        {
            if (actor == null || actor.data == null) return 0f;

            if (actor.data.custom_data_float == null || !actor.data.custom_data_float.TryGetValue("current_chakra", out float current))
            {
                float max = GetMax(actor);
                actor.data.set("current_chakra", max);
                return max;
            }

            return current;
        }

        public static void SetCurrent(Actor actor, float value)
        {
            if (actor == null || actor.data == null) return;
            float max = GetMax(actor);
            actor.data.set("current_chakra", Mathf.Clamp(value, 0f, max));
        }

        public static float GetMaxTailedBeastChakra(Actor actor)
        {
            if (actor == null || !actor.isAlive()) return 0f;
            if (actor.hasTrait("ten_tails_jinchuriki")) return 1000f;
            if (actor.hasTrait("nine_tails_jinchuriki")) return 500f;
            return 0f;
        }

        public static float GetTailedBeastChakra(Actor actor)
        {
            return GetResource(actor, TailedBeastChakraKey, GetMaxTailedBeastChakra(actor));
        }

        public static void SetTailedBeastChakra(Actor actor, float value)
        {
            SetResource(actor, TailedBeastChakraKey, value, GetMaxTailedBeastChakra(actor));
        }

        public static void AddTailedBeastChakra(Actor actor, float amount)
        {
            SetTailedBeastChakra(actor, GetTailedBeastChakra(actor) + amount);
        }

        public static float GetMaxSageChakra(Actor actor)
        {
            if (actor == null || !actor.isAlive()) return 0f;
            if (actor.hasTrait("frog_sage_mode") || actor.hasTrait("snake_sage_mode") ||
                actor.hasTrait("slug_sage_mode") || actor.hasTrait("wood_sage_mode")) return 300f;
            return 0f;
        }

        public static float GetSageChakra(Actor actor)
        {
            return GetResource(actor, SageChakraKey, GetMaxSageChakra(actor));
        }

        public static void SetSageChakra(Actor actor, float value)
        {
            SetResource(actor, SageChakraKey, value, GetMaxSageChakra(actor));
        }

        public static void AddSageChakra(Actor actor, float amount)
        {
            SetSageChakra(actor, GetSageChakra(actor) + amount);
        }

        public static bool TryConsumeSageChakra(Actor actor, float amount)
        {
            if (amount <= 0f) return true;
            float current = GetSageChakra(actor);
            if (current < amount) return false;
            SetSageChakra(actor, current - amount);
            return true;
        }

        public static void UpdateSpecialResources(Actor actor, float deltaTime)
        {
            if (actor == null || !actor.isAlive()) return;

            float tailedBeastMax = GetMaxTailedBeastChakra(actor);
            if (tailedBeastMax > 0f)
            {
                float regeneration = tailedBeastMax * 0.01f * deltaTime;
                AddTailedBeastChakra(actor, regeneration);
            }

            float sageMax = GetMaxSageChakra(actor);
            if (sageMax > 0f)
            {
                float regenerationRate = GetActorStatValue(actor, "sage_chakra_regen");
                if (regenerationRate <= 0f)
                {
                    regenerationRate = 6f;
                }

                float regeneration = regenerationRate * deltaTime;
                AddSageChakra(actor, regeneration);
            }
        }

        private static float GetResource(Actor actor, string key, float max)
        {
            if (actor?.data == null || max <= 0f) return 0f;
            if (!actor.data.custom_data_float.TryGetValue(key, out float current))
            {
                current = max;
                actor.data.set(key, current);
            }

            float clamped = Mathf.Clamp(current, 0f, max);
            if (!Mathf.Approximately(current, clamped)) actor.data.set(key, clamped);
            return clamped;
        }

        private static void SetResource(Actor actor, string key, float value, float max)
        {
            if (actor?.data == null || max <= 0f) return;
            actor.data.set(key, Mathf.Clamp(value, 0f, max));
        }

        public static bool IsChakraUser(Actor actor)
        {
            if (actor == null || actor.data == null || !actor.isAlive()) return false;
            return GetMax(actor) > 0f; 
        }

        public static float GetRegenRate(Actor actor)
        {
            if (!IsChakraUser(actor)) return 0f;

            float baseRate = 0.45f;
            baseRate += actor.data.level * 0.10f;
            baseRate += Mathf.Min(6f, GetMax(actor) * 0.004f);
            baseRate += GetActorStatValue(actor, "chakra_regen");

            if (actor.hasStatus("status_frog_sage") || actor.hasStatus("status_slug_sage") || actor.hasStatus("status_snake_sage"))
            {
                baseRate *= 1.35f;
            }

            if (actor.hasStatus("chakra_exhaustion"))
            {
                baseRate *= 0.35f;
            }

            return Mathf.Clamp(baseRate, 0.15f, 12f);
        }

        public static float GetAbilityCostMultiplier(Actor actor)
        {
            if (actor == null) return 1f;

            if (actor.hasTrait("09rank_god_of_shinobi")) return 0.84f;
            if (actor.hasTrait("08rank_ghost_of_uchiha")) return 0.88f;
            if (actor.hasTrait("07rank_sannin")) return 0.90f;
            if (actor.hasTrait("06rank_kage")) return 0.92f;
            if (actor.hasTrait("05rank_anbu")) return 0.96f;
            if (actor.hasTrait("04rank_jonin")) return 1.00f;
            if (actor.hasTrait("03rank_chunin")) return 1.05f;
            if (actor.hasTrait("02rank_genin")) return 1.10f;
            if (actor.hasTrait("01rank_academy_student")) return 1.18f;

            return 1.22f;
        }

        public static void AddChakra(Actor actor, float amount)
        {
            float current = GetCurrent(actor);
            SetCurrent(actor, current + amount);
        }

        public static void RemoveChakra(Actor actor, float amount)
        {
            float current = GetCurrent(actor);
            SetCurrent(actor, current - Mathf.Abs(amount));
        }

        public static bool TryConsume(Actor actor, float cost)
        {
            if (actor == null || !actor.isAlive() || !IsChakraUser(actor)) return false;
            if (actor.hasStatus("chakra_exhaustion")) return false;
            if (cost <= 0f) return true;

            if (actor.hasTrait("07_indra_chakra"))
            {
                cost *= 0.4f;
            }

            cost = Mathf.Max(0f, cost);
            float current = GetCurrent(actor);

            if (current + 0.001f < cost) return false;

            SetCurrent(actor, current - cost);
            return true;
        }

        public static void UpdateMax(Actor actor)
        {
            if (actor == null || actor.data == null) return;
            
            float max = GetMax(actor);
            float current = GetCurrent(actor);
            
            if (current > max)
            {
                SetCurrent(actor, max);
            }
        }

        public static float CalculateBaseMaxChakra(Actor actor)
        {
            if (actor == null || actor.data == null) return 0f;
            float traitChakra = GetActorStatValue(actor, "chakra");
            float traitChakraMultiplier = GetActorStatValue(actor, "multiplier_chakra");

            float birthChakra = 0f;
            if (actor.data.custom_data_float != null && actor.data.custom_data_float.TryGetValue("birth_chakra", out float savedChakra))
            {
                birthChakra = savedChakra;
            }
            else
            {
                birthChakra = OnBirthChakra();
                actor.data.set("birth_chakra", birthChakra);
            }

            float total = birthChakra;
            total += traitChakra;
            total += actor.data.level * 42f;
            total += actor.data.kills * 10f;

            total = Mathf.Min(total, 4200f);

            float multiplier = 1.0f + traitChakraMultiplier;

            if (actor.hasTrait("frog_sage_mode") && actor.hasStatus("status_frog_sage")) multiplier += 0.20f;
            if (actor.hasTrait("slug_sage_mode") && actor.hasStatus("status_slug_sage")) multiplier += 0.20f;

            float tsukuyomiBonusMultiplier = 0f;
            actor.data.get("it_bonus_multiplier", out tsukuyomiBonusMultiplier);
            if (tsukuyomiBonusMultiplier > 0f) multiplier += tsukuyomiBonusMultiplier;

            int stoleSharinganEye = 0;
            actor.data.get("stolen_sharingan_eye", out stoleSharinganEye);
            if (stoleSharinganEye > 0) multiplier *= 0.90f;

            int stoleByakuganEye = 0;
            actor.data.get("stolen_byakugan_eye", out stoleByakuganEye);
            if (stoleByakuganEye > 0) multiplier *= 0.95f;

            multiplier = Mathf.Max(0f, multiplier);
            total *= multiplier;

            return Mathf.Max(0f, total);
        }

        private static float OnBirthChakra()
        {
            float roll = Randy.randomFloat(0f, 1f);

            if (roll < 0.02f) 
            {
                return Randy.randomFloat(600f, 1200f);
            }
            else if (roll < 0.25f) 
            {
                return Randy.randomFloat(250f, 600f);
            }
            else if (roll < 0.50f)
            {
                return Randy.randomFloat(0f, 250f);
            }

            return 0f;
        }

        private static float GetActorStatValue(Actor actor, string statId)
        {
            try { return actor.stats[statId]; }
            catch { return 0f; }
        }

        public static void AddMangekyoUsage(Actor actor, int amount = 1)
        {
            if (actor == null || !actor.hasTrait("mangekyo_sharingan")) return;

            int currentUsage = 0;
            actor.data.get("ms_usage", out currentUsage);
            int newUsage = currentUsage + amount;
            actor.data.set("ms_usage", newUsage);

            if (newUsage >= 25)
            {
                actor.removeTrait("mangekyo_sharingan");
                actor.addTrait("trait_blind");
            }
        }


        public static void ProcessDrain(Actor actor, float dt)
        {
            if (actor == null || !actor.isAlive()) return;

            AddMangekyoUsage(actor, 0);

            bool drainsChakra = false;
            float drainPerSecond = 0f;

            // Nine Tails Jinchuriki
            if (actor.hasStatus("status_jinchuriki_initial_release")) { drainPerSecond += 1f; drainsChakra = true; }
            if (actor.hasStatus("status_jinchuriki_v1_cloak")) { drainPerSecond += 2f; drainsChakra = true; }
            if (actor.hasStatus("status_jinchuriki_v2_cloak")) { drainPerSecond += 4f; drainsChakra = true; }
            if (actor.hasStatus("status_jinchuriki_incomplete_beast")) { drainPerSecond += 8f; drainsChakra = true; }
            if (actor.hasStatus("status_jinchuriki_kcm1")) { drainPerSecond += 10f; drainsChakra = true; }

            float eyeDrain = 0f;
            bool hasActiveEyeStatus = false;
            
            // Sharingan
            if (actor.hasStatus("status_sharingan_1t")) { eyeDrain += 0.2f; hasActiveEyeStatus = true; }
            if (actor.hasStatus("status_sharingan_2t")) { eyeDrain += 0.5f; hasActiveEyeStatus = true; }
            if (actor.hasStatus("status_sharingan_3t")) { eyeDrain += 1.0f; hasActiveEyeStatus = true; }
            if (actor.hasStatus("status_mangekyo")) { eyeDrain += 3.0f; hasActiveEyeStatus = true; }
            if (actor.hasStatus("byakuganP")) { eyeDrain += 0.8f; hasActiveEyeStatus = true; } 
            if (actor.hasStatus("tenseigan_cm")) { eyeDrain += 10f; hasActiveEyeStatus = true; }


            if (hasActiveEyeStatus)
            {
                if (!actor.hasTrait("uchiha_clan"))
                {
                    eyeDrain *= 2.0f;
                }
                drainPerSecond += eyeDrain;
                drainsChakra = true;
            }

            // Sage Mode / Senjutsu
            if (actor.hasStatus("status_frog_sage") || actor.hasStatus("status_snake_sage") || actor.hasStatus("status_slug_sage"))
            {

                float sageCost = actor.hasTrait("jugo_transformation") ? 0.5f : 2.0f;
                drainPerSecond += sageCost;
                drainsChakra = true;
            }

            if (!drainsChakra || drainPerSecond <= 0) return;

            float efficiencyMultiplier = GetAbilityCostMultiplier(actor);
            float finalDrainRate = drainPerSecond * efficiencyMultiplier;

            float actualDrain = finalDrainRate * dt;
            bool hasEnoughChakra = TryConsume(actor, actualDrain);

            if (!hasEnoughChakra)
            {
                if (!actor.hasTrait("taijutsu_master") && !actor.hasStatus("chakra_exhaustion"))
                {
                    actor.addStatusEffect("chakra_exhaustion", 15f);
                }

                ShinobiProgression.RemoveLowerForms(actor);
                actor.finishStatusEffect("status_sharingan_1t");
                actor.finishStatusEffect("status_sharingan_2t");
                actor.finishStatusEffect("status_sharingan_3t");
                actor.finishStatusEffect("status_mangekyo");
                actor.finishStatusEffect("byakuganP");
                actor.finishStatusEffect("status_frog_sage");
                actor.finishStatusEffect("status_snake_sage");
                actor.finishStatusEffect("status_slug_sage");
                actor.finishStatusEffect("tenseigan_cm");
            }
        }
    }
}