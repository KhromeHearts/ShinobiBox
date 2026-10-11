using System;
using UnityEngine;

namespace ShinobiBox
{
    public static class Ketsuryugan
    {
        public static bool KetsuryuganAction(BaseSimObject pSelf, BaseSimObject pAttackedBy, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            Actor a = pSelf.a;
            if (!JutsuLibrary.CheckAndConsumeChakra(a, 50f)) return false;

            if (!a.hasStatus("status_ketsuryugan") && a.data.level >= 2)
            {
                a.addStatusEffect("status_ketsuryugan", 45f);
                return true;
            }

            return false;
        }

        public static bool KetsuryuganEHT(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;


            Actor a = pSelf.a;
            Actor b = pTarget.a;

            if (!b.hasStatus("status_ketsuryuganEHT") && Randy.randomChance(0.10f))
            {
                if (!JutsuLibrary.CheckAndConsumeChakra(pSelf.a, 75f)) return false;
                b.addStatusEffect("status_ketsuryuganEHT", 30f);
                return true;
            }

            return false;
        }

        public static bool KetsuryuganExplosion(BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || !pTarget.a.isAlive()) return false;

            if (Randy.randomChance(0.15f))
            {
                float damage = pTarget.a.getMaxHealth() * 0.40f;
                MapAction.damageWorld(pTarget.current_tile, 3, AssetManager.terraform.get("grenade"), null);
                EffectsLibrary.spawnAtTileRandomScale("fx_explosion_small", pTarget.current_tile, 0.5f, 0.7f);
                pTarget.a.getHit(damage, true, AttackType.Explosion, pTarget.a, true);
                return true;
            }
            return false;
        }

    }
}