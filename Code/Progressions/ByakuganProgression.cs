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
            bool isOtsutsuki = a.hasTrait("otsutsuki_clan");

            if (a.has_attack_target)
            {
                if (level >= 5 && isOtsutsuki)
                {
                    a.addStatusEffect("byakuganG", 60f);
                    return true;
                }
                else if (!a.hasStatus("byakuganP") && (level >= 3 || kills >= 3))
                {
                    a.addStatusEffect("byakuganP", 60f);
                    return true;
                }
                else if (level >= 1)
                {
                    a.addStatusEffect("byakugan", 60f);
                    return true;
                }
                return false;  
            }

            return false;

        }
    }
}