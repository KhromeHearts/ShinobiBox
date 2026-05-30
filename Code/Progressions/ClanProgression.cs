using System;
using UnityEngine;

namespace ShinobiBox
{
    public static class ClanProgression
    {
        public static bool Kaguya(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isAlive()) return false;
            Actor a = pTarget.a;

            a.addTrait("shikotsumyaku");
            return true;
        }

        public static bool Senju(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isAlive()) return false;
            Actor a = pTarget.a;
            int level = a.data.level;
            bool done = false;

            a.data.get("awakened", out done, false);

            if (done) return false;

            if (Randy.randomChance(0.04f) && !a.hasTrait("hashi_cells"))
            {
                a.addTrait("hashi_cells");

                if (ShinobiConfig.EnableWorldTips)
                {
                    ShinobiWorldLogs.AddWorldLog("log_hashi_cells", "worldlog_hashi_cells", "ui/icons/hashirama_cells", a);
                }
                a.data.set("awakened", true);
                return true;
            }
            else
            {
                a.addTrait("wood_release");

                if (ShinobiConfig.EnableWorldTips)
                {
                    ShinobiWorldLogs.AddWorldLog("log_wood_release", "worldlog_wood_release", "ui/icons/wood_release", a);
                }
                a.data.set("awakened", true);
                return true;
            }
            return false;
        }

        public static bool Hyuga(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isAlive()) return false;
            Actor a = pTarget.a;
            Debug.Log($"ClanProgression.Hyuga invoked for actor: {a?.data?.name} (id:{a?.id})");

            a.addTrait("byakugan");

            return true;
        }

        public static bool Lee(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isAlive()) return false;
            Actor a = pTarget.a;
            int level = a.data.level;
            int age = a.data.getAge();
            if (!a.hasTrait("lee_clan")) return false;
            bool done = false;

            if (age >= 24 && !a.hasTrait("eight_inner_gates") && Randy.randomChance(0.10f))
            {
                a.addTrait("eight_inner_gates");
                return true;
            }

            a.data.get("awakened", out done, false);
            if (done) return false;

            if (level >= 4 && !a.hasTrait("taijutsu_master"))
            {
                a.addTrait("taijutsu_master");
                a.data.set("awakened", true);
                return true;
            }

            return false;
        }

        public static bool OtsutsukiBirth(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isAlive()) return false;
            Actor a = pTarget.a;
            Debug.Log($"ClanProgression.OtsutsukiBirth invoked for actor: {a?.data?.name} (id:{a?.id})");

            string currentName = a.data.name;
            string clanName = "Otsutsuki";

            a.addTrait("byakugan");
            a.data.name = $"{currentName} {clanName}";

            if (Randy.randomChance(0.10f))
            {
                a.addTrait("rinnegan");
            }
            return true;
        }

    }
}