using UnityEngine;

namespace ShinobiBox
{
    public static class Byakugan
    {
        public static bool ByakuganAction(BaseSimObject pSelf = null, BaseSimObject pAttackedBy = null, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || pSelf.a.data == null) return false;

            Actor a = pSelf.a;
            int level = a.data.level;
            int kills = a.data.kills;

            if (!a.hasStatus("byakuganP") && (level >= 3 || kills >= 3))
            {
                a.addStatusEffect("byakuganP", 45f);
                return true;
            }

            return false;
        }
        public static bool TenseiganAction(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || pTarget.a == null || pTarget.a.data == null) return false;

            Actor a = pTarget.a;
            bool done = false;
            a.data.get("tenseigan_done", out done);
            if (done) return false;

            if (a.hasTrait("06_hamura_chakra") && !a.hasTrait("tenseigan"))
            {
                a.removeTrait("byakugan");
                a.addTrait("tenseigan");

                if (ShinobiConfig.EnableWorldTips)
                {
                    ShinobiWorldLogs.AddWorldLog("log_tenseigan", "worldlog_tenseigan", "ui/icons/tenseigan", a);
                }
                a.data.set("tenseigan_done", true);
                return true;
            }
            return false;
        }

        public static bool TenseiganCM(BaseSimObject pSelf = null, BaseSimObject pAttackedBy = null, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || pSelf.a.data == null) return false;

            Actor a = pSelf.a;
            if (!a.hasStatus("tenseigan_cm"))
            {
                a.addStatusEffect("tenseigan_cm", 60f);
                return true;
            }

            return false;
        }
    }
}