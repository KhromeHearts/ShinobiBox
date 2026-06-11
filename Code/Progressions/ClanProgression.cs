using System;
using System.Data;
using UnityEngine;

namespace ShinobiBox
{
    public static class ClanProgression
    {
        public static bool Kaguya(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isAlive()) return false;
            Actor a = pTarget.a;
            bool done = false;

            a.data.get("shiko_awk", out done, false);

            if (done) return false;

            if (!a.hasTrait("shikotsumyaku") && Randy.randomChance(0.20f))
            {
                a.addTrait("shikotsumyaku");
                a.data.set("shiko_awk", true);
                return true;
            }
            return false;
            
        }

        public static bool Senju(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isAlive()) return false;
            Actor a = pTarget.a;
            int level = a.data.level;
            bool done = false;

            a.data.get("Senju_awakened", out done, false);

            if (done) return false;

            if (Randy.randomChance(0.04f) && !a.hasTrait("hashi_cells"))
            {
                a.addTrait("hashi_cells");

                if (ShinobiConfig.EnableWorldTips)
                {
                    ShinobiWorldLogs.AddWorldLog("log_hashi_cells", "worldlog_hashi_cells", "ui/icons/hashirama_cells", a);
                }
                a.data.set("Senju_awakened", true);
                return true;
            }
            else
            {
                a.addTrait("wood_release");

                if (ShinobiConfig.EnableWorldTips)
                {
                    ShinobiWorldLogs.AddWorldLog("log_wood_release", "worldlog_wood_release", "ui/icons/wood_release", a);
                }
                a.data.set("Senju_awakened", true);
                return true;
            }
            return false;
        }

        public static bool Hyuga(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isAlive()) return false;
            Actor a = pTarget.a;
            if (!a.hasTrait("hyuga_clan")) return false;
            if (a.hasTrait("byakugan")) return false;
            bool done = false;

            a.data.get("byakugan_awk", out done, false);

            if (done) return false;

            a.addTrait("byakugan");
            a.data.set("byakugan_awk", true);

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

            a.data.get("EIG_awakened", out done, false);
            if (done) return false;

            if (level >= 4 && !a.hasTrait("taijutsu_master"))
            {
                a.addTrait("taijutsu_master");
                a.data.set("EIG_awakened", true);
                return true;
            }

            return false;
        }

        public static bool OtsutsukiBirth(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isAlive()) return false;
            
            Actor a = pTarget.a;
            bool done = false;
            a.data.get("otsutsuki_birth_done", out done, false);


            if (Randy.randomChance(0.10f))
            {
                a.addTrait("rinnegan");
                a.data.set("otsutsuki_birth_done", true);
                return true;
            }
            else if (Randy.randomChance(0.10f))
            {
                a.addTrait("golden_byakugan");
                a.data.set("otsutsuki_birth_done", true);
                return true;
            }
            else if (Randy.randomChance(0.10f) && a.hasTrait("hamura_chakra"))
            {
                a.addTrait("Tenseigan");
                a.data.set("otsutsuki_birth_done", true);
                return true;
            }
            else
            {
                a.addTrait("byakugan");
                a.data.set("otsutsuki_birth_done", true);
                return true;
            }

            return false;
        }

    }
}