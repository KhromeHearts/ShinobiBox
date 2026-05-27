using UnityEngine;

namespace ShinobiBox
{
    public static class EightGatesProgression
    {
        private static readonly string[] InnerGateStatusIds =
        {
            "inner_gate_1",
            "inner_gate_2",
            "inner_gate_3",
            "inner_gate_4",
            "inner_gate_5",
            "inner_gate_6",
            "inner_gate_7",
            "inner_gate_8"
        };

        private static readonly int[] InnerGateLevelRequirements = { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static readonly float[] InnerGateChakraRequirements = { 100f, 150f, 225f, 300f, 350f, 400f, 450f, 500f };
        private static readonly float[] InnerGateHealthDrainPct = { 0f, 0f, 0f, 0.01f, 0.03f, 0.05f, 0.08f, 0.13f };
        private static readonly float[] InnerGateChakraDrainPct = { 0f, 0f, 0f, 0.01f, 0.01f, 0.01f, 0.03f, 0.05f };

        public static bool EightInnerGates(BaseSimObject pSelf, BaseSimObject pAttackedBy = null, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            Actor actor = pSelf.a;

            if (!actor.hasTrait("eight_inner_gates")) return false;
            if (actor.hasStatus("status_ability_cooldown")) return false;

            float maxHealth = actor.getMaxHealth();
            float health = actor.data.health;
            if (health > maxHealth / 1.5f) return false;
            if (ChakraSystem.GetMax(actor) < 100f) return false;

            if (UnityEngine.Random.value > 0.20f) return false;

            int currentGate = GetActiveInnerGate(actor);
            int nextGate = Mathf.Clamp(currentGate + 1, 1, 8);
            if (currentGate >= 8) return false;

            int idx = nextGate - 1;
            if (actor.data.level < InnerGateLevelRequirements[idx]) return false;
            if (ChakraSystem.GetMax(actor) < InnerGateChakraRequirements[idx]) return false;
            if (!ChakraSystem.TryConsume(actor, InnerGateChakraRequirements[idx])) return false;

            ClearInnerGateStatuses(actor);
            actor.addStatusEffect(InnerGateStatusIds[idx], 25f + idx * 5f);
            actor.data.set("inner_gates_active_gate", nextGate);
            actor.data.set("inner_gates_pulse_t", Time.time);

            if (nextGate == 2)
            {
                actor.restoreHealth((int)(actor.getMaxHealth() * 0.20f));
            }

            if (nextGate >= 5 && UnityEngine.Random.value < 0.25f && !actor.hasTrait("crippled"))
            {
                actor.addTrait("crippled");
            }

            return true;
        }

        public static void TickEightInnerGates(Actor actor)
        {
            if (actor == null || !actor.isAlive()) return;

            int activeGate = 0;
            actor.data.get("inner_gates_active_gate", out activeGate);
            if (activeGate <= 0) return;

            int currentGate = GetActiveInnerGate(actor);
            if (currentGate <= 0)
            {
                ApplyInnerGateEndEffects(actor, activeGate);
                actor.data.set("inner_gates_active_gate", 0);
                return;
            }

            actor.data.set("inner_gates_active_gate", currentGate);

            if (ChakraSystem.GetCurrent(actor) <= 0f)
            {
                StopEightInnerGates(actor, true);
            }
        }

        public static bool InnerGatesPulseAction(BaseSimObject pSelf, WorldTile pTile)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            Actor actor = pSelf.a;

            if (!actor.hasTrait("eight_inner_gates")) return false;

            int gate = GetActiveInnerGate(actor);
            if (gate <= 0) return false;

            float lastPulse = 0f;
            actor.data.get("inner_gates_pulse_t", out lastPulse);
            if (Time.time - lastPulse < 1f) return false;
            actor.data.set("inner_gates_pulse_t", Time.time);

            if (gate >= 4 && UnityEngine.Random.value < 0.30f)
            {
                int idx = gate - 1;
                float healthLoss = actor.getMaxHealth() * InnerGateHealthDrainPct[idx];
                if (healthLoss > 0f)
                {
                    actor.getHit(healthLoss, true, AttackType.Other, actor, false);
                }

                float chakraLoss = ChakraSystem.GetMax(actor) * InnerGateChakraDrainPct[idx];
                if (chakraLoss > 0f)
                {
                    ChakraSystem.RemoveChakra(actor, chakraLoss);
                }
            }

            if (gate >= 5 && actor.current_tile != null && UnityEngine.Random.value < 0.01f)
            {
                World.world.applyForceOnTile(actor.current_tile, 6, 1.6f, true, 0);
            }

            if (ChakraSystem.GetCurrent(actor) <= 0f)
            {
                StopEightInnerGates(actor, true);
            }

            return true;
        }

        private static void StopEightInnerGates(Actor actor, bool chakraBroke)
        {
            if (actor == null || !actor.isAlive()) return;

            int activeGate = 0;
            actor.data.get("inner_gates_active_gate", out activeGate);
            if (activeGate > 0)
            {
                ApplyInnerGateEndEffects(actor, activeGate);
            }

            ClearInnerGateStatuses(actor);
            actor.data.set("inner_gates_active_gate", 0);

            if (chakraBroke)
            {
                actor.addStatusEffect("chakra_exhaustion", 25f);
                actor.addStatusEffect("status_ability_cooldown", 80f);
            }
        }

        private static void ApplyInnerGateEndEffects(Actor actor, int gate)
        {
            if (actor == null || !actor.isAlive()) return;

            if (gate == 3)
            {
                actor.addStatusEffect("chakra_exhaustion", 20f);
            }

            if (gate >= 5)
            {
                actor.addStatusEffect("eight_gate_strain", 30f);
            }
        }

        private static int GetActiveInnerGate(Actor actor)
        {
            if (actor == null) return 0;

            for (int i = InnerGateStatusIds.Length - 1; i >= 0; i--)
            {
                if (actor.hasStatus(InnerGateStatusIds[i]))
                {
                    return i + 1;
                }
            }

            return 0;
        }

        private static void ClearInnerGateStatuses(Actor actor)
        {
            if (actor == null) return;

            for (int i = 0; i < InnerGateStatusIds.Length; i++)
            {
                actor.finishStatusEffect(InnerGateStatusIds[i]);
            }
        }
    }
}