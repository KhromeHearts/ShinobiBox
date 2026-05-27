using System;
using NeoModLoader.api;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using ReflectionUtility;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Threading.Tasks;
using System.Reflection;
using ai;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;
using NeoModLoader.constants;
using System.Net;

namespace ShinobiBox
{
    public static class JutsuLibrary
    {
        private const float ChaosAgeDurationSeconds = 60f;
        private static readonly Dictionary<long, Kingdom> MadaraTamedOriginalKingdoms = new Dictionary<long, Kingdom>();
        private static bool _chaosRevertPending;
        private static float _chaosRevertTimer;
        #region Sharingan Genjutsu 1-3
        // Genjutsu 1
        public static bool Sharingan1Action(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.10f) return false;
            if (pTarget.a.hasStatus("genjutsu_1")) return false;

            // 20 Chakra Cost
            if (!CheckAndConsumeChakra(pSelf.a, 20f)) return false;

            pTarget.a.addStatusEffect("genjutsu_1", 3f);
            return true;
        }

        // Genjutsu 2
        public static bool Sharingan2Action(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.10f) return false;
            if (pTarget.a.hasStatus("genjutsu_2")) return false;

            // 30 Chakra Cost
            if (!CheckAndConsumeChakra(pSelf.a, 30f)) return false;

