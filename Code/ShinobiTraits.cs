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
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using NCMS.Utils;
using UnityEngine.Events;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace ShinobiBox
{
    internal static class ShinobiTraits
    {
        // Group IDs
        private static string GroupId_Abilities = "Shinobi_Box_Abilities";
        private static string GroupId_Bloodlines = "Shinobi_Box_Clans";
        private static string GroupId_Jinchuriki = "Shinobi_Box_Jinchuriki";
        private static string GroupId_Dojutsu = "Shinobi_Box_Dojutsu";
        private static string GroupId_Ranks = "Shinobi_Box_Ranks";
        private static string GroupId_ChakraN = "Shinobi_Box_ChakraN";
        private static string GroupId_SpecialChakra = "Shinobi_Box_Special_Chakra";
        private static string GroupId_Jutsus = "Shinobi_Box_Jutsus";
        private static string GroupId_Senjutsu = "Shinobi_Box_Senjutsu";
        private static string GroupId_FightingStyles = "Shinobi_Box_Fighting_Styles";

        private static List<ActorTrait> myListTraits = new List<ActorTrait>();

        private static int veryRare = 2;
        private static int rare = 5;
        private static int lowChance = 10;
        private static int highChance = 40;
        private static int alwaysChance = 100;

        public static void Init()
        {
            ChakraSystem.RegisterChakraBaseStat();
            loadCustomTraitGroup();
            loadShinobiTraits();
            loadClans();
            loadDojutsu();
            loadShinobiRanks();
            loadSenjutsu();
            loadJutsus();
            populateListOppositeTraits();
        }

        #region Trait Groups
        private static void loadCustomTraitGroup()
        {
            ActorTraitGroupAsset shinobiArts = new ActorTraitGroupAsset();
            shinobiArts.id = GroupId_Abilities;
            shinobiArts.name = "Shinobi Traits";
            shinobiArts.color = "#FFFFFF";
            AssetManager.trait_groups.add(shinobiArts);

            ActorTraitGroupAsset shinobiClans = new ActorTraitGroupAsset();
            shinobiClans.id = GroupId_Bloodlines;
            shinobiClans.name = "Shinobi Bloodlines";
            shinobiClans.color = "#FF0000";
            AssetManager.trait_groups.add(shinobiClans);

            ActorTraitGroupAsset jinchurikiGroup = new ActorTraitGroupAsset();
            jinchurikiGroup.id = GroupId_Jinchuriki;
            jinchurikiGroup.name = "Jinchuriki";
            jinchurikiGroup.color = "#FF6600";
            AssetManager.trait_groups.add(jinchurikiGroup);

            ActorTraitGroupAsset dojutsuGroup = new ActorTraitGroupAsset();
            dojutsuGroup.id = GroupId_Dojutsu;
            dojutsuGroup.name = "Dojutsu";
            dojutsuGroup.color = "#800080";
            AssetManager.trait_groups.add(dojutsuGroup);

            ActorTraitGroupAsset shinobiRanks = new ActorTraitGroupAsset();
            shinobiRanks.id = GroupId_Ranks;
            shinobiRanks.name = "Shinobi Ranks";
            shinobiRanks.color = "#FFD700";
            AssetManager.trait_groups.add(shinobiRanks);

            ActorTraitGroupAsset chakraNGroup = new ActorTraitGroupAsset();
            chakraNGroup.id = GroupId_ChakraN;
            chakraNGroup.name = "Chakra Natures";
            chakraNGroup.color = "#FF6600";
            AssetManager.trait_groups.add(chakraNGroup);

            ActorTraitGroupAsset specialChakraGroup = new ActorTraitGroupAsset();
            specialChakraGroup.id = GroupId_SpecialChakra;
            specialChakraGroup.name = "Special Chakra";
            specialChakraGroup.color = "#66CCFF";
            AssetManager.trait_groups.add(specialChakraGroup);

            ActorTraitGroupAsset jutsuGroup = new ActorTraitGroupAsset();
            jutsuGroup.id = GroupId_Jutsus;
            jutsuGroup.name = "Shinobi Jutsus";
            jutsuGroup.color = "#009dd1";
            AssetManager.trait_groups.add(jutsuGroup);

            ActorTraitGroupAsset senjutsuGroup = new ActorTraitGroupAsset();
            senjutsuGroup.id = GroupId_Senjutsu;
            senjutsuGroup.name = "Senjutsu";
            senjutsuGroup.color = "#6FAF7C";
            AssetManager.trait_groups.add(senjutsuGroup);
        }
        #endregion
        private static void loadClans()
        {

            #region Hyuga Clan
            // Hyuga Clan
            ActorTrait hyuga = new ActorTrait
            {
                id = "hyuga_clan",
                group_id = GroupId_Bloodlines,
                path_icon = "ui/icons/hyuga",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_inherit = alwaysChance,
            };
            hyuga.base_stats = new BaseStats();
            hyuga.base_stats.set("accuracy", 0.35f);
            hyuga.base_stats.set("critical_chance", 0.10f);
            hyuga.base_stats.set("damage", 10f);
            hyuga.base_stats.set("intelligence", 10f);
            hyuga.base_stats.set("chakra", 25f);
            hyuga.base_stats.set("experience", 15f);
            addTraitToGame(hyuga);
            #endregion

            #region Uzumaki Clan
            // Uzumaki Clan
            ActorTrait uzumaki = new ActorTrait
            {
                id = "uzumaki_clan",
                group_id = GroupId_Bloodlines,
                path_icon = "ui/icons/uzumaki",
                can_be_given = true,
                rate_inherit = alwaysChance,
                needs_to_be_explored = false
            };
            uzumaki.base_stats = new BaseStats();
            uzumaki.base_stats.set("health", 250f);
            uzumaki.base_stats.set("multiplier_health", 0.05f);
            uzumaki.base_stats.set("multiplier_lifespan", 0.35f);
            uzumaki.base_stats.set("intelligence", 10f);
            uzumaki.base_stats.set("stamina", 50f);
            uzumaki.base_stats.set("experience", 15f);
            uzumaki.base_stats.set("chakra", 100f);
            uzumaki.base_stats.set("multiplier_chakra", 0.75f);

            uzumaki.action_attack_target = new AttackAction(JutsuLibrary.AdamantiteChainsAction);

            addTraitToGame(uzumaki);
            #endregion

            #region Senju Clan
            // Senju Clan
            ActorTrait senju = new ActorTrait
            {
                id = "senju_clan",
                group_id = GroupId_Bloodlines,
                path_icon = "ui/icons/senju",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_inherit = alwaysChance,
            };
            senju.base_stats = new BaseStats();
            senju.base_stats.set("health", 100f);
            senju.base_stats.set("damage", 20f);
            senju.base_stats.set("diplomacy", 15f);
            senju.base_stats.set("warfare", 5f);
            senju.base_stats.set("stewardship", 5f);
            senju.base_stats.set("intelligence", 10f);
            senju.base_stats.set("chakra", 25f);
            senju.base_stats.set("experience", 20f);
            addTraitToGame(senju);
            #endregion

            #region Uchiha Clan
            // Uchiha Clan
            ActorTrait uchiha_clan = new ActorTrait
            {
                id = "uchiha_clan",
                group_id = GroupId_Bloodlines,
                path_icon = "ui/icons/uchiha_fan",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_inherit = alwaysChance,
            };
            uchiha_clan.base_stats = new BaseStats();
            uchiha_clan.base_stats.set("damage", 40f);
            uchiha_clan.base_stats.set("health", 100f);
            uchiha_clan.base_stats.set("skill_combat", 0.30f);
            uchiha_clan.base_stats.set("multiplier_speed", 0.05f);
            uchiha_clan.base_stats.set("critical_chance", 0.05f);
            uchiha_clan.base_stats.set("warfare", 10f);
            uchiha_clan.base_stats.set("diplomacy", -3f);
            uchiha_clan.base_stats.set("stewardship", -3f);
            uchiha_clan.base_stats.set("experience", 15f);
            uchiha_clan.base_stats.set("chakra", 25f);
            addTraitToGame(uchiha_clan);
            #endregion

            #region Akimichi Clan
            // Akimichi Clan
            ActorTrait akimichi_clan = new ActorTrait
            {
                id = "akimichi_clan",
                group_id = GroupId_Bloodlines,
                path_icon = "ui/icons/akimichi_clan",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_inherit = alwaysChance,
            };
            akimichi_clan.base_stats = new BaseStats();
            akimichi_clan.base_stats.set("health", 120f);
            akimichi_clan.base_stats.set("multiplier_health", 0.10f);
            akimichi_clan.base_stats.set("damage", 20f);
            akimichi_clan.base_stats.set("stamina", 100f);
            akimichi_clan.base_stats.set("armor", 10f);
            akimichi_clan.base_stats.set("experience", 10f);
            akimichi_clan.base_stats.set("chakra", 25f);

            akimichi_clan.action_attack_target = new AttackAction(JutsuLibrary.AkimichiSizeManipulationAction);

            addTraitToGame(akimichi_clan);
            #endregion

            #region Lee Clan
            // Lee Clan
            ActorTrait lee_clan = new ActorTrait
            {
                id = "lee_clan",
                group_id = GroupId_Bloodlines,
                path_icon = "ui/icons/lee_clan",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_inherit = alwaysChance,
            };
            lee_clan.base_stats = new BaseStats();
            lee_clan.base_stats.set("health", 110f);
            lee_clan.base_stats.set("damage", 22f);
            lee_clan.base_stats.set("speed", 8f);
            lee_clan.base_stats.set("multiplier_speed", 0.05f);
            lee_clan.base_stats.set("attack_speed", 0.45f);
            lee_clan.base_stats.set("critical_chance", 0.04f);
            lee_clan.base_stats.set("stamina", 60f);
            lee_clan.base_stats.set("intelligence", 4f);
            lee_clan.base_stats.set("chakra", 20f);
            lee_clan.base_stats.set("experience", 12f);

            addTraitToGame(lee_clan);
            #endregion
        }

        private static void loadDojutsu()
        {
            #region Byakugan
            // Byakugan
            ActorTrait byakugan = new ActorTrait
            {
                id = "byakugan",
                group_id = GroupId_Dojutsu,
                path_icon = "ui/icons/byakugan",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R2_Epic
            };
            byakugan.base_stats = new BaseStats();
            byakugan.base_stats.set("damage", 40f);
            byakugan.base_stats.set("accuracy", 2f);
            byakugan.base_stats.set("critical_chance", 0.12f);
            byakugan.base_stats.set("critical_damage_multiplier", 0.12f);
            byakugan.base_stats.set("skill_combat", 0.12f);
            byakugan.base_stats.set("chakra", 32f);

            byakugan.action_attack_target = new AttackAction(JutsuLibrary.PalmRotationAttackAction);
            byakugan.action_attack_target = (AttackAction)Delegate.Combine(byakugan.action_attack_target, new AttackAction(JutsuLibrary.EightTrigrams64PalmsAction));

            addTraitToGame(byakugan);
            #endregion

            #region Rinnegan
            ActorTrait rinnegan = new ActorTrait
            {
                id = "rinnegan",
                group_id = GroupId_Dojutsu,
                path_icon = "ui/icons/rinnegan",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R3_Legendary
            };
            rinnegan.base_stats = new BaseStats();
            rinnegan.base_stats.set("damage", 65f);
            rinnegan.base_stats.set("multiplier_damage", 0.28f);
            rinnegan.base_stats.set("multiplier_health", 0.25f);
            rinnegan.base_stats.set("multiplier_speed", 0.15f);
            rinnegan.base_stats.set("intelligence", 30f);
            rinnegan.base_stats.set("multiplier_lifespan", -0.10f);
            rinnegan.base_stats.set("multiplier_attack_speed", 0.10f);
            rinnegan.base_stats.set("critical_chance", 0.25f);
            rinnegan.base_stats.set("critical_damage_multiplier", 0.25f);
            rinnegan.base_stats.set("skill_combat", 0.15f);
            rinnegan.base_stats.set("accuracy", 2f);
            rinnegan.base_stats.set("chakra", 148f);

            rinnegan.action_attack_target = new AttackAction(JutsuLibrary.RinneganMeteorAction);
            rinnegan.action_attack_target = (AttackAction)Delegate.Combine(rinnegan.action_attack_target, new AttackAction(JutsuLibrary.NarakaPathHealAction));
            rinnegan.action_attack_target = (AttackAction)Delegate.Combine(rinnegan.action_attack_target, new AttackAction(JutsuLibrary.SixPathsTechnique));
            rinnegan.action_get_hit = (GetHitAction)Delegate.Combine(rinnegan.action_get_hit, new GetHitAction(JutsuLibrary.DevaShinraTensei));

            addTraitToGame(rinnegan);
            #endregion

            #region Rinne Sharingan
            ActorTrait rinnesharingan = new ActorTrait
            {
                id = "rinnesharingan",
                group_id = GroupId_Dojutsu,
                path_icon = "ui/icons/rinne_sharingan",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R3_Legendary
            };
            rinnesharingan.base_stats = new BaseStats();
            rinnesharingan.base_stats.set("damage", 202f);
            rinnesharingan.base_stats.set("health", 222f);
            rinnesharingan.base_stats.set("multiplier_damage", 0.40f);
            rinnesharingan.base_stats.set("multiplier_health", 0.40f);
            rinnesharingan.base_stats.set("intelligence", 80f);
            rinnesharingan.base_stats.set("multiplier_speed", 0.25f);
            rinnesharingan.base_stats.set("accuracy", 4f);
            rinnesharingan.base_stats.set("critical_chance", 0.40f);
            rinnesharingan.base_stats.set("critical_damage_multiplier", 0.40f);
            rinnesharingan.base_stats.set("chakra", 282f);

            rinnesharingan.action_attack_target = new AttackAction(JutsuLibrary.InfiniteTsukuyomiAction);
            rinnesharingan.action_attack_target = (AttackAction)Delegate.Combine(rinnesharingan.action_attack_target, new AttackAction(JutsuLibrary.HeavenlyIceChamberAction));

            addTraitToGame(rinnesharingan);
            #endregion

            #region Sharingan
            ActorTrait sharingan = new ActorTrait
            {
                id = "sharingan",
                group_id = GroupId_Dojutsu,
                path_icon = "ui/icons/sharingan_3t",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R3_Legendary
            };
            sharingan.action_attack_target = new AttackAction(JutsuLibrary.SharinganActions);
            sharingan.action_special_effect = (WorldAction)Delegate.Combine(sharingan.action_special_effect, new WorldAction(JutsuLibrary.SharinganProgression));
            sharingan.special_effect_interval = 1f;

            addTraitToGame(sharingan);

            #endregion

            #region Mangekyo
            ActorTrait mangekyo = new ActorTrait
            {
                id = "mangekyo_sharingan",
                group_id = GroupId_Dojutsu,
                path_icon = "ui/icons/mangekyo",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R3_Legendary
            };
            mangekyo.base_stats = new BaseStats();
            mangekyo.base_stats.set("damage", 142f);
            mangekyo.base_stats.set("multiplier_damage", 0.23f);
            mangekyo.base_stats.set("health", 226f);
            mangekyo.base_stats.set("multiplier_health", 0.11f);
            mangekyo.base_stats.set("intelligence", 60f);
            mangekyo.base_stats.set("speed", 15f);
            mangekyo.base_stats.set("critical_chance", 0.30f);
            mangekyo.base_stats.set("critical_damage_multiplier", 0.30f);
            mangekyo.base_stats.set("attack_speed", 0.10f);
            mangekyo.base_stats.set("accuracy", 2.5f);
            mangekyo.base_stats.set("chakra", 120f);

            bool sharinganRoll = false;

            if (!sharinganRoll && Randy.randomChance(0.50f))
            {
                mangekyo.action_attack_target = new AttackAction(JutsuLibrary.AmaterasuAction);
                mangekyo.action_special_effect = (WorldAction)Delegate.Combine(mangekyo.action_special_effect, new WorldAction(JutsuLibrary.SusanooRibcage));
                mangekyo.action_attack_target = (AttackAction)Delegate.Combine(mangekyo.action_attack_target, new AttackAction(JutsuLibrary.BaseSusanooAction));
                mangekyo.action_get_hit = (GetHitAction)Delegate.Combine(mangekyo.action_get_hit, new GetHitAction(JutsuLibrary.KamuiAction));
                sharinganRoll = true;
            }
            else
            {
                /* mangekyo.action_attack_target = new AttackAction(JutsuLibrary.YasakaMagatamaAction);
                mangekyo.action_attack_target = (AttackAction)Delegate.Combine(mangekyo.action_attack_target, new AttackAction(JutsuLibrary.ItachiSusanooAction));
                mangekyo.action_attack_target = (AttackAction)Delegate.Combine(mangekyo.action_attack_target, new AttackAction(JutsuLibrary.AmaterasuAction));
                mangekyo.action_special_effect = (WorldAction)Delegate.Combine(mangekyo.action_special_effect, new WorldAction(JutsuLibrary.ItachiSusanooRibcage));
                */
                Debug.Log("itachi mangekyo was picked");
                
            }


            addTraitToGame(mangekyo);
            #endregion

            #region Madara Eternal Mangekyo
            ActorTrait madaraEternalMangekyo = new ActorTrait
            {
                id = "madara_eternal_mangekyo",
                group_id = GroupId_Dojutsu,
                path_icon = "ui/icons/madara_ems",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R3_Legendary
            };
            madaraEternalMangekyo.base_stats = new BaseStats();
            madaraEternalMangekyo.base_stats.set("damage", 284f);
            madaraEternalMangekyo.base_stats.set("multiplier_damage", 0.28f);
            madaraEternalMangekyo.base_stats.set("health", 354f);
            madaraEternalMangekyo.base_stats.set("multiplier_health", 0.15f);
            madaraEternalMangekyo.base_stats.set("intelligence", 60f);
            madaraEternalMangekyo.base_stats.set("speed", 15f);
            madaraEternalMangekyo.base_stats.set("critical_chance", 0.30f);
            madaraEternalMangekyo.base_stats.set("critical_damage_multiplier", 0.30f);
            madaraEternalMangekyo.base_stats.set("attack_speed", 0.20f);
            madaraEternalMangekyo.base_stats.set("accuracy", 2.5f);
            madaraEternalMangekyo.base_stats.set("chakra", 182f);

            madaraEternalMangekyo.action_attack_target = new AttackAction(JutsuLibrary.MadaraTengaiShinsei);
            madaraEternalMangekyo.action_attack_target = (AttackAction)Delegate.Combine(madaraEternalMangekyo.action_attack_target, new AttackAction(JutsuLibrary.MadaraPerfectSusanoo));
            madaraEternalMangekyo.action_attack_target = (AttackAction)Delegate.Combine(madaraEternalMangekyo.action_attack_target, new AttackAction(JutsuLibrary.MadaraEMSGenjutsu));
            madaraEternalMangekyo.action_special_effect = (WorldAction)Delegate.Combine(madaraEternalMangekyo.action_special_effect, new WorldAction(JutsuLibrary.MadaraSusanooRibcage));


            addTraitToGame(madaraEternalMangekyo);
            #endregion

            #region Eternal Mangekyo
            ActorTrait ems = new ActorTrait
            {
                id = "eternal_mangekyo",
                group_id = GroupId_Dojutsu,
                path_icon = "ui/icons/ems",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R3_Legendary

            };
            ems.base_stats = new BaseStats();
            ems.base_stats.set("damage", 304f);
            ems.base_stats.set("multiplier_damage", 0.28f);
            ems.base_stats.set("health", 354f);
            ems.base_stats.set("multiplier_health", 0.15f);
            ems.base_stats.set("intelligence", 60f);
            ems.base_stats.set("speed", 15f);
            ems.base_stats.set("critical_chance", 0.30f);
            ems.base_stats.set("critical_damage_multiplier", 0.30f);
            ems.base_stats.set("attack_speed", 0.20f);
            ems.base_stats.set("accuracy", 2.5f);
            ems.base_stats.set("chakra", 182f);

            ems.action_attack_target = new AttackAction(JutsuLibrary.AmaterasuAction);
            ems.action_attack_target = (AttackAction)Delegate.Combine(ems.action_attack_target, new AttackAction(JutsuLibrary.PerfectSusanooAction));
            ems.action_special_effect = (WorldAction)Delegate.Combine(ems.action_special_effect, new WorldAction(JutsuLibrary.SusanooRibcage));
            ems.action_get_hit = (GetHitAction)Delegate.Combine(ems.action_get_hit, new GetHitAction(JutsuLibrary.KamuiAction));


            addTraitToGame(ems);
            #endregion

            #region Sasuke's Rinnegan
            // sasukes rinnegan has the same abilities as rinnegan and eternal mangekyo, but with different stats and a unique icon
            ActorTrait sasukeRinnegan = new ActorTrait
            {
                id = "sasuke_rinnegan",
                group_id = GroupId_Dojutsu,
                path_icon = "ui/icons/sasuke_rinnegan",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R3_Legendary
            };
            sasukeRinnegan.base_stats = new BaseStats();
            sasukeRinnegan.base_stats.set("damage", 202f);
            sasukeRinnegan.base_stats.set("health", 222f);
            sasukeRinnegan.base_stats.set("multiplier_damage", 0.40f);
            sasukeRinnegan.base_stats.set("multiplier_health", 0.40f);
            sasukeRinnegan.base_stats.set("intelligence", 80f);
            sasukeRinnegan.base_stats.set("multiplier_speed", 0.25f);
            sasukeRinnegan.base_stats.set("accuracy", 4f);
            sasukeRinnegan.base_stats.set("critical_chance", 0.40f);
            sasukeRinnegan.base_stats.set("critical_damage_multiplier", 0.40f);
            sasukeRinnegan.base_stats.set("chakra", 282f);
            // All abilities from rinnegan + base EMS moves + tailed beast genjutsu.
            sasukeRinnegan.action_attack_target = new AttackAction(JutsuLibrary.RinneganMeteorAction);
            sasukeRinnegan.action_attack_target = (AttackAction)Delegate.Combine(sasukeRinnegan.action_attack_target, new AttackAction(JutsuLibrary.NarakaPathHealAction));
            sasukeRinnegan.action_attack_target = (AttackAction)Delegate.Combine(sasukeRinnegan.action_attack_target, new AttackAction(JutsuLibrary.SixPathsTechnique));
            sasukeRinnegan.action_attack_target = (AttackAction)Delegate.Combine(sasukeRinnegan.action_attack_target, new AttackAction(JutsuLibrary.AmaterasuAction));
            sasukeRinnegan.action_attack_target = (AttackAction)Delegate.Combine(sasukeRinnegan.action_attack_target, new AttackAction(JutsuLibrary.PerfectSusanooAction));
            sasukeRinnegan.action_attack_target = (AttackAction)Delegate.Combine(sasukeRinnegan.action_attack_target, new AttackAction(JutsuLibrary.MadaraEMSGenjutsu));
            sasukeRinnegan.action_attack_target = (AttackAction)Delegate.Combine(sasukeRinnegan.action_attack_target, new AttackAction(JutsuLibrary.Amenotejikara));

            sasukeRinnegan.action_special_effect = (WorldAction)Delegate.Combine(sasukeRinnegan.action_special_effect, new WorldAction(JutsuLibrary.SusanooRibcage));
            sasukeRinnegan.action_get_hit = (GetHitAction)Delegate.Combine(sasukeRinnegan.action_get_hit, new GetHitAction(JutsuLibrary.DevaShinraTensei));
            sasukeRinnegan.action_get_hit = (GetHitAction)Delegate.Combine(sasukeRinnegan.action_get_hit, new GetHitAction(JutsuLibrary.KamuiAction));

            addTraitToGame(sasukeRinnegan);
    
            /*
            #region Itachi Mangekyo Sharingan
            ActorTrait itachiMangekyo = new ActorTrait
            {
                id = "itachi_mangekyo",
                group_id = GroupId_Dojutsu,
                path_icon = "ui/icons/itachi_mangekyo",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R3_Legendary
            };
            itachiMangekyo.base_stats = new BaseStats();
            itachiMangekyo.base_stats.set("damage", 180f);
            itachiMangekyo.base_stats.set("multiplier_damage", 0.20f);
            itachiMangekyo.base_stats.set("health", 250f);
            itachiMangekyo.base_stats.set("multiplier_health", 0.10f);
            itachiMangekyo.base_stats.set("intelligence", 50f);
            itachiMangekyo.base_stats.set("speed", 12f);
            itachiMangekyo.base_stats.set("critical_chance", 0.30f);
            itachiMangekyo.base_stats.set("critical_damage_multiplier", 0.25f);
            itachiMangekyo.base_stats.set("attack_speed", 0.15f);
            itachiMangekyo.base_stats.set("accuracy", 2.0f);
            itachiMangekyo.base_stats.set("chakra", 140f);

            itachiMangekyo.action_attack_target = new AttackAction(JutsuLibrary.YasakaMagatamaAction);
            itachiMangekyo.action_attack_target = (AttackAction)Delegate.Combine(itachiMangekyo.action_attack_target, new AttackAction(JutsuLibrary.ItachiSusanooAction));
            itachiMangekyo.action_attack_target = (AttackAction)Delegate.Combine(itachiMangekyo.action_attack_target, new AttackAction(JutsuLibrary.AmaterasuAction));
            itachiMangekyo.action_special_effect = (WorldAction)Delegate.Combine(itachiMangekyo.action_special_effect, new WorldAction(JutsuLibrary.ItachiSusanooRibcage));

            addTraitToGame(itachiMangekyo);
            #endregion
            */
            #endregion
        }

        private static void loadShinobiRanks()
        {
            #region Academy Student
            ActorTrait student = new ActorTrait
            {
                id = "rank_academy_student",
                group_id = GroupId_Ranks,
                path_icon = "ui/icons/rank_student",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R0_Normal

            };
            student.base_stats = new BaseStats();
            student.base_stats.set("experience", 3f);
            student.base_stats.set("intelligence", 5);
            student.base_stats.set("chakra", 12f);
            addTraitToGame(student);
            #endregion

            #region Genin
            ActorTrait genin = new ActorTrait
            {
                id = "rank_genin",
                group_id = GroupId_Ranks,
                path_icon = "ui/icons/rank_genin",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R0_Normal

            };
            genin.base_stats = new BaseStats();
            genin.base_stats.set("multiplier_health", 0.03f);
            genin.base_stats.set("multiplier_damage", 0.03f);
            genin.base_stats.set("multiplier_speed", 0.02f);
            genin.base_stats.set("experience", 6f);
            genin.base_stats.set("critical_chance", 0.05f);
            genin.base_stats.set("critical_damage_multiplier", 0.05f);
            genin.base_stats.set("intelligence", 10f);
            genin.base_stats.set("chakra", 31f);
            addTraitToGame(genin);
            #endregion

            #region Chunin
            ActorTrait chunin = new ActorTrait
            {
                id = "rank_chunin",
                group_id = GroupId_Ranks,
                path_icon = "ui/icons/rank_chunin",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R1_Rare

            };
            chunin.base_stats = new BaseStats();
            chunin.base_stats.set("multiplier_health", 0.06f);
            chunin.base_stats.set("multiplier_damage", 0.06f);
            chunin.base_stats.set("multiplier_speed", 0.04f);
            chunin.base_stats.set("attack_speed", 0.02f);
            chunin.base_stats.set("critical_chance", 0.10f);
            chunin.base_stats.set("critical_damage_multiplier", 0.10f);
            chunin.base_stats.set("intelligence", 15f);
            chunin.base_stats.set("experience", 9f);
            chunin.base_stats.set("stewardship", 5f);
            chunin.base_stats.set("chakra", 67f);
            addTraitToGame(chunin);
            #endregion

            #region Jonin
            ActorTrait jonin = new ActorTrait
            {
                id = "rank_jonin",
                group_id = GroupId_Ranks,
                path_icon = "ui/icons/rank_jonin",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R2_Epic

            };
            jonin.base_stats = new BaseStats();
            jonin.base_stats.set("multiplier_health", 0.09f);
            jonin.base_stats.set("multiplier_damage", 0.09f);
            jonin.base_stats.set("multiplier_speed", 0.06f);
            jonin.base_stats.set("attack_speed", 0.04f);
            jonin.base_stats.set("critical_chance", 0.15f);
            jonin.base_stats.set("critical_damage_multiplier", 0.15f);
            jonin.base_stats.set("experience", 12f);
            jonin.base_stats.set("intelligence", 20f);
            jonin.base_stats.set("chakra", 92f);
            addTraitToGame(jonin);
            #endregion

            #region Anbu
            ActorTrait anbu = new ActorTrait
            {
                id = "rank_anbu",
                group_id = GroupId_Ranks,
                path_icon = "ui/icons/rank_anbu",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R2_Epic

            };
            anbu.base_stats = new BaseStats();
            anbu.base_stats.set("multiplier_health", 0.12f);
            anbu.base_stats.set("multiplier_damage", 0.12f);
            anbu.base_stats.set("multiplier_speed", 0.08f);
            anbu.base_stats.set("attack_speed", 0.06f);
            anbu.base_stats.set("critical_chance", 0.20f);
            anbu.base_stats.set("critical_damage_multiplier", 0.20f);
            anbu.base_stats.set("accuracy", 0.45f);
            anbu.base_stats.set("experience", 15f);
            anbu.base_stats.set("intelligence", 25f);
            anbu.base_stats.set("chakra", 132f);
            addTraitToGame(anbu);
            #endregion

            #region Kage
            ActorTrait kage = new ActorTrait
            {
                id = "rank_kage",
                group_id = GroupId_Ranks,
                path_icon = "ui/icons/rank_kage",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R2_Epic

            };
            kage.base_stats = new BaseStats();
            kage.base_stats.set("health", 168f);
            kage.base_stats.set("damage", 42f);
            kage.base_stats.set("speed", 10f);
            kage.base_stats.set("multiplier_health", 0.20f);
            kage.base_stats.set("multiplier_damage", 0.20f);
            kage.base_stats.set("multiplier_speed", 0.10f);
            kage.base_stats.set("attack_speed", 0.08f);
            kage.base_stats.set("critical_chance", 0.25f);
            kage.base_stats.set("critical_damage_multiplier", 0.25f);
            kage.base_stats.set("intelligence", 40f);
            kage.base_stats.set("diplomacy", 15f);
            kage.base_stats.set("experience", 8f);
            kage.base_stats.set("stewardship", 15f);
            kage.base_stats.set("loyalty_traits", 100f);
            kage.base_stats.set("cities", 2f);
            kage.base_stats.set("scale", 0.03f);
            kage.base_stats.set("chakra", 200f);
            addTraitToGame(kage);
            #endregion

            #region Sannin
            ActorTrait sannin = new ActorTrait
            {
                id = "rank_sannin",
                group_id = GroupId_Ranks,
                path_icon = "ui/icons/rank_sannin",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R3_Legendary
            };
            sannin.base_stats = new BaseStats();
            sannin.base_stats.set("health", 308f);
            sannin.base_stats.set("damage", 102f);
            sannin.base_stats.set("speed", 12f);
            sannin.base_stats.set("attack_speed", 0.15f);
            sannin.base_stats.set("critical_chance", 0.25f);
            sannin.base_stats.set("critical_damage_multiplier", 0.25f);
            sannin.base_stats.set("intelligence", 48f);
            sannin.base_stats.set("experience", 16f);
            sannin.base_stats.set("chakra", 238f);
            addTraitToGame(sannin);
            #endregion

            #region Ghost Of Uchiha
            ActorTrait ghostOfUchiha = new ActorTrait
            {
                id = "rank_ghost_of_uchiha",
                group_id = GroupId_Ranks,
                path_icon = "ui/icons/ghostofuchiha",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R3_Legendary

            };
            ghostOfUchiha.base_stats = new BaseStats();
            ghostOfUchiha.base_stats.set("health", 328f);
            ghostOfUchiha.base_stats.set("damage", 102f);
            ghostOfUchiha.base_stats.set("speed", 12f);
            ghostOfUchiha.base_stats.set("warfare", 10f);
            ghostOfUchiha.base_stats.set("attack_speed", 0.35f);
            ghostOfUchiha.base_stats.set("critical_chance", 0.45f);
            ghostOfUchiha.base_stats.set("critical_damage_multiplier", 0.45f);
            ghostOfUchiha.base_stats.set("intelligence", 48f);
            ghostOfUchiha.base_stats.set("experience", 22f);
            ghostOfUchiha.base_stats.set("chakra", 304f);
            addTraitToGame(ghostOfUchiha);
            #endregion

            #region God Of Shinobi
            ActorTrait godOfShinobi = new ActorTrait
            {
                id = "rank_god_of_shinobi",
                group_id = GroupId_Ranks,
                path_icon = "ui/icons/godofshinobi",
                can_be_given = true,
                needs_to_be_explored = false,
                rarity = Rarity.R3_Legendary
            };
            godOfShinobi.base_stats = new BaseStats();
            godOfShinobi.base_stats.set("health", 502f);
            godOfShinobi.base_stats.set("damage", 203f);
            godOfShinobi.base_stats.set("speed", 16f);
            godOfShinobi.base_stats.set("attack_speed", 0.5f);
            godOfShinobi.base_stats.set("multiplier_damage", 0.25f);
            godOfShinobi.base_stats.set("multiplier_health", 0.35f);
            godOfShinobi.base_stats.set("multiplier_speed", 0.15f);
            godOfShinobi.base_stats.set("critical_chance", 0.65f);
            godOfShinobi.base_stats.set("critical_damage_multiplier", 0.65f);
            godOfShinobi.base_stats.set("intelligence", 92f);
            godOfShinobi.base_stats.set("experience", 38f);
            godOfShinobi.base_stats.set("chakra", 482f);
            addTraitToGame(godOfShinobi);
            #endregion
        }

        private static void loadShinobiTraits()
        {
            #region Core Abilities

            #region Will of Fire
            ActorTrait will_of_fire = new ActorTrait
            {
                id = "will_of_fire",
                group_id = GroupId_Abilities,
                path_icon = "ui/icons/will_of_fire",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_birth = veryRare,
                rate_inherit = 50
            };
            will_of_fire.base_stats = new BaseStats();
            will_of_fire.base_stats.set("health", 50f);
            will_of_fire.base_stats.set("diplomacy", 7f);
            will_of_fire.base_stats.set("stewardship", 7f);
            will_of_fire.base_stats.set("lifespan", 7f);
            will_of_fire.base_stats.set("chakra", 20f);
            will_of_fire.base_stats.set("experience", 5f);

            will_of_fire.action_death = new WorldAction(JutsuLibrary.WOFDeathAction);
            addTraitToGame(will_of_fire);
            #endregion

            #region Curse Of Hatred
            ActorTrait curse_of_hatred = new ActorTrait
            {
                id = "curse_of_hatred",
                group_id = GroupId_Abilities,
                path_icon = "ui/icons/curse_of_hatred",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_birth = veryRare,
                rate_inherit = 50

            };
            curse_of_hatred.base_stats = new BaseStats();
            curse_of_hatred.base_stats.set("damage", 25f);
            curse_of_hatred.base_stats.set("warfare", 7f);
            curse_of_hatred.base_stats.set("critical_damage_multiplier", 0.07f);
            curse_of_hatred.base_stats.set("lifespan", -7f);
            curse_of_hatred.base_stats.set("chakra", 20f);
            curse_of_hatred.base_stats.set("experience", 5f);

            curse_of_hatred.action_death = new WorldAction(JutsuLibrary.COHDeathAction);
            addTraitToGame(curse_of_hatred);
            #endregion

            #region High Chakra Reserves
            ActorTrait high_chakra = new ActorTrait
            {
                id = "high_chakra_reserve",
                group_id = GroupId_SpecialChakra,
                path_icon = "ui/icons/HighChakraReserve",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_inherit = rare,
                rate_birth = 2
            };
            high_chakra.base_stats = new BaseStats();
            high_chakra.base_stats.set("chakra", 100f);
            high_chakra.base_stats.set("multiplier_chakra", 0.10f);
            addTraitToGame(high_chakra);
            #endregion

            #region Greater Chakra Reserves
            ActorTrait greater_chakra = new ActorTrait
            {
                id = "greater_chakra_reserve",
                group_id = GroupId_SpecialChakra,
                path_icon = "ui/icons/GreaterChakraReserve",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_inherit = rare,
                rate_birth = veryRare
            };
            greater_chakra.base_stats = new BaseStats();
            greater_chakra.base_stats.set("chakra", 200f);
            greater_chakra.base_stats.set("multiplier_chakra", 0.25f);
            addTraitToGame(greater_chakra);
            #endregion

            #region Vast Chakra Reserves
            ActorTrait vast_chakra = new ActorTrait
            {
                id = "vast_chakra_reserve",
                group_id = GroupId_SpecialChakra,
                path_icon = "ui/icons/VastChakraReserve",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_inherit = rare,
                rate_birth = 1
            };
            vast_chakra.base_stats = new BaseStats();
            vast_chakra.base_stats.set("chakra", 300f);
            vast_chakra.base_stats.set("multiplier_chakra", 0.50f);
            addTraitToGame(vast_chakra);
            #endregion

            #region Low Chakra Reserves
            ActorTrait low_chakra = new ActorTrait
            {
                id = "low_chakra_reserve",
                group_id = GroupId_SpecialChakra,
                path_icon = "ui/icons/LowChakraReserve",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_inherit = rare,
                rate_birth = 2
            };
            low_chakra.base_stats = new BaseStats();
            low_chakra.base_stats.set("multiplier_chakra", -0.10f);
            addTraitToGame(low_chakra);
            #endregion

            #region Hagoromo Chakra
            ActorTrait hagoromo = new ActorTrait
            {
                id = "hagoromo_chakra",
                group_id = GroupId_SpecialChakra,
                path_icon = "ui/icons/sixpathssenjutsu",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_inherit = 0,
                rate_birth = 0
            };
            hagoromo.base_stats = new BaseStats();
            hagoromo.base_stats.set("multiplier_damage", 0.15f);
            hagoromo.base_stats.set("multiplier_health", 0.15f);
            hagoromo.base_stats.set("multiplier_speed", 0.10f);
            hagoromo.base_stats.set("intelligence", 50f);
            hagoromo.base_stats.set("critical_chance", 0.10f);
            hagoromo.base_stats.set("critical_damage_multiplier", 0.10f);
            hagoromo.base_stats.set("experience", 15f);
            hagoromo.base_stats.set("chakra", 322f);

            addTraitToGame(hagoromo);
            #endregion

            #region Indra's Chakra
            // Indra's Chakra
            ActorTrait indra = new ActorTrait
            {
                id = "indra_chakra",
                group_id = GroupId_SpecialChakra,
                path_icon = "ui/icons/indra_chakra",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_inherit = 0,
                rate_birth = 0,
                action_death = new WorldAction(TransmigrateAction)
            };
            indra.base_stats = new BaseStats();
            indra.base_stats.set("intelligence", 10f);
            indra.base_stats.set("critical_chance", 0.10f);
            indra.base_stats.set("warfare", 5f);
            indra.base_stats.set("experience", 10f);
            indra.base_stats.set("skill_combat", 0.15f);
            indra.base_stats.set("diplomacy", -5f);
            indra.base_stats.set("stewardship", -5f);
            indra.base_stats.set("opinion", -25f);
            indra.base_stats.set("chakra", 322f);

            addTraitToGame(indra);
            #endregion

            #region Asura's Chakra
            // Asura's Chakra
            ActorTrait asura = new ActorTrait
            {
                id = "asura_chakra",
                group_id = GroupId_SpecialChakra,
                path_icon = "ui/icons/asura_chakra",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_inherit = 0,
                rate_birth = 0,
                action_death = new WorldAction(TransmigrateAction)
            };
            asura.base_stats = new BaseStats();
            asura.base_stats.set("multiplier_health", 0.05f);
            asura.base_stats.set("stamina", 20f);
            asura.base_stats.set("experience", 10f);
            asura.base_stats.set("skill_combat", 0.10f);
            asura.base_stats.set("diplomacy", 10f);
            asura.base_stats.set("stewardship", 10f);
            asura.base_stats.set("intelligence", 10f);
            asura.base_stats.set("opinion", 75f);
            asura.base_stats.set("chakra", 322f);

            addTraitToGame(asura);
            #endregion

            #region Blindness
            // Blindness
            ActorTrait blind = new ActorTrait
            {
                id = "trait_blind",
                group_id = GroupId_Dojutsu,
                path_icon = "ui/icons/blind",
                can_be_given = true,
                type = TraitType.Negative,
                needs_to_be_explored = false
            };
            blind.base_stats = new BaseStats();
            blind.base_stats.set("accuracy", -100f);
            blind.base_stats.set("range", -100f);
            addTraitToGame(blind);
            #endregion

            #region Hashirama Cells
            // Hashirama Cells
            ActorTrait hashirama = new ActorTrait
            {
                id = "hashi_cells",
                group_id = GroupId_Abilities,
                path_icon = "ui/icons/hashirama_cells",
                can_be_given = true,
                needs_to_be_explored = false
            };
            hashirama.base_stats = new BaseStats();
            hashirama.base_stats.set("health", 150f);
            hashirama.base_stats.set("multiplier_chakra", 0.15f);
            hashirama.action_attack_target = new AttackAction(JutsuLibrary.WoodReleaseAction);

            addTraitToGame(hashirama);
            #endregion

            #endregion

            #region Chakra Natures

            #region Fire Nature
            ActorTrait fire = new ActorTrait
            {
                id = "fireN",
                group_id = GroupId_ChakraN,
                path_icon = "ui/icons/fire_nature",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_birth = rare,
                rate_inherit = highChance

            };
            fire.base_stats = new BaseStats();
            fire.base_stats.set("multiplier_damage", 0.08f);
            fire.base_stats.set("critical_chance", 0.15f);
            fire.base_stats.set("critical_damage_multiplier", 0.10f);
            fire.base_stats.set("multiplier_speed", 0.05f);
            fire.base_stats.set("experience", 5f);
            fire.base_stats.set("chakra", 20f);
            fire.action_special_effect = new WorldAction(JutsuLibrary.AutoFireReleaseAtTarget);
            fire.special_effect_interval = 1.4f;
            addTraitToGame(fire);
            #endregion

            #region Earth Nature
            ActorTrait earth = new ActorTrait
            {
                id = "earthN",
                group_id = GroupId_ChakraN,
                path_icon = "ui/icons/earth_nature",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_birth = rare,
                rate_inherit = highChance

            };
            earth.base_stats = new BaseStats();
            earth.base_stats.set("multiplier_damage", 0.15f);
            earth.base_stats.set("armor", 4f);
            earth.base_stats.set("critical_chance", 0.04f);
            earth.base_stats.set("critical_damage_multiplier", 0.08f);
            earth.base_stats.set("multiplier_speed", -0.05f);
            earth.base_stats.set("experience", 5f);
            earth.base_stats.set("chakra", 20f);
            earth.action_attack_target = new AttackAction(JutsuLibrary.EarthReleaseAction);
            addTraitToGame(earth);
            #endregion

            #region Water Nature
            ActorTrait water = new ActorTrait
            {
                id = "waterN",
                group_id = GroupId_ChakraN,
                path_icon = "ui/icons/water_nature",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_birth = rare,
                rate_inherit = highChance
            };
            water.base_stats = new BaseStats();
            water.base_stats.set("multiplier_damage", 0.07f);
            water.base_stats.set("attack_speed", 0.10f);
            water.base_stats.set("critical_chance", 0.05f);
            water.base_stats.set("critical_damage_multiplier", 0.07f);
            water.base_stats.set("multiplier_speed", 0.07f);
            water.base_stats.set("experience", 5f);
            water.base_stats.set("chakra", 20f);
            water.action_attack_target = new AttackAction(JutsuLibrary.WaterReleaseAction);
            addTraitToGame(water);
            #endregion


            #region Wind Nature
            ActorTrait wind = new ActorTrait
            {
                id = "windN",
                group_id = GroupId_ChakraN,
                path_icon = "ui/icons/wind_nature",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_birth = rare,
                rate_inherit = highChance
            };
            wind.base_stats = new BaseStats();
            wind.base_stats.set("multiplier_damage", 0.08f);
            wind.base_stats.set("critical_chance", 0.25f);
            wind.base_stats.set("critical_damage_multiplier", 0.15f);
            wind.base_stats.set("multiplier_speed", 0.05f);
            wind.base_stats.set("experience", 5f);
            wind.base_stats.set("chakra", 20f);
            wind.action_special_effect = new WorldAction(JutsuLibrary.AutoWindReleaseAtTarget);
            wind.special_effect_interval = 1.4f;
            addTraitToGame(wind);
            #endregion

            #region Lightning Nature
            ActorTrait lightning = new ActorTrait
            {
                id = "lightningN",
                group_id = GroupId_ChakraN,
                path_icon = "ui/icons/lightning_nature",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_birth = rare,
                rate_inherit = highChance
            };
            lightning.base_stats = new BaseStats();
            lightning.base_stats.set("multiplier_damage", 0.15f);
            lightning.base_stats.set("multiplier_speed", 0.10f);
            lightning.base_stats.set("critical_chance", 0.08f);
            lightning.base_stats.set("critical_damage_multiplier", 0.20f);
            lightning.base_stats.set("experience", 5f);
            lightning.base_stats.set("chakra", 20f);
            lightning.action_attack_target = new AttackAction(JutsuLibrary.LightningReleaseAction);
            addTraitToGame(lightning);
            #endregion

            #region Storm Release
            ActorTrait stormR = new ActorTrait
            {
                id = "stormR",
                group_id = GroupId_ChakraN,
                path_icon = "ui/icons/storm_release",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_birth = 2,
                rate_inherit = alwaysChance

            };
            stormR.base_stats = new BaseStats();
            stormR.base_stats.set("multiplier_damage", 0.17f);
            stormR.base_stats.set("multiplier_speed", 0.15f);
            stormR.base_stats.set("critical_chance", 0.14f);
            stormR.base_stats.set("critical_damage_multiplier", 0.25f);
            stormR.base_stats.set("multiplier_speed", 0.15f);
            stormR.base_stats.set("accuracy", 30f);
            stormR.base_stats.set("chakra", 20f);
            stormR.base_stats.set("experience", 7f);
            stormR.action_attack_target = new AttackAction(JutsuLibrary.StormReleaseAction);
            addTraitToGame(stormR);
            #endregion

            #region Wood Release Trait
            ActorTrait wood = new ActorTrait
            {
                id = "wood_release",
                group_id = GroupId_ChakraN,
                path_icon = "ui/icons/wood_release",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_inherit = highChance,
                rate_birth = 1
            };
            wood.base_stats = new BaseStats();
            wood.base_stats.set("armor", 10f);
            wood.base_stats.set("health", 62f);
            wood.base_stats.set("damage", 34f);
            wood.base_stats.set("multiplier_damage", 0.10f);
            wood.base_stats.set("multiplier_speed", 0.10f);
            wood.base_stats.set("critical_chance", 0.10f);
            wood.base_stats.set("critical_damage_multiplier", 0.10f);
            wood.base_stats.set("chakra", 20f);
            wood.base_stats.set("experience", 7f);

            wood.action_attack_target = new AttackAction(JutsuLibrary.WoodReleaseAction);

            addTraitToGame(wood);
            #endregion

            #region Yin Release Trait
            ActorTrait yin = new ActorTrait
            {
                id = "yin_release",
                group_id = GroupId_ChakraN,
                path_icon = "ui/icons/yin_release",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_birth = 2,
                rate_inherit = highChance
            };
            yin.base_stats = new BaseStats();
            yin.base_stats.set("armor", 10f);
            yin.base_stats.set("health", 62f);
            yin.base_stats.set("damage", 34f);
            yin.base_stats.set("critical_chance", 0.10f);
            yin.base_stats.set("multiplier_damage", 0.10f);
            yin.base_stats.set("multiplier_speed", 0.10f);
            yin.base_stats.set("critical_damage_multiplier", 0.25f);
            yin.base_stats.set("chakra", 25f);
            yin.base_stats.set("experience", 7f);
            yin.action_special_effect = new WorldAction(YinYangFuse);

            addTraitToGame(yin);
            #endregion

            #region Yang Release Trait
            ActorTrait yang = new ActorTrait
            {
                id = "yang_release",
                group_id = GroupId_ChakraN,
                path_icon = "ui/icons/yang_release",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_birth = 2,
                rate_inherit = highChance
            };
            yang.base_stats = new BaseStats();
            yang.base_stats.set("armor", 10f);
            yang.base_stats.set("health", 62f);
            yang.base_stats.set("damage", 34f);
            yang.base_stats.set("critical_chance", 0.10f);
            yang.base_stats.set("multiplier_damage", 0.10f);
            yang.base_stats.set("multiplier_speed", 0.10f);
            yang.base_stats.set("critical_damage_multiplier", 0.25f);
            yang.base_stats.set("chakra", 25f);
            yang.base_stats.set("experience", 7f);

            //yang.action_attack_target = new AttackAction(YinYangFuse);

            addTraitToGame(yang);
            #endregion

            #region YinYang Release Trait
            ActorTrait yinyang = new ActorTrait
            {
                id = "yinyang_release",
                group_id = GroupId_ChakraN,
                path_icon = "ui/icons/yinyang_release",
                can_be_given = true,
                needs_to_be_explored = false
            };
            yinyang.base_stats = new BaseStats();
            yinyang.base_stats.set("armor", 15f);
            yinyang.base_stats.set("health", 124f);
            yinyang.base_stats.set("damage", 58f);
            yinyang.base_stats.set("critical_chance", 0.15f);
            yinyang.base_stats.set("multiplier_damage", 0.15f);
            yinyang.base_stats.set("multiplier_speed", 0.15f);
            yinyang.base_stats.set("critical_damage_multiplier", 0.40f);
            yinyang.base_stats.set("chakra", 50f);
            yinyang.base_stats.set("experience", 12f);
            //yinyang.action_attack_target = new AttackAction(JutsuLibrary.YinYangReleaseAction);

            addTraitToGame(yinyang);
            #endregion

            #endregion

            #region Jinchuriki Progression

            #region Nine-Tails Jinchuriki
            ActorTrait ninetailsjinchuriki = new ActorTrait
            {
                id = "nine_tails_jinchuriki",
                group_id = GroupId_Jinchuriki,
                path_icon = "ui/icons/9Tails",
                can_be_given = true,
                needs_to_be_explored = false,
   
            };
            ninetailsjinchuriki.base_stats = new BaseStats();
            ninetailsjinchuriki.base_stats.set("damage", 10f);
            ninetailsjinchuriki.base_stats.set("speed", 5f);
            ninetailsjinchuriki.base_stats.set("critical_chance", 0.02f);
            ninetailsjinchuriki.base_stats.set("intelligence", 10f);
            ninetailsjinchuriki.base_stats.set("multiplier_lifespan", 0.30f);
            ninetailsjinchuriki.base_stats.set("health", 128f);
            ninetailsjinchuriki.base_stats.set("chakra", 106f);
            ninetailsjinchuriki.action_attack_target = new AttackAction(JutsuLibrary.TailedBeastBombAction);
            ninetailsjinchuriki.action_get_hit = new GetHitAction(JutsuLibrary.NineTailsJinchuriki);
            ninetailsjinchuriki.action_death = new WorldAction(JutsuLibrary.BaryonMode);


            addTraitToGame(ninetailsjinchuriki);
            #endregion

            // Ten-Tails Jinchuriki
            #region Ten Tails Jinchuriki
            ActorTrait tentailsjinchuriki = new ActorTrait
            {
                id = "ten_tails_jinchuriki",
                group_id = GroupId_Jinchuriki,
                path_icon = "ui/icons/10Tails",
                can_be_given = true,
                needs_to_be_explored = false
            };

            tentailsjinchuriki.action_attack_target = new AttackAction(JutsuLibrary.KCMBlastAction);
            tentailsjinchuriki.action_attack_target = (AttackAction)Delegate.Combine(tentailsjinchuriki.action_attack_target, new AttackAction(JutsuLibrary.TailedBeastBombAction));
            tentailsjinchuriki.action_attack_target = (AttackAction)Delegate.Combine(tentailsjinchuriki.action_attack_target, new AttackAction(JutsuLibrary.ArmorBreakAction));
            tentailsjinchuriki.action_get_hit = new GetHitAction(JutsuLibrary.TenTailsJinchuriki);

            addTraitToGame(tentailsjinchuriki);
            #endregion

            #endregion

            #region Taijutsu Master Trait
            // Taijutsu Master
            #region Taijutsu Master
            ActorTrait taijutsu_master = new ActorTrait
            {
                id = "taijutsu_master",
                group_id = GroupId_Abilities,
                path_icon = "ui/icons/taijutsu_fist",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_birth = veryRare,
                rate_inherit = lowChance
            };
            taijutsu_master.base_stats = new BaseStats();
            taijutsu_master.base_stats.set("damage", 12f);
            taijutsu_master.base_stats.set("attack_speed", 0.50f);
            taijutsu_master.base_stats.set("multiplier_speed", 0.50f);
            taijutsu_master.base_stats.set("multiplier_damage", 0.40f);
            taijutsu_master.base_stats.set("skill_combat", 0.40f);

            taijutsu_master.action_attack_target = new AttackAction(JutsuLibrary.ArmorBreakAction);
            taijutsu_master.action_get_hit = (GetHitAction)Delegate.Combine(taijutsu_master.action_get_hit, new GetHitAction(JutsuLibrary.StrongPunch));

            addTraitToGame(taijutsu_master);
            #endregion
            #endregion

            #region Eight Inner Gates Trait
            ActorTrait eight_inner_gates = new ActorTrait
            {
                id = "eight_inner_gates",
                group_id = GroupId_Abilities,
                path_icon = "ui/icons/eight_gates",
                can_be_given = true,
                needs_to_be_explored = false,
                rate_birth = veryRare,
                rate_inherit = lowChance
            };
            eight_inner_gates.base_stats = new BaseStats();
            eight_inner_gates.base_stats.set("health", 50f);
            eight_inner_gates.base_stats.set("multiplier_health", 0.10f);
            eight_inner_gates.base_stats.set("multiplier_speed", 0.10f);
            eight_inner_gates.base_stats.set("multiplier_attack_speed", 0.10f);
            eight_inner_gates.base_stats.set("chakra", 28f);
            eight_inner_gates.action_get_hit = new GetHitAction(JutsuLibrary.EightInnerGates);

            addTraitToGame(eight_inner_gates);
            #endregion

            #region Akatsuki Member Trait
            ActorTrait akatsuki = new ActorTrait
            {
                id = "trait_akatsuki",
                group_id = GroupId_Abilities,
                path_icon = "ui/icons/akatsuki",
                can_be_given = true,
                needs_to_be_explored = false
            };
            akatsuki.base_stats = new BaseStats();
            akatsuki.base_stats.set("health", 98f);
            akatsuki.base_stats.set("damage", 28f);
            akatsuki.base_stats.set("speed", 14f);
            akatsuki.base_stats.set("skill_combat", 0.60f);
            akatsuki.base_stats.set("critical_chance", 0.25f);
            akatsuki.base_stats.set("critical_damage_multiplier", 0.25f);
            akatsuki.base_stats.set("intelligence", 25f);
            akatsuki.base_stats.set("warfare", 15f);
            akatsuki.base_stats.set("diplomacy", -100f);
            akatsuki.base_stats.set("loyalty_traits", -100f);
            akatsuki.base_stats.set("stewardship", -50f);
            akatsuki.action_special_effect = new WorldAction(AkatsukiAwaken);

            addTraitToGame(akatsuki);
            #endregion


        }

        private static void loadJutsus()
        {
            #region Rasengan
            ActorTrait rasenganJ = new ActorTrait
            {
                id = "rasenganJ",
                group_id = GroupId_Jutsus,
                path_icon = "ui/icons/jutsuA",
                can_be_given = true,
                needs_to_be_explored = false
            };
            rasenganJ.action_special_effect = new WorldAction(JutsuLibrary.RasenganAction);
            addTraitToGame(rasenganJ);

            #endregion

            #region Rasenshuriken
            ActorTrait rasenshurikenJ = new ActorTrait
            {
                id = "rasenshurikenJ",
                group_id = GroupId_Jutsus,
                path_icon = "ui/icons/jutsuS",
                can_be_given = true,
                needs_to_be_explored = false
            };
            rasenshurikenJ.action_special_effect = new WorldAction(JutsuLibrary.AutoRasenshurikenAtTarget);
            rasenshurikenJ.special_effect_interval = 7f;
            addTraitToGame(rasenshurikenJ);

            #endregion

            #region Chidori 
            ActorTrait chidoriJ = new ActorTrait
            {
                id = "chidoriJ",
                group_id = GroupId_Jutsus,
                path_icon = "ui/icons/jutsuA",
                can_be_given = true,
                needs_to_be_explored = false
            };
            chidoriJ.action_attack_target = new AttackAction(JutsuLibrary.ChidoriAction);
            addTraitToGame(chidoriJ);
            #endregion

            #region Shadow Clone Jutsu
            ActorTrait shadowCloneJ = new ActorTrait
            {
                id = "shadow_clonej",
                group_id = GroupId_Jutsus,
                path_icon = "ui/icons/jutsuB",
                can_be_given = true,
                needs_to_be_explored = false
            };
            shadowCloneJ.action_get_hit = new GetHitAction(JutsuLibrary.ShadowCloneAction);
            addTraitToGame(shadowCloneJ);

            #endregion
        }

        private static void loadSenjutsu()
        {
            #region Frog Sage Mode Trait
            ActorTrait frogSage = new ActorTrait
            {
                id = "frog_sage_mode",
                group_id = GroupId_Senjutsu,
                path_icon = "ui/icons/sage_mode",
                can_be_given = true,
                needs_to_be_explored = false
            };
            frogSage.base_stats = new BaseStats();
            frogSage.action_attack_target = new AttackAction(JutsuLibrary.FrogSummonAction);
            addTraitToGame(frogSage);
            #endregion

            #region Slug Sage Mode Trait
            ActorTrait slugSage = new ActorTrait
            {
                id = "slug_sage_mode",
                group_id = GroupId_Senjutsu,
                path_icon = "ui/icons/slug_sage",
                can_be_given = true,
                needs_to_be_explored = false
            };
            slugSage.base_stats = new BaseStats();
            slugSage.action_attack_target = new AttackAction(JutsuLibrary.StrengthOfHundredAction);
            addTraitToGame(slugSage);
            #endregion

            #region Snake Sage Mode Trait
            ActorTrait snakeSage = new ActorTrait
            {
                id = "snake_sage_mode",
                group_id = GroupId_Senjutsu,
                path_icon = "ui/icons/snake_sage",
                can_be_given = true,
                needs_to_be_explored = false
            };
            snakeSage.base_stats = new BaseStats();
            addTraitToGame(snakeSage);
            #endregion

            #region Wood Sage Mode Trait
            ActorTrait woodSage = new ActorTrait
            {
                id = "wood_sage_mode",
                group_id = GroupId_Senjutsu,
                path_icon = "ui/icons/wood_sage",
                can_be_given = true,
                needs_to_be_explored = false
            };
            woodSage.base_stats = new BaseStats();
            woodSage.base_stats.set("multiplier_damage", 0.25f);
            woodSage.base_stats.set("multiplier_health", 0.20f);
            woodSage.base_stats.set("armor", 20f);
            addTraitToGame(woodSage);
            #endregion

            #region Six Paths Sage Mode Trait
            ActorTrait sixPathsSage = new ActorTrait
            {
                id = "six_paths_sage_mode",
                group_id = GroupId_Senjutsu,
                path_icon = "ui/icons/iconSPSM",
                can_be_given = true,
                needs_to_be_explored = false
            };
            sixPathsSage.base_stats = new BaseStats();
            sixPathsSage.base_stats.set("damage", 100f);
            sixPathsSage.base_stats.set("health", 1000f);
            sixPathsSage.base_stats.set("speed", 25f);
            sixPathsSage.base_stats.set("armor", 25f);
            sixPathsSage.base_stats.set("multiplier_damage", 0.25f);
            sixPathsSage.base_stats.set("multiplier_health", 0.25f);
            sixPathsSage.base_stats.set("multiplier_speed", 0.25f);
            sixPathsSage.base_stats.set("chakra", 206);
            addTraitToGame(sixPathsSage);
            #endregion
        }

        #region Helpers

        public static bool AkatsukiAwaken(BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (pTarget.a.hasTrait("evil") && !pTarget.a.hasTrait("trait_akatsuki") && pTarget.a.getAge() >= 28)
            {
                if (UnityEngine.Random.value < 0.01f)
                {
                    pTarget.a.addTrait("trait_akatsuki");
                    return true;
                }

            }

            return false;
        }

        #region YinYang Release Fusion
        public static bool YinYangFuse(BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (pTarget.a.hasTrait("yang_release"))
            {
                pTarget.a.removeTrait("yang_release");
                pTarget.a.removeTrait("yin_release");
                pTarget.a.addTrait("yinyang_release");
                return true;
            }
            return false;
        }
        #endregion

        #region addTraitToGame
        private static void addTraitToGame(ActorTrait trait)
        {
            if (AssetManager.traits.dict.ContainsKey(trait.id)) return;

            AssetManager.traits.add(trait);
            myListTraits.Add(trait);
            reAddToBirthTraitPool(trait);
        }

        private static void reAddToBirthTraitPool(ActorTrait trait)
        {
            if (trait == null || trait.rate_birth <= 0) return;
            if (AssetManager.traits?.pot_traits_birth == null) return;

            if (AssetManager.traits.pot_traits_birth.Count == 0) return;

            int existingCount = AssetManager.traits.pot_traits_birth.Count(t => t == trait || t.id == trait.id);
            for (int i = existingCount; i < trait.rate_birth; i++)
            {
                AssetManager.traits.pot_traits_birth.Add(trait);
            }
        }
        #endregion

        #region populateListOppositeTraits
        //credits to darkiexx for this
        private static void populateListOppositeTraits()
        {
            foreach (ActorTrait trait in myListTraits)
            {
                if (trait.opposite_traits == null) trait.opposite_traits = new HashSet<ActorTrait>();
            }

            // Core opposite sets
            string[] chakraReserveIds = {
            "high_chakra_reserve", "low_chakra_reserve", "greater_chakra_reserve", "vast_chakra_reserve"
            };
            setGroupOpposites(chakraReserveIds);

            string[] philosophyIds = {
            "will_of_fire", "curse_of_hatred"
            };
            setGroupOpposites(philosophyIds);

            string[] chakraLineageIds = {
            "indra_chakra", "asura_chakra"
            };
            setGroupOpposites(chakraLineageIds);

            // Dojutsu
            string[] allDojutsu = {
            "sharingan",
            "mangekyo_sharingan", "madara_eternal_mangekyo", "eternal_mangekyo",
            "byakugan", "trait_blind", "sasuke_rinnegan"
            };
            setGroupOpposites(allDojutsu);

            // Clans
            string[] clanIds = { "uchiha_clan", "hyuga_clan", "uzumaki_clan", "senju_clan", "akimichi_clan", "lee_clan" };
            setGroupOpposites(clanIds);

            string[] jinchurikiIds = { "nine_tails_jinchuriki", "ten_tails_jinchuriki" };
            setGroupOpposites(jinchurikiIds);

            // Ranks
            string[] rankIds = {
            "rank_academy_student", "rank_genin", "rank_chunin",
            "rank_jonin", "rank_anbu", "rank_kage"
            };
            setGroupOpposites(rankIds);

            string[] sageModeIds = {
            "frog_sage_mode", "slug_sage_mode", "snake_sage_mode",
            "wood_sage_mode"
            };
            setGroupOpposites(sageModeIds);
        }
        #endregion

        #region setGroupOpposites
        private static void setGroupOpposites(string[] ids)
        {
            foreach (string id in ids)
            {
                ActorTrait current = myListTraits.FirstOrDefault(t => t.id == id);
                if (current == null) continue;

                foreach (string otherId in ids)
                {
                    if (id == otherId) continue;
                    ActorTrait other = myListTraits.FirstOrDefault(t => t.id == otherId);
                    if (other != null) current.opposite_traits.Add(other);
                }
            }
        }
        #endregion

        #region TransmigrateAction
        public static bool TransmigrateAction(BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null) return false;

            string traitToPass = "";
            string clanRequirement = "";

            if (pTarget.a.hasTrait("indra_chakra"))
            {
                traitToPass = "indra_chakra";
                clanRequirement = "uchiha_clan";
            }
            else if (pTarget.a.hasTrait("asura_chakra"))
            {
                traitToPass = "asura_chakra";
                clanRequirement = "senju_uzumaki";
            }

            if (traitToPass == "") return false;

            List<Actor> allUnits = World.world.units.getSimpleList();
            List<Actor> validHeirs = new List<Actor>();

            foreach (Actor a in allUnits)
            {
                if (a.isAlive() && a.asset.id == "human" && a.data.getAge() <= 24)
                {
                    if (!a.hasTrait("indra_chakra") && !a.hasTrait("asura_chakra"))
                    {
                        bool isEligible = false;

                        if (clanRequirement == "uchiha_clan" && a.hasTrait("uchiha_clan"))
                            isEligible = true;
                        else if (clanRequirement == "senju_uzumaki" && (a.hasTrait("senju_clan") || a.hasTrait("uzumaki_clan")))
                            isEligible = true;

                        if (isEligible)
                        {
                            validHeirs.Add(a);
                        }
                    }
                }
            }

            if (validHeirs.Count > 0)
            {
                Actor heir = validHeirs[UnityEngine.Random.Range(0, validHeirs.Count)];
                heir.addTrait(traitToPass);

                string label = (traitToPass == "indra_chakra") ? "Indra" : "Asura";
                ShinobiWorldLogs.AddWorldLog(
                    "log_" + label.ToLower() + "_reincarnation",
                    "worldlog_" + label.ToLower() + "_reincarnation",
                    "ui/icons/" + label.ToLower() + "_chakra",
                    heir);
                return true;
            }

            return false;
        }
        #endregion

        #endregion
    }
}