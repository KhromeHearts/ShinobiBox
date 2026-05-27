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
            if (UnityEngine.Random.value < 0.35f)
            {
                if (level >= 6 && !actor.hasStatus("cursed_mark3"))
                {
                    removeAllMarks(actor);
                    actor.addStatusEffect("cursed_mark3", 185f);
                    return true;
                }
                else if (level > 3 && level < 6 && !actor.hasStatus("cursed_mark2"))
                {
                    removeAllMarks(actor);
                    actor.addStatusEffect("cursed_mark2", 145f);
                    return true;
                }
                else if (level <= 3 && !actor.hasStatus("cursed_mark1"))
                {
                    removeAllMarks(actor);
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

            if (actor.hasStatus("cursed_mark3"))
            {
                actor.data.health -= 3;
                return true;
            }
            else if (actor.hasStatus("cursed_mark2"))
            {
                actor.data.health -= 2;
                return true;
            }
            else if (actor.hasStatus("cursed_mark1"))
            {
                actor.data.health -= 1;
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

        public static void removeAllMarks(Actor actor)
        {
            actor.finishStatusEffect("cursed_mark1");
            actor.finishStatusEffect("cursed_mark2");
            actor.finishStatusEffect("cursed_mark3");
        }
    }
}