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
    public class ShinobiProgression
    {
        #region IsModeActive
        public static bool IsModeActive(Actor a)
        {
            return a.hasStatus("status_jinchuriki_initial_release") ||
                   a.hasStatus("status_jinchuriki_v1_cloak") ||
                   a.hasStatus("status_jinchuriki_v2_cloak") ||
                   a.hasStatus("status_jinchuriki_incomplete_beast") ||
                   a.hasStatus("status_jinchuriki_kcm1") ||
                   a.hasStatus("status_jinchuriki_kcm2") ||
                   a.hasStatus("status_jinchuriki_avatar") ||
                   a.hasStatus("status_jinchuriki_baryon_mode");
        }
        #endregion

        #region CheckProgression
        public static void CheckProgression(Actor actor)
        {
            if (actor == null || !actor.isAlive()) return;

            int level = actor.data.level;
            int kills = actor.data.kills;
            int age = actor.data.getAge();
            float currentInt = actor.intelligence;
            float currentDiplo = actor.diplomacy;

            ApplyNames(actor);
            ApplyAutoFavorite(actor);
            ApplyPassives(actor);

            CleanOppositeTraits(actor);
            RankProgression.CheckRankProgression(actor);

            RemoveFormsIfNoTrait(actor);

            // Rinnegan Awakening
            bool hasIndra = actor.hasTrait("indra_chakra");
            bool hasAsura = actor.hasTrait("asura_chakra");
            bool hasHagoromo = actor.hasTrait("hagoromo_chakra");
            bool hasMangekyo = actor.hasTrait("mangekyo_sharingan") || actor.hasTrait("madara_eternal_mangekyo");
            bool hasHashirama = actor.hasTrait("hashi_cells");
            bool hasAnySenjutsu = actor.hasTrait("frog_sage_mode") || actor.hasTrait("slug_sage_mode") || actor.hasTrait("snake_sage_mode") || actor.hasTrait("wood_sage_mode") || actor.hasTrait("six_paths_sage_mode");
            string currentName = actor.data.name ?? string.Empty;
            bool hasKageInName = currentName.IndexOf("kage", StringComparison.OrdinalIgnoreCase) >= 0;

            bool canAwakenRinnegan =
                (hasHagoromo && age >= 40) ||
                (hasIndra && hasHashirama) ||
                (hasAsura && hasMangekyo) ||
                (hasIndra && hasAsura);

            if (canAwakenRinnegan)
            {
                if (!actor.hasTrait("rinnegan"))
                {
                    actor.removeTrait("trait_blind");
                    actor.addTrait("rinnegan");
                    actor.restoreHealth(actor.getMaxHealth());
                    ShinobiWorldLogs.AddWorldLog("log_awaken_rinnegan", "worldlog_awaken_rinnegan", "ui/icons/rinnegan", actor);
                }
            }

            if (actor.hasTrait("rank_kage") || hasKageInName)
            {
                actor.data.set("was_kage", 1);
            }

            if (level >= 7 && hasAnySenjutsu && !actor.hasTrait("rank_sannin"))
            {
                actor.addTrait("rank_sannin");

                if (ShinobiConfig.EnableWorldTips)
                {
                    ShinobiWorldLogs.AddWorldLog("log_became_sannin", "worldlog_became_sannin", "ui/icons/rank_sannin", actor);
                }
            }

            // Ghost Of Uchiha Giver thingy thangy
            bool hasEternalEyes = actor.hasTrait("madara_eternal_mangekyo") || actor.hasTrait("eternal_mangekyo");
            if (level >= 7 && actor.hasTrait("uchiha_clan") && hasEternalEyes && !actor.hasTrait("rank_ghost_of_uchiha"))
            {
                actor.addTrait("rank_ghost_of_uchiha");
                if (ShinobiConfig.EnableWorldTips)
                {
                    ShinobiWorldLogs.AddWorldLog("log_ghost_of_uchiha", "worldlog_ghost_of_uchiha", "ui/icons/ghostofuchiha", actor);
                }
            }


            // god Of Shinobi Giver thingy thangy
            bool hasAllFiveNatures = actor.hasTrait("fireN") && actor.hasTrait("waterN") && actor.hasTrait("earthN") && actor.hasTrait("lightningN") && actor.hasTrait("windN");
            int wasKage = 0;
            actor.data.get("was_kage", out wasKage);
            bool qualifiesPastKage = actor.hasTrait("rank_kage") || hasKageInName || wasKage > 0;
            if (qualifiesPastKage && level >= 7 && hasAllFiveNatures && !actor.hasTrait("rank_god_of_shinobi"))
            {
                actor.addTrait("rank_god_of_shinobi");
                if (ShinobiConfig.EnableWorldTips)
                {
                    ShinobiWorldLogs.AddWorldLog("log_god_of_shinobi", "worldlog_god_of_shinobi", "ui/icons/godofshinobi", actor);
                }
            }

            if (actor.hasTrait("hagoromo_chakra") && !actor.hasStatus("status_six_paths_senjutsu"))
            {
                actor.addStatusEffect("status_six_paths_senjutsu", 30f);
            }

            if (actor.hasTrait("ten_tails_jinchuriki") && actor.hasTrait("rinnegan") && !actor.hasTrait("rinnesharingan"))
            {
                actor.addTrait("rinnesharingan");
                if (!actor.hasStatus("status_six_paths_senjutsu"))
                {
                    actor.addStatusEffect("status_six_paths_senjutsu", -1f);
                }
                if (ShinobiConfig.EnableWorldTips)
                {
                    ShinobiWorldLogs.AddWorldLog("log_awaken_rinnesharingan", "worldlog_awaken_rinnesharingan", "ui/icons/rinne_sharingan", actor);
                }
            }

            // If unit is an anbu or jonin, they are automatically in the military
            if (actor.hasTrait("rank_anbu") || actor.hasTrait("rank_jonin"))
            {
                actor.setProfession(UnitProfession.Warrior);
            }


            // Blindness must apply as soon as the Mangekyo usage threshold is reached.
            int totalMsUsage = 0;
            actor.data.get("ms_usage", out totalMsUsage);
            if (totalMsUsage >= 25 && actor.hasTrait("mangekyo_sharingan"))
            {
                actor.removeTrait("mangekyo_sharingan");
                actor.addTrait("trait_blind");
            }

            HandleNatureProgressionExp(actor, level, kills);
            SageProgression.HandleSageProgression(actor, level);

            bool hasKCM2Plus = actor.hasStatus("status_jinchuriki_kcm2") || actor.hasStatus("status_jinchuriki_avatar");

            if (hasKCM2Plus && hasAnySenjutsu)
            {
                actor.finishStatusEffect("status_frog_sage");
                actor.finishStatusEffect("status_slug_sage");
                actor.finishStatusEffect("status_snake_sage");
                actor.addStatusEffect("kurama_sage_mode", 60f);
            }
            // Sharingan Progression
            if (actor.hasTrait("uchiha_clan"))
            {
                bool hasThreeTomoe = actor.hasStatus("status_sharingan_3t");

                // Mangekyo Awakening
                if (!actor.hasTrait("mangekyo_sharingan") && !actor.hasTrait("trait_blind") && !hasEternalEyes && hasThreeTomoe)
                {
                    if (HasTrauma(actor))
                    {
                        JutsuLibrary.ClearSharinganTomoeStatuses(actor);

                        actor.addTrait("mangekyo_sharingan");
                        actor.addStatusEffect("power_up");
                        ShinobiWorldLogs.AddWorldLog("log_mangekyo_awakened", "worldlog_mangekyo_awakened", "ui/icons/mangekyo", actor);
                    }
                }

                if (!actor.hasTrait("sharingan") && !HasBetterEye(actor, 1))
                {
                    if (kills >= 1 || (age >= 12 && UnityEngine.Random.value < 0.001f)) actor.addTrait("sharingan");
                }

            }

            #region Ten Tails Progression Stuff

            bool hasTen = actor.hasTrait("ten_tails_jinchuriki");
            bool hasSPS = actor.hasStatus("status_six_paths_senjutsu");

            if (hasSPS && !hasTen && !actor.hasTrait("hagoromo_chakra"))
            {
                actor.finishStatusEffect("status_six_paths_senjutsu");
            }
            #endregion
            

            
        }
        #endregion

        #region Naming, Auto Favorite, and Hashirama Cells Helpers
        private static void ApplyNames(Actor actor)
        {
            if (!ShinobiConfig.EnableClanNames) return;

            if (actor.hasTrait("uchiha_clan")) ApplyClanName(actor, "Uchiha");
            else if (actor.hasTrait("hyuga_clan")) ApplyClanName(actor, "Hyuga");
            else if (actor.hasTrait("uzumaki_clan")) ApplyClanName(actor, "Uzumaki");
            else if (actor.hasTrait("senju_clan")) ApplyClanName(actor, "Senju");
            else if (actor.hasTrait("akimichi_clan")) ApplyClanName
            (actor, "Akimichi");
            else if (actor.hasTrait("lee_clan")) ApplyClanName(actor, "Lee");

            if (actor.hasTrait("rank_kage"))
            {
                ApplyKageName(actor, "Kage");
            }
        }

        private static void ApplyAutoFavorite(Actor actor)
        {
            if (!ShinobiConfig.EnableAutoFavorite) return;

            int favoriteOptOut = 0;
            actor.data.get("auto_favorite_opt_out", out favoriteOptOut);
            if (favoriteOptOut > 0) return;

            int autoFavoritedBefore = 0;
            actor.data.get("auto_favorited_once", out autoFavoritedBefore);
            if (autoFavoritedBefore > 0 && !actor.data.favorite)
            {
                actor.data.set("auto_favorite_opt_out", 1);
                return;
            }

            if (actor.data.favorite) return;

            bool hasAnySenjutsu = actor.hasTrait("frog_sage_mode") || actor.hasTrait("slug_sage_mode") || actor.hasTrait("snake_sage_mode") || actor.hasTrait("wood_sage_mode") || actor.hasTrait("six_paths_sage_mode");

            if (actor.hasTrait("indra_chakra") || actor.hasTrait("asura_chakra") ||
                actor.hasTrait("mangekyo_sharingan") || actor.hasTrait("madara_eternal_mangekyo") || actor.hasTrait("eternal_mangekyo") ||
                actor.hasTrait("rinnegan") || actor.hasTrait("nine_tails_jinchuriki") || actor.hasTrait("rinnesharingan") ||
                actor.hasTrait("hagoromo_chakra") || actor.hasTrait("ten_tails_jinchuriki") || hasAnySenjutsu)
            {
                actor.data.favorite = true;
                actor.data.set("auto_favorited_once", 1);
            }
        }

        private static void ApplyPassives(Actor actor)
        {
            if (actor.hasTrait("hashi_cells"))
            {
                if (actor.hasTrait("crippled")) actor.removeTrait("crippled");
                if (actor.hasTrait("one_eyed")) actor.removeTrait("one_eyed");
            }
        }
        #endregion

        #region Nature Progression EXP
        private static void HandleNatureProgressionExp(Actor actor, int currentLevel, int currentKills)
        {
            if (actor == null) return;

            int initialized = 0;
            actor.data.get("nature_progress_initialized", out initialized);
            if (initialized == 0)
            {
                actor.data.set("nature_progress_initialized", 1);
                actor.data.set("nature_last_level", currentLevel);
                actor.data.set("nature_last_kills", currentKills);
                return;
            }

            int prevLevel = 0;
            actor.data.get("nature_last_level", out prevLevel);

            int prevKills = 0;
            actor.data.get("nature_last_kills", out prevKills);

            if (currentLevel > prevLevel)
            {
                JutsuLibrary.AddNatureProgressExp(actor, 75f * (currentLevel - prevLevel));
                ChakraSystem.AddChakra(actor, ChakraSystem.GetMax(actor) * 0.06f * (currentLevel - prevLevel));
            }

            if (currentKills > prevKills)
            {
                JutsuLibrary.AddNatureProgressExp(actor, 50f * (currentKills - prevKills));
                ChakraSystem.AddChakra(actor, ChakraSystem.GetMax(actor) * 0.02f * (currentKills - prevKills));
            }

            if (actor.hasTrait("windN") && !actor.hasTrait("rasenshurikenJ"))
            {
                float windExp = JutsuLibrary.GetNatureExp(actor, "wind");
                if (windExp >= 250f && UnityEngine.Random.value < 0.02f)
                {
                    actor.addTrait("rasenshurikenJ");
                }
            }

            if (!actor.hasTrait("rasenganJ") && !JutsuLibrary.HasAnyNatureTrait(actor))
            {
                float totalNatureExp = JutsuLibrary.GetNatureProgressExp(actor);
                int rasenganRollDone = 0;
                actor.data.get("rasengan_unlock_roll_done", out rasenganRollDone);

                if (rasenganRollDone == 0 && totalNatureExp >= 500f)
                {
                    actor.data.set("rasengan_unlock_roll_done", 1);
                    if (UnityEngine.Random.value < 0.50f)
                    {
                        actor.addTrait("rasenganJ");
                    }
                }
            }

            if (actor.hasTrait("lightningN") && !actor.hasTrait("chidoriJ"))
            {
                float lightningExp = JutsuLibrary.GetNatureExp(actor, "lightning");
                if (lightningExp >= 250f && UnityEngine.Random.value < 0.02f)
                {
                    actor.addTrait("chidoriJ");
                }
            }

            actor.data.set("nature_last_level", currentLevel);
            actor.data.set("nature_last_kills", currentKills);
        }
        #endregion

        #region HasTrauma (such a dumb name right?)
        private static bool HasTrauma(Actor a)
        {
            foreach (var historyItem in a.happiness_change_history)
            {
                if (historyItem.asset != null)
                {
                    string id = historyItem.asset.id;
                    if (id == "death_family_member" || id == "death_best_friend" || id == "death_child" || id == "death_lover")
                    {
                        if (UnityEngine.Random.value < 0.80f)
                        {
                            return true;
                        }
                    }
                }
            }

            if (a.data.kills >= 25)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region ApplyClanName
        private static void ApplyClanName(Actor actor, string clanName)
        {
            string currentName = actor.data.name;
            if (string.IsNullOrEmpty(currentName)) return;

            if (currentName.IndexOf(clanName, StringComparison.OrdinalIgnoreCase) < 0)
            {
                actor.data.name = $"{currentName} {clanName}";
            }
        }
        #endregion

        #region ApplyKageName
        private static void ApplyKageName(Actor actor, string kageTitle)
        {
            string currentName = actor.data.name;
            if (string.IsNullOrEmpty(currentName)) return;

            if (currentName.IndexOf(kageTitle, StringComparison.OrdinalIgnoreCase) < 0)
            {
                actor.data.name = $"{kageTitle} {currentName}";
            }
        }
        #endregion

        #region HasBetterEye
        private static bool HasBetterEye(Actor a, int currentTier)
        {
            if (currentTier < 2 && (a.hasStatus("status_sharingan_2t") || a.hasStatus("status_sharingan_3t") || a.hasTrait("mangekyo_sharingan") || a.hasTrait("trait_blind"))) return true;
            if (currentTier < 3 && (a.hasStatus("status_sharingan_3t") || a.hasTrait("mangekyo_sharingan") || a.hasTrait("trait_blind"))) return true;
            if (currentTier < 4 && (a.hasTrait("mangekyo_sharingan") || a.hasTrait("trait_blind"))) return true;
            return false;
        }
        #endregion

        #region RemoveLowerForms
        public static void RemoveLowerForms(Actor a)
        {
            if (a == null) return;
            a.finishStatusEffect("status_jinchuriki_initial_release");
            a.finishStatusEffect("status_jinchuriki_v1_cloak");
            a.finishStatusEffect("status_jinchuriki_v2_cloak");
            a.finishStatusEffect("status_jinchuriki_incomplete_beast");
            a.finishStatusEffect("status_jinchuriki_kcm1");
            a.finishStatusEffect("status_jinchuriki_kcm2");
            a.finishStatusEffect("status_jinchuriki_avatar");
            a.finishStatusEffect("status_jinchuriki_baryon_mode");
        }

        public static void RemoveFormsIfNoTrait(Actor a)
        {
            if (a == null) return;

            if (!a.hasTrait("nine_tails_jinchuriki"))
            {
                a.finishStatusEffect("status_jinchuriki_initial_release");
                a.finishStatusEffect("status_jinchuriki_v1_cloak");
                a.finishStatusEffect("status_jinchuriki_v2_cloak");
                a.finishStatusEffect("status_jinchuriki_incomplete_beast");
                a.finishStatusEffect("status_jinchuriki_kcm1");
                a.finishStatusEffect("status_jinchuriki_kcm2");
                a.finishStatusEffect("status_jinchuriki_avatar");
                a.finishStatusEffect("status_jinchuriki_baryon_mode");
            }
        }
        #endregion

        #region CleanOppositeTraits
        public static void CleanOppositeTraits(Actor actor)
        {
            if (actor.hasTrait("uchiha_clan"))
            {
                actor.removeTrait("hyuga_clan");
                actor.removeTrait("uzumaki_clan");
                actor.removeTrait("senju_clan");
                actor.removeTrait("akimichi_clan");
            }
            if (actor.hasTrait("hyuga_clan"))
            {
                actor.removeTrait("uchiha_clan");
                actor.removeTrait("uzumaki_clan");
                actor.removeTrait("senju_clan");
                actor.removeTrait("akimichi_clan");
            }
            if (actor.hasTrait("uzumaki_clan"))
            {
                actor.removeTrait("uchiha_clan");
                actor.removeTrait("hyuga_clan");
                actor.removeTrait("senju_clan");
                actor.removeTrait("akimichi_clan");
            }
            if (actor.hasTrait("senju_clan"))
            {
                actor.removeTrait("uchiha_clan");
                actor.removeTrait("hyuga_clan");
                actor.removeTrait("uzumaki_clan");
                actor.removeTrait("akimichi_clan");
            }
            if (actor.hasTrait("akimichi_clan"))
            {
                actor.removeTrait("uchiha_clan");
                actor.removeTrait("hyuga_clan");
                actor.removeTrait("uzumaki_clan");
                actor.removeTrait("senju_clan");
            }

            if (actor.hasTrait("high_chakra_reserve")) actor.removeTrait("low_chakra_reserve");
            if (actor.hasTrait("low_chakra_reserve")) actor.removeTrait("high_chakra_reserve");
            if (actor.hasTrait("greater_chakra_reserve"))
            {
                actor.removeTrait("high_chakra_reserve");
                actor.removeTrait("low_chakra_reserve");
                actor.removeTrait("vast_chakra_reserve");
            }
            if (actor.hasTrait("vast_chakra_reserve"))
            {
                actor.removeTrait("high_chakra_reserve");
                actor.removeTrait("low_chakra_reserve");
                actor.removeTrait("greater_chakra_reserve");
            }

            bool hasIndra = actor.hasTrait("indra_chakra");
            bool hasAsura = actor.hasTrait("asura_chakra");

            if (hasIndra && hasAsura)
            {
                actor.removeTrait("indra_chakra");
                actor.removeTrait("asura_chakra");
                if (!actor.hasTrait("hagoromo_chakra"))
                {
                    actor.addTrait("hagoromo_chakra");
                }
                return;
            }

            if (actor.hasTrait("hagoromo_chakra"))
            {
                actor.removeTrait("indra_chakra");
                actor.removeTrait("asura_chakra");

            }
        }
        #endregion

    }
}