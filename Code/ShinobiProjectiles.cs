using System;
using System.Linq;
using UnityEngine;

namespace ShinobiBox
{
    internal static class ShinobiProjectiles
    {
        public static void Init()
        {
            loadProjectiles();
        }

        private static void loadProjectiles()
        {
            AddProjectileIfMissing(new ProjectileAsset
            {
                id = "projectile_ftg_kunai",
                speed = 30f,
                texture = "projectile_ftg",
                look_at_target = true,
                texture_shadow = "shadows/projectiles/shadow_arrow",
                hit_shake = false,
                scale_start = 0.07f,
                scale_target = 0.07f,
                draw_light_area = true,
                draw_light_size = 0.1f,
                sound_launch = "event:/SFX/WEAPONS/WeaponStartArrow",
                sound_impact = "event:/SFX/HIT/HitGeneric",
                can_be_blocked = false,
                can_be_collided = true,
                world_actions = new AttackAction(FTGTeleport)
            });

            AddProjectileIfMissing(new ProjectileAsset
            {
                id = "projectile_basic_kunai",
                speed = 25f,
                texture = "projectile_kunai",
                look_at_target = true,
                texture_shadow = "shadows/projectiles/shadow_arrow",
                hit_shake = false,
                scale_start = 0.055f,
                scale_target = 0.055f,
                sound_launch = "event:/SFX/WEAPONS/WeaponStartArrow",
                sound_impact = "event:/SFX/HIT/HitGeneric",
                can_be_blocked = true,
                can_be_collided = true
            });

            AddProjectileIfMissing(new ProjectileAsset
            {
                id = "projectile_paper_bomb_kunai",
                speed = 24f,
                texture = "projectile_bomb_kunai",
                look_at_target = true,
                texture_shadow = "shadows/projectiles/shadow_arrow",
                end_effect = "fx_explosion",
                hit_shake = true,
                scale_start = 0.075f,
                scale_target = 0.075f,
                sound_launch = "event:/SFX/WEAPONS/WeaponStartArrow",
                sound_impact = "event:/SFX/HIT/HitGeneric",
                can_be_blocked = true,
                can_be_collided = true,
                world_actions = new AttackAction(PaperBombExplode)
            });

            AddProjectileIfMissing(new ProjectileAsset
            {
                id = "projectile_rasenshuriken",
                speed = 20f,
                texture = "projectile_rasenshuriken",
                look_at_target = true,
                texture_shadow = "shadows/projectiles/shadow_arrow",
                end_effect = "fx_shinobiantimatter",
                hit_shake = true,
                scale_start = 0.05f,
                scale_target = 0.05f,
                draw_light_area = true,
                draw_light_size = 0.35f,
                sound_launch = "event:/SFX/WEAPONS/WeaponStartArrow",
                sound_impact = "event:/SFX/EXPLOSIONS/ExplosionAntimatterBomb",
                can_be_blocked = true,
                can_be_collided = true,
                animation_speed = 10f,
                world_actions = new AttackAction(RasenshurikenImpact)
            });

            AddProjectileIfMissing(new ProjectileAsset
            {
                id = "projectile_wind_cutter",
                speed = 28f,
                texture = "projectile_windcutter",
                look_at_target = true,
                texture_shadow = "shadows/projectiles/shadow_arrow",
                hit_shake = false,
                scale_start = 0.06f,
                scale_target = 0.06f,
                sound_launch = "event:/SFX/WEAPONS/WeaponStartArrow",
                sound_impact = "event:/SFX/HIT/HitGeneric",
                can_be_blocked = true,
                can_be_collided = true,
                world_actions = new AttackAction(WindCutterImpact)
            });

            AddProjectileIfMissing(new ProjectileAsset
            {
                id = "projectile_tailed_beast_bomb",
                speed = 28f,
                texture = "projectile_TBB",
                look_at_target = false,
                texture_shadow = "shadows/projectiles/shadow_arrow",
                end_effect = "fx_shinobi_antimatter",
                hit_shake = true,
                scale_start = 0.16f,
                draw_light_area = true,
                draw_light_size = 0.50f,
                sound_launch = "event:/SFX/WEAPONS/WeaponStartArrow",
                sound_impact = "event:/SFX/EXPLOSIONS/ExplosionAntimatterBomb",
                can_be_blocked = false,
                can_be_collided = true,
                animation_speed = 10f,
                world_actions = new AttackAction(TailedBeastBombImpact)
            });

            AddProjectileIfMissing(new ProjectileAsset
            {
                id = "projectile_truth_seeking_orb",
                speed = 24f,
                texture = "projectile_tsorbs",
                look_at_target = true,
                texture_shadow = "shadows/projectiles/shadow_arrow",
                end_effect = "fx_shinobi_antimatter",
                hit_shake = true,
                scale_start = 0.045f,
                scale_target = 0.13f,
                draw_light_area = true,
                draw_light_size = 0.28f,
                sound_launch = "event:/SFX/WEAPONS/WeaponStartArrow",
                sound_impact = "event:/SFX/EXPLOSIONS/ExplosionAntimatterBomb",
                can_be_blocked = false,
                can_be_collided = true,
                world_actions = new AttackAction(TruthSeekingOrbImpact)
            });
        }

        private static void AddProjectileIfMissing(ProjectileAsset asset)
        {
            if (asset == null || string.IsNullOrEmpty(asset.id)) return;
            if (AssetManager.projectiles.get(asset.id) != null) return;
            AssetManager.projectiles.add(asset);
        }

