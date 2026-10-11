using System;
using System.Data;
using UnityEngine;

namespace ShinobiBox
{
    public static class ClanProgression
    {
        #region Kaguya Clan
        public static bool Kaguya(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isAlive()) return false;
            Actor a = pTarget.a;
            bool done = false;

            a.data.get("shiko_awk", out done, false);

            if (done) return false;

            if (!a.hasTrait("shikotsumyaku") && Randy.randomChance(0.01f))
            {
                a.addTrait("shikotsumyaku");
                a.data.set("shiko_awk", true);
                return true;
            }
            return false;

        }
        #endregion

        #region Hyuga Clan
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
        #endregion

        #region Lee Clan
        public static bool Lee(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isAlive()) return false;
            Actor a = pTarget.a;
            int level = a.data.level;
            int age = a.data.getAge();
            if (!a.hasTrait("lee_clan")) return false;
            bool done = false;

            // One-time roll when the unit becomes an adult. (This hook runs every few seconds, so a plain
            // "1% chance each time" would give nearly every Lee unit the Eight Inner Gates within minutes.)
            bool gatesRolled = false;
            a.data.get("lee_gates_rolled", out gatesRolled, false);
            if (age >= 18 && !gatesRolled)
            {
                a.data.set("lee_gates_rolled", true);
                if (!a.hasTrait("eight_inner_gates") && Randy.randomChance(0.01f))
                {
                    a.addTrait("eight_inner_gates");
                    return true;
                }
            }

            a.data.get("EIG_awakened", out done, false);
            if (done) return false;

            if (level >= 3 && !a.hasTrait("taijutsu_master"))
            {
                a.addTrait("taijutsu_master");
                a.data.set("EIG_awakened", true);
                return true;
            }

            return false;
        }
        #endregion

        #region Otsutsuki Clan
        public static bool OtsutsukiBirth(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isAlive()) return false;
            Actor a = pTarget.a;

            if (!a.hasTrait("otsutsuki_clan")) return false;

            bool done = false;
            a.data.get("otsutsuki_birth_done", out done, false);
            if (done) return false;

            float roll = UnityEngine.Random.value;

            if (roll < 0.05f)
            {
                a.addTrait("rinnegan");
            }
            else if (roll < 0.10f)
            {
                a.addTrait("golden_byakugan");
            }
            else if (roll < 0.30f && a.hasTrait("06_hamura_chakra"))
            {
                a.addTrait("tenseigan"); 
            }
            else
            {
                a.addTrait("byakugan");
                a.data.set("otsutsuki_birth_done", true);
                return true;
            }

            a.data.set("otsutsuki_birth_done", true);
            return true;
        }
        #endregion

        #region Chinoike Clan
        public static bool Chinoike(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isAlive()) return false;
            Actor a = pTarget.a;
            bool done = false;

            a.data.get("chinoike_awk", out done, false);

            if (done) return false;

            a.addTrait("ketsuryugan");
            a.data.set("chinoike_awk", true);

            return true;
        }
        #endregion

    }
}