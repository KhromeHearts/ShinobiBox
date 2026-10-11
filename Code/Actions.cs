using System;
using UnityEngine;


namespace ShinobiBox
{
    internal static class ShinobiActions
    {
        #region Hashirama Cells Action
        public static bool HashiCells(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            Actor a = pTarget.a;

            if (Randy.randomChance(1f))
            {
                a.restoreHealth((int)(a.getMaxHealth() * 0.01f));
                a.removeTrait("skin_burns");
                a.removeTrait("eyepatch");
                a.removeTrait("crippled");
                return true;
            }
            return false;
        }
        #endregion

        #region Sage Mode Actions
        public static bool SageMode(BaseSimObject pSelf, BaseSimObject pAttackedBy = null, WorldTile pTile = null)
        {
            if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false;
            Actor a = pSelf.a;

            if (a.hasTrait("slug_sage_mode"))
            {
                if (!a.hasStatus("status_100_sealings") && Randy.randomChance(0.07f))
                {
                    addShinobiStatus(a, "status_100_sealings", 0.6f, 0, 0);
                }
            }

            return false;
        }

        public static bool OneHundredHealing(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            Actor a = pTarget.a;

            if (Randy.randomChance(0.8f))
            {
                a.restoreHealth((int)(10 + a.data.level * a.getMaxHealth() * 0.07f));
                if (Randy.randomChance(0.6f))
                {
                    a.removeTrait("skin_burns");
                    a.removeTrait("eyepatch");
                    a.removeTrait("crippled");
                }
            }
            return true;
        }
        #endregion

        #region addStatus Method
        // Method inspired from code by ClonnedSchrott's on Discord
        public static bool addShinobiStatus(Actor a, string status, float removechakra = 0f, float chakra = 0f, int heal = 0)
        {
            if (a == null || string.IsNullOrEmpty(status)) return false;

            var statusAsset = AssetManager.status.get(status);
            if (statusAsset == null)
            {
                Debug.LogWarning("missing status: " + status);
                return false;
            }

            if (removechakra > 0f && !JutsuLibrary.CheckAndConsumeChakra(a, removechakra))
            {
                return false;
            }

            float healthPercent = 0f;

            if (a.isAlive() && a.data != null && a.getMaxHealth() > 0)
            {
                healthPercent = Mathf.Clamp01((float)a.data.health / (float)a.getMaxHealth());
            }
            a.addStatusEffect(status);
            a.updateStats();

            if (a.isAlive() && a.data != null && a.getMaxHealth() > 0)
            {
                int maxHealth = a.getMaxHealth();
                a.data.health = Mathf.Clamp((int)(maxHealth * healthPercent), 1, maxHealth);

                if (heal != 0)
                {
                    heal = (int)statusAsset.base_stats["health"];
                }

                if (heal != 0)
                {
                    a.data.health = Mathf.Clamp(a.data.health + heal, 1, maxHealth);
                }
            }

            if (chakra != 0 && a != null)
            {
                ChakraSystem.AddChakra(a, chakra);
            }

            return true;
        }
        #endregion

    }
}