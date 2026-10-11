/*

using UnityEngine;

namespace ShinobiBox
{
    public static class SageProgression
    {
        
        public static void HandleSageProgression(Actor actor, int level)
        {
            if (actor == null || !actor.isAlive()) return;

            bool hasFrog = actor.hasTrait("frog_sage_mode");
            bool hasSlug = actor.hasTrait("slug_sage_mode");
            bool hasSnake = actor.hasTrait("snake_sage_mode");
            bool hasWood = actor.hasTrait("wood_sage_mode");
            bool hasAnySenjutsu = hasFrog || hasSlug || hasSnake || hasWood;

            if (hasAnySenjutsu) return;

            if (!actor.isSapient() || level < 9 || ChakraSystem.CalculateBaseMaxChakra(actor) < 100f) return;

            if (actor.hasTrait("wood_release") && actor.hasTrait("hashi_cells"))
            {
                actor.addTrait("wood_sage_mode");
                return;
            }

            if (Randy.randomChance(0.006f))
            {
                float roll = UnityEngine.Random.value;
                if (roll < 0.33f)
                {
                    actor.addTrait("frog_sage_mode");
                }
                else if (roll < 0.66f)
                {
                    actor.addTrait("slug_sage_mode");
                }
                else
                {
                    actor.addTrait("snake_sage_mode");
                }
            }
        }

        public static bool Senjutsu(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (!pTarget.isAlive()) return false;
            Actor a = pTarget.a;

            EffectsLibrary.spawnExplosionWave(pTarget.current_tile.posV3, 1f, 1f);

            float roll = Randy.randomFloat(0f, 1f);

            if (roll < 0.33f)
            {
                a.addTrait("frog_sage_mode");
                if (ShinobiConfig.EnableWorldTips)
                {
                    ShinobiWorldLogs.AddWorldLog("log_frog_sage_mode", "worldlog_frog_sage_mode", "ui/icons/frog_sage", a);
                }
                return true;
            }
            else if (roll < 0.66f) 
            {
                a.addTrait("slug_sage_mode");
                if (ShinobiConfig.EnableWorldTips) 
                {
                    ShinobiWorldLogs.AddWorldLog("log_slug_sage_mode", "worldlog_slug_sage_mode", "ui/icons/slug_sage", a);
                }
                return true;
            }
            else
            {
                a.addTrait("snake_sage_mode");
                if (ShinobiConfig.EnableWorldTips)
                {
                    ShinobiWorldLogs.AddWorldLog("log_snake_sage_mode", "worldlog_snake_sage_mode", "ui/icons/snake_sage", a);
                }
                return true;
            }
            return false;
        }

        public static bool SenjutsuStatus(BaseSimObject pSelf, BaseSimObject pAttackedBy = null, WorldTile pTile = null)
        {
            if (!pSelf.isAlive()) return false;
            Actor a = pSelf.a;

            if (a.hasTrait("frog_sage_mode") && !a.hasStatus("status_frog_sage"))
            {
                a.addStatusEffect("status_frog_sage");
            }
            else if (a.hasTrait("slug_sage_mode") && !a.hasStatus("status_slug_sage"))
            {
                a.addStatusEffect("status_slug_sage");
            }
            else if (a.hasTrait("snake_sage_mode") && !a.hasStatus("status_snake_sage"))
            {
                a.addStatusEffect("status_snake_sage");
            }

            return true;
        }

        public static bool ClearMeditatingStatus(BaseSimObject pSelf, BaseSimObject pAttackedBy = null, WorldTile pTile = null)
        {
            if (!pSelf.isAlive()) return false;

            pSelf.a.finishStatusEffect("status_meditating");

            return true;
        }

        public static bool Meditate(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (!pTarget.isAlive()) return false;
            Actor a = pTarget.a;

            a.stopMovement();
            a.makeWait(1f);

            return true;
        }
    }
    
}

*/