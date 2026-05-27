using UnityEngine;

namespace ShinobiBox
{
    public static class RankProgression
    {
        public static void CheckRankProgression(Actor a)
        {
            if (!ShinobiConfig.EnableAutoRanking) return;

            int level = a.data.level;
            int kills = a.data.kills;

            if (a.isKing() && !a.hasTrait("rank_kage"))
            {
                RemoveAllRanks(a);
                a.addTrait("rank_kage");
                a.restoreHealth(a.getMaxHealth());
                return;
            }

            bool gifted = a.hasTrait("high_chakra_reserve") || a.hasTrait("will_of_fire") || a.hasTrait("uchiha_clan") || a.hasTrait("nine_tails_jinchuriki") ||
            a.hasTrait("hyuga_clan") || a.hasTrait("uzumaki_clan") ||
            a.hasTrait("akimichi_clan") ||
            a.hasTrait("senju_clan") || a.hasTrait("great_chakra_reserve") ||
            a.hasTrait("vast_chakra_reserve") || a.hasTrait("genius") || level >= 3 || kills >= 4;

            bool isUnranked = !a.hasTrait("rank_academy_student") &&
            !a.hasTrait("rank_genin") && !a.hasTrait("rank_chunin") &&
            !a.hasTrait("rank_jonin") && !a.hasTrait("rank_anbu") &&
            !a.hasTrait("rank_kage") && !a.hasTrait("rank_sannin") &&
            !a.hasTrait("rank_ghost_of_uchiha") &&
            !a.hasTrait("rank_god_of_shinobi");

            if (isUnranked && a.data.getAge() < 50)
            {
                if (gifted)
                {
                    if (UnityEngine.Random.value < 0.01f)
                    {
                        a.addTrait("rank_academy_student");
                    }
                }
            }

            if (a.hasTrait("rank_anbu") && level >= 9)
            {
                a.removeTrait("rank_anbu");
                a.addTrait("rank_kage");
                return;
            }

            if (a.hasTrait("rank_jonin") && level >= 7)
            {
                a.removeTrait("rank_jonin");
                a.addTrait("rank_anbu");
                return;
            }

            if (a.hasTrait("rank_chunin") && level >= 5)
            {
                a.removeTrait("rank_chunin");
                a.addTrait("rank_jonin");
                return;
            }

            if (a.hasTrait("rank_genin") && level >= 3)
            {
                a.removeTrait("rank_genin");
                a.addTrait("rank_chunin");
                return;
            }

            if (a.hasTrait("rank_academy_student") && level >= 2)
            {
                a.removeTrait("rank_academy_student");
                a.addTrait("rank_genin");
                return;
            }
        }

        private static void RemoveAllRanks(Actor a)
        {
            a.removeTrait("rank_academy_student");
            a.removeTrait("rank_genin");
            a.removeTrait("rank_chunin");
            a.removeTrait("rank_jonin");
            a.removeTrait("rank_anbu");
        }
    }
}