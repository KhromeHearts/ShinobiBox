using UnityEngine;

namespace ShinobiBox
{
    public static class EightGatesProgression
    {
        public static bool EightInnerGates(BaseSimObject pSelf, BaseSimObject pAttackedBy = null, WorldTile pTile = null)
        {
            if (pSelf == null || !pSelf.isAlive()) return false;
            Actor actor = pSelf.a;

            // Simple Gate Checks
            if (!actor.hasTrait("eight_inner_gates")) return false;
            if (actor.hasStatus("status_ability_cooldown")) return false;

            // Only activates if health drops below 70%
            float maxHealth = actor.getMaxHealth();
            if (actor.data.health > maxHealth * 0.70f) return false;

            if (UnityEngine.Random.value > 0.20f) return false;

            // Gate 8: Death Gate
            if (!actor.hasStatus("inner_gate_8") && actor.data.level >= 8)
            {
                removeAllGates(actor);
                actor.addStatusEffect("inner_gate_8", 45f);
                return true;
            }
            // Gate 7: Wonder Gate
            else if (!actor.hasStatus("inner_gate_7") && actor.data.level >= 7)
            {
                removeAllGates(actor);
                actor.addStatusEffect("inner_gate_7", 60f);
                return true;
            }
            // Gate 6: View Gate
            else if (!actor.hasStatus("inner_gate_6") && actor.data.level >= 6)
            {
                removeAllGates(actor);
                actor.addStatusEffect("inner_gate_6", 55f);
                return true;
            }
            // Gate 5: Limit Gate
            else if (!actor.hasStatus("inner_gate_5") && actor.data.level >= 5)
            {
                removeAllGates(actor);
                actor.addStatusEffect("inner_gate_5", 50f);
                if (UnityEngine.Random.value < 0.25f && !actor.hasTrait("crippled"))
                {
                    actor.addTrait("crippled");
                }
                return true;
            }
            // Gate 4: Wound Gate
            else if (!actor.hasStatus("inner_gate_4") && actor.data.level >= 4)
            {
                removeAllGates(actor);
                actor.addStatusEffect("inner_gate_4", 45f);
                return true;
            }
            // Gate 3: Life Gate
            else if (!actor.hasStatus("inner_gate_3") && actor.data.level >= 3)
            {
                removeAllGates(actor);
                actor.addStatusEffect("inner_gate_3", 40f);
                return true;
            }
            // Gate 2: Healing Gate
            else if (!actor.hasStatus("inner_gate_2") && actor.data.level >= 2)
            {
                removeAllGates(actor);
                actor.addStatusEffect("inner_gate_2", 35f);
                actor.restoreHealth((int)(maxHealth * 0.20f));
                return true;
            }
            // Gate 1: Opening Gate
            else if (!actor.hasStatus("inner_gate_1") && actor.data.level >= 1)
            {
                removeAllGates(actor);
                actor.addStatusEffect("inner_gate_1", 30f);
                return true;
            }

            return false;
        }

        public static bool InnerGatesStrain(BaseSimObject pSelf, WorldTile pTile = null)
        {
            if (pSelf == null || !pSelf.isAlive()) return false;
            Actor actor = pSelf.a;

            int damageAmount = 0;

            if (actor.hasStatus("inner_gate_8")) damageAmount = 25;
            else if (actor.hasStatus("inner_gate_7")) damageAmount = 12;
            else if (actor.hasStatus("inner_gate_6")) damageAmount = 8;
            else if (actor.hasStatus("inner_gate_5")) damageAmount = 5;
            else if (actor.hasStatus("inner_gate_4")) damageAmount = 2;

            if (damageAmount > 0)
            {
                actor.getHit(damageAmount, true, AttackType.Other, null, false, false);

                if (damageAmount >= 5 && actor.current_tile != null && UnityEngine.Random.value < 0.15f)
                {
                    World.world.applyForceOnTile(actor.current_tile, 5, 1.5f, true, 0);
                }
                return true;
            }

            return false;
        }

        public static void removeAllGates(Actor actor)
        {
            actor.finishStatusEffect("inner_gate_1");
            actor.finishStatusEffect("inner_gate_2");
            actor.finishStatusEffect("inner_gate_3");
            actor.finishStatusEffect("inner_gate_4");
            actor.finishStatusEffect("inner_gate_5");
            actor.finishStatusEffect("inner_gate_6");
            actor.finishStatusEffect("inner_gate_7");
            actor.finishStatusEffect("inner_gate_8");
        }
    }
}