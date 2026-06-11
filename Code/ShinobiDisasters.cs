using System;
using System.Collections.Specialized;
using UnityEngine;

namespace ShinobiBox
{
    public static class ShinobiDisasters
    {
        public static void Init()
        {
            #region Hagoromo's Blessing
            DisasterAsset blessing = new DisasterAsset
            {
                id = "hagoromo_blessing",
                rate = 1,
                chance = 12f,
                world_log = "log_hagoromo_blessing",
                min_world_population = 50,
                min_world_cities = 0
            };
            blessing.action = new DisasterAction(HagBlessing);
            AssetManager.disasters.add(blessing);
            #endregion
        }

        public static void HagBlessing(DisasterAsset pAsset)
        {
            Actor actor = World.world.units.GetRandom();
            
            if (!actor.hasTrait("07_indra_chakra") || !actor.hasTrait("08_asura_chakra"))
            {
                if (Randy.randomChance(0.50f))
                {
                    actor.addTrait("07_indra_chakra");
                }
                else
                {
                    actor.addTrait("08_asura_chakra");
                }

                ShinobiWorldLogs.AddWorldLog("log_hagoromo_blessing", "worldlog_hagoromo_blessing", "ui/icons/units/IconHagoromo", actor, "disasters");
            }

        }
    }
}