        public static bool PaperBombExplode(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            WorldTile destination = pTarget?.current_tile ?? pTile;
            if (destination == null) return false;
            if (pSelf.current_tile == null || pSelf.hasStatus("status_ability_cooldown")) return false;

            pSelf.addStatusEffect("status_ability_cooldown", 5f);
            JutsuLibrary.AddNatureProgressExp(pSelf.a, 10f);
            EffectsLibrary.spawnAtTile("fx_explosion_small", destination, 0.3f);
            MapBox.instance.drop_manager.spawn(destination, "fire");
            return true;
        }

        public static bool RasenshurikenImpact(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            WorldTile destination = pTarget?.current_tile ?? pTile;
            if (destination == null) return false;

            EffectsLibrary.spawnAtTile("fx_explosion_small", destination, 0.2f);

            float directDamage = 200f;
            if (pTarget?.a != null && pTarget.a.isAlive())
            {
                pTarget.a.getHit(directDamage, true, AttackType.Other, pSelf?.a, true);
                pTarget.a.addTrait("crippled");
                pTarget.a.addStatusEffect("slow", 4f);
            }

            var nearbyUnits = Finder.getUnitsFromChunk(destination, 1);
            foreach (var unit in nearbyUnits)
            {
                if (unit?.a == null || !unit.a.isAlive()) continue;
                if (pSelf?.a != null && unit.a == pSelf.a) continue;
                if (unit.current_tile == null || unit.current_tile.distanceTo(destination) > 2f) continue;

                unit.a.getHit(90f, true, AttackType.Other, pSelf?.a, true);
                unit.a.addStatusEffect("slow", 2f);
            }

            return true;
        }

        public static bool WindCutterImpact(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            WorldTile destination = pTarget?.current_tile ?? pTile;
            if (destination == null) return false;

            if (pTarget?.a != null && pTarget.a.isAlive())
            {
                pTarget.a.getHit(75f, true, AttackType.Other, pSelf?.a, true);
                pTarget.a.addStatusEffect("slow", 1.5f);
            }

            var nearbyUnits = Finder.getUnitsFromChunk(destination, 1);
            foreach (var unit in nearbyUnits)
            {
                if (unit?.a == null || !unit.a.isAlive()) continue;
                if (pSelf?.a != null && unit.a == pSelf.a) continue;
                if (unit.current_tile == null || unit.current_tile.distanceTo(destination) > 1f) continue;

                unit.a.getHit(25f, true, AttackType.Other, pSelf?.a, true);
            }

            return true;
        }

        public static bool TailedBeastBombImpact(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            WorldTile destination = pTarget?.current_tile ?? pTile;
            if (destination == null) return false;

            MapAction.damageWorld(destination, 6, AssetManager.terraform.get("grenade"), null);
            EffectsLibrary.spawnExplosionWave(destination.posV3, 2.8f, 1.2f);

            if (pTarget?.a != null && pTarget.a.isAlive())
            {
                pTarget.a.getHit(200f, true, AttackType.Other, pSelf?.a, true);
            }

            var nearbyUnits = Finder.getUnitsFromChunk(destination, 2);
            foreach (var unit in nearbyUnits)
            {
                if (unit?.a == null || !unit.a.isAlive()) continue;
                if (pSelf?.a != null && unit.a == pSelf.a) continue;
                if (unit.current_tile == null || unit.current_tile.distanceTo(destination) > 2f) continue;

                unit.a.getHit(150f, true, AttackType.Other, pSelf?.a, true);
                World.world.applyForceOnTile(unit.current_tile, 6, 1.2f, true, 0);
            }

            return true;
        }

        public static bool TruthSeekingOrbImpact(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            WorldTile destination = pTarget?.current_tile ?? pTile;
            if (destination == null) return false;

            EffectsLibrary.spawnExplosionWave(destination.posV3, 1.6f, 1.0f);

            if (pTarget?.a != null && pTarget.a.isAlive())
            {
                pTarget.a.getHit(40f, true, AttackType.Other, pSelf?.a, true);
            }

            var nearbyUnits = Finder.getUnitsFromChunk(destination, 1);
            foreach (var unit in nearbyUnits)
            {
                if (unit?.a == null || !unit.a.isAlive()) continue;
                if (pSelf?.a != null && unit.a == pSelf.a) continue;
                if (unit.current_tile == null || unit.current_tile.distanceTo(destination) > 1f) continue;

                unit.a.getHit(30f, true, AttackType.Other, pSelf?.a, true);
            }

            return true;
        }

        public static bool FTGTeleport(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;

            WorldTile destination = null;
            if (pTarget != null && pTarget.a != null)
            {
                destination = pTarget.current_tile;
            }
            else if (pTile != null)
            {
                destination = pTile;
            }

            if (destination == null) return false;
            if (pSelf.current_tile == null || pSelf.hasStatus("status_ability_cooldown")) return false;

            pSelf.addStatusEffect("status_ability_cooldown", 4f);
            JutsuLibrary.AddNatureProgressExp(pSelf.a, 5f);
            EffectsLibrary.spawnAtTile("fx_teleport_yellow", pSelf.current_tile, 0.1f);

            pSelf.a.spawnOn(destination);
            pSelf.a.cancelAllBeh();

            EffectsLibrary.spawnAtTile("fx_teleport_yellow", destination, 0.01f);

            return true;
        }
    }
}
