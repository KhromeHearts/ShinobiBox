using UnityEngine;

namespace ShinobiBox
{
    public static class CursedMarkProgression
    {
        public static bool CursedMark(BaseSimObject pSelf, BaseSimObject pAttackedBy = null, WorldTile pTile = null)
        {
            if (pSelf == null || !pSelf.isAlive()) return false;
            Actor actor = pSelf.a;

            int level = actor.data.level;

            if (Randy.randomChance(0.35f))
            {
                if (level >= 7 && !actor.hasStatus("cursed_mark3"))
                {
                    actor.addStatusEffect("cursed_mark3", 185f);
                    return true;
                }
                else if (level > 4 && level < 7 && !actor.hasStatus("cursed_mark2"))
                {
                    actor.addStatusEffect("cursed_mark2", 145f);
                    return true;
                }
                else if (level <= 4 && !actor.hasStatus("cursed_mark1"))
                {
                    actor.addStatusEffect("cursed_mark1", 95f);
                    return true;
                }
            }

            return false;
        }

        public static bool CurseMarkStrain(BaseSimObject pSelf, WorldTile pTile = null)
        {
            if (pSelf == null || !pSelf.isAlive()) return false;
            Actor actor = pSelf.a;

            int damageAmount = 0;

            if (actor.hasStatus("cursed_mark3")) damageAmount = 3 * (int)(actor.getMaxHealth() * 0.01f);
            else if (actor.hasStatus("cursed_mark2")) damageAmount = 2 * (int)(actor.getMaxHealth() * 0.01f);
            else if (actor.hasStatus("cursed_mark1")) damageAmount = 1 * (int)(actor.getMaxHealth() * 0.01f);

            if (damageAmount > 0)
            {
                actor.getHit(damageAmount, true, AttackType.Other, null, false, false);
                return true;
            }

            return false;
        }

        public static bool OrochimaruCM(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;

            if (pSelf.a.asset.id != ShinobiActors.OrochimaruActorId) return false;

            if (pTarget.a.hasTrait("cursed_mark")) return false;
            if (UnityEngine.Random.value > 0.20f) return false;

            pTarget.a.addTrait("cursed_mark");
            return true;
        }
    }
}