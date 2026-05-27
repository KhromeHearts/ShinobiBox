using UnityEngine;

namespace ShinobiBox
{
    public static class ClanProgression
    {
        public static void KaguyaProgression(Actor actor)
        {
            if (actor == null || !actor.isAlive()) return;
            if (!actor.hasTrait("kaguya_clan")) return;
            if (actor.data.getAge() < 15) return;
            if (actor.hasTrait("shikotsumyaku")) return;

            if (UnityEngine.Random.value < 0.005f)
            {
                actor.addTrait("shikotsumyaku");
            }
        }

        public static void SenjuProgression(Actor actor, int level)
        {
            if (actor == null || !actor.isAlive()) return;
            if (!actor.hasTrait("senju_clan")) return;
            if (level < 5) return;
            if (actor.hasTrait("hashi_cells")) return;

            if (UnityEngine.Random.value < 0.01f)
            {
                actor.addTrait("hashi_cells");

                if (ShinobiConfig.EnableWorldTips)
                {
                    ShinobiWorldLogs.AddWorldLog("log_hashi_cells", "worldlog_hashi_cells", "ui/icons/hashirama_cells", actor);
                }
            }
        }

        public static void HyugaProgression(Actor actor)
        {
            if (actor == null || !actor.isAlive()) return;
            if (!actor.hasTrait("hyuga_clan")) return;
            if (actor.hasTrait("byakugan")) return;

            actor.addTrait("byakugan");
        }

        public static void LeeProgression(Actor actor, int level, int age)
        {
            if (actor == null || !actor.isAlive()) return;
            if (!actor.hasTrait("lee_clan")) return;

            if (level >= 4 && !actor.hasTrait("taijutsu_master"))
            {
                actor.addTrait("taijutsu_master");
            }

            if (age >= 24 && !actor.hasTrait("eight_inner_gates") && UnityEngine.Random.value < 0.01f)
            {
                actor.addTrait("eight_inner_gates");
            }
        }
    }
}