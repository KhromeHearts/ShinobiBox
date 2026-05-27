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
            bool hasSixPaths = actor.hasTrait("six_paths_sage_mode");

            bool hasAnySenjutsu = hasFrog || hasSlug || hasSnake || hasWood || hasSixPaths;

            actor.data.get("sage_progression_opt_out", out int sageProgressionOptOut);
            actor.data.get("sage_progression_had_trait", out int hadSageTraitBefore);

            if (hasAnySenjutsu)
            {
                if (hadSageTraitBefore == 0) actor.data.set("sage_progression_had_trait", 1);
            }
            else
            {
                if (hadSageTraitBefore > 0 && sageProgressionOptOut == 0)
                {
                    actor.data.set("sage_progression_opt_out", 1);
                    sageProgressionOptOut = 1;
                }
                if (sageProgressionOptOut > 0) return;
            }

            if (!hasWood && actor.hasTrait("wood_release") && actor.hasTrait("hashi_cells"))
            {
                if (JutsuLibrary.GetNatureExp(actor, "wood") >= 600f)
                {
                    actor.addTrait("wood_sage_mode");
                    hasAnySenjutsu = true;
                }
            }

            if (level >= 4 && !hasAnySenjutsu && actor.current_tile != null)
            {
                string biomeId = actor.current_tile.Type?.biome_id;

                if (biomeId == "biome_jungle" || biomeId == "biome_swamp")
                {
                    actor.addTrait("frog_sage_mode");
                    hasFrog = true;
                }
                else if (biomeId == "biome_lemon" || biomeId == "biome_enchanted")
                {
                    actor.addTrait("slug_sage_mode");
                    hasSlug = true;
                }
                else if (biomeId == "biome_rocklands" || biomeId == "biome_desert")
                {
                    actor.addTrait("snake_sage_mode");
                    hasSnake = true;
                }
            }

            if (hasFrog || hasSlug || hasSnake)
            {
                bool isPerfectEligible = level >= 6 && ChakraSystem.CalculateBaseMaxChakra(actor) >= 400f;

                if (hasFrog) ManageSageStatus(actor, "frog", isPerfectEligible);
                if (hasSlug) ManageSageStatus(actor, "slug", isPerfectEligible);
                if (hasSnake) ManageSageStatus(actor, "snake", isPerfectEligible);
            }

            if (!hasSixPaths && actor.hasStatus("status_six_paths_senjutsu") && actor.hasStatus("status_perfect_frog_sage"))
            {
                actor.addTrait("six_paths_sage_mode");
            }
        }

        private static void ManageSageStatus(Actor actor, string sageType, bool isPerfectEligible)
        {
            string imperfectStatus = $"status_imperfect_{sageType}_sage";
            string perfectStatus = $"status_perfect_{sageType}_sage";

            if (isPerfectEligible)
            {
                if (!actor.hasStatus(perfectStatus))
                {
                    actor.finishStatusEffect(imperfectStatus);
                    actor.addStatusEffect(perfectStatus, 45f);
                }
            }
            else
            {
                if (!actor.hasStatus(imperfectStatus) && !actor.hasStatus(perfectStatus))
                {
                    actor.addStatusEffect(imperfectStatus, 45f);
                }
            }
        }
    }
}