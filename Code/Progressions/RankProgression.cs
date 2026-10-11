using UnityEngine;

namespace ShinobiBox
{
    public static class RankProgression
    {
        public static void CheckRankProgression(Actor a)
        {
            if (!ShinobiConfig.EnableAutoRanking) return;
            if (!a.isSapient()) return;

            int level = a.data.level;
            int kills = a.data.kills;
            int age = a.data.getAge();

            if (a.isKing() && !a.hasTrait("06rank_kage"))
            {
                RemoveAllRanks(a);
                a.addTrait("06rank_kage");
                a.restoreHealth(a.getMaxHealth());
                return;
            }

            bool anyChakraNature = a.hasTrait("fireN") || a.hasTrait("waterN") || a.hasTrait("earthN") || a.hasTrait("windN") || a.hasTrait("lightningN") || a.hasTrait("yin_release") || a.hasTrait("yang_release") || a.hasTrait("wood_release") || a.hasTrait("ice_release") || a.hasTrait("lava_release") || a.hasTrait("boil_release") || a.hasTrait("stormR") || a.hasTrait("dust_release");

            bool gifted = a.hasTrait("02_high_chakra_reserve") || a.hasTrait("will_of_fire") || a.hasTrait("curse_of_hatred") ||
            a.hasTrait("03_greater_chakra_reserve") ||
            a.hasTrait("04_vast_chakra_reserve") || a.hasTrait("genius") || anyChakraNature;

            bool isUnranked = !a.hasTrait("01rank_academy_student") &&
            !a.hasTrait("02rank_genin") && !a.hasTrait("03rank_chunin") &&
            !a.hasTrait("04rank_jonin") && !a.hasTrait("05rank_anbu") &&
            !a.hasTrait("06rank_kage") && !a.hasTrait("07rank_sannin") &&
            !a.hasTrait("08rank_ghost_of_uchiha") &&
            !a.hasTrait("09rank_god_of_shinobi");

            if (isUnranked && age >= 12 && age < 40 && gifted)
            {
                float chance = gifted ? 0.04f : 0.0f;
                if (Randy.randomChance(chance))
                {
                    a.addTrait("01rank_academy_student");
                }
            }

            if (a.hasTrait("01rank_academy_student") && (level >= 3 || (age >= 16 && kills >= 2)))
            {
                a.removeTrait("01rank_academy_student");
                a.addTrait("02rank_genin");
                return;
            }

            if (a.hasTrait("02rank_genin") && (level >= 5 || kills >= 8))
            {
                a.removeTrait("02rank_genin");
                a.addTrait("03rank_chunin");
                return;
            }

            if (a.hasTrait("03rank_chunin") && (level >= 7 || kills >= 20))
            {
                a.removeTrait("03rank_chunin");
                a.addTrait("04rank_jonin");
                return;
            }

            if (a.hasTrait("04rank_jonin") && (level >= 9 || kills >= 35))
            {
                a.removeTrait("04rank_jonin");
                a.addTrait("05rank_anbu");
                return;
            }
        }

        private static void RemoveAllRanks(Actor a)
        {
            a.removeTrait("01rank_academy_student");
            a.removeTrait("02rank_genin");
            a.removeTrait("03rank_chunin");
            a.removeTrait("04rank_jonin");
            a.removeTrait("05rank_anbu");
            a.removeTrait("06rank_kage");
        }
    }
}