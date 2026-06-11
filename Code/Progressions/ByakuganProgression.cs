using UnityEngine;

namespace ShinobiBox
{
    public static class Byakugan
    {
        public static bool ByakuganAction(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (pTarget == null || pTarget.a == null || pTarget.a.data == null) return false;

            Actor a = pTarget.a;
            int level = a.data.level;
            int kills = a.data.kills;

            if (a.has_attack_target)
            {
                if (!a.hasStatus("byakuganP") && (level >= 3 || kills >= 3))
                {
                    a.addStatusEffect("byakuganP", 60f);
                    return true;
                }
                return false;  
            }

            return false;

        }
    }
}