            pTarget.a.addStatusEffect("genjutsu_2", 4f);
            return true;
        }

        // Genjutsu 3
        public static bool Sharingan3Action(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.10f) return false;
            if (pTarget.a.hasStatus("genjutsu_3")) return false;

            // 50 Chakra Cost
            if (!CheckAndConsumeChakra(pSelf.a, 50f)) return false;

            pTarget.a.addStatusEffect("genjutsu_3", 5f);
            return true;
        }
        #endregion

        #region Sharingan Progression
        public static bool SharinganProgression(BaseSimObject pSelf, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            Actor actor = pSelf.a;

            if (actor.hasTrait("mangekyo_sharingan") || actor.hasTrait("eternal_mangekyo") || actor.hasTrait("madara_eternal_mangekyo") || actor.hasTrait("trait_blind"))
            {
                ClearSharinganTomoeStatuses(actor);
                return false;
            }

            if (!actor.has_attack_target || actor.attack_target == null || actor.attack_target.a == null || !actor.attack_target.a.isAlive()) return false;

            BaseSimObject attackTarget = actor.attack_target;
            WorldTile targetTile = attackTarget.current_tile;

            int kills = actor.data.kills;
            int age = actor.data.getAge();

            if (kills >= 7 || (age >= 38 && UnityEngine.Random.value < 0.001f))
            {
                actor.addStatusEffect("status_sharingan_3t", 30f);
                actor.finishStatusEffect("status_sharingan_2t");
                actor.finishStatusEffect("status_sharingan_1t");
                return true;
            }

            if (kills >= 4 || (age >= 24 && UnityEngine.Random.value < 0.001f))
            {
                actor.addStatusEffect("status_sharingan_2t", 30f);
                actor.finishStatusEffect("status_sharingan_3t");
                actor.finishStatusEffect("status_sharingan_1t");
                return true;
            }

            if (kills >= 1 || (age >= 12 && UnityEngine.Random.value < 0.008f))
            {
                actor.addStatusEffect("status_sharingan_1t", 30f);
                actor.finishStatusEffect("status_sharingan_3t");
                actor.finishStatusEffect("status_sharingan_2t");
                return true;
            }

            return false;
        }

        public static void ClearSharinganTomoeStatuses(Actor actor)
        {
            if (actor == null) return;

            actor.finishStatusEffect("status_sharingan_1t");
            actor.finishStatusEffect("status_sharingan_2t");
            actor.finishStatusEffect("status_sharingan_3t");
        }

        public static bool SharinganActions(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            Actor actor = pSelf.a;

            if (actor.hasStatus("status_sharingan_3t"))
            {
                return Sharingan3Action(pSelf, pTarget, pTile);
            }
            if (actor.hasStatus("status_sharingan_2t"))
            {
                return Sharingan2Action(pSelf, pTarget, pTile);
            }
            if (actor.hasStatus("status_sharingan_1t"))
            {
                return Sharingan1Action(pSelf, pTarget, pTile);
            }

            return false;
        }
        #endregion

        #region Rasengan
        public static bool RasenganAction(BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            if (pTarget.a.has_attack_target)
            {
                if (UnityEngine.Random.value > 0.90f) return false;
                // 50 Chakra Cost
                if (!CheckAndConsumeChakra(pTarget.a, 50f)) return false;

                pTarget.a.addStatusEffect("status_rasengan", 5f);
                return true;
            }
            return false;
        }
        #endregion

        #region Chidori
        public static bool ChidoriAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.10f) return false;

            if (!CheckAndConsumeChakra(pSelf.a, 50f)) return false;

            pSelf.a.addStatusEffect("status_chidori", 4f);

            if (pTarget.current_tile != null)
            {
                pTarget.a.getHit(110f, true, AttackType.Other, pSelf.a, true);

                if (pSelf.a.hasStatus("status_chidori"))
                {
                    MapBox.spawnLightningSmall(pTarget.current_tile, 0.25f, pTarget.a);
                    pTarget.a.getHit(80f, true, AttackType.Other, pSelf.a, true);
                }
            }

            return true;
        }
        #endregion

        public static bool AutoChidoriAtTarget(BaseSimObject pSelf, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            Actor target = GetValidAttackTarget(pSelf.a, 24f);
            if (target == null) return false;

            return ChidoriAction(pSelf, target, target.current_tile);
        }


        #region Rasenshuriken
        public static bool RasenshurikenAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget.a == null || !pTarget.a.isAlive()) return false;
            if (UnityEngine.Random.value > 0.10f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 100f)) return false;
            bool hasAttackTarget = pSelf.a.has_attack_target;

            if (pTarget.current_tile != null && pSelf.current_tile != null && hasAttackTarget)
            {
                World.world.projectiles.spawn(pSelf, pTarget, "projectile_rasenshuriken", pSelf.current_tile.posV3, pTarget.current_tile.posV3);
                return true;
            }
            return false;
        }

        public static bool AutoRasenshurikenAtTarget(BaseSimObject pSelf, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            Actor target = GetValidAttackTarget(pSelf.a, 24f);
            if (target == null) return false;

            return RasenshurikenAction(pSelf, target, target.current_tile);
        }
        #endregion

        #region Rinne-Sharingan Jutsus

        #region Rinne-Sharingan: Infinite Tsukuyomi
        public static bool InfiniteTsukuyomiAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (!pSelf.a.hasTrait("yin_release") && !pSelf.a.hasTrait("yinyang_release")) return false;

            int used = 0;
            pSelf.a.data.get("it_used_once", out used);
            if (used > 0) return false;

            if (UnityEngine.Random.value > 0.01f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 400f)) return false;

            int affectedCount = 0;
            foreach (Actor unit in MapBox.instance.units)
            {
                if (unit == null || !unit.isAlive()) continue;
                unit.addStatusEffect("status_infinite_tsukuyomi", 60f);
                unit.addStatusEffect("sleeping", 60f);
                unit.addStatusEffect("had_good_dream", 60f);
                affectedCount++;
            }

            // 0.1% max chakra bonus per living unit affected
            float bonusMultiplier = affectedCount * 0.001f;
            pSelf.a.data.set("it_bonus_multiplier", bonusMultiplier);

            var state = ChakraSystem.Get(pSelf.a);
            state.max = ChakraSystem.CalculateBaseMaxChakra(pSelf.a);
            state.current = state.max;

            pSelf.a.restoreHealth(pSelf.a.getMaxHealth());
            pSelf.a.finishStatusEffect("sleeping");

            SetWorldAgeChaos();
            pSelf.a.data.set("it_used_once", 1);

            ShinobiWorldLogs.AddWorldLog("log_infinite_tsukuyomi", "worldlog_infinite_tsukuyomi", "ui/icons/rinne_sharingan", pSelf.a);

            return true;
        }
        #endregion

        #region Rinne-Sharingan: Heavenly Ice Chamber
        public static bool HeavenlyIceChamberAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            if (pSelf.current_tile == null) return false;

            if (UnityEngine.Random.value > 0.10f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 200f)) return false;

            for (int i = 0; i < 40; ++i)
            {
                World.world.loopWithBrush(pSelf.current_tile, Brush.get(16, "circ_"), new PowerActionWithID(PowerLibrary.drawTemperatureMinus), "temperatureMinus");
            }

            pSelf.a.finishStatusEffect("frozen");
            return true;
        }
        #endregion

        #endregion

        #region Rinnegan Abilities

        #region Rinnegan Meteor
        public static bool RinneganMeteorAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.008f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 500f)) return false;

            if (pTarget.current_tile != null)
            {
                Meteorite.spawnMeteorite(pTarget.current_tile, pSelf.a);

                ShinobiWorldLogs.AddWorldLog("log_rinnegan_meteor", "worldlog_rinnegan_meteor", "ui/icons/rinnegan", pSelf.a);
                return true;
            }
            return false;
        }
        #endregion

        #region Rinnegan: Naraka Path (King of Hell)
        public static bool NarakaPathHealAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || !pSelf.a.isAlive()) return false;

            if (pSelf.a.data.health >= pSelf.a.getMaxHealth() * 0.3f) return false;

            if (UnityEngine.Random.value > 0.05f) return false;

            if (pSelf.a.hasStatus("koh_sprite")) return false;

            if (!CheckAndConsumeChakra(pSelf.a, 500f)) return false;

            pSelf.a.addStatusEffect("frozen", 4f);
            pSelf.a.addStatusEffect("koh_sprite", 4f);
            pSelf.a.addStatusEffect("powerup", 25f);
            int healAmount = (int)(pSelf.a.getMaxHealth() * 0.40f) + (pSelf.a.data.kills * 5);
            pSelf.a.restoreHealth(healAmount);

            if (ShinobiConfig.EnableWorldTips)
            {
                ShinobiWorldLogs.AddWorldLog("log_naraka_path", "worldlog_naraka_path", "ui/icons/rinnegan", pSelf.a);
            }

            return true;
        }
        #endregion

        #region Rinnegan: Six Paths Technique

        public static bool SixPathsTechnique(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.10f) return false;

            float roll = UnityEngine.Random.value;
            if (roll < 0.35f)
            {
                return HumanPathAttack(pSelf, pTarget);
            }
            if (roll < 0.10f)
            {
                return AsuraPathAttack(pSelf, pTarget);
            }
            return AnimalPathSummon(pSelf, pTarget);
        }

        // Deva Path: Shinra Tensei (Almighty Push)
        public static bool DevaShinraTensei(BaseSimObject pSelf, BaseSimObject pAttackedBy, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (pSelf.current_tile == null) return false;

            if (UnityEngine.Random.value > 0.15f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 80f)) return false;

            WorldTile forceTile = pSelf.current_tile;
            if (pAttackedBy != null && pAttackedBy.a != null && pAttackedBy.a.isAlive() && pAttackedBy.current_tile != null)
            {
                forceTile = pAttackedBy.current_tile;
            }
            else if (pTile != null)
            {
                forceTile = pTile;
            }

            EffectsLibrary.spawnExplosionWave(forceTile.posV3, 2.2f, 1.2f);
            World.world.applyForceOnTile(forceTile, 10, 2.6f, true, 0);

            return true;
        }



        private static bool AsuraPathAttack(BaseSimObject pSelf, BaseSimObject pTarget)
        {
            if (pTarget.current_tile == null || pSelf.current_tile == null) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 110f)) return false;

            ActionLibrary.throwBombAtTile(pSelf, pTarget.current_tile);
            pTarget.a.getHit(75f, true, AttackType.Fire, pSelf.a, true);
            return true;
        }

        private static bool HumanPathAttack(BaseSimObject pSelf, BaseSimObject pTarget)
        {
            if (!CheckAndConsumeChakra(pSelf.a, 200f)) return false;

            pTarget.a.getHit(250f, true, AttackType.Other, pSelf.a, true);

            if (UnityEngine.Random.value < 0.95f && !pSelf.a.hasTrait("strong_minded"))
            {
                pSelf.a.restoreHealth((int)(pSelf.a.getMaxHealth() * 0.10f));
            }

            if (UnityEngine.Random.value < 0.40f)
            {
                pSelf.a.stats["intelligence"] += 1f;
            }
            else if (UnityEngine.Random.value < 0.02f)
            {
                pSelf.a.stats["intelligence"] += 3f;
            }

            return true;
        }

        private static bool AnimalPathSummon(BaseSimObject pSelf, BaseSimObject pTarget)
        {
            if (!CheckAndConsumeChakra(pSelf.a, 100f)) return false;
            if (pSelf.current_tile == null) return false;

            string[] summonPool = { "wolf", "bear", "snake", "crocodile" };
            int summonCount = UnityEngine.Random.Range(1, 3);
            int successCount = 0;

            for (int i = 0; i < summonCount; i++)
            {
                string unitId = summonPool[UnityEngine.Random.Range(0, summonPool.Length)];
                WorldTile spawnTile = Toolbox.getRandomTileWithinDistance(pSelf.current_tile, 2) ?? pSelf.current_tile;
                Actor summon = World.world.units.createNewUnit(unitId, spawnTile, true, 1f);
                if (summon == null) continue;

                if (pSelf.a.kingdom != null)
                {
                    summon.setKingdom(pSelf.a.kingdom);
                }
                if (pSelf.a.city != null)
                {
                    summon.joinCity(pSelf.a.city);
                }

                summon.addStatusEffect("status_rinnegan_controlled", 120f);

                // Focus summons toward the current target.
                summon.attack_target = pTarget.a;
                summon.beh_actor_target = pTarget.a;
                successCount++;
            }

            return successCount > 0;
        }

        #endregion


        #endregion

        #region Nine Tails Jinchuriki

        #region Progression
        public static bool NineTailsJinchuriki(BaseSimObject pSelf, BaseSimObject pAttackedBy, WorldTile pTile = null)
        {
            if (pSelf?.a == null || !pSelf.a.isAlive()) return false;

            Actor actor = pSelf.a;
            if (!actor.hasTrait("nine_tails_jinchuriki")) return false;
            if (actor.hasStatus("status_jinchuriki_baryon_mode")) return false;

            int level = actor.data.level;
            bool hasAttackTarget = actor.has_attack_target;

            if (hasAttackTarget)
            {
                if (level >= 11 && !actor.hasStatus("status_jinchuriki_avatar"))
                {
                    ClearLowerForms(actor);
                    actor.addStatusEffect("status_jinchuriki_avatar", 45f);
                    if (!ShinobiShi.ava)
                    {
                        actor.restoreHealth(actor.getMaxHealth());
                        ShinobiShi.ava = true;
                    }
                    return true;
                }

                if (actor.hasStatus("status_jinchuriki_avatar"))
                {
                    return false;
                }

                if (level >= 9 && (actor.intelligence >= 20f || actor.diplomacy >= 20f) && !actor.hasStatus("status_jinchuriki_kcm2"))
                {
                    ClearLowerForms(actor);
                    actor.addStatusEffect("status_jinchuriki_kcm2", 45f);
                    if (!ShinobiShi.kura2)
                    {
                        actor.restoreHealth(actor.getMaxHealth());
                        if (ShinobiConfig.EnableWorldTips)
                            ShinobiWorldLogs.AddWorldLog("log_mastered_kurama", "worldlog_mastered_kurama", "ui/icons/kcm2", actor);
                        ShinobiShi.kura2 = true;
                    }
                    return true;
                }

                if (actor.hasStatus("status_jinchuriki_kcm2"))
                {
                    return false;
                }

                if (level >= 7 && (actor.intelligence >= 20f || actor.diplomacy >= 20f) && !actor.hasStatus("status_jinchuriki_kcm1"))
                {
                    ClearLowerForms(actor);
                    actor.addStatusEffect("status_jinchuriki_kcm1", 120f);
                    if (!ShinobiShi.kura1)
                    {
                        actor.restoreHealth(actor.getMaxHealth());
                        if (ShinobiConfig.EnableWorldTips)
                            ShinobiWorldLogs.AddWorldLog("log_controlled_kurama", "worldlog_controlled_kurama", "ui/icons/kcm1", actor);
                        ShinobiShi.kura1 = true;
                    }
                    return true;
                }

                if (actor.hasStatus("status_jinchuriki_kcm1"))
                {
                    return false;
                }

                if (level >= 5 && !actor.hasStatus("status_jinchuriki_incomplete_beast"))
                {
                    ClearLowerForms(actor);
                    actor.addStatusEffect("status_jinchuriki_incomplete_beast", 120f);
                    if (!ShinobiShi.incomp)
                    {
                        actor.restoreHealth(actor.getMaxHealth());
                        if (ShinobiConfig.EnableWorldTips)
                            ShinobiWorldLogs.AddWorldLog("log_beast_awakens", "worldlog_beast_awakens", "ui/icons/incomplete", actor);
                        ShinobiShi.incomp = true;
                    }
                    return true;
                }

                if (actor.hasStatus("status_jinchuriki_incomplete_beast"))
                {
                    return false;
                }

                if (level >= 4 && !actor.hasStatus("status_jinchuriki_v2_cloak"))
                {
                    ClearLowerForms(actor);
                    actor.addStatusEffect("status_jinchuriki_v2_cloak", 120f);
                    actor.restoreHealth(actor.getMaxHealth());
                    if (ShinobiConfig.EnableWorldTips && !ShinobiShi.beastAwk)
                    {
                        ShinobiWorldLogs.AddWorldLog("log_beast_awakens", "worldlog_beast_awakens", "ui/icons/v2", actor);
                        ShinobiShi.beastAwk = true;
                    }
                    return true;
                }

                if (actor.hasStatus("status_jinchuriki_v2_cloak"))
                {
                    return false;
                }

                if (level >= 3 && !actor.hasStatus("status_jinchuriki_v1_cloak"))
                {
                    ClearLowerForms(actor);
                    actor.addStatusEffect("status_jinchuriki_v1_cloak", 120f);
                    if (ShinobiConfig.EnableWorldTips && !ShinobiShi.demShroud)
                    {
                        ShinobiWorldLogs.AddWorldLog("log_demon_shroud", "worldlog_demon_shroud", "ui/icons/v1", actor);
                        ShinobiShi.demShroud = true;
                    }
                    return true;
                }

                if (level >= 2 && !actor.hasStatus("status_jinchuriki_initial_release"))
                {
                    actor.addStatusEffect("status_jinchuriki_initial_release", 120f);
                    if (ShinobiConfig.EnableWorldTips && !ShinobiShi.sealWeak)
                    {
                        ShinobiWorldLogs.AddWorldLog("log_seal_weakens", "worldlog_seal_weakens", "ui/icons/initial_release", actor);
                        ShinobiShi.sealWeak = true;
                    }
                    return true;
                }
            }

            return false;
        }

        private static void ClearLowerForms(Actor actor)
        {
            actor.finishStatusEffect("status_jinchuriki_initial_release");
            actor.finishStatusEffect("status_jinchuriki_v1_cloak");
            actor.finishStatusEffect("status_jinchuriki_v2_cloak");
            actor.finishStatusEffect("status_jinchuriki_incomplete_beast");
            actor.finishStatusEffect("status_jinchuriki_kcm1");
            actor.finishStatusEffect("status_jinchuriki_kcm2");
            actor.finishStatusEffect("status_jinchuriki_avatar");
        }
        #endregion

        #region Baryon Mode
        public static bool BaryonMode(BaseSimObject pSelf, WorldTile pTile = null)
        {
            if (pSelf?.a == null || !pSelf.a.isAlive()) return false;
            Actor actor = pSelf.a;

            if (actor.data.level < 7) return false;
            if (actor.hasStatus("status_jinchuriki_baryon_mode")) return false;
            if (UnityEngine.Random.value > 0.30f) return false;
            //Actor baryonUnit = World.world.units.createNewUnit(actor.asset.id, actor.current_tile, true, 1f);
            ClearLowerForms(actor);

            /*
            ActorTool.copyUnitToOtherUnit(actor, baryonUnit, true);
            baryonUnit.setKingdom(actor.kingdom);

                if (actor.hasStatus("status_jinchuriki_v1_cloak"))
                {
                    return false;
                }
            baryonUnit.joinCity(actor.city);
            baryonUnit.data.name = actor.data.name;
            baryonUnit.data.level = actor.data.level;
            baryonUnit.data.kills = actor.data.kills;
            baryonUnit.data.health = (int)(actor.getMaxHealth());

            baryonUnit.addStatusEffect("status_jinchuriki_baryon_mode", 60f);
            baryonUnit.restoreHealth(actor.getMaxHealth());
            */

            actor.addStatusEffect("status_jinchuriki_baryon_mode", 60f);
            actor.restoreHealth((int)(actor.getMaxHealth() * 0.5f));
            if (ShinobiConfig.EnableWorldTips)
            {
                ShinobiWorldLogs.AddWorldLog("log_baryon_mode", "worldlog_baryon_mode", "ui/icons/Baryon", actor);
            }

            return true;
        }
        #endregion

        #endregion

        #region Tailed Beast Bombs
        // KCM/BARYON Explosion
        public static bool KCMBlastAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.05f) return false;

            // 70 Chakra Cost
            if (!CheckAndConsumeChakra(pSelf.a, 70f)) return false;

            if (pTarget.current_tile != null)
            {
                MapAction.damageWorld(pTarget.current_tile, 8, AssetManager.terraform.get("bomb"), null);
                EffectsLibrary.spawnAtTile("fx_explosion_tiny", pTarget.current_tile, 0.1f);
                EffectsLibrary.spawnExplosionWave(pTile.posV3, 1f, 1f);
                World.world.applyForceOnTile(pTile: pTarget.current_tile, pByWho: pSelf);
                pTarget.a.getHit(50f, true, AttackType.Other, pSelf, false);
            }
            return true;
        }

        public static bool TailedBeastBombAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            if (pSelf.current_tile == null || pTarget.current_tile == null) return false;

            float distance = pSelf.a.distanceToActorTile(pTarget.current_tile);
            if (distance > 80f) return false;

            if (UnityEngine.Random.value > 0.90f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 140f)) return false;

            Vector2Int targetPos = pTarget.current_tile.pos;
            float pointDistance = Vector2.Distance(pTarget.current_position, targetPos);
            Vector3 from = Toolbox.getNewPoint(
                pSelf.current_position.x,
                pSelf.current_position.y + 1.5f,
                targetPos.x + 1f,
                targetPos.y + 1f,
                pointDistance,
                true
            );
            Vector3 to = Toolbox.getNewPoint(
                pTarget.current_position.x,
                pTarget.current_position.y,
                targetPos.x,
                targetPos.y,
                Mathf.Max(0.35f, pTarget.a.stats["scale"]),
                true
            );

            World.world.projectiles.spawn(pSelf, pTarget, "projectile_tailed_beast_bomb", from, to);

            return true;
        }

        public static bool TruthSeekingOrbAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            if (pSelf.current_tile == null || pTarget.current_tile == null) return false;
            if (!pSelf.a.hasStatus("status_six_paths_senjutsu")) return false;

            // No chakra cost by design. Keep chance gate to avoid spam.
            if (UnityEngine.Random.value > 0.20f) return false;

            World.world.projectiles.spawn(
                pSelf,
                pTarget,
                "projectile_truth_seeking_orb",
                pSelf.current_tile.posV3,
                pTarget.current_tile.posV3
            );
            return true;
        }

        public static bool AutoTruthSeekingOrbAtTarget(BaseSimObject pSelf, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            Actor target = GetValidAttackTarget(pSelf.a, 24f);
            if (target == null) return false;

            return TruthSeekingOrbAction(pSelf, target, target.current_tile);
        }

        public static bool TenTailsJinchuriki(BaseSimObject pSelf, BaseSimObject pAttackedBy, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            Actor actor = pSelf.a;
            if (!actor.hasTrait("ten_tails_jinchuriki")) return false;

            float maxHealth = actor.getMaxHealth();
            if (maxHealth <= 0f) return false;

            float healthPct = actor.data.health / maxHealth;
            int level = actor.data.level;

            bool hasJuubi1 = actor.hasStatus("juubi1");
            bool hasJuubi2 = actor.hasStatus("juubi2");
            bool hasSps = actor.hasStatus("status_six_paths_senjutsu");

            if (!hasJuubi2 && level >= 5 && hasJuubi1)
            {
                actor.finishStatusEffect("juubi1");
                actor.addStatusEffect("juubi2", 60f);
                actor.restoreHealth(actor.getMaxHealth());
                hasJuubi2 = true;
            }
            else if (!hasJuubi1 && !hasJuubi2)
            {
                actor.addStatusEffect("juubi1", 60f);
                hasJuubi1 = true;
            }

            if ((hasJuubi1 || hasJuubi2) && !hasSps)
            {
                actor.addStatusEffect("status_six_paths_senjutsu", 60f);
                actor.setFlying(true);
            }

            return hasJuubi1 || hasJuubi2;
        }


        public static bool NineTailsStatusMoves(BaseSimObject pSelf, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            Actor actor = pSelf.a;
            if (!actor.hasTrait("nine_tails_jinchuriki")) return false;

            Actor target = GetValidAttackTarget(actor, 48f);
            if (target == null) return false;
            float distance = actor.distanceToActorTile(target.current_tile);
            bool isClose = distance <= 2.2f;

            if (actor.hasStatus("status_jinchuriki_baryon_mode"))
            {
                if (isClose)
                {
                    return BaryonLifespanDrainAction(pSelf, target, target.current_tile)
                    || TailedBeastBombAction(pSelf, target, target.current_tile)
                    || ArmorBreakAction(pSelf, target, target.current_tile);
                }
                return TailedBeastBombAction(pSelf, target, target.current_tile);
            }

            if (actor.hasStatus("status_jinchuriki_avatar"))
            {
                if (isClose)
                {
                    return ChakraArmSwipeAction(pSelf, target, target.current_tile)
                    || TailedBeastBombAction(pSelf, target, target.current_tile)
                    || ArmorBreakAction(pSelf, target, target.current_tile);
                }
                return TailedBeastBombAction(pSelf, target, target.current_tile);
            }

            if (actor.hasStatus("status_jinchuriki_kcm2"))
            {
                if (isClose)
                {
                    return ChakraArmSwipeAction(pSelf, target, target.current_tile)
                    || TailedBeastBombAction(pSelf, target, target.current_tile)
                    || ArmorBreakAction(pSelf, target, target.current_tile);
                }
                return TailedBeastBombAction(pSelf, target, target.current_tile);
            }

            if (actor.hasStatus("status_jinchuriki_kcm1"))
            {
                if (isClose)
                {
                    return ChakraArmSwipeAction(pSelf, target, target.current_tile)
                    || TailedBeastBombAction(pSelf, target, target.current_tile)
                    || ArmorBreakAction(pSelf, target, target.current_tile);
                }
                return TailedBeastBombAction(pSelf, target, target.current_tile);
            }

            if (actor.hasStatus("status_jinchuriki_incomplete_beast") || actor.hasStatus("status_jinchuriki_v2_cloak"))
            {
                if (isClose)
                {
                    return KCMBlastAction(pSelf, target, target.current_tile)
                    || ArmorBreakAction(pSelf, target, target.current_tile);
                }
                return TailedBeastBombAction(pSelf, target, target.current_tile);
            }

            if (actor.hasStatus("status_jinchuriki_v1_cloak") || actor.hasStatus("status_jinchuriki_initial_release"))
            {
                if (!isClose) return false;

                return StrongPunch(pSelf, target, target.current_tile)
                || ArmorBreakAction(pSelf, target, target.current_tile);
            }

            return false;
        }

        #endregion

        #region Mangekyo & EMS Abilities
        // Amaterasu
        public static bool AmaterasuAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.03f) return false;

            // 120 Chakra Cost
            if (!CheckAndConsumeChakra(pSelf.a, 120f)) return false;

            if (!pTarget.a.hasStatus("status_amaterasu"))
            {
                pTarget.a.addStatusEffect("status_amaterasu", 60f);

                // Blindness Logic
                int currentUsage = 0;
                pSelf.a.data.get("ms_usage", out currentUsage);
                pSelf.a.data.set("ms_usage", currentUsage + 1);

                return true;
            }
            return false;
        }

        // Susanoo Ribcage
        public static bool SusanooRibcage(BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            // Only triggers if health below 35%
            float healthPct = (float)pTarget.a.data.health / (float)pTarget.a.getMaxHealth();
            if (healthPct < 0.35f)
            {
                if (!pTarget.a.hasStatus("susanoo_ribcage"))
                {
                    // 75 Chakra Cost
                    if (!CheckAndConsumeChakra(pTarget.a, 75f)) return false;

                    pTarget.a.addStatusEffect("susanoo_ribcage");

                    // Blindness Logic
                    int currentUsage = 0;
                    pTarget.a.data.get("ms_usage", out currentUsage);
                    pTarget.a.data.set("ms_usage", currentUsage + 1);

                    return true;
                }
            }
            return false;
        }

        // Base Susanoo (Mangekyo)
        public static bool BaseSusanooAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.10f) return false;

            // 100 Chakra Cost
            if (!CheckAndConsumeChakra(pSelf.a, 100f)) return false;

            if (!pSelf.a.hasStatus("status_base_susanoo"))
            {
                pSelf.a.addStatusEffect("status_base_susanoo");

                // Blindness Logic
                int currentUsage = 0;
                pSelf.a.data.get("ms_usage", out currentUsage);
                pSelf.a.data.set("ms_usage", currentUsage + 1);
            }

            EffectsLibrary.spawnExplosionWave(pSelf.current_tile.posV3, 1f, 1f);
            World.world.applyForceOnTile(pTile: pTarget.current_tile, pByWho: pSelf);
            return true;
        }

        // Perfect Susanoo (EMS)
        public static bool PerfectSusanooAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.10f) return false;

            // 15 Chakra Cost
            if (!CheckAndConsumeChakra(pSelf.a, 15f)) return false;

            if (!pSelf.a.hasStatus("status_perfect_susanoo"))
            {
                pSelf.a.addStatusEffect("status_perfect_susanoo");
            }

            EffectsLibrary.spawnExplosionWave(pSelf.current_tile.posV3, 2f, 1f);
            World.world.applyForceOnTile(pTile: pTarget.current_tile, pByWho: pSelf);

            return true;
        }

        #region Amenotejikara

        public static bool Amenotejikara(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            Actor actor = pTarget.a;
            WorldTile actorTile = actor.current_tile;
            if (actorTile == null) return false;
            if (!actor.isTask("run_away")) return false;
            if (!CheckAndConsumeChakra(actor, 25f)) return false;


            List<Actor> nearbyUnits = new List<Actor>();
            foreach (var unit in Finder.getUnitsFromChunk(actorTile, 151))
            {
                if (unit == null || unit.a == null) continue;
                if (!unit.a.isAlive() || unit.a == actor) continue;
                if (unit.current_tile == null) continue;
                if (actorTile.distanceTo(unit.current_tile) > 151f) continue;

                nearbyUnits.Add(unit.a);
            }

            if (nearbyUnits.Count == 0) return false;

            Actor swapTarget = nearbyUnits[UnityEngine.Random.Range(0, nearbyUnits.Count)];

            if (swapTarget == null || swapTarget.current_tile == null) return false;

            WorldTile targetTile = swapTarget.current_tile;

            actor.spawnOn(targetTile);
            swapTarget.spawnOn(actorTile);
            actor.cancelAllBeh();
            swapTarget.cancelAllBeh();

            PlayWavSound("sharingan_sound.wav", actor.current_tile.posV3);

            return true;
        }
        #endregion
        #endregion

        #region Madaras EMS Abilities

        public static bool MadaraPerfectSusanoo(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.30f) return false;

            // 500 Chakra Cost
            if (!CheckAndConsumeChakra(pSelf.a, 15f)) return false;

            if (!pSelf.a.hasStatus("madara_ps"))
            {
                pSelf.a.addStatusEffect("madara_ps");
            }

            EffectsLibrary.spawnExplosionWave(pSelf.current_tile.posV3, 2f, 1f);
            // World.world.applyForceOnTile(pTile: pTarget.current_tile, pByWho: pSelf);

            return true;
        }

        public static bool MadaraTengaiShinsei(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.01f) return false;

            // 500 Chakra Cost
            if (!CheckAndConsumeChakra(pSelf.a, 500f)) return false;

            if (pTarget.current_tile != null)
            {

                pSelf.a.addStatusEffect("tengai_shinsei", 3);
                EffectsLibrary.spawnExplosionWave(pSelf.current_tile.posV3, 2f, 1f);
                Meteorite.spawnMeteorite(pTarget.current_tile, pSelf.a);

                ShinobiWorldLogs.AddWorldLog("log_tengai_shinsei", "worldlog_tengai_shinsei", "ui/icons/madara_ems", pSelf.a);
                return true;
            }
            return false;
        }

        public static bool MadaraEMSGenjutsu(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (pSelf.a.kingdom == null) return false;

            if (!IsTailedBeastTarget(pTarget.a)) return false;
            if (pTarget.a.hasStatus("status_madara_tamed")) return false;

            if (UnityEngine.Random.value > 0.30f) return false;

            long targetId = pTarget.a.getID();
            if (!MadaraTamedOriginalKingdoms.ContainsKey(targetId))
            {
                MadaraTamedOriginalKingdoms[targetId] = pTarget.a.kingdom;
            }

            pTarget.a.setKingdom(pSelf.a.kingdom);
            pTarget.a.addStatusEffect("status_madara_tamed", 15f);

            return true;
        }

        public static bool RestoreKingdom(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || pTarget.a == null) return false;

            long targetId = pTarget.a.getID();
            if (!MadaraTamedOriginalKingdoms.TryGetValue(targetId, out Kingdom originalKingdom)) return false;

            MadaraTamedOriginalKingdoms.Remove(targetId);

            if (!pTarget.a.isAlive()) return true;
            if (originalKingdom == null) return true;

            pTarget.a.setKingdom(originalKingdom);
            return true;
        }

        // Madara Susanoo Ribcage
        public static bool MadaraSusanooRibcage(BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            float healthPct = (float)pTarget.a.data.health / (float)pTarget.a.getMaxHealth();
            if (healthPct < 0.40f)
            {
                if (!pTarget.a.hasStatus("susanoo_ribcage_madara"))
                {
                    if (!CheckAndConsumeChakra(pTarget.a, 75f)) return false;

                    pTarget.a.addStatusEffect("susanoo_ribcage_madara");

                    return true;
                }
            }
            return false;
        }

        private static bool IsTailedBeastTarget(Actor target)
        {
            if (target == null || target.asset == null) return false;

            if (target.asset.id == ShinobiActors.KuramaActorId) return true;
            if (target.asset.id == ShinobiActors.JuubiActorId) return true;

            return false;
        }

        #endregion

        #region Baryon Mode Lifespan Drain
        // Baryon Mode Life Decay
        public static bool BaryonLifespanDrainAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (UnityEngine.Random.value < 0.05f && !pTarget.a.hasStatus("baryon_life_decay"))
            {
                pTarget.a.addStatusEffect("baryon_life_decay", 5f);
                pTarget.getHit(150f, true, AttackType.Fire, null, true, false);
                return true;
            }

            return false;
        }
        #endregion

        #region Byakugan: Eight Trigrams 64 Palms
        public static bool EightTrigrams64PalmsAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            float dist = Toolbox.Dist(pSelf.current_tile.x, pSelf.current_tile.y, pTarget.current_tile.x, pTarget.current_tile.y);
            if (dist > 4.0f) return false;

            if (UnityEngine.Random.value > 0.10f) return false;

            // 80 Chakra Cost
            if (!CheckAndConsumeChakra(pSelf.a, 80f)) return false;

            pSelf.a.addStatusEffect("eightAttack");
            pTarget.a.addStatusEffect("status_getting_hit_64");


            return true;
        }
        #endregion

        #region Byakugan: Eight Trigrams Palm Rotation
        public static bool PalmRotationAttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            if (pSelf.a.hasStatus("kaiten_cooldown")) return false;
            if (pSelf.a.hasStatus("status_kaiten")) return false;

            if (UnityEngine.Random.value > 0.03f) return false;

            // 80 Chakra Cost
            if (!CheckAndConsumeChakra(pSelf.a, 80f)) return false;

            pSelf.a.addStatusEffect("status_kaiten");
            pSelf.a.addStatusEffect("kaiten_cooldown");

            EffectsLibrary.spawnExplosionWave(pTile.posV3, 1f, 1f);

            return true;
        }
        #endregion

        #region Armor Break (Gentle Fist / Taijutsu)
        public static bool ArmorBreakAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.05f) return false;

            if (pTarget.a.hasStatus("chakra_break")) return false;

            // 40 Chakra Cost
            if (!CheckAndConsumeChakra(pSelf.a, 40f)) return false;

            pTarget.a.addStatusEffect("chakra_break");
            pTarget.a.addTrait("crippled");
            return true;
        }

        public static bool OrochimaruCursedMarkAttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (pTarget.a.hasTrait("cursed_mark")) return false;
            if (UnityEngine.Random.value > 0.01f) return false;

            pTarget.a.addTrait("cursed_mark");
            return true;
        }

        public static bool StrongPunch(BaseSimObject pSelf, BaseSimObject pAttackedBy, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (pAttackedBy == null || pAttackedBy.a == null || !pAttackedBy.a.isAlive()) return false;

            Actor self = pSelf.a;
            Actor attacker = pAttackedBy.a;

            if (attacker == self) return false;
            if (UnityEngine.Random.value > 0.10f) return false;
            if (!CheckAndConsumeChakra(self, 10f)) return false;

            if (attacker.current_tile != null)
            {
                World.world.applyForceOnTile(attacker.current_tile, 4, 3f, true, 80);
            }

            return true;
        }
        #endregion

        #region Wood Release: Mokuton Tree
        public static bool WoodReleaseAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.10f) return false;

            // 50 Chakra Cost
            if (!CheckAndConsumeChakra(pSelf.a, 50f)) return false;

            pTarget.a.addStatusEffect("status_mokuton_tree");
            pTarget.a.getHit(50f, true, AttackType.Fire, pSelf.a, true);
            return true;
        }
        #endregion

        #region Small Bijuu Bomb
        public static bool SmallBijuuBomb(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || !pTarget.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.10f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 80f)) return false;

            if (pTarget.current_tile != null)
            {
                MapAction.damageWorld(pTarget.current_tile, 5, AssetManager.terraform.get("grenade"), null);
                EffectsLibrary.spawnAtTileRandomScale("fx_explosion_small", pTarget.current_tile, 0.5f, 0.7f);
                pTarget.a.getHit(100f, true, AttackType.Fire, pSelf.a, true);
            }
            return true;
        }
        #endregion

        #region Kurama Avatar
        /* Idk if i wanna use this or not
        // Kurama Avatar Action
        public static bool KuramaAvatarAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            // 5% chance to activate if not active
            if (UnityEngine.Random.value < 0.05f && !pSelf.a.hasStatus("status_kurama_avatar"))
            {
                pSelf.a.addStatusEffect("status_kurama_avatar");

                // Spawn a Tailed Beast Bomb effect (Explosion)
                EffectsLibrary.spawnExplosionWave(pSelf.current_tile.posV3, 3f, 1f);
                return true;
            }
            return false;
        }
        */
        #endregion


        public static bool AutoTailedBeastBombAtTarget(BaseSimObject pSelf, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            Actor target = GetValidAttackTarget(pSelf.a, 48f);
            if (target == null) return false;

            return TailedBeastBombAction(pSelf, target, target.current_tile);
        }
        #region Avatar: Bijuu Bite
        // Bijuu Bite (Avatar)
        /*public static bool BijuuBiteAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.03f) return false;

            // 50 Chakra Cost
            if (!CheckAndConsumeChakra(pSelf.a, 50f)) return false;

            EffectsLibrary.spawnAtTile("fx_bijuu_bite", pTarget.current_tile, 0.25f);
            pTarget.a.getHit(300f, true, AttackType.Other, pSelf.a, true);
            EffectsLibrary.spawnExplosionWave(pTarget.current_tile.posV3, 2f, 0.5f);
            return true;
        } */
        #endregion

        #region Avatar: Chakra Arm Swipe
        // Chakra Arm Swipe
        public static bool ChakraArmSwipeAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pTarget == null) return false;

            if (UnityEngine.Random.value > 0.05f) return false;

            // 50 Chakra Cost
            if (!CheckAndConsumeChakra(pSelf.a, 50f)) return false;

            EffectsLibrary.spawnAtTile("fx_chakra_arm", pSelf.current_tile, 0.1f);
            var nearbyUnits = Finder.getUnitsFromChunk(pTarget.current_tile, 1);

            if (nearbyUnits.Any())
            {
                foreach (var unit in nearbyUnits)
                {
                    if (unit.a == pSelf.a || (pSelf.a.kingdom != null && unit.a.kingdom == pSelf.a.kingdom)) continue;
                    if (!unit.a.isAlive()) continue;

                    unit.a.getHit(250f, true, AttackType.Other, pSelf.a, true);
                    World.world.applyForceOnTile(unit.current_tile, 5, 1f, true, 0);
                    EffectsLibrary.spawnExplosionWave(pTarget.current_tile.posV3, 2f, 0.5f);
                }
            }
            return true;
        }
        #endregion

        #region Uzumaki Chains
        public static bool AdamantiteChainsAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || !pTarget.a.isAlive()) return false;

            if (UnityEngine.Random.value > 0.05f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 100f)) return false;

            pTarget.a.addStatusEffect("status_adamantite_chains");

            float drain = pTarget.a.getMaxHealth() * 0.25f;
            pTarget.a.getHit(drain, true, AttackType.Other, pSelf.a, true);
            pSelf.a.restoreHealth((int)drain);

            return true;
        }
        #endregion

        #region Chakra Nature Helpers
        private static readonly string[] BaseNatureTraits =
        {
            "fireN",
            "earthN",
            "waterN",
            "windN",
            "lightningN"
        };

        public static bool HasAnyNatureTrait(Actor actor)
        {
            if (actor == null) return false;

            return actor.hasTrait("fireN") || actor.hasTrait("earthN") || actor.hasTrait("waterN") ||
                   actor.hasTrait("windN") || actor.hasTrait("lightningN") || actor.hasTrait("stormR") ||
                   actor.hasTrait("wood_release") || actor.hasTrait("yin_release") ||
                   actor.hasTrait("yang_release") || actor.hasTrait("yinyang_release");
        }

        public static void AddNatureProgressExp(Actor actor, float amount)
        {
            if (actor == null || amount <= 0f) return;

            float exp = GetNatureProgressExp(actor);
            exp = Mathf.Min(exp + amount, 1000f);
            actor.data.set("chakra_nature_exp", exp);

            int rollStepsDone = 0;
            actor.data.get("chakra_nature_roll_steps", out rollStepsDone);
            int availableRollSteps = Mathf.FloorToInt(exp / 100f);

            while (rollStepsDone < availableRollSteps)
            {
                rollStepsDone++;
                if (UnityEngine.Random.value <= 0.10f)
                {
                    GrantRandomBaseNature(actor);
                }
            }

            actor.data.set("chakra_nature_roll_steps", rollStepsDone);
        }

        public static float GetNatureProgressExp(Actor actor)
        {
            if (actor == null) return 0f;
            float exp = 0f;
            actor.data.get("chakra_nature_exp", out exp);
            return exp;
        }

        private static void GrantRandomBaseNature(Actor actor)
        {
            if (actor == null) return;

            List<string> missingTraits = new List<string>();
            foreach (string traitId in BaseNatureTraits)
            {
                if (!actor.hasTrait(traitId))
                {
                    missingTraits.Add(traitId);
                }
            }

            if (missingTraits.Count == 0) return;

            int randomIndex = UnityEngine.Random.Range(0, missingTraits.Count);
            actor.addTrait(missingTraits[randomIndex]);
        }

        public static void AddNatureExp(Actor actor, string _nature, float amount)
        {
            AddNatureProgressExp(actor, amount);
        }

        public static float GetNatureExp(Actor actor, string _nature)
        {
            return GetNatureProgressExp(actor);
        }
        #endregion

        #region Fire Release Ability
        public static bool FireReleaseAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            if (UnityEngine.Random.value > 0.10f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 50f)) return false;
            if (GetNatureExp(pSelf.a, "fire") < 125f) return false;
            if (pTarget.current_tile != null && pSelf.current_tile != null)
            {
                World.world.projectiles.spawn(pSelf, pTarget, "red_orb", pSelf.current_tile.posV3, pTarget.current_tile.posV3);
                pTarget.a.addStatusEffect("katon_burn", 2f);
                return true;
            }
            return false;
        }

        public static bool AutoFireReleaseAtTarget(BaseSimObject pSelf, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            Actor target = GetValidAttackTarget(pSelf.a, 24f);
            if (target == null) return false;

            return FireReleaseAction(pSelf, target, target.current_tile);
        }
        #endregion

        #region Earth Release Ability
        public static bool EarthReleaseAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (UnityEngine.Random.value > 0.10f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 60f)) return false;
            if (GetNatureExp(pSelf.a, "earth") < 125f) return false;

            pSelf.addStatusEffect("weight_rock", 5f);
            return true;
        }
        #endregion

        #region Water Release Ability
        public static bool WaterReleaseAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            if (UnityEngine.Random.value > 0.10f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 50f)) return false;
            if (GetNatureExp(pSelf.a, "water") < 125f) return false;
            if (pTarget.current_tile != null)
            {
                pTarget.a.addStatusEffect("water_prison", 5f);
                pTarget.a.addStatusEffect("drowning", 5f);
                return true;
            }
            return false;
        }
        #endregion

        #region Wind Release Ability
        public static bool WindReleaseAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            if (UnityEngine.Random.value > 0.10f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 50f)) return false;
            if (GetNatureExp(pSelf.a, "wind") < 130f) return false;
            if (pTarget.current_tile != null)
            {
                if (pSelf.current_tile == null) return false;
                World.world.projectiles.spawn(pSelf, pTarget, "projectile_wind_cutter", pSelf.current_tile.posV3, pTarget.current_tile.posV3);
                return true;
            }
            return false;
        }

        public static bool AutoWindReleaseAtTarget(BaseSimObject pSelf, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            Actor target = GetValidAttackTarget(pSelf.a, 24f);
            if (target == null) return false;

            return WindReleaseAction(pSelf, target, target.current_tile);
        }
        #endregion

        #region Lightning Release Ability
        public static bool LightningReleaseAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            if (UnityEngine.Random.value > 0.10f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 60f)) return false;
            if (GetNatureExp(pSelf.a, "lightning") < 60f) return false;
            if (pTarget.current_tile != null)
            {
                ActionLibrary.castLightning(pSelf, pTarget, pTile);
                if (GetNatureExp(pSelf.a, "lightning") >= 200f)
                {
                    ActionLibrary.castLightning(pSelf, pTarget, pTile);
                    ActionLibrary.castLightning(pSelf, pTarget, pTile);
                }
                return true;
            }
            return false;
        }
        #endregion

        #region Storm Release Ability
        public static bool StormReleaseAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            if (UnityEngine.Random.value > 0.10f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 80f)) return false;
            if (GetNatureExp(pSelf.a, "storm") < 250f) return false;
            ActionLibrary.castLightning(pSelf, pTarget, pTile);
            if (pTarget.current_tile != null)
            {
                World.world.projectiles.spawn(pSelf, pTarget, "torch", pSelf.current_tile.posV3, pTarget.current_tile.posV3);
            }
            return true;
        }
        #endregion

        #region Akimichi Size Manipulation
        public static bool AkimichiSizeManipulationAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            if (!CheckAndConsumeChakra(pSelf.a, 40f)) return false;

            bool grow = UnityEngine.Random.value > 0.70f; //30% chance

            if (grow)
            {
                if (!pSelf.a.hasStatus("status_akimichi_grown") && !pSelf.a.hasStatus("status_akimichi_shrunk"))
                {
                    pSelf.a.addStatusEffect("status_akimichi_grown", 6f);
                }
            }
            else
            {
                if (!pSelf.a.hasStatus("status_akimichi_shrunk") && !pSelf.a.hasStatus("status_akimichi_grown"))
                {
                    pSelf.a.addStatusEffect("status_akimichi_shrunk", 6f);
                }
            }

            return true;
        }
        #endregion

        #region Frog Summon
        public static bool FrogSummonAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (UnityEngine.Random.value > 0.02f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 120f)) return false;

            WorldTile spawnTile = pSelf.current_tile;
            if (spawnTile == null) return false;

            Actor frog = World.world.units.createNewUnit("frog", spawnTile, true, 1f);
            if (frog == null) return false;

            if (pSelf.a.city != null)
            {
                frog.joinCity(pSelf.a.city);
            }

            return true;
        }
        #endregion

        #region Strength Of A Hundred
        public static bool StrengthOfHundredAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (!pSelf.a.hasTrait("slug_sage_mode")) return false;
            if (!pSelf.a.hasStatus("status_perfect_slug_sage")) return false;
            if (pSelf.a.hasStatus("status_strength_hundred")) return false;
            if (UnityEngine.Random.value > 0.05f) return false;
            if (!CheckAndConsumeChakra(pSelf.a, 160f)) return false;

            pSelf.a.addStatusEffect("status_strength_hundred", 15f);
            return true;
        }
        #endregion

        

        #region Helpers
        private static bool SetWorldAgeChaos()
        {
            WorldAgeAsset chaosAge = AssetManager.era_library.get("age_chaos");
            if (chaosAge == null || World.world?.era_manager == null) return false;

            int slotIndex = World.world.era_manager.getCurrentSlotIndex();
            World.world.era_manager.setAgeToSlot(chaosAge, slotIndex);

            _chaosRevertPending = true;
            _chaosRevertTimer = ChaosAgeDurationSeconds;
            return true;
        }

        public static void TickWorldAgeRevert(float dt)
        {
            if (!_chaosRevertPending) return;

            _chaosRevertTimer -= dt;
            if (_chaosRevertTimer > 0f) return;

            _chaosRevertPending = false;
            RevertWorldAgeToHope();
        }

        private static void RevertWorldAgeToHope()
        {
            if (World.world?.era_manager == null) return;

            string currentAgeId = GetCurrentWorldAgeId();
            if (currentAgeId != null && currentAgeId != "age_chaos") return;

            WorldAgeAsset hopeAge = AssetManager.era_library.get("age_hope");
            if (hopeAge == null) return;

            int slotIndex = World.world.era_manager.getCurrentSlotIndex();
            World.world.era_manager.setAgeToSlot(hopeAge, slotIndex);
        }

        private static string GetCurrentWorldAgeId()
        {
            object eraManager = World.world?.era_manager;
            if (eraManager == null) return null;

            Type managerType = eraManager.GetType();
            object currentAge = null;

            MethodInfo getCurrentAge = managerType.GetMethod("getCurrentAge", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (getCurrentAge != null)
            {
                currentAge = getCurrentAge.Invoke(eraManager, null);
            }

            if (currentAge == null)
            {
                FieldInfo field = managerType.GetField("current_age", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)
                {
                    currentAge = field.GetValue(eraManager);
                }
            }

            if (currentAge == null) return null;

            Type ageType = currentAge.GetType();
            FieldInfo idField = ageType.GetField("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (idField != null)
            {
                return idField.GetValue(currentAge) as string;
            }

            PropertyInfo idProp = ageType.GetProperty("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (idProp != null)
            {
                return idProp.GetValue(currentAge, null) as string;
            }

            return null;
        }

        public static bool EightInnerGates(BaseSimObject pSelf, BaseSimObject pAttackedBy = null, WorldTile pTile = null)
        {
            return EightGatesProgression.EightInnerGates(pSelf, pAttackedBy, pTile);
        }

        public static void TickEightInnerGates(Actor actor)
        {
            EightGatesProgression.TickEightInnerGates(actor);
        }

        public static bool InnerGatesPulseAction(BaseSimObject pSelf, WorldTile pTile)
        {
            return EightGatesProgression.InnerGatesPulseAction(pSelf, pTile);
        }

        public static bool CheckAndConsumeChakra(Actor actor, float cost)
        {
            if (actor == null) return false;
            if (!ChakraSystem.IsChakraUser(actor)) return false;
            if (actor.hasStatus("status_ability_cooldown")) return false;
            //Plays Jutsu Sound
            PlayWavSound("jutsu_use.wav", actor.current_tile.posV3);
            actor.addStatusEffect("jutsuuse", 1f);

            // Cost Checker
            float adjustedCost = Mathf.Max(0f, cost * ChakraSystem.GetAbilityCostMultiplier(actor));

            if (ChakraSystem.GetCurrent(actor) + 0.001f < adjustedCost)
            {
                if (!actor.hasTrait("taijutsu_master") && !actor.hasStatus("chakra_exhaustion"))
                {
                    actor.addStatusEffect("chakra_exhaustion", 30f);
                }
                return false;
            }

            bool used = ChakraSystem.TryConsume(actor, adjustedCost);

            if (!used)
            {
                if (!actor.hasTrait("taijutsu_master") && !actor.hasStatus("chakra_exhaustion"))
                {
                    actor.addStatusEffect("chakra_exhaustion", 30f);
                }
                return false;
            }

            actor.addStatusEffect("status_ability_cooldown", 7f);
            AddNatureProgressExp(actor, 2f);
            ChakraSystem.SetLastMoveTime(actor, Time.time);
            return true;
        }

        private static Actor GetValidAttackTarget(Actor actor, float maxDistance)
        {
            if (actor == null || !actor.isAlive()) return null;

            Actor target = actor.attack_target?.a;
            if (target == null || !target.isAlive()) return null;
            if (target == actor) return null;

            if (actor.current_tile == null || target.current_tile == null) return null;

            if (actor.distanceToActorTile(target.current_tile) > maxDistance) return null;

            return target;
        }

        public static bool COHDeathAction(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || pTarget.a == null) return false;

            Actor target = pTarget.a;

            if (target.city != null && target.current_tile != null)
            {
                var nearby = Finder.getUnitsFromChunk(target.current_tile, 1);
                foreach (var u in nearby)
                {
                    if (u.a != null && u.a.isAlive() && u.a != target && u.a.city == target.city)
                    {
                        u.a.addStatusEffect("cursed", 25f);
                    }
                }
            }

            return true;
        }

        public static bool WOFDeathAction(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || pTarget.a == null) return false;

            Actor target = pTarget.a;

            if (target.city != null && target.current_tile != null)
            {
                var nearby = Finder.getUnitsFromChunk(target.current_tile, 1);
                foreach (var u in nearby)
                {
                    if (u.a != null && u.a.isAlive() && u.a != target && u.a.city == target.city)
                    {
                        u.a.addStatusEffect("power_up", 25f);
                    }
                }
            }

            return true;
        }

        public static bool ShadowCloneAction(BaseSimObject pSelf, BaseSimObject pAttackedBy = null, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            Actor actor = pSelf.a;

            if (actor.hasStatus("shadow_clone")) return false;
            if (actor.current_tile == null) return false;
            if (actor.hasStatus("status_ability_cooldown")) return false;
            if (UnityEngine.Random.value > 0.30f) return false;

            int level = actor.data.level;
            int maxClones = Mathf.Clamp(level, 1, 9);
            float costPerClone = 25f;
            int spawned = 0;

            for (int i = 0; i < maxClones; i++)
            {
                if (!ChakraSystem.TryConsume(actor, costPerClone)) break;

                WorldTile spawnTile = Toolbox.getRandomTileWithinDistance(actor.current_tile, 1) ?? actor.current_tile;
                Actor clone = World.world.units.createNewUnit(actor.asset.id, spawnTile, true, 1f);
                if (clone == null) continue;

                EffectsLibrary.spawnAtTile("fx_smoke", spawnTile, 0.05f);

                ActorTool.copyUnitToOtherUnit(actor, clone, true);
                PlayWavSound("shadow_clone_spawn.wav", spawnTile.posV3);
                clone.addStatusEffect("shadow_clone", 20f);
                clone.removeTrait("shadow_clonej");
                clone.kingdom = actor.kingdom;
                clone.joinCity(actor.city);
                clone.data.name = $"Clone {actor.data.name}";
                clone.data.health = (int)(actor.getMaxHealth() * 0.40f);
                clone.data.level = actor.data.level;
                clone.data.favorite = false;

                if (actor.has_attack_target)
                {
                    clone.attack_target = actor.attack_target;
                    clone.beh_actor_target = actor.attack_target.a;
                }

                float chakraShare = Mathf.Max(0f, ChakraSystem.GetCurrent(actor) / (maxClones + 1f));
                ChakraSystem.SetCurrent(clone, chakraShare);
                spawned++;
            }

            return spawned > 0;
        }

        public static bool KamuiAction(BaseSimObject pSelf, BaseSimObject pAttackedBy = null, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            Actor actor = pSelf.a;

            if (actor.current_tile == null) return false;
            if (actor.hasStatus("kamui_cooldown")) return false;
            if (UnityEngine.Random.value > 0.10f) return false;
            if (!CheckAndConsumeChakra(actor, 100f)) return false;

            actor.addStatusEffect("kamui_intangibility", 6f);
            actor.addStatusEffect("invincible", 6f);
            actor.addStatusEffect("kamui_cooldown", 60f);
            AddNatureProgressExp(actor, 10f);
            int currentUsage = 0;

            if (actor.hasTrait("mangekyo_sharingan"))
            {
                actor.data.get("ms_usage", out currentUsage);
                actor.data.set("ms_usage", currentUsage + 1);
            }

            PlayWavSound("kamui.wav", actor.current_tile.posV3);
            return true;
        }

        public static void PlayWavSound(string fileName, Vector3 position)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;

            string soundPath = Application.dataPath.Replace("worldbox_Data", "Mods/ShinobiBox/GameResources/Sounds/" + fileName);

            Isekai.PlayWavDirectly.Instance.PlaySoundAtPosition(soundPath, position);
        }
        #endregion
    }
}